using System;
using static Project1.Statics;

namespace Project1
{
    class Class1
    {
        public void Main() {
            //for(int i=0; i<50; i++)
            Test_Delegates();
        }

        delegate void MyDel(int param);

        public static void Test_Delegates() {
            int rnd = new Random().Next(99);
            MyDel md = rnd < 50 ? new MyDel(LessThen50) : (rnd > 50 ? new MyDel( GreaterThen50) : new MyDel(Equalent50));
            md(rnd);
        }

        public static void LessThen50(int value) {
            Log(value + " less then 50");
        }

        public static void GreaterThen50(int value) {
            Log(value + " greater then 50");
        }

        public static void Equalent50(int value) {
            Log("equal 50");
        }
    }
}
