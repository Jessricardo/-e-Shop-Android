
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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ClientePizzasApp
{
	[Activity(Label = "pedidos")]
	public class pedidos : Activity
	{
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		pedidoAdapter pedidoAdapter2;
		List<PedidoModel> pedidosList=new List<PedidoModel>();
		string name;
		//ArrayAdapter adapter;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.pedidos);
			// Create your application here
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Pedidos";

			mRecyclerView = FindViewById<RecyclerView>(Resource.Id.pedidoRV);
			mLayoutManager = new LinearLayoutManager(this);
			mRecyclerView.SetLayoutManager(mLayoutManager);
			var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Obteniendo Pedidos", true);


			//PEDIDOS recycle
			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			if (preferences.GetBoolean("is_login", false))
				{
				name = preferences.GetString("name", "");
				pedidosList = await obtenerPedidos2();
				pedidosList = await obtenerPedidos2();
				//Toast.MakeText(this, pedidosList.Count().ToString(), ToastLength.Long).Show();
				pedidoAdapter2 = new pedidoAdapter(pedidosList);
				pedidoAdapter2.ItemClick += OnItemClick;

				mRecyclerView.SetAdapter(pedidoAdapter2);
				}
			    

			progressDialog.Dismiss();
		}
		public async Task<List<PedidoModel>> obtenerPedidos2()
		{
			List<PedidoModel> model = new List<PedidoModel>();
			string token = "";


			token = name.ToString();
				//token = "prueba@hotmail.com";
				string baseurl = "http://pushstart.azurewebsites.net/api/ApiPedido/"+token+"/";
				var Client = new HttpClient();
				Client.MaxResponseContentBufferSize = 256000;
				var uril = new Uri(baseurl);
				var response = await Client.GetAsync(uril);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var items = JsonConvert.DeserializeObject<List<PedidoModel>>(content);
				return items;
			}
			else
			{
				return model;
				}
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
		void OnItemClick(object sender, int e)
		{
			var intento = new Intent(this, typeof(segundaProductos));
			Bundle contenedor = new Bundle();
			contenedor.PutString("idPedido", pedidosList[e].id);
			intento.PutExtra("bundle", contenedor);
			StartActivity(intento);
		}

}
}
