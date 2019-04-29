namespace Project1
{
    class c02_Classes
    {
        private void Dummy() { 
        //public static void Main() {
            /*
            c01_Basics c01 = new c01_Basics("Middle");
            c01[0] = 20;
            c01[1] = 175.2;
            c01[2] = 63.3;

            Console.WriteLine($"age: {c01[0]}\t height: {c01[1]}\t weight: {c01[2]}");
            Console.ReadLine();
            */

            Class3P class3P = new Class3P();
            Base @base = new Base();
            Derived derived = new Derived();
            Base check = (Base)derived;

            @base.MPrint();
            @base.OPrint();
            @base.VPrint();
            derived.MPrint();
            derived.OPrint();
            derived.VPrint();
            check.MPrint();
            check.OPrint();
            check.VPrint();

            Sealed sl = new Sealed(13, 26);
            c01_Basics.Log("Average:\t" + sl.Average().ToString());
            c01_Basics.Log("Sum:\t\t" + sl.Sum().ToString());
        }
    }

    public static class SealedExt{
        public static int Sum(this Sealed s) => (int)( s.Average() * 2f);
    }

    public sealed class Sealed {
        private readonly int a;
        private readonly int b;

        public Sealed(int a, int b) {
            this.a = a;
            this.b = b;
        }
        public double Average() => (a + b) / 2f;        
    }

    partial class Class3P {
        public Class3P()
        {
            this.Print(1);
        }

        partial void Print(int x);
    }
    class Base {
        public virtual void VPrint()
        {
            c01_Basics.Log("Base.Print virtual");
        }
        public void MPrint()
        {
            c01_Basics.Log("Base.Print for mask");
        }

        public void OPrint() {
            c01_Basics.Log("Base.Print");            
        }
    }
    class Derived :Base{
        public override void VPrint()
        {
            c01_Basics.Log("Derived.Print virtual");
        }

        new public void MPrint() {
            c01_Basics.Log("Derived.Print masked");
        }
    }
}
