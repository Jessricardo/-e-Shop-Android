
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ClientePizzasApp
{
	[Activity(Label = "perfil")]
	public class perfil : Activity
	{
		TextView id, nombre;
		Button btnIrPedidos;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.perfilLayout);
			// Create your application here
var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
ActionBar.Title = "Información de Usuario";
			id = FindViewById<TextView>(Resource.Id.txtIdPerfilUsuario);
			nombre = FindViewById<TextView>(Resource.Id.txtNombreUsuarioPerfil);
			btnIrPedidos = FindViewById<Button>(Resource.Id.btnPerfilPedidos);
			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			nombre.Text=preferences.GetString("name", "");
			id.Text=preferences.GetString("token", "");
			btnIrPedidos.Click += irPedidos;
		}

		void irPedidos(object sender, EventArgs e)
		{
		Intent intento = new Intent(this, typeof(pedidos));
		StartActivity(intento);
		}
public override bool OnCreateOptionsMenu(IMenu menu)
{
	MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);			}
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
}
}
