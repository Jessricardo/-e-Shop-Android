
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

namespace ClientePizzasApp
{
	[Activity(Label = "detalleYaPa")]
	public class detalleYaPa : Activity
	{
		TextView txtcuenta, txtcantidad;
		string cantidad, cuenta;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.detalleYaPago);
			// Create your application here
			//toolbar
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			ActionBar.Title = "Información de pago";
			txtcuenta = FindViewById<TextView>(Resource.Id.textView2Cuenta);
			txtcantidad = FindViewById<TextView>(Resource.Id.textView3Cantidad);
			Bundle paquete = Intent.GetBundleExtra("bundle");
			cuenta = paquete.GetString("cuenta");
			cantidad = paquete.GetString("dinero");
			txtcuenta.Text = cuenta;
			txtcantidad.Text = cantidad;
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
	}
}
