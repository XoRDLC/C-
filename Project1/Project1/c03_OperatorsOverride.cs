using System;
using System.Reflection;

namespace Learning
{
    class C03_OperatorsOverride
    {   
        public void Main()
        {
            Test_typeof();
            Test_operatorsOverride();
        }
        #region "typeof iterations, Test_typeof()"
        protected static void Test_typeof() {
            Type t = typeof(C03_OperatorsOverride);
            FieldInfo[] fi = t.GetFields();
            MethodInfo[] mti = t.GetMethods();
            MemberInfo[] mmi = t.GetMembers();

            foreach (FieldInfo f in fi) {
                Statics.Log(f.Name);
            }

            Statics.Log("----");

            foreach (MethodInfo m in mti) {
                Statics.Log(m.Name);
            }

            Statics.Log("----");

            foreach (MemberInfo m in mmi) {
                Statics.Log(m.Name);
            }
        }
        #endregion
        #region "operator override, Test_operatorsOverride()"
        protected static void Test_operatorsOverride()
        {
            Operators op = new Operators(10, "Tester");
            op = op + 10;
            Statics.Log(op.Age.ToString());
            ++op;
            Statics.Log(op.Age.ToString());
            op -= 4;
            Statics.Log(op.Age.ToString());
            Operators op1 = 40;
            Statics.Log(op1.Name);
        }
        #endregion
    }

    #region "operators override"
    class Operators
    {
        private readonly string name;
        private float age;

        private Operators() { }
        public Operators(float age, string name)
        {
            this.age = age;
            this.name = name;
        }        

        public float Age
        {
            get => age;
            set => this.age = value;
        }

        public string Name
        {
            get => this.name;
        }

        public static Operators operator +(Operators op, float newage)
        {
            Operators operators = new Operators(op.Age, op.Name);
            operators.Age = op.Age + newage;
            return operators;
        }
        public static Operators operator -(Operators op, float newage)
        {
            Operators operators = new Operators(op.Age, op.Name);
            operators.Age = op.Age - newage;
            return operators;
        }

        public static Operators operator ++(Operators op)
        {
            Operators operators = new Operators(op.Age, op.Name);
            operators.Age = ++op.Age;
            return operators;
        }
        public static implicit operator int(Operators op)
        {
            return (int)op.Age;
        }

        public static implicit operator Operators(int newAge)
        {
            Operators oper = new Operators(newAge, "Dummy");
            return oper;
        }
    }
    #endregion
}
