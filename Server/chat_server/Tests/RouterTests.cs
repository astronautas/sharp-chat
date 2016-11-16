using chat_server.Entities;
using chat_server.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Tests
{
    class RouterTests : Test
    {
        public static void Init()
        {
            TestIncorrectCommandHandling();
        }

        // Should throw exception
        private static void TestIncorrectCommandHandling()
        {
            var exceptionThrowed = false;
            var router = new Router();

            try
            {
                router.Route("unexistingRoute", new User(), "");
            } catch (UserCommandNotRecognizedException e)
            {
                exceptionThrowed = true;
            }

            Assert(exceptionThrowed, "Incorrect command passed to the router should cause exception");
        }
    }
}
