using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class C12_Generics
    {
        public void Main() {
            Test_Generics();
            Test_GenericMethod();
            Test_Delegates();
            Test_NonGeneric();
            Test_Variance();

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

        #region Generic Methods, Test_GenericMethod()
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
        #endregion

        #region Use delegates, Test_Delegates()
        private delegate void PriIn<T>(T value);
        private delegate R PriOut<T, R>(T value);

        private class A {
            public static void PrintNormal<T>(T value) {
                Console.WriteLine(value.ToString());
            }
        }
        private class B {
            public static void PrintUpperCase<T>(T value) {
                Console.WriteLine(value.ToString().ToUpper());
            }
        }
        private class C {
            public static R ReturnNormal<T, R>(T value) {
                return default(R);
            }
        }
        private class D
        {
            public static R ReturnWithPrint<T, R>(T value)
            {
                switch (typeof(R))
                {
                    case null:
                        Console.WriteLine($"Null type");
                        break;
                    default:
                        Console.WriteLine($"typeof R: {typeof(R)}");
                        break;
                }
                return default(R);
            }
        }

        private static void Test_Delegates() {
            var pri  = new PriIn<string>(A.PrintNormal);
            pri += B.PrintUpperCase;

            pri.Invoke("Test");

            var priout = new PriOut<string, string>(C.ReturnNormal<string, string>);
            priout += D.ReturnWithPrint<string, string>;

            priout.Invoke("Test");

            Console.ReadLine();
        }
        #endregion

        #region Use generic inteface with non generic class, Test_NonGeneric()
        private interface IA<T> {
            void Print(T value);
        }

        private class NonGeneric : IA<string>, IA<int>{
            
            public void Print(string value){
                Console.WriteLine(value);
            }
            public void Print(int value)
            {
                Console.WriteLine(value.ToString());
            }
        }

        public static void Test_NonGeneric() {
            var ng = new NonGeneric();
            ng.Print("sdsd");
            ng.Print(13);

            Console.ReadLine();
        }
        #endregion

        #region Covariance, Contravariance, Test_Variance()
        private class Animal { public int Paws = 4; }
        private class Cat : Animal { }
        private class Programm {
            public static Cat GetCat() => new Cat();
            public static void Print(Animal animal) {
                Console.WriteLine(animal.Paws);
            }
        }

        delegate T FactoryR<out T>();       //T covariat
        delegate void FactoryV<in T>(T t);  //T contravariant
        delegate void FactoryI<T>(T t);     //T invariat

        public static void Test_Variance() {
            FactoryR<Cat> cat = Programm.GetCat;
            FactoryR<Animal> animal = cat;

            Console.WriteLine(animal().Paws.ToString());
            Console.WriteLine(cat().Paws.ToString());

            FactoryV<Animal> animal1 = Programm.Print;
            FactoryV<Cat> cat1 = animal1;

            animal1(new Animal());
            cat1(new Cat());

            Console.ReadLine();
        }
        #endregion
    }
}
