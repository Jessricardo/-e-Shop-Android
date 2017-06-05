
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Newtonsoft.Json;
using SQLite;

using System.IO;
namespace ClientePizzasApp
{
	[Activity(Label = "MenuActivity")]
	public class MenuActivity : Activity
	{
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		ProductosAdapter ProductosAdapter;
		List<ProductModel> ProductList;
		Spinner spCategorias;
		ProductosAdapter a;
		string[] classes2; 
		List<string> classes;
		ArrayAdapter adapter;
		IPedidoRepository db;
		user usuario;
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.menu_activity);

			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Productos";

			mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
			spCategorias = FindViewById<Spinner>(Resource.Id.spCategorias);

			mLayoutManager = new GridLayoutManager(this, 2, GridLayoutManager.Vertical, false);
			//mLayoutManager = new LinearLayoutManager(this);
			mRecyclerView.SetLayoutManager(mLayoutManager);
			var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Obteniendo Productos", true);

			//PRODUCTOS recycle

			ProductList = await getProductos();
			ProductosAdapter = new ProductosAdapter(ProductList, this);
			ProductosAdapter.ItemClick += OnItemClick;
			mRecyclerView.SetAdapter(ProductosAdapter);

			//CATEGORIAS spinner
			classes2 = await getCategorias2();
			adapter = new ArrayAdapter<string>(
				this, Android.Resource.Layout.SimpleListItem1, classes2);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spCategorias.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
			spCategorias.Adapter = adapter;
			progressDialog.Dismiss();
			//btnCarritoG.Click += mostrarCarrito;



		}
		public void mostrarCarrito(object sender, EventArgs e)
		{
			
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
			else
			{
				Intent intento = new Intent(this, typeof(MenuActivity));
				StartActivity(intento);
			}

	return base.OnOptionsItemSelected(item);
}
			public async Task<string[]> getCategorias2()
			{
						string baseurl = "http://pushstart.azurewebsites.net/api/allcategories";
						var Client = new HttpClient();
						Client.MaxResponseContentBufferSize = 256000;
						var uri = new Uri(baseurl);
						var response = await Client.GetAsync(uri);
						if (response.IsSuccessStatusCode)
						{
							var content = await response.Content.ReadAsStringAsync();
							var items = JsonConvert.DeserializeObject<string[]>(content);
						//	Toast.MakeText(this, "Exito", ToastLength.Long).Show();
							return items;

						}
						else
						{
							Toast.MakeText(this, "Ocurrio un error :c", ToastLength.Long).Show();
							return null;
						}
				}

		public string[] getCategorias()
		{
			string[] classesAux = new string[ProductList.Count];
			int cont = 0;
			foreach (ProductModel a in ProductList)
			{
					classesAux[cont] = a.Categoria;
					cont++;
				
			}

			return classesAux;
		}
		public List<ProductModel> getProductosPorCategoria(string tipo)
			{
			List<ProductModel> aux = new List<ProductModel>();
				
				
				foreach (ProductModel a in ProductList)
				{
						if (tipo == a.Categoria)
						{
								aux.Add(a);
						}
				}
				return aux;
			}


				void OnItemClick(object sender, int e)
				{

					ProductModel producto = a.getItem(e);
					var intento = new Intent(this, typeof(detalleProducto));
					Bundle contenedor = new Bundle();
					contenedor.PutString("id", producto.Codigo);
					contenedor.PutString("nombre", producto.Nombre);
					contenedor.PutString("categoria", producto.Categoria);
					contenedor.PutString("precio", producto.Precio.ToString());
					contenedor.PutString("descripcion", producto.Descripcion);
					contenedor.PutString("url", producto.url);
					intento.PutExtra("bundle", contenedor);
					StartActivity(intento);		
				 }


		private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			string opcion = (string)spinner.GetItemAtPosition(e.Position);
			List<ProductModel> aux = getProductosPorCategoria(opcion);

					
					var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Obteniendo PRODUCTOS", true);
					a = new ProductosAdapter(aux, this);
					a.ItemClick += OnItemClick;
					mRecyclerView.SetAdapter(a);
					progressDialog.Dismiss();
		}
	}
}