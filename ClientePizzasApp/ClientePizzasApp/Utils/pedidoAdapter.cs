using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ClientePizzasApp
{
public class pedidoAdapter : RecyclerView.Adapter
{

		List<PedidoModel> pedidoList = new List<PedidoModel>();
	
	public event EventHandler<int> ItemClick;

		public pedidoAdapter(List<PedidoModel> pedidoLista)
	{
           	this.pedidoList = pedidoLista;
		
	}
	public override int ItemCount
	{
		get
		{
				return pedidoList.Count;

		}
	}

	public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
	{
		ViewHolder cv = holder as ViewHolder;
			cv.textViewCodigo.Text = pedidoList[position].estado;
			cv.textViewNombre.Text = pedidoList[position].id.ToString();
			cv.precio.Text = "$" + pedidoList[position].total.ToString() + " M.N.";
		
	}

	public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
	{
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_view_pedidos, parent, false);
		ViewHolder cv = new ViewHolder(itemView, OnClick);
		return cv;
	}

	public class ViewHolder : RecyclerView.ViewHolder
	{

		public TextView textViewNombre { get; private set; }
	
		public TextView textViewCodigo { get; private set; }
		public TextView precio { get; private set; }
		
		public ViewHolder(View itemView, Action<int> listener) : base(itemView)
		{

			precio = itemView.FindViewById<TextView>(Resource.Id.textView1pedido);
			textViewCodigo = itemView.FindViewById<TextView>(Resource.Id.textView3pedido);
			textViewNombre = itemView.FindViewById<TextView>(Resource.Id.textView2pedido);
			
			itemView.Click += (sender, e) => listener(base.Position);

		}
	}

	void OnClick(int position)
	{
		if (ItemClick != null)
			ItemClick(this, position);
	}

		public PedidoModel getItem(int position)
		{ return pedidoList[position]; }
	}
}
