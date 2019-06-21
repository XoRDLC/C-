using System.Windows.Forms;
using Extensibility;
using System;
using Microsoft.Office.Core;
using System.Collections.Generic;
using System.IO;
using System.Text;

//https://www.mztools.com/articles/2012/MZ2012013.aspx
namespace VBEAddInCS
{
    public class Connect : IDTExtensibility2
    {
        private VBIDE.VBE _VBE;
        private VBIDE.AddIn _AddIn;

        private delegate void DelegateEventHandler(CommandBarButton sender, ref bool Cancel);
        
        private CommandBarButton _myStandardCommandBarButton;
        private CommandBarButton _myToolsCommandBarButton;
        private CommandBarButton _myCodeWindowCommandBarButton;
        private CommandBarButton _myToolBarButton;
        private CommandBarButton _myCommandBarPopup1Button;
        private CommandBarButton _myCommandBarPopup2Button;

        private CommandBar _myToolbar;
        private CommandBarPopup _myCommandBarPopup1;
        private CommandBarPopup _myCommandBarPopup2;

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
            try {
                switch (RemoveMode) {
                    case ext_DisconnectMode.ext_dm_HostShutdown:
                    case ext_DisconnectMode.ext_dm_UserClosed:

                        if (_myCodeWindowCommandBarButton != null)
                        {
                            _myCodeWindowCommandBarButton.Click -= _myCodeWindowCommandBarButton_Click;
                            _myCodeWindowCommandBarButton.Delete();
                        }
                        if (_myStandardCommandBarButton != null)
                        {
                            _myStandardCommandBarButton.Click -= _myStandardCommandBarButton_Click;
                            _myStandardCommandBarButton.Delete();
                        }
                        if (_myToolsCommandBarButton != null)
                        {
                            _myToolsCommandBarButton.Click -= _myToolsCommandBarButton_Click;
                            _myToolsCommandBarButton.Delete();
                        }
                        if (_myToolBarButton != null)
                        {
                            _myToolBarButton.Click -= _myToolBarButton_Click;
                            _myToolBarButton.Delete();
                        }
                        if (_myCommandBarPopup1 != null)
                            _myCommandBarPopup1.Delete();
                        if (_myCommandBarPopup2 != null)
                            _myCommandBarPopup2.Delete();
                        if (_myCommandBarPopup1Button != null)
                            _myCommandBarPopup1Button.Click -= _myCommandBarPopup1Button_Click;
                        if (_myCommandBarPopup2Button != null)
                            _myCommandBarPopup2Button.Click -= _myCommandBarPopup2Button_Click;

                        break;
                }
            }
            catch(Exception e) {
                MessageBox.Show(e.ToString());
            }
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
        
        private CommandBarButton AddCommandBarButton(CommandBar commandBar) {

            CommandBarButton commandBarButton;
            CommandBarControl commandBarControl;

            commandBarControl = commandBar.Controls.Add(MsoControlType.msoControlButton);
            commandBarButton = (CommandBarButton)commandBarControl;

            commandBarButton.Caption = "My button";
            commandBarButton.FaceId = 59;            

            return commandBarButton;
        }
        
