
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ClientePizzasApp
{
	[Activity(Label = "detalleYaPa")]
	public class detalleYaPa : Activity
	{
		TextView txtcuenta, txtcantidad;
		string cantidad, cuenta, idPedido;
		List<PartidasModel> partidasList;
		List<ProductModel> ProductList;
		ListView lvPago;
		Button btnDeDetalleMenu;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.detalleYaPago);
			// Create your application here
			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Información de pago";
			txtcuenta = FindViewById<TextView>(Resource.Id.textView2Cuenta);
			txtcantidad = FindViewById<TextView>(Resource.Id.textView3Cantidad);
			btnDeDetalleMenu = FindViewById<Button>(Resource.Id.btnDetallesAmenu);
			btnDeDetalleMenu.Click += detalle;

			Bundle paquete = Intent.GetBundleExtra("bundle");
			cuenta = paquete.GetString("cuenta");
			cantidad = paquete.GetString("dinero");
			idPedido = paquete.GetString("idPedido");
			txtcuenta.Text = cuenta;
			txtcantidad.Text = cantidad;

			//var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Obteniendo Productos", true);
			//partidasList = await obtenerPartidas();
			//Toast.MakeText(this, partidasList.Count(), ToastLength.Long).Show();
			//ProductList = await obtenerProductosPorPartidas();
			//List<string> nombres = new List<string>();

			//for (int i = 0; i < ProductList.Count; i++)
			//{
			//	ProductModel a = ProductList.ElementAt(i);
			//	string nombreCompleto = a.Nombre + " - " + a.Precio;
			//	nombres.Add(nombreCompleto);
			//}
			//ArrayAdapter ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, nombres);
			//lvPago.SetAdapter(ListAdapter);

			//progressDialog.Dismiss();

		}

		void detalle(object sender, EventArgs e)
		{
			StartActivity(typeof(MenuActivity));
		}
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{

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
			else if (item.TitleFormatted.ToString() == "Edit")
			{
				Intent intento = new Intent(this, typeof(MenuActivity));
				StartActivity(intento);
			}
			else if (item.TitleFormatted.ToString() == "Perfil")
			{
Intent intento = new Intent(this, typeof(perfil));
			StartActivity(intento);
			}
			else if (item.TitleFormatted.ToString() == "Pedidos")
			{
				Intent intento = new Intent(this, typeof(pedidos));
				StartActivity(intento);
			}
			return base.OnOptionsItemSelected(item);
		}
		public async Task<List<ProductModel>> obtenerProductosPorPartidas()
		{

			List<ProductModel> a = new List<ProductModel>();

			for (int i = 0; i < partidasList.Count; i++)
			{

				ProductModel producto = await obtenerProductosPorId(partidasList.ElementAt(i).productoId);
				a.Add(producto);

			}
			return a;
		}
		public async Task<ProductModel> obtenerProductosPorId(string id)
		{

			List<ProductModel> aux = await getProductos();
			ProductModel aux2 = new ProductModel();
			for (int i = 0; i < aux.Count; i++)
			{
				if (aux.ElementAt(i).Codigo == id)
				{
					aux2 = aux.ElementAt(i);
					return aux2;
				}
			}
			return null;
		}
		public async Task<List<ProductModel>> getProductos()
		{
			string baseurl = "http://pushstart.azurewebsites.net/api/listaProductos";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uri = new Uri(baseurl);
			var response = await Client.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var items = JsonConvert.DeserializeObject<List<ProductModel>>(content);
				//	Toast.MakeText(this, "Exito", ToastLength.Long).Show();
				return items;

			}
			else
			{
				Toast.MakeText(this, "Ocurrio un error :c", ToastLength.Long).Show();
				return null;
			}
		}
		public async Task<List<PartidasModel>> obtenerPartidas()
		{
			List<PartidasModel> aux = new List<PartidasModel>();
			string baseurl = "http://pushstart.azurewebsites.net/api/pedido/" + idPedido;
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uril = new Uri(baseurl);
			var response = await Client.GetAsync(uril);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var items = JsonConvert.DeserializeObject<List<PartidasModel>>(content);
				return items;
			}
			else
			{
				return aux;
			}
		}
	}
}
