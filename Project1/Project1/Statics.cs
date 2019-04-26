using System.Diagnostics;

namespace Project1
{
    class Statics
    {
        public static void Log(string msg)
        {
            Debugger.Log(0, "", msg + "\n");
        }
    }
}
