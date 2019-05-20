using System;
using System.Diagnostics;

namespace Learning
{
    class Statics
    {
        public static void Log(object msg)
        {
            if (msg == null)
            {
                Console.WriteLine("null");   
                Debugger.Log(0, "", "null" + "\n");
            }
            else {
                Console.WriteLine(msg);
                Debugger.Log(0, "", msg.ToString() + "\n");
            }
                
        }
    }
}
