using Microsoft.Office.Core;
using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace ScreenShoter
{
    class BrowserHandler
    {
      
        public const string PROCESS_CHROME = "chrome";
        public const string PROCESS_UNKNOWN = "other";

        public static string GetURL(string processName) {
            if (processName.Equals(PROCESS_CHROME))
            {
                string result = GetChromeUrlNew();
                return result == null ? PROCESS_UNKNOWN : result;
            }
                
            else
                return PROCESS_UNKNOWN;
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

        //https://stackoverflow.com/questions/18897070/getting-the-current-tabs-url-from-google-chrome-using-c-sharp
        #region new url from chrmoe 
        private static string GetChromeUrlNew()
        {
            // there are always multiple chrome processes, so we have to loop through all of them to find the
            // process with a Window Handle and an automation element of name "Address and search bar"
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero) continue;

                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

                // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
                AutomationElement elmUrlBar = null;
                AutomationElement sslExists = null;
                try
                {
                    // walking path found using inspect.exe (Windows SDK) for Chrome 86.0.4240.75 
                    /*   
                        "Google Chrome" панель                                                  <== elm0
                            "" панель                                                           <== elm1
                            "" панель                                                           <== elm2
                                "" панель                                                       <== elm3
                                    "" панель                                                   <== elm4
                                    "" панель                                                   <== elm5
                                        "Назад" кнопка
                                        "Вперёд" кнопка
                                        "Перезагрузка" кнопка
                                        "Главная страница" кнопка
                                        "" группа                                               <== elm6
                                            "" элемент меню                                     <== sslExists
                                            "Адресная строка и строка поиска" элемент меню      <== elmUrlBar

                    */
                    var elm0 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                    if (elm0 == null) { continue; } // not the right chrome.exe
                    //                                // here, you can optionally check if Incognito is enabled:
                    //                                //bool bIncognito = TreeWalker.RawViewWalker.GetFirstChild(TreeWalker.RawViewWalker.GetFirstChild(elm1)) != null;
                    //var elm1 = TreeWalker.RawViewWalker.GetFirstChild(elm0);
                    //var elm2 = TreeWalker.RawViewWalker.GetNextSibling(elm1);
                    //var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    //var elm4 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    //var elm5 = TreeWalker.RawViewWalker.GetNextSibling(elm4);
                    //var elm6 = elm5.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    //sslExists = TreeWalker.RawViewWalker.GetFirstChild(elm6);
                    ////elmUrlBar = elm6.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Адресная строка и строка поиска"));
                    //elmUrlBar = elm6.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    //AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
                    //переменная для получения элемента управления с признаком защищённого соединения https (свойство Name == "Сведения о сайте"; 
                    //если http, то свойство имеет другое имя, но пока не знаю какое.
                    sslExists = elm0.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "Сведения о сайте"));

                    //Получение элемента управления с ссылкой, по умолчанию не содержит типа протокола (https или http, случаи с ftp, file не предусматривал) 
                    //Протокол вытаскивается из sslExists
                    elmUrlBar = elm0.FindFirst(TreeScope.Descendants,
                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                    if (elmUrlBar==null) elmUrlBar = elm0.FindFirst(TreeScope.Descendants,
                        new PropertyCondition(AutomationElement.NameProperty, "Адресная строка и строка поиска"));
                }
                catch
                {
                    // Chrome has probably changed something, and above walking needs to be modified. :(
                    // put an assertion here or something to make sure you don't miss it
                    continue;
                }

                // make sure it's valid
                if (elmUrlBar == null) 
                    continue;

                // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
                if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty)) continue;
                
                // there might not be a valid pattern to use, so we have to make sure we have one
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                string conType = "http://";

                /*  https://stackoverflow.com/questions/6837935/legacyiaccessible-in-windows-automation
                    using UIAWrapper, for LegacyIAccesible properties
                    признак защищённой ссылки hhtps) хранится в поле LegacyIAccesible.Description, нормально доступ к нему не получить
                    для этого используется адаптер(установлен через nuget). Адаптер перегружает методы System.Windows.Automation и конфликтует с ними
                    чтобы не кофликтовало нужно удалить из ссылок UIAutomationClient и UIAutomationTypes
                    
                    Если защищенное соединение, то находится элемент с именем "Сведения о сайте", и присваивается sslExists
                    И LegacyIAccesible.Description содержит "Защищено". В противном случае получаем null, хотя элемент управления существует, но у него другое имя, какое - пока не знаю
                */

                if (sslExists != null)
                {
                    if ((bool)sslExists.GetCurrentPropertyValue(AutomationElementIdentifiers.IsLegacyIAccessiblePatternAvailableProperty))
                    {
                        var pattern = ((LegacyIAccessiblePattern)sslExists.GetCurrentPattern(LegacyIAccessiblePattern.Pattern));
                        string state = pattern.Current.Description;
                        if (state.ToLower().Equals("защищено"))
                            conType = "https://";
                    }
                }
                else { }
                if (patterns.Length >= 1)
                {
                    string ret = "";
                    try
                    {
                        ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                        if (ret != "") return conType + ret;
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.ToString());                    
                    }                    
                    continue;
                }
            }
            return null;
        }
    #endregion

    }
}