        #region All variants of additional buttons
        private void InitializeAddIn_Full()
        {
            // Constants for names of built-in commandbars of the VBA editor
            const string STANDARD_COMMANDBAR_NAME = "Standard";
            const string MENUBAR_COMMANDBAR_NAME = "Menu Bar";
            const string TOOLS_COMMANDBAR_NAME = "Tools";
            const string CODE_WINDOW_COMMANDBAR_NAME = "Code Window";

            // const stringants for names of commandbars created by the add-in
            const string MY_COMMANDBAR_POPUP1_NAME = "MyTemporaryCommandBarPopup1";
            const string MY_COMMANDBAR_POPUP2_NAME = "MyTemporaryCommandBarPopup2";

            // const stringants for captions of commandbars created by the add-in
            const string MY_COMMANDBAR_POPUP1_CAPTION = "My sub menu";
            const string MY_COMMANDBAR_POPUP2_CAPTION = "My main menu";
            const string MY_TOOLBAR_CAPTION = "My toolbar";

            // Built-in commandbars of the VBA editor
            CommandBar standardCommandBar = null;
            CommandBar menuCommandBar = null;
            CommandBar codeCommandBar = null;

            // Other variables
            CommandBarControl toolsCommandBarControl;

            int position;
            try
            {
                // Retrieve some built-in commandbars
                //List<string> list = new List<string>();
                foreach (CommandBar CB in _VBE.CommandBars) {
                    //list.Add(CB.Name + "\t" + CB.Type.ToString());
                    switch (CB.Name) {
                        case STANDARD_COMMANDBAR_NAME:
                            standardCommandBar = CB;
                            break;
                        case MENUBAR_COMMANDBAR_NAME:
                            menuCommandBar = CB;
                            break;
                        case CODE_WINDOW_COMMANDBAR_NAME:
                            codeCommandBar = CB;
                            break;
                        default:
                            break;
                    }
                }

                // Add a button to the built-in "Standard" toolbar
                _myStandardCommandBarButton = AddCommandBarButton(standardCommandBar);
                _myStandardCommandBarButton.Click += _myCodeWindowCommandBarButton_Click;

                // Add a button to the built-in "Code Window" context menu
                _myCodeWindowCommandBarButton = AddCommandBarButton(codeCommandBar);
                _myCodeWindowCommandBarButton.Click += _myCodeWindowCommandBarButton_Click;

                // ------------------------------------------------------------------------------------
                // New toolbar
                // ------------------------------------------------------------------------------------

                // Add a new toolbar 
                _myToolbar = _VBE.CommandBars.Add(MY_TOOLBAR_CAPTION, MsoBarPosition.msoBarTop, System.Type.Missing, true);

                // Add a new button on that toolbar
                _myToolBarButton = AddCommandBarButton(_myToolbar);
                _myToolBarButton.Click += _myToolBarButton_Click;

                // Make visible the toolbar
                _myToolbar.Visible = true;

                // ------------------------------------------------------------------------------------
                // New submenu under the "Tools" menu
                // ------------------------------------------------------------------------------------

                // Add a new commandbar popup 
                _myCommandBarPopup1 = (CommandBarPopup)(menuCommandBar.Controls.Add(
                MsoControlType.msoControlPopup, System.Type.Missing, System.Type.Missing,
                menuCommandBar.Controls.Count + 1, true));

                // Add a button to the built-in "Tools" menu
                _myToolsCommandBarButton = AddCommandBarButton(menuCommandBar);
                _myToolsCommandBarButton.Click += _myToolsCommandBarButton_Click;

                // Change some commandbar popup properties
                _myCommandBarPopup1.CommandBar.Name = MY_COMMANDBAR_POPUP1_NAME;
                _myCommandBarPopup1.Caption = MY_COMMANDBAR_POPUP1_CAPTION;

                // Add a new button on that commandbar popup
                _myCommandBarPopup1Button = AddCommandBarButton(_myCommandBarPopup1.CommandBar);
                _myCommandBarPopup1Button.Click += _myCommandBarPopup1Button_Click;

                // Make visible the commandbar popup
                _myCommandBarPopup1.Visible = true;

                // ------------------------------------------------------------------------------------
                // New main menu
                // ------------------------------------------------------------------------------------

                // Calculate the position of a new commandbar popup to the right of the "Tools" menu
                //если ошибка CS0656 Отсутствует обязательный для компилятора член "Microsoft.CSharp.RuntimeBinder.Binder.Convert"
                //https://stackoverflow.com/questions/24321962/dynamic-return-type-present-when-using-tlbimp-to-generate-interop-assembly/24359907#24359907
                //https://ru.stackoverflow.com/questions/901285/Отсутствует-обязательный-для-компилятора-член

                toolsCommandBarControl = null;
                foreach (CommandBarControl control in menuCommandBar.Controls)
                    if (control.accName.Equals(TOOLS_COMMANDBAR_NAME)) toolsCommandBarControl = control;
                position = toolsCommandBarControl.Index + 1;
                
                // Add a new commandbar popup 
                _myCommandBarPopup2 = (CommandBarPopup)(menuCommandBar.Controls.Add(
                MsoControlType.msoControlPopup, System.Type.Missing, System.Type.Missing,
                position, true));

                // Change some commandbar popup properties
                _myCommandBarPopup2.CommandBar.Name = MY_COMMANDBAR_POPUP2_NAME;
                _myCommandBarPopup2.Caption = MY_COMMANDBAR_POPUP2_CAPTION;

                // Add a new button on that commandbar popup
                _myCommandBarPopup2Button = AddCommandBarButton(_myCommandBarPopup2.CommandBar);
                _myCommandBarPopup2Button.Click += _myCommandBarPopup2Button_Click;

                // Make visible the commandbar popup
                _myCommandBarPopup2.Visible = true;
            }
            catch (Exception e)
            { MessageBox.Show(e.ToString()); }
        }
        #endregion

