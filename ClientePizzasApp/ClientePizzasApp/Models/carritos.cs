using System;
using System.Collections.Generic;

namespace ClientePizzasApp
{
	public class carritos
	{
		public Guid Id { get; set;}
		public string fecha { get; set;}
		public string Estado { get; set;}
		//los productos
		public List<PartidasModel> partidas { get;  set; } = new List<PartidasModel>();
	}
	public class PartidasModel
	{
		public string id { get; set; }
		public string idCarrito { get; set;}
		public string productoId { get; set;}
		public int cantidad { get; set; }


	}
}
