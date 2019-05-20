using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScreenShoter
{
    static class _Main
    {
        //https://vscode.ru/prog-lessons/kak-delat-skrinshotyi-s-sharp.html


        [STAThread]
        public static void Main()
        {
            InterceptKeys iKey = new InterceptKeys(Keys.PrintScreen);
            iKey.Run();
        }
    }
}
