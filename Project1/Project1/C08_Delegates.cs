using System;
using static Learning.Statics;

namespace Learning
{
    class C08_Delegates
    {
        public void Main() {
            Test_NamedDelegates();
            Test_AnonDelegates();
        }


        #region Named Delegates, Test_NamedDelegates()
        public delegate void ShowResult(int x);
        public delegate int GetCasualInt(int x);
        public delegate int GetRefInt(ref int x);

        public static void Test_NamedDelegates() {
            int x = 51;
            int xr = 51;

            ShowResult sr = x < 50 ? new ShowResult( LessThen ) : new ShowResult ( EqualOrMore );
            sr?.Invoke(x);

            GetCasualInt gci = MultiplyBy2;
            gci             += MultiplyBy4;
            GetRefInt gri    = MultiplyBy2;
            gri             += MultiplyBy4;

            Log("x: " + x + "\txr: " + xr);
            Log("gci: " + gci?.Invoke(x));
            Log("gri: " + gri?.Invoke(ref xr));

            gci -= MultiplyBy2;            gci -= MultiplyBy2;
            gri -= MultiplyBy2;            gri -= MultiplyBy2;


            Log("x: " + x + "\txr: " + xr);
            Log("gci: " + gci?.Invoke(x));
            Log("gri: " + gri?.Invoke(ref xr));

            gci -= MultiplyBy4;
            gri -= MultiplyBy4;

            Log("x: " + x + "\txr: " + xr);
            Log("gci: " + gci?.Invoke(x));
            Log("gri: " + gri?.Invoke(ref xr));
        }

        public static void LessThen(int x) {
            Log(x + " less then 50");
        }

        public static void EqualOrMore(int x) {
            if (x == 50) Log(x + " equal 50");
            else Log(x + " more then 50");
        }

        public static int MultiplyBy2(int x) { x *= 2; return x; }
        public static int MultiplyBy4(int x) { x *= 4; return x; }
        public static int MultiplyBy2(ref int x) { x *= 2; return x; }
        public static int MultiplyBy4(ref int x) { x *= 4; return x; }

        #endregion

        #region Delegates with Anonymous methods, Test_AnonDelegates()
        private delegate void AnonD(ref int x);
        public static void Test_AnonDelegates() {
            AnonD an = delegate (ref int x)
            {
                Log("x: " + x);
                x *= 3;
            };
            an      += delegate (ref int x)
            {
                Log("x: " + x);
            };
            int y = 4;
            an?.Invoke(ref y);

            AnonD anX;
            {
                //scope of t
                int t = 4;
                anX = delegate
                {
                    Log("anX, t: " + t);
                };
            }
            //Log(t); not exists

            //t still exists in delegate anX
            anX.Invoke(ref y);

        }
        #endregion
    }
}
