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
			db.Delete<user>(c.tokenUsuario);
		}

		public List<user> Read()
		{
			var table = db.Table<user>();
			int cant=table.Count();
			if (cant > 0)
			{
				return table.Select(c => c).ToList();
			}
			else
			{
				return null;
			}
		}

		public user readById(string id)
		{
			return db.Table<user>().FirstOrDefault(t => t.tokenUsuario == id);
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

		public user readById(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
