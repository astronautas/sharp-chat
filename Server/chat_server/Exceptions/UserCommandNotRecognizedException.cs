using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Exceptions
{
    class UserCommandNotRecognizedException : Exception
    {
        public UserCommandNotRecognizedException()
        {

        }

        public UserCommandNotRecognizedException(string message) : base(message)
        {

        }

        public UserCommandNotRecognizedException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
