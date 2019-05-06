using System;
using static Project1.Statics;

namespace Project1
{
    class _Main
    {
        public static void Main() {
            Log("\n--- start --- [" + DateTime.Now + "]");
            Class1 class1 = new Class1();
            class1.Main();
            Log("---  end  ---\n");
        }
    }
}
