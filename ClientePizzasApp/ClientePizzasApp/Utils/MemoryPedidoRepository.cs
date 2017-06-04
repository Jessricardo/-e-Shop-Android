//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ClientePizzasApp
//{
//	public class MemoryPedidoRepository : IPedidoRepository
//	{
//		static List<carritos> pedido;
//		//static MemoryPedidoRepository()
//		//{
//		//	contacts = new List<Contact>();
//		//	contacts.Add(new Contact()
//		//	{
//		//		contactId = 1,
//		//		contactName = "Jesús Apodaca",
//		//		contactStreet = "Random St",
//		//		contactEmail = "some@email.com",
//		//		contactClass = "Contact",
//		//	});
//		//	contacts.Add(new Contact()
//		//	{
//		//		contactId = 2,
//		//		contactName = "Jesús Abraham",
//		//		contactStreet = "Other Random St",
//		//		contactEmail = "someother@email.com",
//		//		contactClass = "Contact",
//		//	});
//		//}
//		public void Crear(carritos c)
//		{
//		pedido.Add(c);
//		}

//		public void Delete(carritos c)
//		{
//			throw new NotImplementedException();
//		}

//		public object LeerPedidoPorId(int id)
//		{
//			throw new NotImplementedException();
//		}

//		public object LeerPedidosDeUsuario(string usuario)
//		{
//			throw new NotImplementedException();
//		}

//		public List<carritos> Read()
//		{
//			return pedido;
//		}

//		//public carritos readById(int id)
//		//{
//		//	throw new NotImplementedException();
//		//}

//		public carritos readById(int id)
//		{
//			return pedido.FirstOrDefault(c => c.Id == id);
//		}

//		public void Update(carritos c)
//		{
//			//PEDIDO . ID
//			//ACTUALIZAR DONDE IGUAL EN LA LISTA
//			throw new NotImplementedException();

//			//var a = readById(c.Id);


//		}
//	}
//}
