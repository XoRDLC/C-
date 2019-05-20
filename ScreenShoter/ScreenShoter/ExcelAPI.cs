
using System;
using System.Threading;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace ScreenShoter
{
    static class ExcelAPI
    {

        private static Excel.Application exlApp;
        private static Excel.Workbook wb;
        private static Excel.Worksheet sh;
        public static void Dummy(Graphics BM, string URL)
        //public static void Main()
        {
            Thread thread = new Thread(() =>
            {
                if (exlApp == null) { exlApp = new Excel.Application();
                    exlApp.Visible = true;
                    exlApp.DisplayAlerts = false;
                    wb = exlApp.Workbooks.Add();
                    sh = wb.ActiveSheet;
                }
                Excel.Range cell = sh.Cells.Item[1];
                //if (Clipboard.ContainsText()) cell.Value = Clipboard.GetText();
                //cell.Value = Clipboard.GetText();
                cell.Value = URL;
                sh.Paste(cell);
                //cell.Value = "Test";
                //exlApp.Application.Quit();
                //exlApp = null;
            });

            thread.Start();
            thread.Join();
            thread.Abort();
        }
    }
}