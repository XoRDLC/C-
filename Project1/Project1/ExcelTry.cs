using System;
using Excel = Microsoft.Office.Interop.Excel;
namespace Learning
{
    class ExcelTry
    {
        private const string filePath = "d:\\Programming\\VBA\\011 Переводчик\\РСО-Перевод.xlsm";

        public void Dummy()
        //public static void Main()
        {
            Excel.Application exlApp = new Excel.Application();
            exlApp.Visible = true;
            Excel.Workbook wb = exlApp.Workbooks.Open(filePath);
            exlApp.DisplayAlerts = false;
            Excel.Worksheet sh = wb.ActiveSheet;
            Excel.Range cell = sh.Cells.Item[1];
            cell.Value = "Test";

            //exlApp.Quit();
        }
    }    
}