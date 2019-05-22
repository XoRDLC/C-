using System;
using System.Windows.Automation;
using System.Diagnostics;
namespace ScreenShoter
{
    class BrowserHandler
    {
        public const string APP = "StringFromApp";
        public const string PROCESS_CHROME = "chrome";
        public const string PROCESS_UNKNOWN = "other";

        public static string GetURL(string processName) {
            if (processName.Equals(PROCESS_CHROME))
            {
                string result = GetChromeURL();
                return result == null ? APP : result;
            }
                
            else
                return APP;
        }

        private static string GetChromeURL()
        {
            string url = "";
            foreach (Process process in Process.GetProcessesByName(PROCESS_CHROME))
            {
                url = GetChromeUrl(process);
                if (url == null||url.Equals(""))
                    continue;
                else break;
            }
            return url?.ToString();
        }

        private static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            PropertyCondition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);
            AutomationElementCollection elm1 = element.FindAll(TreeScope.Subtree, condition);
            if (elm1 == null || elm1.Count == 0)
                return null;
            AutomationElement elm = elm1[0];
            string vp = ((ValuePattern)elm.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            return vp;
        }
    }
}
