using chat_server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Entities
{
    public class EntityManager<T>
    {
        public static event Action<T> onEntityCreate = delegate { };

        private IStore _storage;

        // Contains entity for current entity manager
        public List<T> Entities;

        public EntityManager(IStore storage) {
            Entities = new List<T>();

            _storage = storage;
        }

        public T Create(T entityToSave)
        {
            Entities.Add(entityToSave);

            onEntityCreate.Invoke(entityToSave);

            return _storage.Save<T>(entityToSave, "ID");
        }

        public T Get(Dictionary<string, string> entityParams)
        {
            //T entityToGet = default(T);

            //var type = typeof(T);
            //var typeName = typeof(T).Name;
            //var typeProperties = type.GetProperties();
            //var paramsName = entityParams["name"];
            //var paramsValue = entityParams["value"];

            //entityToGet = Entities.Find( (item) => 
            //{
            //    var itemCondition = false;

            //    // Iterate item properties to find the matching one
            //    foreach (var property in typeProperties)
            //    {
            //        // FIX
            //        if (property.Name == paramsName && property.GetValue(item).ToString() == paramsValue)
            //        {
            //            itemCondition = true;
            //            break;
            //        }
            //    }

            //    return itemCondition;
            //});

            //return (T)Convert.ChangeType(entityToGet, typeof(T));

            var elements = _storage.Load<T>(entityParams["key"], entityParams["value"]);

            if (elements.Count != 0)
            {
                return elements[0];
            } else
            {
                return default(T);
            }
        }

        // TODO - should cache
        public List<T> All()
        {
            return _storage.Load<T>();      
        }

        public T Update<T>(T entityToUpdate, Dictionary<string, string> entityParams)
        {
            return _storage.Save<T>(entityToUpdate, "ID", update: true);
        }

        public T Delete<T>(T entityToDelete, Dictionary<string, string> entityParams)
        {
            // Todo - cache
            return _storage.Destroy<T>(entityToDelete, "ID");
        }

        // Helpers methods
        public bool Exists(string key, string value)
        {
            var entity = _storage.Load<T>(key, value);

            // Storage returns list
            if (entity.Count != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
