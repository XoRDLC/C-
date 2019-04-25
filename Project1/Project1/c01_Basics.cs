using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    enum CAT {
        ROUTINE = 0,
        ERROR,
        ABNORMAL        
    }
    public class c01_Basics
    {
        readonly public string name;
        private static int _instanceCount;
        private int _max;
        private int _min;
        private int _readonly;

        private double height;
        private double weight;
        private byte age;

        static c01_Basics() {
            _instanceCount = 0;
        }

        //private void Dummy() { 
        public static void Main(String[] args)
        {
            Log(c01_Basics.GetInstanceCount.ToString());
            c01_Basics class2 = new c01_Basics("Programmer");
            Log(c01_Basics.GetInstanceCount.ToString());
            Log(class2.Name);
            c01_Basics class1 = new c01_Basics(64);
            Log(c01_Basics.GetInstanceCount.ToString());
            c01_Basics class3 = new c01_Basics("Senior Programmer");
            Log(c01_Basics.GetInstanceCount.ToString());
        }

        public double this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0: age = (byte)(value); break;
                    case 1: height = (double)(value); break;
                    case 2: weight = (double)(value); break;
                    default: throw new ArgumentOutOfRangeException("index: " + index);
                }
            }
            get
            {
                switch (index)
                {
                    case 0: return (double)(age);
                    case 1: return height;
                    case 2: return weight;
                    default: throw new ArgumentOutOfRangeException("index: " + index);
                }
            }
        }
        public string Name {
            get => this.name;
        }

        private c01_Basics() {
            Log(name);
            _instanceCount++;
        }

        public c01_Basics(string name):this() {
            this.name = name;
        }

        public c01_Basics(int _readonly ):this() {
            Log(_readonly.ToString());
            this._readonly = _readonly;
        }
        
        public static int GetInstanceCount {            
            get => _instanceCount;
        }

        private int AutoP {
            get; set;
        }

        private int Write {
            set => _readonly = value;
        }

        private int Read {
            get => _readonly;
        }

        private int Max {
            set { _max = value; }
            get { return _max; }
        }

        private int Min {
            set => _min = value<_min ?  value: (_min == 0 ? value : _min);
            get => _min;
        }

        private void ShowParams(params int[] iArray) {
            CAT cat = CAT.ERROR;
            foreach (int i in iArray){
                TryMe(ref cat,  i, out string o);
                Log(cat.ToString() + "\t" +  o, cat);
            }
        }

        private ref int GetMax(ref int a, ref int b) {
            if (a > b) return ref a;
            else return ref b;
        }

        private void TryMe(ref CAT cat, int iValue, out string sOut) {

            if (iValue < 0) {
                cat = CAT.ERROR;
                sOut = "negative value";
            }
            else if (iValue > 0) {
                cat = CAT.ROUTINE;
                sOut = "all ok";
            }
            else {
                cat = CAT.ABNORMAL;
                sOut = "infinity";
            }
        }

        public static void Log(string msg) {
            Log(msg, CAT.ROUTINE);
        }

        private static void Log(string msg, CAT cat) {
            Debugger.Log(0, cat.ToString(), msg + "\n");
        }
    }

    partial class Class3P
    {
        partial void Print(int x) { c01_Basics.Log(x.ToString()); }
    }
}