        private void InitializeAddIn()
        {
            // Constants for names of built-in commandbars of the VBA editor
            const string MENUBAR_COMMANDBAR_NAME = "Menu Bar";

            // const stringants for captions of commandbars created by the add-in
            const string MY_COMMANDBAR_POPUP2_CAPTION = "CodeStats";
            const string MY_COMMANDBAR_POPUP2_NAME = "Settings";

            // Built-in commandbars of the VBA editor
            CommandBar menuCommandBar = null;

            try
            {
                // Retrieve some built-in commandbars
                foreach (CommandBar CB in _VBE.CommandBars)
                {
                    if (CB.Name.Equals(MENUBAR_COMMANDBAR_NAME)) {
                        menuCommandBar = CB;
                        break;
                    }
                }
                //for accurate Exciption line number
                int position = menuCommandBar.Controls.Count + 1;

                // Add a new commandbar popup 
                _myCommandBarPopup2 = (CommandBarPopup)(menuCommandBar.Controls.Add(
                MsoControlType.msoControlPopup, System.Type.Missing, System.Type.Missing,
                position, true));

                // Change some commandbar popup properties
                _myCommandBarPopup2.CommandBar.Name = MY_COMMANDBAR_POPUP2_NAME;
                _myCommandBarPopup2.Caption = MY_COMMANDBAR_POPUP2_CAPTION;

                // Add a new button on that commandbar popup
                _myCommandBarPopup2Button = AddCommandBarButton(_myCommandBarPopup2.CommandBar);
                _myCommandBarPopup2Button.Click += _codeStatsSettings_Click;

                // Make visible the commandbar popup
                _myCommandBarPopup2.Visible = true;
            }
            catch (Exception e)
            { MessageBox.Show(e.ToString()); }
        }
        #region Empty Events
        private void _myToolBarButton_Click(CommandBarButton Ctrl,
            ref bool CancelDefault)
        {
            MessageBox.Show("Clicked myToolBarButton " + Ctrl.Caption);
        }

        private void _myToolsCommandBarButton_Click(CommandBarButton Ctrl,
            ref bool CancelDefault)
        {
            MessageBox.Show("Clicked myToolsCommandBarButton " + Ctrl.Caption);
        }

        private void _myStandardCommandBarButton_Click(CommandBarButton Ctrl,
            ref Boolean CancelDefault)
        {
            MessageBox.Show("Clicked myStandardCommandBarButton " + Ctrl.Caption);
        }

        private void _myCodeWindowCommandBarButton_Click(CommandBarButton Ctrl,
            ref Boolean CancelDefault)
        {
            MessageBox.Show("Clicked myCodeWindowCommandBarButton " + Ctrl.Caption);
        }

        private void _myCommandBarPopup1Button_Click(CommandBarButton Ctrl,
            ref Boolean CancelDefault)
        {
            MessageBox.Show("Clicked myCommandBarPopup1Button " + Ctrl.Caption);
        }

        private void _myCommandBarPopup2Button_Click(CommandBarButton Ctrl,
            ref Boolean CancelDefault)
        {
            MessageBox.Show("Clicked myCommandBarPopup2Button " + Ctrl.Caption);
        }
        #endregion
        
        private void _codeStatsSettings_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                string s = String.Empty;
                foreach (VBIDE.CodePane codepane in _VBE.CodePanes)
                {
                    VBIDE.CodeModule codeModule = codepane.CodeModule;
                    s += codepane.CodeModule.CountOfLines + "\t" + codepane.CodeModule.Name  + "\n";
                }

                MessageBox.Show(s);
                s = String.Empty;
                foreach (VBIDE.VBProject component in _VBE.VBProjects)
                    s += component.Name + "\t" + component.FileName + "\n";

                MessageBox.Show(s);
                MessageBox.Show(_VBE.CodePanes.Current.CodeModule.Name);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
