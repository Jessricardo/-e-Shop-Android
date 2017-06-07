using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using System;
using Newtonsoft.Json;
using Android.OS;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Xamarin.Android.Net;
using System.Collections.Generic;
using Android.Preferences;
using Square.Picasso;

namespace ClientePizzasApp
{
	//Icon = "@mipmap/icon"
	[Activity(Label = "PushStart", MainLauncher = true, Icon = "@mipmap/ic_add_shopping_cart_black_24dp")]
	public class MainActivity : Activity
	{
		IPedidoRepository db;
		EditText txtPassword, txtCorreo;
		Button btnEntrar, btnRegistar, btnInvitado;
		ImageView imgLogo;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			btnEntrar = FindViewById<Button>(Resource.Id.myButton);
			txtCorreo = FindViewById<EditText>(Resource.Id.txtUsuario);
			txtPassword = FindViewById<EditText>(Resource.Id.txtContraseña);
			btnRegistar = FindViewById<Button>(Resource.Id.btnRegistrar);
			btnInvitado = FindViewById<Button>(Resource.Id.btnInvitado);
			imgLogo = FindViewById<ImageView>(Resource.Id.imgMain);
			btnRegistar.Click += registrar;
			btnInvitado.Click += invitado;
			btnEntrar.Click += login;
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Acceso";
			Picasso.With(this).Load("http://itcs98g1.blob.core.windows.net/imagenes/PushStartLogo.png").Resize(400, 300).Into(imgLogo);
			btnInvitado.Visibility = ViewStates.Invisible;
		}

		async void invitado(object sender, EventArgs e)
		{
			
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
			ISharedPreferencesEditor editor = prefs.Edit();

			editor.PutBoolean("is_login", false);
			if (prefs.GetString("NotToken", "") == "")
			{
				editor.PutString("NotToken", Guid.NewGuid().ToString());
			}

			editor.Apply();
			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			String myValue = preferences.GetString("NotToken", "");
			Toast.MakeText(this, "estas logueado con "+myValue, ToastLength.Long).Show();
			StartActivity(typeof(MenuActivity));
		}

		async void login(object sender, EventArgs e)
		{
			try
			{
//var progressDialog = ProgressDialog.Show(this, "Espere un momento", "Validando usuario", true);
				Usuario user1 = new Usuario();
				user1.contraseña = txtPassword.Text;
				user1.nombre = txtCorreo.Text;
				user1.APIKey = "";
				string baseurl = "http://pushstart.azurewebsites.net/api/login";
				var Client = new HttpClient();
				Client.MaxResponseContentBufferSize = 256000;
				var uri = new Uri(baseurl);
				var json = JsonConvert.SerializeObject(user1);
				StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = Client.PostAsync(uri, content).Result;

				if (response.IsSuccessStatusCode)
				{
					var content2 = await response.Content.ReadAsStringAsync();
					var items = JsonConvert.DeserializeObject<Usuario>(content2);

					ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
					ISharedPreferencesEditor editor = prefs.Edit();

					editor.PutBoolean("is_login", true);

					// Storing name in pref
					editor.PutString("name", txtCorreo.Text);
					editor.PutString("token", items.APIKey);

				//	Toast.MakeText(this, items.APIKey, ToastLength.Long).Show();


					editor.Apply();
					ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
					String myValue = preferences.GetString("token", "");
				//	Toast.MakeText(this, "estas logueado con" + myValue, ToastLength.Long).Show();
					Intent intento = new Intent(this, typeof(MenuActivity));
					//Bundle paquete = new Bundle();
					//paquete.PutString("tokenLogin", items.APIKey);

					StartActivity(intento);
				}
				else
				{
					//Toast.MakeText(this, "No lo se, me pareces fake", ToastLength.Short).Show();

				}
			//	progressDialog.Dismiss();
			}
			catch (Exception en)
			{
				Toast.MakeText(this, "Usuario y/o contraseña son erroneas ", ToastLength.Short).Show();
			}


		}
		public async System.Threading.Tasks.Task<Usuario> seLogueo()
		{
			Usuario user1 = new Usuario();
			user1.contraseña = txtPassword.Text;
			user1.nombre = txtCorreo.Text;
			user1.APIKey = "";
			string baseurl = "http://pushstart.azurewebsites.net/api/login";
			var Client = new HttpClient();
			Client.MaxResponseContentBufferSize = 256000;
			var uri = new Uri(baseurl);
			var json = JsonConvert.SerializeObject(user1);
			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = Client.PostAsync(uri, content).Result;

			if (response.IsSuccessStatusCode)
			{
				var content2 = await response.Content.ReadAsStringAsync();
				var items = JsonConvert.DeserializeObject<Usuario>(content2);

				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
				ISharedPreferencesEditor editor = prefs.Edit();

				editor.PutBoolean("is_login", true);

				// Storing name in pref
				editor.PutString("name", txtCorreo.Text);
				editor.PutString("token", items.APIKey);

				Toast.MakeText(this, items.APIKey, ToastLength.Long).Show();
				editor.Apply();
				ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
				String myValue = preferences.GetString("token", "");
				//Toast.MakeText(this, "estas logueado con"+myValue, ToastLength.Long).Show();
				return items;
			}
			else
			{
				return null;

			}

		}


		void registrar(object sender, EventArgs e)
		{

			var uri = Android.Net.Uri.Parse("http://pushstart.azurewebsites.net/Account/Register");
			Intent intent = new Intent(Intent.ActionView, uri);
			StartActivity(intent);
		}
	}
}

