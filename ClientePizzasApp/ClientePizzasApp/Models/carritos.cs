using System;
using System.Collections.Generic;

namespace ClientePizzasApp
{
	public class carritos
	{
		public Guid Id { get; set;}
		public string fecha { get; set;}
		public string Estado { get; set;}
		public string idUsuario { get; set;}
		//los productos
		public List<PartidasModel> partidas { get;  set; } = new List<PartidasModel>();
	}
	public class PartidasModel
	{
		public string id { get; set; }
		public string idCarrito { get; set;}
		public string productoId { get; set;}
		public int cantidad { get; set; }
		public double costo { get; set; }
		public string nombre { get; set; }
		public string pedidoId { get; set; }

	}

public class PedidoModel
{
	public string id { get; set; }
	public string idUsuario { get; set; }
	public string estado { get; set; }
	public double total { get; set; }
	public string cuenta { get; set; } 
	}

public class PedidoPost
{
	public string idUsuario { get; set; }
	public double total { get; set; }
	public string APIKey { get; set; } }
}
