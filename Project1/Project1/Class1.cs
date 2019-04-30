using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class1
    {
        public void Main() {
            ;
            Statics.Log(Choises.YES);
            for(int i = 0; i< 10; i++) 
                Statics.Log(new Choises().MagicBall());
        }
    }

    public struct Choises : IDecisions {
        public const string YES = "yes";
        public const string NO = "no";

        public string MagicBall() {
            var d = new Object().GetHashCode();
            return Math.Cos(d) >= 0 ? YES : NO;
        }

    }

    public interface IDecisions {
        string MagicBall();
    }
}
