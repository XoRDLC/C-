using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class2
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
        }
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
