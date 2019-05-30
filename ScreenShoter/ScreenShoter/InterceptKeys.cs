using System.Threading;
using System.Drawing;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

//https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-keyboard-hook-in-c/
namespace ScreenShoter
{
    class InterceptKeys

    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private readonly LowLevelKeyboardProc _proc ;
        private static IntPtr _hookID = IntPtr.Zero;

        private const Keys stopKeyZP = Keys.PrintScreen;
        private const Keys stopKeySP = Keys.F10;
        private const Keys stopKeyDeleteLast_Key1 = Keys.RControlKey;
        private const Keys stopKeyDeleteLast_Key2 = Keys.Back;
        private Keys lastPressedKey = Keys.None;

        private ExcelAPI screensZP = null;
        private ExcelAPI screensSP = null;
        private ExcelAPI currentApi = null;

        private FNotification fNotification = null;

        [STAThread]
        public void Run()        {
            _hookID = SetHook(_proc);
            fNotification = new FNotification();
            Application.Run(fNotification);
            UnhookWindowsHookEx(_hookID);
        }

        public InterceptKeys() {
            _proc = HookCallback;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        [STAThread]
        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool isDeleteLast =
                    (lastPressedKey == stopKeyDeleteLast_Key1 &&
                     (Keys)vkCode == stopKeyDeleteLast_Key2);

                FNotification.TextLine = $"{vkCode, -3}\t{(Keys)vkCode, -20}\t{lastPressedKey, -20}\t{isDeleteLast}";
                lastPressedKey = (Keys)vkCode;

                if ((Keys)vkCode == stopKeyZP || 
                    (Keys)vkCode == stopKeySP ||
                    isDeleteLast) 
                {
                    Publisher.HideBallonTip();
                    Thread thread = new Thread(() => {
                        Bitmap BM = null;
                        string url = null;
                        if (!isDeleteLast) {
                            BM = ScreenShoter.GetBitmap(out string process);
                            url = BrowserHandler.GetURL(process); }
                        switch ((Keys)vkCode)
                        {
                            case stopKeyZP:
                                if (screensZP == null) screensZP = new ExcelAPI();
                                screensZP.PasteInWorksheet(url, ref BM);
                                currentApi = screensZP;
                                break;
                            case stopKeySP:
                                if (screensSP == null) screensSP = new ExcelAPI();
                                screensSP.PasteInWorksheet(url, ref BM);
                                currentApi = screensSP;
                                break;
                            default:
                                if (isDeleteLast && currentApi != null)
                                    currentApi.DeleteLastShape();
                                break;
                        }
                    });
                    thread.Start();
                    thread.Join();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
