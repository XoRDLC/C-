using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class Class1
    {
        public void Main()
        {
            Test_CheckedUnchecked();
        }

        public static void Test_CheckedUnchecked()
        {
            int i = 1_000_000;
            byte b = unchecked((byte)i);
            Console.WriteLine(b);
            try
            {
                byte c = checked((byte)i);
                Console.WriteLine(c);
            }
            catch { Console.WriteLine("checked Conversion overflow"); }

            checked {
                double d = i;
                byte t;
                byte j;
                unchecked
                {
                    t = (byte)(i - 1);
                    j = (byte)(i - 2);
                }
                Console.WriteLine(t);
                Console.WriteLine(j);
                Console.WriteLine(d);
            }

            Console.ReadLine();

        }
    }


}
