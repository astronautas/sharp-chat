using chat_server.Interfaces;
using System;
using chat_server.Entities;
using System.Collections.Generic;
using chat_server.Exceptions;

namespace chat_server
{
    class Router : IRouteUsers
    {
        private Dictionary<string, Action<User, string>> _router = new Dictionary<string, Action<User, string>>() { };

        public void AddRoute(string route, Action<User, string> callback)
        {
            _router.Add(route, callback);
        }

        public void Route(string route, User user, string parameters)
        {
            if (_router.ContainsKey(route))
            {
                _router[route](user, parameters);
            } else
            {
                throw new UserCommandNotRecognizedException($"User {user.Name} sent command not recognized.{Environment.NewLine}{parameters}");
            }
        }
    }
}
