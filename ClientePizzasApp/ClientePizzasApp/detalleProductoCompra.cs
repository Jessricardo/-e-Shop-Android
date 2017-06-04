
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Square.Picasso;

namespace ClientePizzasApp
{
	[Activity(Label = "detalleProductoCompra")]
	public class detalleProductoCompra : Activity
	{
IPedidoRepository db;
		TextView txtNombre, txtDescripcion, txtCategoria, txtPrecio;
		Button btnQuitar;
		string idProducto;
		string idPartida, url;
		int cantidad;
		ImageView img;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.detalleProductoCompra);
			// Create your application here
			txtNombre = FindViewById<TextView>(Resource.Id.txtNombreP);
			txtDescripcion = FindViewById<TextView>(Resource.Id.txtDescripcionP);
			txtCategoria = FindViewById<TextView>(Resource.Id.txtCategoriaP);
			txtPrecio = FindViewById<TextView>(Resource.Id.txtPrecioP);
			btnQuitar = FindViewById<Button>(Resource.Id.btnQuitar);
			img = FindViewById<ImageView>(Resource.Id.imageView1P);
			btnQuitar.Click += quitar;
			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Productos";
			Bundle paquete = Intent.GetBundleExtra("bundle");
			txtNombre.Text = paquete.GetString("nombre");
			idProducto= paquete.GetString("id");
			txtPrecio.Text = "$"+paquete.GetString("precio");
			txtCategoria.Text = paquete.GetString("categoria");
			txtDescripcion.Text = paquete.GetString("descripcion");
			idPartida = paquete.GetString("idPartida");
				url = paquete.GetString("url");
			cantidad = paquete.GetInt("cantidad");
			Picasso.With(this).Load(url).Resize(550,700).Into(img);
		}

		void quitar(object sender, EventArgs e)
		{
			
			string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dbTienda.db3");
			db = new SQLitePedidoRepository(dbPath);
			List<user> usuarios = db.Read();
			PartidasModel partida = new PartidasModel();
			partida.cantidad = cantidad;
			partida.productoId = idProducto;
			partida.idCarrito = usuarios[0].tokenNoUsuario.ToString();
			partida.id = idPartida;
			string baseurl = "http://pushstart.azurewebsites.net/Carrito/quitar";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uri = new Uri(baseurl);
			var json = JsonConvert.SerializeObject(partida);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = Client.PutAsync(uri, content).Result;  

			if (response.IsSuccessStatusCode)
			{
				Toast.MakeText(this, "Exito", ToastLength.Long).Show();
				Intent intento = new Intent(this, typeof(carrito));
				StartActivity(intento);
			}
			else
			{
				Toast.MakeText(this, "Ocurrio un error :c", ToastLength.Long).Show();

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
				StartActivity(Intent);
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
	}
}
