using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ScreenShoter
{
    public static class ScreenShoter
    {
        private const int CUT_TOP_DEF = 36;
        private const int CUT_BOTTOM_DEF = 63;
        private const int CUT_LEFT_DEF = 0;
        private const int CUT_RIGHT_DEF = 0;
        private const double ZOOM_DEF = 0.5;

        public static Bitmap GetBitmap(out string process)
        {
            ScreenShoter.GetSettings(
                out int cutTop, out int cutBottom,
                out int cutLeft, out int cutRight, 
                out double zoom, out process);

            Bitmap BM = new Bitmap(
                Screen.PrimaryScreen.Bounds.Width - cutRight, 
                Screen.PrimaryScreen.Bounds.Height - cutBottom);

            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(cutLeft, cutTop, 0, 0, BM.Size);

            Size newSize = new Size(
                (int)(BM.Width * zoom), 
                (int)(BM.Height * zoom));

            BM = new Bitmap(BM, newSize);

            GH.Dispose();
            return BM;
        }
        private static void GetSettings(out int cutTop, out int cutBottom,
            out int cutLeft, out int cutRight, out double zoom, 
            out string process) {
            string activeWindow = GetActiveWindowTitle();
            if (activeWindow == null)
            {
                cutTop = CUT_TOP_DEF;
                cutBottom = CUT_BOTTOM_DEF;
                cutLeft = CUT_LEFT_DEF;
                cutRight = CUT_RIGHT_DEF;
                zoom = ZOOM_DEF;
                process = BrowserHandler.APP;
            }
            else if (activeWindow.IndexOf("Google Chrome") >= 0)
            {
                cutTop = CUT_TOP_DEF;
                cutBottom = CUT_BOTTOM_DEF;
                cutLeft = CUT_LEFT_DEF;
                cutRight = CUT_RIGHT_DEF;
                zoom = ZOOM_DEF;
                process = BrowserHandler.PROCESS_CHROME;
            }
            else if (activeWindow.IndexOf("Яндекс.Браузер") >= 0 ||
                activeWindow.IndexOf("Opera") >= 0 ||
                activeWindow.IndexOf("Firefox") >= 0)
            {
                cutTop = CUT_TOP_DEF;
                cutBottom = CUT_BOTTOM_DEF;
                cutLeft = CUT_LEFT_DEF;
                cutRight = CUT_RIGHT_DEF;
                zoom = ZOOM_DEF;
                process = BrowserHandler.PROCESS_UNKNOWN;
            }
            else
            {
                cutTop = 0;
                cutBottom = CUT_BOTTOM_DEF;
                cutLeft = 0;
                cutRight = 0;
                zoom = 0.75;
                process = BrowserHandler.PROCESS_UNKNOWN;
            }
                
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }
}
