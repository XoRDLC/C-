using System;
using static Learning.Statics;

namespace Learning
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
