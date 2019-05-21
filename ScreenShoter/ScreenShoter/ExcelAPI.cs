
using System;
using System.Threading;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

//Environment: https://docs.microsoft.com/en-us/dotnet/api/system.environment?view=netframework-4.8
//Notifications: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?redirectedfrom=MSDN&view=netframework-4.8
namespace ScreenShoter
{
    public class ExcelAPI
    {
        private enum SOUND {
            PASTE, DELETE, ERROR
        }

        private static Excel.Application exlApp;
        static ExcelAPI() {
            exlApp = new Excel.Application
            {
                Visible = true,
                DisplayAlerts = true
            };
        }

        private const string prefix = "Снимок ";
        private const string suffix = ", источник: ";

        private Excel.Workbook wb;
        private Excel.Worksheet sh;
        private const byte urlColumn = 2;
        private string sURL = "";
        private int multiScreenCount = 0;

        public void PasteInWorksheet(string URL, ref Bitmap BM)
        {
            if (wb == null)
            {
                wb = exlApp.Workbooks.Add();
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
             * При тестировании wb.Activate() решило проблему
             * поискать адекватное решение
            */

            wb.Activate();

            int iStartPosition = 2;
            int iFreeColumn = 1;
            int iShCount = sh.Shapes.Count;
            
            /* 
             * смещение снимка относительно предыдущего
             * если снимки от одной ссылки, размещать справо налево
             */

            if (iShCount > 0) {
                Excel.Shape shape = sh.Shapes.Item(iShCount);
                if (!compositeScreen)
                    iStartPosition = shape.BottomRightCell.Row + 1;
                else {
                    iStartPosition = shape.TopLeftCell.Row;
                    iFreeColumn = shape.BottomRightCell.Column + 1;
                }                
            }

            //TODO найти как вставить снимок экрана без вставки его в буфер

            PutUrlToClipboard(BM);
            sh.Paste(sh.Cells[iStartPosition + (compositeScreen?0:1), iFreeColumn]);
            if (!compositeScreen && !URL.Equals(BrowserHandler.APP))
            {
                sh.Cells[iStartPosition, 2].Value = URL;
                sh.Cells[iStartPosition, 1].Select(); // выделение последней добавленной ссылки
            }

            //если снимок не браузера (пока даже если не из Хрома), то не вставлять текстовую заглушку

            if (!URL.Equals(BrowserHandler.APP))
            {
                sh.Shapes.Item(iShCount + 1).AlternativeText = 
                    prefix + multiScreenCount + suffix + URL;
                PutUrlToClipboard(URL);
            }

            if (iShCount == 0) wb.SaveAs();
            else if (iShCount % 10 == 0) wb.Save();

            PlaySound(SOUND.PASTE);
        }

        public void DeleteLastShape()
        {
            if (wb != null&& sh != null)
            {
                int iShapesCount = sh.Shapes.Count;
                if (iShapesCount > 0 && multiScreenCount > 0) { 
                    sh.Shapes.Item(iShapesCount).Delete();
                    multiScreenCount--;
                    if (multiScreenCount == 0) sURL = "";

                    PlaySound(SOUND.DELETE);
                }
            }
        }
        //без отдельного потока в буфер не запиховывается
        private static void PutUrlToClipboard(object obj) {
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

        private static void PlaySound(SOUND soundType) {
            System.Media.SoundPlayer player = null;
            string winPath = Environment.ExpandEnvironmentVariables("%SystemRoot%") + "\\Media\\";
            switch (soundType)
            {
                case SOUND.PASTE:
                    player = new System.Media.SoundPlayer(winPath + "Speech On.wav");
                    break;
                case SOUND.DELETE:
                    player = new System.Media.SoundPlayer(winPath + "Speech Off.wav");
                    break;
                case SOUND.ERROR:
                    break;
                default:
                    throw new ArgumentException("add argument: " + soundType.ToString());
            }
            player?.Play();
        }
    }
}