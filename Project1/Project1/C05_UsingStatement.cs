using System;
using System.IO;

namespace Learning
{
    class C05_UsingStatement
    {
        public void Main()
        {
            Test_Statements();
        }

        #region "using and statements, Test_Statements()"
        public static void Test_Statements()
        {
            const string fileName = "testfile.txt";

            using (TextWriter writer = File.CreateText(fileName))
            {
                writer.WriteLine("Hello, is it me you looking for");
                Statics.Log(writer.ToString());
            }

            using (TextReader reader = File.OpenText(fileName))
            {
                string inputString;
                while (null!=(inputString = reader.ReadLine()))
                    Statics.Log(inputString);
            }

            //небезопасный вариант, переменная вне блока 'using', избегать использования
            //less safe variant, 'tr' not incapsulate. Avoid using
            TextReader tr = File.OpenText(fileName);
            using (tr) {
                string inputString;
                while (null != (inputString = tr.ReadLine()))
                    Statics.Log("2: " + inputString);
            }
            //ошибка выполнения: tr уже закрыт (Dispose), но ссылка на переменную существует
            //tr is closed, but variable is declared outside of 'using' block
            //Statics.Log(tr.ReadLine());
            
        }
        #endregion
    }
}
