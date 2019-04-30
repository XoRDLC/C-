using System.Diagnostics;

namespace Project1
{
    class Statics
    {
        public static void Log(object msg)
        {
            Debugger.Log(0, "", msg.ToString() + "\n");
        }
    }
}
