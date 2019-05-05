using System.Diagnostics;

namespace Project1
{
    class Statics
    {
        public static void Log(object msg)
        {
            if (msg == null)
                Debugger.Log(0, "", "null" + "\n");
            else
                Debugger.Log(0, "", msg.ToString() + "\n");
        }
    }
}
