using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning
{
    class C10_Interfaces
    {
        public void Main() {
            Test_UsageOfIComparable1();
            Test_UsageOfIComparable2();
            Test_InterfaceByRefAccess1();
            Test_InterfaceByRefAccess2();
        }

        #region simple CompareTo check, Test_UsageOfIComparable1()
        private static void Test_UsageOfIComparable1() {
            UsageOfIComparable uoic1 = new UsageOfIComparable("1", 10f);
            UsageOfIComparable uoic2 = new UsageOfIComparable("2", 11f);
            UsageOfIComparable uoic3 = new UsageOfIComparable("3", 9f);
            UsageOfIComparable uoic4 = new UsageOfIComparable("4", 11f);

            Console.WriteLine($"{uoic1.Name} vs {uoic2.Name}: {uoic1.CompareTo(uoic2)}");
            Console.WriteLine($"{uoic1.Name} vs {uoic3.Name}: {uoic1.CompareTo(uoic3)}");
            Console.WriteLine($"{uoic2.Name} vs {uoic4.Name}: {uoic2.CompareTo(uoic4)}");

            Console.ReadLine();
        }
        #endregion

        #region Array.Sort test with CompareTo impl., Test_UsageOfIComparable2()
        private static void Test_UsageOfIComparable2() {
            UsageOfIComparable test1 = new UsageOfIComparable("test1", 14);
            UsageOfIComparable test2 = new UsageOfIComparable("test2", 3);
            UsageOfIComparable test3 = null;
            Object a = new Object();
            UsageOfIComparable[] usageOfI = new UsageOfIComparable[] {
                test2,
                new UsageOfIComparable("1", 11),
                new UsageOfIComparable("2", 14),
                new UsageOfIComparable("3", 9),
                new UsageOfIComparable("4", 17),
                new UsageOfIComparable("5", 2),
                new UsageOfIComparable("6", 4),
                test1,
                test2, test3
            };

            foreach (UsageOfIComparable u in usageOfI) {
                try
                {
                    Console.WriteLine($"{u.Name} - {u.Age}");
                }
                catch { Console.WriteLine("Wrong Argument"); }
            }

            Array.Sort(usageOfI);
            Console.WriteLine();
            foreach (UsageOfIComparable u in usageOfI)
            {
                try
                {
                    Console.WriteLine($"{u.Name} - {u.Age}");
                }
                catch { Console.WriteLine("Wrong Argument"); }
            }
            Console.ReadLine();
        }
        #endregion

        #region Access by interface reference, Test_InterfaceByRefAccess
        private static void Test_InterfaceByRefAccess1() {
            Person person = new Person("Vlad", "Tepes", 591);
            Console.WriteLine(person.GetFullName());

            IPersonA a = person as IPersonA;
            IPersonB b = person as IPersonB;

            Console.WriteLine(a.GetFullName());
            Console.WriteLine(b.GetFullName());
            Console.ReadLine();
        }
        #endregion

        #region Access by interface ref, 'as' usage, Test_InterfaceByRefAccess2()
        public static void Test_InterfaceByRefAccess2() {
            Animal[] animals = new Animal[] {
                new Cat(), new Dog(), new Bird(), new Sloth()
            };

            foreach (Animal animal in animals) {
                ILiveBirth liveBirth = animal as ILiveBirth;
                if (liveBirth != null)
                    Console.WriteLine(animal.ToString() + ": " + liveBirth.BabyCalled());
            }
            Console.ReadLine();
        }
        #endregion
    }

    #region Interface impl.
    public interface ILiveBirth
    {
        string BabyCalled();
    }

    public class Animal { }

    public class Cat : Animal, ILiveBirth
    {
        public string BabyCalled() => "kitty";
    }

    public class Dog : Animal, ILiveBirth
    {
        public string BabyCalled() => "puppy";
    }

    public class Bird : Animal { }

    public class Sloth : Animal, ILiveBirth
    {
        public string BabyCalled() => "slothy";
    }
    #endregion

    #region Explicit method impl.
    public interface IPersonA {     
        //no access modifiers
        string GetFullName();
    }
    public interface IPersonB
    {
        //no access modifiers
        string GetFullName();
    }

    class Person : UsageOfIComparable,  IPersonA, IPersonB {
        public string LastName { get; set; }
        public Person(string Name, string LastName, float Age) : base(Name, Age)
        {
            this.Name = Name;
            this.LastName = LastName;
            this.Age = Age;
        }

        public string GetFullName() { return this.Name + " " + this.LastName; }
        string IPersonA.GetFullName() { return this.LastName + " " + this.Name + " A"; }
        string IPersonB.GetFullName() { return this.LastName + " " + this.Name + " B"; }
    }
    #endregion

    #region ComparetTo impl. to user class
    class UsageOfIComparable : IComparable
    {
        public string Name { set; get; }
        public float Age { set; get; }

        public UsageOfIComparable(string Name, float Age) {
            if (Name == null || Name.Equals(String.Empty))
                throw new ArgumentException();
            this.Name = Name;
            this.Age = Age;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) throw new NullReferenceException();
            if (!(obj is UsageOfIComparable)) throw new ArgumentException();
            UsageOfIComparable uoi = (UsageOfIComparable)obj;
            if (this.Equals(uoi)) return 0;
            if (this.Age == uoi.Age) return 0;
            if (this.Age < uoi.Age) return -1;
            else return 1;
        }

    }
    #endregion
}
