using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace ClientePizzasApp
{


	public class ProductosAdapterPartida : RecyclerView.Adapter
	{
		List<PartidasModel> partidaList = new List<PartidasModel>();
		List<ProductModel> ProductoList = new List<ProductModel>();
		Context hola;
		public event EventHandler<int> ItemClick;

		public ProductosAdapterPartida(List<ProductModel> ProductoList2, Context hola, 	List<PartidasModel> partidas)
		{
			this.ProductoList = ProductoList2;
			this.hola = hola;
			this.partidaList = partidas;
		}
		public override int ItemCount
		{
			get
			{
				return ProductoList.Count;

			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			ViewHolder cv = holder as ViewHolder;
			//cv.Caption.Text = ProductoList[position].Nombre;
			String id = ProductoList[position].Codigo.ToString();
			cv.textViewNombre.Text = ProductoList[position].Nombre;
			cv.textViewCodigo.Text = ProductoList[position].Codigo.ToString();

			cv.precio.Text = "$" + ProductoList[position].Precio.ToString() + " M.N.";
			cv.cantidad.Text = partidaList[position].cantidad.ToString()+" en carrito";
			cv.idPartida.Text = partidaList[position].id.ToString();
			Picasso.With(hola).Load(ProductoList[position].url).Resize(400,550).Into(cv.imagen);

		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_view_productoPartida, parent, false);
			ViewHolder cv = new ViewHolder(itemView, OnClick);
			return cv;
		}

		public class ViewHolder : RecyclerView.ViewHolder
		{
			//public TextView Caption { get; private set; }
			public TextView textViewNombre { get; private set; }
			//public ImageView imagen { get; private set;}
			public TextView textViewCodigo { get; private set; }
			public TextView precio { get; private set; }
			public ImageView imagen { get; private set; }
			public TextView cantidad { get; private set;}
			public TextView idPartida { get; private set;}
			public ViewHolder(View itemView, Action<int> listener) : base(itemView)
			{
				//Caption = itemView.FindViewById<TextView>(Resource.Id.textView1);
				textViewNombre = itemView.FindViewById<TextView>(Resource.Id.textView12);
				textViewCodigo = itemView.FindViewById<TextView>(Resource.Id.textView22);
				precio = itemView.FindViewById<TextView>(Resource.Id.textView32);
				imagen = itemView.FindViewById<ImageView>(Resource.Id.imageView12);
				//imagen = itemView.FindViewById<ImageView>(Resource.Id.imageView1);
				cantidad=itemView.FindViewById<TextView>(Resource.Id.textView323);
				idPartida=itemView.FindViewById<TextView>(Resource.Id.textView324);
				itemView.Click += (sender, e) => listener(base.Position);

			}
		}

		void OnClick(int position)
		{
			if (ItemClick != null)
				ItemClick(this, position);
		}

		public ProductModel getItem(int position)
		{
			return ProductoList[position];
		}

	}
}