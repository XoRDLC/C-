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
            //Test_Generics();
            Test_GenericMethod();
        }
        #region Use generics in 'stack', Test_Generics() 
        public static void Test_Generics() {
            Stack<string> stack = new Stack<string>(5);

            stack.Push("Jhon");
            stack.Push("Whick");
            stack.Push("Poppy");
            stack.Push("Smith");
            stack.Print();

            stack.Pop();
            stack.Pop();
            stack.Print();

            Console.ReadLine();
        }

        private class Stack<T>
        {
            private T[] StackArray;
            private int Index = 0;
            public Stack(int MaxIndex) {
                StackArray = new T[MaxIndex];
            }
            public void Push(T elem) {
                if (!IsFull()) StackArray[Index++] = elem;
            }

            public T Pop() {
                return (!IsEmpty()) ? StackArray[--Index] : StackArray[0];
            }

            public void Print()
            {
                for (int i = Index - 1; i >= 0; i--)
                    Console.WriteLine($"{i}\t{StackArray[i]}");
            }

            bool IsFull() => Index >= StackArray.Length;
            bool IsEmpty() => Index <=0;     
        }
        #endregion

        public static void Test_GenericMethod() {
            var s1 = new int[]  { 1, 3, 6, 7 };
            var s2 = new string[] { "one", "two", "three", "four", "five" };

            Simple.ReverseAndPrint(s1);
            Simple.ReverseAndPrint<int>(s1);
            Simple.ReverseAndPrint(s2);
            Simple.ReverseAndPrint<string>(s2);
            Simple.ReverseAndPrint(s2);

            Console.ReadLine();
        }

        private class Simple{
            public static void ReverseAndPrint<T>(T[] array) {
                Array.Reverse(array);
                foreach (T t in array)
                    Console.Write($"{t} ");
                Console.WriteLine();
            }
        }
    }
}
