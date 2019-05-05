using System;
using static Project1.ArabicEnumerates;

namespace Project1
{
    class C06_StructEnum
    {
        public void Main() {
            Test_Struct();
            Test_Enumerations();
        }

        #region struct test, Test_Struct()
        public static void Test_Struct()
        {        
            Statics.Log(Choises.YES);
            for (int i = 0; i < 10; i++)
                Statics.Log(new Choises().MagicBall());
            Statics.Log(Choises.Maybe() == null);
        }
        #endregion
        #region Enum test, Test_Enumerations()
        public static void Test_Enumerations() {
            Statics.Log(ARABICF.Four | ARABICF.Seven);
            Statics.Log(ARABICN.Four | ARABICN.Seven);
            Statics.Log(DuplicatesF.NotUniqueOne | DuplicatesF.NotUniqueTwo);
            Statics.Log(DuplicatesN.NotUniqueOne | DuplicatesN.NotUniqueTwo);
            DuplicatesF a = DuplicatesF.NotUniqueOne;
            DuplicatesF b = DuplicatesF.NotUniqueTwo;
            Statics.Log(a + "\t" + b + "\t" + (a == b) + "\t" + a.Equals(b)); //two-two
            DuplicatesN x = DuplicatesN.NotUniqueOne;
            DuplicatesN y = DuplicatesN.NotUniqueTwo;
            Statics.Log(x + "\t" + y + "\t" + (x == y) + "\t" + x.Equals(y)); //two-two

            DuplicatesF f = DuplicatesF.NotUniqueOne | DuplicatesF.UniqueOne | DuplicatesF.UniqueTwo | DuplicatesF.NotUniqueTwo;
            Statics.Log(f + "\t" + f.HasFlag(DuplicatesF.UniqueOne));

            DuplicatesN n = DuplicatesN.NotUniqueOne | DuplicatesN.UniqueOne | DuplicatesN.UniqueTwo | DuplicatesN.NotUniqueTwo;
            Statics.Log(n + "\t" + n.HasFlag(DuplicatesN.UniqueOne));

            Statics.Log(Enum.GetName(typeof(DuplicatesF), 0x008));
            foreach(String @enum in Enum.GetNames(typeof(DuplicatesF)))
                Statics.Log(@enum);
        }
        #endregion
    }


    #region Struct class
    public struct Choises : IDecisions {
        public const string YES = "yes";
        public const string NO = "no";

        public string MagicBall() {
            var d = new Object().GetHashCode();
            return Math.Cos(d) >= 0 ? YES : NO;
        }

        public static string Maybe() {
            return null;
        }
        
    }

    public interface IDecisions {
        string MagicBall();
    }
    #endregion
    #region Enumerations
    static class ArabicEnumerates
    {
        [Flags]
        public enum ARABICF : uint {
            One     = 0x001,
            Two     = 0x002,
            Three   = 0x004,
            Four    = 0x008,
            Give    = 0x010,
            Six     = 0x020,
            Seven   = 0x040,
            Eight   = 0x080,
            Nine    = 0x100,
            Ten     = 0x200
        }

        public enum ARABICN : uint
        {
            One = 0x001,
            Two = 0x002,
            Three = 0x004,
            Four = 0x008,
            Give = 0x010,
            Six = 0x020,
            Seven = 0x040,
            Eight = 0x080,
            Nine = 0x100,
            Ten = 0x200
        }

        [Flags]
        public enum DuplicatesF : uint
        {
            UniqueOne = 0x001,
            UniqueTwo = 0x002,
            NotUniqueOne = 0x008,
            NotUniqueTwo = NotUniqueOne
        }

        public enum DuplicatesN : uint
        {
            UniqueOne = 0x001,
            UniqueTwo = 0x002,
            NotUniqueOne = 0x008,
            NotUniqueTwo = NotUniqueOne
        }
    }
    #endregion
}