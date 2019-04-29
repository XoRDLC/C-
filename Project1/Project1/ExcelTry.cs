using System;
using Excel = Microsoft.Office.Interop.Excel;
namespace Project1
{
    class ExcelTry
    {
        private const string filePath = "d:\\Programming\\VBA\\011 Переводчик\\РСО-Перевод.xlsm";

        public void Dummy()
        //public static void Main()
        {
            Excel.Application exlApp = new Excel.Application();
            exlApp.Visible = true;
            exlApp.Workbooks.Open(filePath);
            exlApp.DisplayAlerts = false;
            exlApp.Quit();
        }
    }    
}
