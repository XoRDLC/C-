using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class Class1
    {
        public void Main() {
            Test_Generics();
        }

        public static void Test_Generics() { }

        private class Stack<T> {
            private int Index = 0;
            T[] Array;

            public void Push(T elem) { }

            public T Pop() {

                return default(T);
            }
        }

    }


}
