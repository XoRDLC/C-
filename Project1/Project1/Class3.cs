using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class3
    {
        public static void Main() {

            int  a = 0B0101010101111100000101010;
            int b = 0x1f;
            string vst = @"sttt
                    sdsd
                    ddfdf";
            Statics.Log("" + a + "\t" + b + "\t" + 5_000_000_000);
            Statics.Log(vst);
        }
    }
}
