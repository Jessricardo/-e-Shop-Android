
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
	[Activity(Label = "segundaProductos")]
	public class segundaProductos : Activity
	{
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		ProductosAdapterPartida ProductosAdapter;
		List<PartidasModel> partidasList;
		string idPedido;
	
		List<ProductModel> ProductList;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.pedidoProductosLista);
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Productos del Pedido";

			Bundle paquete = Intent.GetBundleExtra("bundle");
			idPedido = paquete.GetString("idPedido");
			mRecyclerView = FindViewById<RecyclerView>(Resource.Id.rvpedidoProducto);
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
		}

		void OnItemClick(object sender, int e)
		{
			throw new NotImplementedException();
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
				return null;
			}
		}
}
}
