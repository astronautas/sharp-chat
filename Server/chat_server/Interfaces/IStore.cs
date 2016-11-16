using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Interfaces
{
    public interface IStore
    {
        // Save specific object to file, DB etc. by key
        T Save<T>(T objectToSave, string key, bool update = false);

        // Load objects from DB, file etc.
        List<T> Load<T>(string key = null, string value = null, bool all = false);

        // Destroys specified item by key and value
        T Destroy<T>(T objectToDestroy, string key);
    }
}
