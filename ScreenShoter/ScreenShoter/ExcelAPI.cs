
using System;
using System.Threading;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.ComponentModel;

//Environment: https://docs.microsoft.com/en-us/dotnet/api/system.environment?view=netframework-4.8
//Notifications: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?redirectedfrom=MSDN&view=netframework-4.8
//OpenFileDialog: https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.openfiledialog?view=netframework-4.8
namespace ScreenShoter
{
    public class ExcelAPI
    {

        private static Excel.Application exlApp;
        static ExcelAPI() {
            if (exlApp != null) return;

            try
            {
                exlApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch (System.Runtime.InteropServices.COMException ex) {
                exlApp = new Excel.Application();
            }
            finally {
                exlApp.Visible = true;
            }
        }

        private const string PREFIX = "Снимок ";
        private const string SUFFIX = "; Источник: ";
        private const byte URLColumn = 2;

        private Excel.Workbook wb;
        private Excel.Worksheet sh;
        private string sURL = "";
        private int multiScreenCount = 0;
        
        public void PasteInWorksheet(string URL, ref Bitmap BM)
        {    
            if (wb != null) {
                try {
                    wb.Name.ToString();
                }
                catch(System.Runtime.InteropServices.COMException ex) {
                    FNotification.CustomBallonTip = "Книга была закрыта\t" + ex.ToString();
                    wb = null;
                }
            }
            if (wb == null)
            {
                string fileName =  GetFilePath();
                if (fileName == null || fileName.Equals(string.Empty))
                    wb = exlApp.Workbooks.Add();
                else {
                    try
                    {
                        wb = exlApp.Workbooks.Open(fileName);
                    }
                    catch(System.Runtime.InteropServices.COMException ex) {
                        FNotification.CustomBallonTip = "Формат выбранного файла не Excel" + ex.ToString();
                        return;
                    }
                }
                sh = wb.ActiveSheet;
            }

            bool compositeScreen = sURL.Equals(URL);
            if (!compositeScreen)
            {
                multiScreenCount = 1;
                sURL = URL;
            }
            else multiScreenCount++;

            /*
             * TODO
             * При переключениями между экземплярами ExcelAPI 
             * иногда вставляет картинки не в ту книгу.
             * При тестировании wb.Activate() решило проблему.
             * поискать адекватное решение
            */
            
            if (exlApp.Ready)
                wb.Activate();
            else
            {
                FNotification.CustomBallonTip = 
                    "Excel ожидает подтверждения, снимок не сохранён";
                return;
            }

            int iStartPosition = 2;
            int iFreeColumn = 1;
            int iShCount = sh.Shapes.Count;
            
            /* 
             * смещение снимка относительно предыдущего
             * если снимки от одной ссылки, размещать слева направо
             */

            if (iShCount > 0) {
                Excel.Shape shape = sh.Shapes.Item(iShCount);
                if (shape.Type != Microsoft.Office.Core.MsoShapeType.msoPicture)
                    shape = sh.Shapes.Item(iShCount - 1);

                if (!compositeScreen)
                    iStartPosition = shape.BottomRightCell.Row + 1;
                else {
                    iStartPosition = shape.TopLeftCell.Row;
                    iFreeColumn = shape.BottomRightCell.Column + 1;
                }                
            }

            //TODO найти как вставить снимок экрана без вставки его в буфер

            PutlToClipboard(BM);
            sh.Paste(sh.Cells[iStartPosition + (compositeScreen?0:1), iFreeColumn]);
            if (!compositeScreen && !URL.Equals(BrowserHandler.PROCESS_UNKNOWN))
            {
                if (exlApp.Ready) sh.Activate();
                sh.Cells[iStartPosition, 2].Value = URL;
                try
                {
                    sh.Cells[iStartPosition, 1].Select(); // выделение последней добавленной ссылки
                }
                catch (System.Runtime.InteropServices.COMException ex) {
                    FNotification.CustomBallonTip = "ошибка при выделении: " + ex.ToString();
                }
            }

            //если снимок не браузера (пока даже если не из Хрома), то не вставлять текстовую заглушку

            if (!URL.Equals(BrowserHandler.PROCESS_UNKNOWN))
            {
                sh.Shapes.Item(iShCount + 1).AlternativeText = 
                    PREFIX + multiScreenCount + SUFFIX + URL;
                PutlToClipboard(URL);
            }

            try
            {
                /*
                if (iShCount == 0)
                    wb.SaveAs(Type.Missing, Type.Missing, Type.Missing);
                else if (iShCount % 10 == 0) */
                    wb.Save();
                Publisher.ScreenCaptured();
            }
            catch(System.Runtime.InteropServices.COMException ex)
            {
                FNotification.CustomBallonTip = "ошибка при сохранении: " + ex.ToString();
            }
        }

        private string GetFilePath()
        {
            var filePath = string.Empty;
            Thread thread = new Thread(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "Excel files (*.xls*)|*.xls*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return filePath;
        }

        public void DeleteLastShape()
        {
            if (wb != null&& sh != null)
            {
                int iShapesCount = sh.Shapes.Count;
                if (iShapesCount > 0 && multiScreenCount > 0) { 
                    sh.Shapes.Item(iShapesCount).Delete();
                    if (--multiScreenCount == 0) sURL = "";

                    Publisher.ScreenDeleted();
                }
            }
        }

        private static void PutlToClipboard(object obj)
        {
            //без отдельного потока в буфер не запиховывается
            Thread thread = new Thread(() =>
            {
                if (obj is string)
                    Clipboard.SetText((string)obj);
                else if (obj is Image)
                    Clipboard.SetImage((Image)obj);
                else
                    new ArgumentException();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
