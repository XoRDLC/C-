using System.Windows.Forms;
using Extensibility;
using System;

//https://www.mztools.com/articles/2012/MZ2012013.aspx
namespace VBEAddInCS
{
    public class Connect : IDTExtensibility2
    {
        private VBIDE.VBE _VBE;
        private VBIDE.AddIn _AddIn;
        public void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            try
            {
                _VBE = (VBIDE.VBE)Application;
                _AddIn = (VBIDE.AddIn)AddInInst;

                switch (ConnectMode)
                {
                    case ext_ConnectMode.ext_cm_Startup:
                        //OnStartupComplete will be called
                        break;
                    case ext_ConnectMode.ext_cm_AfterStartup:
                        //
                        InitializeAddIn();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
        }

        public void OnAddInsUpdate(ref Array custom)
        {
        }

        public void OnStartupComplete(ref Array custom)
        {
            InitializeAddIn();
        }

        public void OnBeginShutdown(ref Array custom)
        {
        }

        private void InitializeAddIn()
        {
            
        }
    }
}
