using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class C11_Conversions
    {
        public void Main()
        {
            Test_CheckedUnchecked();
            Test_ValidReferenceConversion();
            Test_BoxingAndUserDefConversion();
        }
        #region checked / unchecked conversion examples, Test_CheckedUnchecked()
        private static void Test_CheckedUnchecked()
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
                Console.WriteLine("i " + i);
                Console.WriteLine("i -> d " + (double)i);
                Console.WriteLine("fMax " + float.MaxValue);
                Console.WriteLine("fMin " + float.MinValue);
                unchecked
                {
                    Console.WriteLine("i-1->b " + (byte)(i - 1));
                    Console.WriteLine("i-2->b " + (byte)(i - 2));
                    Console.WriteLine("fMax->i " + (int)float.MaxValue);
                    Console.WriteLine("fMin->i " + (int)float.MinValue);
                    Console.WriteLine("dMax->i " + (int)double.MaxValue);
                    Console.WriteLine("dMin->i " + (int)double.MinValue);
                    Console.WriteLine("dMax->f " + (float)double.MaxValue);
                    Console.WriteLine("dMin->f " + (float)double.MinValue);
                }
                Console.WriteLine("fInf+ == dInf+ " + (float.NegativeInfinity == double.NegativeInfinity));
                Console.WriteLine("fInf- == dInf- " + (float.PositiveInfinity == double.PositiveInfinity));
                Console.WriteLine("1/dMax -> dc " + (decimal)(1d / double.MaxValue));
                Console.WriteLine("1/dMax -> f " + (float)(1d / double.MaxValue));
                Console.WriteLine("1/fMax -> d " + (double)(1f / float.MaxValue));
                Console.WriteLine("-1/dMax -> dc " + (decimal)(-1d / double.MaxValue));
                Console.WriteLine("-1/dMax -> f " + (float)(-1d / double.MaxValue));
                Console.WriteLine("-1/fMax -> d " + (double)(-1f / float.MaxValue));
                Console.WriteLine("dcMax -> f " + (float)decimal.MaxValue);
                Console.WriteLine("dcMin -> f " + (float)decimal.MinValue);
                Console.WriteLine("dcMax -> d " + (double)decimal.MaxValue);
                Console.WriteLine("dcMin -> f " + (double)decimal.MinValue);
                Console.WriteLine("dcMax -> sing " + (Single)decimal.MaxValue);
                Console.WriteLine("dcMin -> sing " + (Single)decimal.MinValue);
            }

            Console.ReadLine();

        }
        #endregion

        #region Valid Explicit reference conversions, Test_ValidReferenceConversion()
        private static void Test_ValidReferenceConversion()
        {
            B b1 = new B();
            A a1 = b1; //impicit conversion (from base class)
            A a2 = (A)b1; //explicit, no need, bcs ^

            Console.WriteLine(a1.Field1);
            Console.WriteLine(a2.Field1);
            Console.WriteLine(b1.Field1);

            B b2 = new B();
            A a3 = b2; //impicit conversion (from base class)
            B b3 = (B)a3; //explicit conv, B->A->B, no InvalidCastException in A->B

            Console.WriteLine(b2.Field1);
            Console.WriteLine(a3.Field1);
            Console.WriteLine(b3.Field1);

            A a4 = null;
            B b4 = (B)a4;  //explicit conv, no InvalidCastException in A->B, bcs null

            Console.WriteLine(a4?.Field1);
            Console.WriteLine(b4?.Field1);
            Console.ReadLine();
        }

        private class A { public int Field1 = 1; }
        private class B: A { public int Field2 = 2; new public int Field1 = 2; }
        #endregion

        #region Boxing, User def. (ex)implicit conversions, Test_BoxingAndUserDefConversion() 
        private static void Test_BoxingAndUserDefConversion() {
            int i = 4;
            object obj = null;
            obj = i;
            i = 5;

            Console.WriteLine(i);
            Console.WriteLine(obj);

            int j = (int)obj;
            obj = 6;

            Console.WriteLine(j);
            Console.WriteLine(obj);

            ValueType value = new ValueType() { Count = 2, Name = "Test" };
            object v = value;
            IBox box = value;
            int istr = value;
            value.Name = "Jhon";
            value.PrintOut();
            ((ValueType)v).PrintOut();
            ((ValueType)v).PrintOut();
            ((ValueType)box).PrintOut();
            Console.WriteLine("value class " + (value).ToString());
            Console.WriteLine("implicit conv to int - 1+value: " + 1 + value);
            Console.WriteLine("boxed to object " + v);
            Console.WriteLine("boxed to interface " + box);
            Console.WriteLine("explicit conv to ValueType: " + (ValueType)4);
            ((ValueType)10).PrintOut();
            Animal animal = new Animal("Cat", 4);
            Cat cat = new Cat("Vsevolod", 4);
            Console.WriteLine($"cow has {0+animal} legs");            

            if (cat is Animal)
                Console.WriteLine($"cat has {(int)cat} paws");
            Animal a = cat as Animal;
            if (a != null)
                Console.WriteLine($"animal has {(int)a} legs");
            Console.ReadLine();
        }

        private interface IBox {
            void PrintOut();
        }

        private struct ValueType : IBox
        {
            public int Count;
            public string Name;

            public static implicit operator int(ValueType value) {
                return value.Count;
            }

            public static explicit operator ValueType(int value)
            {
                return new ValueType() { Count = value , Name = String.Empty};
            }

            public void PrintOut() => Console.WriteLine($"{this.Count} {this.Name}");
        }

        private class Animal {
            public string Name { get; set; }
            public int Paws { get; private set; }
            public static implicit operator int(Animal animal) => animal.Paws;
            public Animal(string Name, int Paws) {
                this.Name = Name;
                this.Paws = Paws;
            }
        }

        private class Cat : Animal
        {
            public Cat(string Name, int Paws) : base(Name, Paws) { }
        }
        #endregion
    }
}
