using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using System.Linq;


//sqllite-pcl nose que 1.2.1
namespace ClientePizzasApp
{
	public class SQLitePedidoRepository : IPedidoRepository
	{
		private string PATH;
		private SQLiteConnection db;
		public SQLitePedidoRepository(string path)
		{
            PATH = path;
            db = new SQLiteConnection(PATH);
			db.CreateTable<user>();
        }
		public void Crear(user c)
		{
            db.Insert(c);
		}

		public void Delete(user c)
		{
			db.Delete<user>(c.tokenNoUsuario);
		}

		public List<user> Read()
		{
			var table = db.Table<user>();
			return table.Select(c => c).ToList();
		}

		public user readById(Guid id)
		{
			return db.Table<user>().FirstOrDefault(t => t.tokenNoUsuario == id);
		}

		public void Update(user c) 
		{
			db.Update(c);
		}



		public object LeerPedidosDeUsuario(string usuario)
		{
			throw new NotImplementedException();
		}

		public object LeerPedidoPorId(int id)
		{
			throw new NotImplementedException();
		}
	}
}
