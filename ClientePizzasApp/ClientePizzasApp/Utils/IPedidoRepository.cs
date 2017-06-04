using System;
using System.Collections.Generic;

namespace ClientePizzasApp
{
	public interface IPedidoRepository
	{

		object LeerPedidosDeUsuario(string usuario);
		object LeerPedidoPorId(int id);



		//FUNCIONES PARA CREAR PEDIDOS
		void Crear(user c);
		user readById(Guid id);
		List<user> Read();
		void Update(user c);
		void Delete(user c);

	}
}
