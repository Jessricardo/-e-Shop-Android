
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
//@string/app_name
namespace ClientePizzasApp
{
	[Activity(Label = "carrito")]
	public class carrito : Activity
	{
		IPedidoRepository db;
		Button btnCarrito;

		user usuario;
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		ProductosAdapterPartida ProductosAdapter;
		List<PartidasModel> partidasList;
		//List<string> ids;
		ArrayAdapter adapter;
		Button btnComprar;
		List<ProductModel> ProductList;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.misProductosCarritos);
			// Create your application here
			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Carrito";
			btnComprar = FindViewById<Button>(Resource.Id.btnComprar);

			mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewCarrito);
			mLayoutManager = new LinearLayoutManager(this);
			mRecyclerView.SetLayoutManager(mLayoutManager);
			var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Obteniendo PRODUCTOS", true);


			//PRODUCTOS recycle
			partidasList = await obtenerPartidas();

			ProductList = await obtenerProductosPorPartidas();


			ProductosAdapter = new ProductosAdapterPartida(ProductList, this, partidasList);
			ProductosAdapter.ItemClick += OnItemClick;

			mRecyclerView.SetAdapter(ProductosAdapter);
			progressDialog.Dismiss();
			btnComprar.Click += comprar;
		}

		public async void comprar(object sender, EventArgs e)
		{
			
			int a = await obtenerCantidadPorProductosPorPartidas();
			double total =  await obtenerTotalPorProductosPorPartidas();

			var intento = new Intent(this, typeof(pago));
			Bundle contenedor = new Bundle();
			contenedor.PutInt("cantidadProductos", a);
			contenedor.PutDouble("total", total);
			intento.PutExtra("bundle", contenedor);
			StartActivity(intento);
		}

		protected override void OnResume()
		{
			base.OnResume();

		}



		void OnItemClick(object sender, int e)
		{
			PartidasModel a = partidasList[e];
			ProductModel producto = ProductList[e];
			var intento = new Intent(this, typeof(detalleProductoCompra));
			Bundle contenedor = new Bundle();
			contenedor.PutString("id", producto.Codigo);
			contenedor.PutString("nombre", producto.Nombre);
			contenedor.PutString("categoria", producto.Categoria);
			contenedor.PutString("precio", producto.Precio.ToString());
			contenedor.PutString("descripcion", producto.Descripcion);
			contenedor.PutString("idPartida", a.id);
			contenedor.PutString("url", producto.url);
			//contenedor.PutString("idProducto", a.productoId);
			contenedor.PutInt("cantidad", a.cantidad);
			intento.PutExtra("bundle", contenedor);
			StartActivity(intento);
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

		public async Task<double> obtenerTotalPorProductosPorPartidas()
		{

			double total = 0;

			for (int i = 0; i < partidasList.Count; i++)
			{

				ProductModel producto = await obtenerProductosPorId(partidasList.ElementAt(i).productoId);
				total += producto.Precio * partidasList.ElementAt(i).cantidad;
			

			}
			return total;        
	}

		public async Task<int> obtenerCantidadPorProductosPorPartidas()
		{

			int total = 0;

			for (int i = 0; i < partidasList.Count; i++)
			{

				ProductModel producto = await obtenerProductosPorId(partidasList.ElementAt(i).productoId);
				total +=  partidasList.ElementAt(i).cantidad;


			}
			return total;
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
		public async Task<List<PartidasModel>> obtenerPartidas()
		{
			
			string token = "";

		
				ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);

				 if (preferences.GetBoolean("is_login", false))
			{
				token = preferences.GetString("token", "");
			}
			else
			{ 
				token = preferences.GetString("NotToken", "");
			}
				string baseurl = "http://pushstart.azurewebsites.net/Carrito/detalles/?id=" + token;
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
					return null;
				}

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
			//	Intent intento = new Intent(this, typeof(MenuActivity));
			//	StartActivity(intento);
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
