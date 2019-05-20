using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace ScreenShoter
{
    class Class3
    {

        public static string GetURL(string processName)
        {
            string url = "";
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                url = GetChromeUrl(process);
                if (url == null)
                    continue;
                else break;
            }
            System.Diagnostics.Debugger.Log(0, "", "\n" + url + "\n");
            return url?.ToString();
        }

        public static void RunMe2()
        {
            foreach (Process process in Process.GetProcessesByName("chrome"))
            {
                string url = GetChromeUrl(process);
                if (url == null)
                    continue;

                Console.WriteLine("CH Url for '" + process.MainWindowTitle + "' is " + url);
            }
            Console.ReadLine();
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


            AutomationElementCollection elm1 = element.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            if (elm1.Count == 0)
                return null;

            AutomationElement elm = elm1[0];
            string vp = ((ValuePattern)elm.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            Console.WriteLine(vp);
            return vp;
        }
    }
}
