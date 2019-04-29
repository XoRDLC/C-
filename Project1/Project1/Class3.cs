using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class3
    {
        public static void Main()
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
    }

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
}
