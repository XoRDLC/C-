using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class2
    {
        public static void Main() {
            c01_Basics c01 = new c01_Basics("Middle");
            c01[0] = 20;
            c01[1] = 175.2;
            c01[2] = 63.3;

            Console.WriteLine($"age: {c01[0]}\t height: {c01[1]}\t weight: {c01[2]}");
            Console.ReadLine()
        }
    }
}
