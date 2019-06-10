using System;
using System.Collections;

namespace Learning
{
    class Class1
    {
        public void Main() {
            Test_NonGenericIEnumerator();
            Test_GenericIEnumerator();
        }

        #region Implementing non generic IEnumerator & IEnumerable, Test_NonGenericIEnumerator()
        private static void Test_NonGenericIEnumerator() {
            CollectionEnum collectionEnum = new CollectionEnum();
            collectionEnum.AddElement("Kot");
            collectionEnum.AddElement("Vasiliy");
            collectionEnum.AddElement("Get");
            collectionEnum.AddElement("Your");
            collectionEnum.AddElement("Bill");

            foreach (Text s in collectionEnum) {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            MyEnumerable myEnumerable = new MyEnumerable(6);
            myEnumerable.Add("1");
            myEnumerable.Add("2");
            myEnumerable.Add("3");
            myEnumerable.Add("4");
            myEnumerable.Add("5");
            myEnumerable.Add("6");
            myEnumerable.Add("7");

            IEnumerator enumerator = myEnumerable.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = (string)enumerator.Current;
                Console.WriteLine($"MyEn: {current}");
            }

            enumerator.Reset();
            while (enumerator.MoveNext()) {
                Console.Write($"{enumerator.Current} ");
            }
            Console.WriteLine();
            Console.ReadLine();
        }

        private class Text  {
            public string Name { get; private set; }

            public object Current => throw new NotImplementedException();

            public static implicit operator Text(string s) {
                return new Text(s);
            }

            public static implicit operator string(Text t)
            {
                return t.Name;
            }

            public Text(string Name) {
                this.Name = Name;
            }
        }

        private class CollectionEnum : IEnumerator, IEnumerable
        {
            private const int _DefaultArraySize = 4;

            private readonly Text[] elements;            
            private int currentIndex = -1;
            private int counter = -1;

            public CollectionEnum() {
                elements = new Text[_DefaultArraySize];
            }
            public CollectionEnum(int arraySize)
            {
                elements = new Text[arraySize];
            }

            private CollectionEnum(Text[] stringsv)
            {
                elements = stringsv;
                foreach (Text t in stringsv) 
                    if (t != null) currentIndex++;                
            }

            public object Current
            {
                get {
                    if (counter < 0 || counter > currentIndex)
                        throw new NotImplementedException();
                    else
                        return elements[counter];
                }
            }

            public void AddElement(Text element)
            {
                if (currentIndex < elements.Length - 1)
                {
                    elements[++currentIndex] = element;
                }
                else
                    Console.WriteLine("out of range");
            }

            public IEnumerator GetEnumerator() => new CollectionEnum(elements);

            public bool MoveNext() => ++counter <= currentIndex && currentIndex >=0;

            public void Reset() => counter = -1;
        }

        private class MyEnumerable : IEnumerable {
            private string[] values;
            private int counter = -1;
            public MyEnumerable()
            { values = new string[5];  }
            public MyEnumerable(int ArraySize)
            { values = new string[ArraySize]; }

            public void Add(string value) {
                Console.WriteLine($"\tItem [{value}] added");
                if (counter < values.Length - 1) {
                    values[++counter] = value;
                }
            }

            public IEnumerator GetEnumerator()
            {
                return new MyEnumerator(values);
            }
        }

        private class MyEnumerator : IEnumerator
        {
            private string[] values;
            private int Position = -1;

            public MyEnumerator(string[] values) {
                this.values = values;
            }

            public object Current {
                get {
                    if (Position < 0 || Position >= values.Length )
                        throw new InvalidOperationException();
                    return values[Position];
                }
            } 

            public bool MoveNext()
            {
                return ++Position < values.Length;
            }

            public void Reset()
            {
                Position = -1;
            }
        }
        #endregion

        public static void Test_GenericIEnumerator() {
            MyEnumerable<int> myEnumerable = new MyEnumerable<int>(6);
            myEnumerable.Add(10);
            myEnumerable.Add(17);
            myEnumerable.Add(19);
            myEnumerable.Add(100);
            myEnumerable.Add(101);
            myEnumerable.Add(103);
            myEnumerable.Add(105);

            foreach (int i in myEnumerable) {
                Console.Write($"{i} ");
            }
            Console.WriteLine();

            MyEnumerable<MyEnumerable> me = new MyEnumerable<MyEnumerable>(2);
            me.Add(new MyEnumerable(2) { "Test1", "Test2" });
            me.Add(new MyEnumerable(2) { "Test3", "Test4" });

            foreach (MyEnumerable my in me) {
                Console.WriteLine($"{my.GetType()} {me.GetType()}");
                foreach (string s in my) {
                    Console.Write($"{s} ");
                }
                Console.WriteLine();
            }


            Console.ReadLine();
        }

        private class MyEnumerable<T> : IEnumerable
        {
            private T[] values;
            private int counter = -1;

            public MyEnumerable() => values = new T[5];
            public MyEnumerable(int ArraySize) => values = new T[ArraySize];
            public void Add(T value)
            {
                if (counter < values.Length - 1)
                {
                    values[++counter] = value;
                }
            }
            public IEnumerator GetEnumerator()
            {
                return new MyEnumerator<T>(values);
            }
        }

        private class MyEnumerator<T> : IEnumerator
        {
            private readonly T[] valuesT;
            private int Position = -1;

            public MyEnumerator(T[] valuesT) 
            {
                this.valuesT = valuesT;
            }

            public object Current
            {
                get
                {
                    return valuesT[Position];
                }
            }
            public bool MoveNext()
            {
                return ++Position < valuesT.Length;
            }

            public void Reset()
            {
                Position = -1;
            }
        }
    }
}
