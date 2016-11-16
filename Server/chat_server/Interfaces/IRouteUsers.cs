using chat_server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Interfaces
{
    interface IRouteUsers
    {
        void Route(string route, User user, string parameters);
        void AddRoute(string route, Action<User, string> callback);
    }
}
