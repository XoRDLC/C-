using System;
using System.Collections;

namespace Learning
{
    class Class1
    {
        public void Main() {
            Test_IEnumerator();
        }


        private static void Test_IEnumerator() {
            CollectionEnum collectionEnum = new CollectionEnum();
            collectionEnum.AddElement("Kot");
            collectionEnum.AddElement("Vasiliy");
            collectionEnum.AddElement("Get");
            collectionEnum.AddElement("Your");
            collectionEnum.AddElement("Bill");

            foreach (Text s in collectionEnum) {
                Console.WriteLine(s);
            }

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
    }
}
