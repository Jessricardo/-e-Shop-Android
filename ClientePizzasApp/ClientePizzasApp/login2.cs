
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

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
	[Activity(Label = "login2")]
	public class login2 : Activity
	{
EditText txtPassword, txtCorreo;
Button btnEntrar, btnRegistar, btnInvitado;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.login2);
			btnEntrar = FindViewById<Button>(Resource.Id.myButtonLogin2);
			txtCorreo = FindViewById<EditText>(Resource.Id.txtUsuarioLogin2);
			txtPassword = FindViewById<EditText>(Resource.Id.txtContraseñaLogin2);
			btnRegistar = FindViewById<Button>(Resource.Id.btnRegistrarLogin2);
			btnInvitado = FindViewById<Button>(Resource.Id.btnInvitadoLogin2);
			btnRegistar.Click += registrar;
			btnInvitado.Click += invitado;
			btnEntrar.Click += login;
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Bienvenido";
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
						Toast.MakeText(this, "estas logueado con " + myValue, ToastLength.Long).Show();
						StartActivity(typeof(MenuActivity));
				}

				async void login(object sender, EventArgs e)
				{
			try
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
					editor.PutString("token", items.APIKey);

					Toast.MakeText(this, items.APIKey, ToastLength.Long).Show();


					editor.Apply();
					ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
					String myValue = preferences.GetString("token", "");
					Toast.MakeText(this, "estas logueado con" + myValue, ToastLength.Long).Show();

					//METODO DE MANDAR PARTIDAS DE UN TOKEN A OTRO

					actualizarPartidas(preferences.GetString("token", ""), preferences.GetString("NotToken", ""));
					Intent intento = new Intent(this, typeof(MenuActivity));
					//Bundle paquete = new Bundle();
					//paquete.PutString("tokenLogin", items.APIKey);

					StartActivity(intento);
				}
				else
				{
					

				}
			}
			catch (Exception en)
			{Toast.MakeText(this, "No lo se, me pareces fake", ToastLength.Short).Show(); }

				}
		public void actualizarPartidas(string token, string NotToken)
		{
			
		
		}

				void registrar(object sender, EventArgs e)
				{

					var uri = Android.Net.Uri.Parse("http://pushstart.azurewebsites.net/Account/Register");
					Intent intent = new Intent(Intent.ActionView, uri);
					StartActivity(intent);
				}
	}
}
