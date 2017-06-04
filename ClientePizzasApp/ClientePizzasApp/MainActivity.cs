using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using System;
using Android.OS;
namespace ClientePizzasApp
{
//Icon = "@mipmap/icon"
	[Activity(Label = "PushStart", MainLauncher = true, Icon = "@mipmap/ic_add_shopping_cart_black_24dp")]
	public class MainActivity : Activity
	{
		EditText txtPassword, txtCorreo;
		Button btnEntrar, btnRegistar;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			btnEntrar = FindViewById<Button>(Resource.Id.myButton);
			txtCorreo = FindViewById<EditText>(Resource.Id.textView1);
			txtPassword = FindViewById<EditText>(Resource.Id.txtContraseña);
			btnRegistar = FindViewById<Button>(Resource.Id.btnRegistrar);
			btnRegistar.Click += registrar;
			btnEntrar.Click += login;
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Bienvenido";

		}

		void login(object sender, EventArgs e)
		{
			
            StartActivity(typeof(MenuActivity));
		}

		void registrar(object sender, EventArgs e)
		{
			
				var uri = Android.Net.Uri.Parse("http://pushstart.azurewebsites.net/Account/Register");
				Intent intent = new Intent(Intent.ActionView, uri);
				StartActivity(intent);
		}
	}
}

