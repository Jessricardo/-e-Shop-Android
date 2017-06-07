
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
using Android.Preferences;

namespace ClientePizzasApp
{
	[Activity(Label = "detalleProducto")]
	public class detalleProducto : Activity
	{IPedidoRepository db;
user usuario;

		TextView txtNombre, txtCategoria, txtPrecio, txtDescripcion, txtcantidad;
		Button btnCarrito, btnMas, btnMenos;
		string id, url, nombre;
		double costo;
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
			nombre = paquete.GetString("nombre");
			txtNombre.Text = nombre;
			id= paquete.GetString("id");
			costo= Convert.ToDouble(paquete.GetString("precio"));
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

		//	Toast.MakeText(this, usuarios[0].tokenUsuario,ToastLength.Long).Show();
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
			

			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);


			string myValue = "";
	
					PartidasModel partida = new PartidasModel();
					partida.cantidad = Convert.ToInt32(txtcantidad.Text);
					partida.productoId = id.ToString();
			if (preferences.GetBoolean("is_login", false))
			{
				myValue = preferences.GetString("token", "");
			}
			else
			{ 
				myValue = preferences.GetString("NotToken", "");
			}
					partida.idCarrito = myValue;
					partida.id = Guid.NewGuid().ToString();
					partida.nombre = nombre;
					//partida.costo = 12;
					partida.pedidoId = "hola";
					partida.costo = costo * Convert.ToInt32(txtcantidad.Text);
					string baseurl = "http://pushstart.azurewebsites.net/Carrito/agregar";
					var Client = new HttpClient();
					Client.MaxResponseContentBufferSize = 256000;
					var uri = new Uri(baseurl);
					var json = JsonConvert.SerializeObject(partida);
					StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
					var response = Client.PostAsync(uri, content).Result;

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

		//public void mostrarCarrito(object sender, EventArgs e)
		//{
		//	//MOSTRAR LO QUE SE TIENE EN EL CARRITO Y PODER PAGAR Y PERMITIR QUITAR PRODUCTOS O CANTIDAD
		//	Intent i = new Intent(this, typeof(carrito));
		//	StartActivity(i);
		//}




	}
}
