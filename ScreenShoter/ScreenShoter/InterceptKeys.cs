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


        /*https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/ms182351(v=vs.80)?redirectedfrom=MSDN
         * STAThreadAttribute indicates that the COM threading model for the application is single-threaded apartment. 
         * This attribute must be present on the entry point of any application that uses Windows Forms; 
         * if it is omitted, the Windows components might not work correctly. If the attribute is not present, 
         * the application uses the multithreaded apartment model, which is not supported for Windows Forms.
         */
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

        /* SetWindowsHookEx
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexw
         * Installs an application-defined hook procedure into a hook chain. You would install a 
         * hook procedure to monitor the system for certain types of events. These events are associated 
         * either with a specific thread or with all threads in the same desktop as the calling thread.
         */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        /* MarshalAs
         * https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/ms172514(v=vs.85)
         * https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshalasattribute?redirectedfrom=MSDN&view=net-5.0
         * Indicates how to marshal the data between managed and unmanaged code.
         * 
         * UnhookWindowsHookEx
         * https://docs.microsoft.com/en-us/previous-versions/windows/embedded/dn529146(v=winembedded.70)
         * Removes a hook procedure installed in a system by the SetWindowsHookEx function.
         */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        /* CallNextHookEx
         * https://docs.microsoft.com/ru-ru/windows/win32/api/winuser/nf-winuser-callnexthookex?redirectedfrom=MSDN
         * Passes the hook information to the next hook procedure in the current hook chain. 
         * A hook procedure can call this function either before or after processing the hook information.         * 
         */
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        /* GetModuleHandle
         * https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea
         * Retrieves a module handle for the specified module. The module must have been loaded by the calling process.
         * To avoid the race conditions described in the Remarks section, use the GetModuleHandleEx function.
         */
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

}
