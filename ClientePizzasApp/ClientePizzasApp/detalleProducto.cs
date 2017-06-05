
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Java.Lang;
using Newtonsoft.Json;
using Square.Picasso;
using Java.IO;

namespace ClientePizzasApp
{
	[Activity(Label = "detalleProducto")]
	public class detalleProducto : Activity
	{IPedidoRepository db;
user usuario;

		TextView txtNombre, txtCategoria, txtPrecio, txtDescripcion, txtcantidad;
		Button btnCarrito, btnMas, btnMenos;
		string id, url;
		int cantidad = 1;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.detalleProducto);

			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Productos";


			btnCarrito = FindViewById<Button>(Resource.Id.btnCarrito);
			btnMas = FindViewById<Button>(Resource.Id.btnMas);
			btnMenos = FindViewById<Button>(Resource.Id.btnMenos);
			txtNombre = FindViewById<TextView>(Resource.Id.txtNombre);
			txtPrecio = FindViewById<TextView>(Resource.Id.txtPrecio);
			txtCategoria = FindViewById<TextView>(Resource.Id.txtCategoria);
			txtDescripcion = FindViewById<TextView>(Resource.Id.txtDescripcion);
			txtcantidad = FindViewById<TextView>(Resource.Id.txtCantidad);


			//recibiendo valores de la vista menu_activity
			Bundle paquete = Intent.GetBundleExtra("bundle");
			txtNombre.Text = paquete.GetString("nombre");
			id= paquete.GetString("id");
			txtPrecio.Text = "$"+paquete.GetString("precio");
			txtCategoria.Text = paquete.GetString("categoria");
			txtDescripcion.Text = paquete.GetString("descripcion");
			txtcantidad.Text = cantidad.ToString();
			btnCarrito.Click += agregarPartida;
			url = paquete.GetString("url");
			var imageView = FindViewById<ImageView>(Resource.Id.imageView1);
			Picasso.With(this).Load(url).Resize(450,600).Into(imageView);
			btnMas.Click += mas;
			btnMenos.Click += menos;
			var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Cargando Imagen", true);

			progressDialog.Dismiss();
			//android:src="@android:drawable/ic_menu_gallery"

		}

		public void mas(object sender, EventArgs e)
		{
			cantidad++;
			txtcantidad.Text = cantidad.ToString();

		}
		public void menos(object sender, EventArgs e)
		{
			if (cantidad > 1)
			{
				cantidad--;
				txtcantidad.Text = cantidad.ToString();
			}
			else
			{
				Toast.MakeText(this, "Cantidad es cero", ToastLength.Long).Show();
			}

		}
		public async void agregarPartida(object sender, EventArgs e)
		{
			try
			{
			string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dbTienda.db3");
			db = new SQLitePedidoRepository(dbPath);
			List<user> usuarios = db.Read();

				if (usuarios==null)
				{
					usuario = new user();
					usuario.bandera = true;
					usuario.tokenNoUsuario = Guid.NewGuid();
					db.Crear(usuario);

					//Toast.MakeText(this, "guid hecho " + usuario.tokenNoUsuario.ToString(), ToastLength.Short).Show();
				}
				else
				{
					//Toast.MakeText(this, "ya tienes guid ", ToastLength.Short).Show();
				}
			List<user> usuarios2 = db.Read();
			PartidasModel partida = new PartidasModel();
			partida.cantidad = Convert.ToInt32(txtcantidad.Text);
			partida.productoId = id.ToString();
			partida.idCarrito = usuarios2[0].tokenNoUsuario.ToString();
			partida.id = "";
			string baseurl = "http://pushstart.azurewebsites.net/Carrito/agregar";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uri = new Uri(baseurl);
			var json = JsonConvert.SerializeObject(partida);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = Client.PutAsync(uri, content).Result;  

			if (response.IsSuccessStatusCode)
			{
				Toast.MakeText(this, "Exito", ToastLength.Short).Show();
			}
			else
			{
				Toast.MakeText(this, "Ocurrio un error :c", ToastLength.Short).Show();

}
			Intent intento2 = new Intent(this, typeof(MenuActivity));
			StartActivity(intento2);
			}
			catch (System.Exception ex)
			{ Toast.MakeText(this, "funciono", ToastLength.Short).Show();}






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

		//public void mostrarCarrito(object sender, EventArgs e)
		//{
		//	//MOSTRAR LO QUE SE TIENE EN EL CARRITO Y PODER PAGAR Y PERMITIR QUITAR PRODUCTOS O CANTIDAD
		//	Intent i = new Intent(this, typeof(carrito));
		//	StartActivity(i);
		//}




	}
}
