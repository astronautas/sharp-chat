using chat_server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Entities
{
    public class EntityManagerFactory
    {
        private static readonly EntityManagerFactory _instance = new EntityManagerFactory();

        public static EntityManagerFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        // Define storage method
        private IStore _storage = new BaseStorage();

        public EntityManager<T> GetManager<T>()
        {
            return new EntityManager<T>(storage: _storage);
        }
    }
}
