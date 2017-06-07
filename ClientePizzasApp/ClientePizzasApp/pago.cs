
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ClientePizzasApp
{
	[Activity(Label = "pago")]
	public class pago : Activity
	{
		IPedidoRepository db;
		TextView cantidad, total, codigoTexto, codigo;
		Button btnComprar;
		double tota;
		int cantidadP;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.pagar);
			// Create your application here
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Detalles de pago";

			cantidad = FindViewById<TextView>(Resource.Id.txtCantidadArticulos);
			total = FindViewById<TextView>(Resource.Id.txtTotal);
			codigo = FindViewById<TextView>(Resource.Id.txtCodigo);
			codigoTexto = FindViewById<TextView>(Resource.Id.txtCodigoTexto);
			btnComprar = FindViewById<Button>(Resource.Id.btnComprar);
			//btnComprobarPago = FindViewById<Button>(Resource.Id.btnComprobarPago);
			btnComprar.Click += generarCodigo;
			//btnComprobarPago.Click += comprobarPago;
			codigo.Visibility=ViewStates.Invisible;
			codigoTexto.Visibility=ViewStates.Invisible;
		//	btnComprobarPago.Visibility = ViewStates.Invisible;
			Bundle paquete = Intent.GetBundleExtra("bundle");
			tota = paquete.GetDouble("total");
			cantidadP = paquete.GetInt("cantidadProductos");
			cantidad.Text = cantidadP.ToString();
			total.Text = tota.ToString();
		}

		void comprobarPago(object sender, EventArgs e)
		{
			
		}

		async void generarCodigo(object sender, EventArgs e)
		{
			
			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			string myValue = "";

			if (preferences.GetBoolean("is_login", false))
			{
				myValue = preferences.GetString("token", "");
				PedidoPost pedido = new PedidoPost();
				PedidoModel pedido2 = new PedidoModel();
				pedido.idUsuario = preferences.GetString("name", "");
				pedido.APIKey = myValue;
				pedido.total = tota;
			
				string baseurl = "http://pushstart.azurewebsites.net/api/ApiPedido";
				var Client = new HttpClient();
				Client.MaxResponseContentBufferSize = 256000;
				var uri = new Uri(baseurl);
				var json = JsonConvert.SerializeObject(pedido);
				StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = Client.PostAsync(uri, content).Result;

									if (response.IsSuccessStatusCode)
									{
										var content2 = await response.Content.ReadAsStringAsync();
										var items = JsonConvert.DeserializeObject<PedidoModel>(content2);
										Toast.MakeText(this, "Exito", ToastLength.Short).Show();
										Intent esIntento = new Intent(this, typeof(detalleYaPa));
										Bundle esPaquete = new Bundle();
										esPaquete.PutString("cuenta", items.cuenta);
										esPaquete.PutString("dinero", items.total.ToString());
										esPaquete.PutString("idPedido", items.idUsuario.ToString());
										esIntento.PutExtra("bundle",esPaquete);

										//Toast.MakeText(this, items.cuenta, ToastLength.Long).Show();
										StartActivity(esIntento);
									}
									else
									{
										Toast.MakeText(this, "Algo ha salido mal, lo sentimos. Intente más tarde", ToastLength.Short).Show();

									}
			}
			else
			{
				Toast.MakeText(this, "No ha iniciado sesión", ToastLength.Long).Show();
				myValue = preferences.GetString("NotToken", "");
				Intent intento = new Intent(this, typeof(login2));
				StartActivity(intento);

			}

		}
		public override bool OnCreateOptionsMenu(IMenu menu)
{
MenuInflater.Inflate(Resource.Menu.top_menus, menu);
return base.OnCreateOptionsMenu(menu);
}
public override bool OnOptionsItemSelected(IMenuItem item)
{
	//Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
	//	ToastLength.Short).Show();

	if (item.TitleFormatted.ToString() == "Cerrar sesión")
	{
		Intent intento = new Intent(this, typeof(MainActivity));
				StartActivity(intento);
	}
		else if (item.TitleFormatted.ToString() == "Save")
			{
				Intent intento = new Intent(this, typeof(carrito));
				StartActivity(intento);
			}
			else if(item.TitleFormatted.ToString()=="Edit")
			{
				Intent intento = new Intent(this, typeof(MenuActivity));
				StartActivity(intento);
			}
			else if(item.TitleFormatted.ToString()=="Perfil")
			{
Intent intento = new Intent(this, typeof(perfil));
			StartActivity(intento);
			}
			else if(item.TitleFormatted.ToString()=="Pedidos")
			{
				Intent intento = new Intent(this, typeof(pedidos));
				StartActivity(intento);
			}
	return base.OnOptionsItemSelected(item);
}

}
}
