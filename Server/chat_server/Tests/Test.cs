using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Tests
{
    public class Test
    {
        public static bool errorsExist = false;

        protected static void Assert(bool condition, string testName)
        {
            if (condition)
            {
                Debug.Write("[TEST] [PASSED ]" + testName + Environment.NewLine);
            }
            else
            {
                Debug.Write("[TEST] [FAILED] " + testName + Environment.NewLine);
                errorsExist = true;
            }
        }
    }
}
