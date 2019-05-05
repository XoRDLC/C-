using System;
using static Project1.Statics;


namespace Project1
{
    class C07_Arrays
    {

        public void Main() {
            Test_Arrays();
        }

        #region Array using test, Test_Arrays()
        public static void Test_Arrays() {
            var twoDim1 = new int[2, 3];
            int[,] twoDim2 = { { 1, 1, 1, 1 }, { 2, 2, 2, 2 } };
            int[][] twoDim3 = new int[2][];
            {
                twoDim3[0] = new int[4];
                twoDim3[1] = new int[2];
            }
            int[][,] threeDim = new int[2][,];
            {
                threeDim[0] = new int[,] { {1,2,3 }, { 4,5,6 } };
                threeDim[1] = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            }

            string[] oneDim1 = new string[1];
            string[] oneDim2 = new string[] { };
            string[] oneDim3 = new string[] { "value", "not value", String.Concat(1,twoDim2[1,1],3,4,5,6,7) };
            Log(twoDim1.Rank + "\t" + twoDim1.Length);
            Log(twoDim2.Rank + "\t" + twoDim2.Length);
            Log(twoDim3.Rank + "\t" + twoDim3.Length);
            Log(oneDim1[0]==null);
            Log(oneDim2.Rank + "\t" + oneDim2.Length);
            Log(oneDim3[oneDim3.Length-1]);

            int[] oneDim4 = new int[] { 1, 3, -1, 5, -6, 10 };
            
            ref int refMaxValue = ref GetMaxValuePointer(oneDim4);
            foreach (int i in oneDim4) Log("i: " + i);
            Log("MaxValue: " + refMaxValue);
            refMaxValue = 140;
            foreach (int i in oneDim4) Log("i: " + i);
            
            Log("3D Rank: " + threeDim.Rank + "\t3D Len: " + threeDim.Length + "\t3D Type: " + threeDim.GetEnumerator().ToString());
            PrintJaggedArrayList(threeDim);
        }

        public static ref int GetMaxValuePointer(int[] array) {
            int max = 0;
            int maxInxdex = 0;
            for (int i = 0; i < array.Length; i++) {
                if (max < array[i]) {
                    max = array[i];
                    maxInxdex = i;
                }
            }

            return ref array[maxInxdex];
        }

        private static void PrintJaggedArrayList(int[][,] array) {
            foreach (int[,] i in array) {
                Log("Size: " + i.GetLength(0));
                foreach (int y in i) {
                    Log("Item: " + y);
                }
            }
        }
        #endregion
    }
}
