using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?redirectedfrom=MSDN&view=netframework-4.8
//https://stackoverflow.com/questions/14273718/how-do-i-create-a-popup-notification-form
//async https://ru.stackoverflow.com/questions/418461/%D0%A0%D0%B0%D0%B1%D0%BE%D1%82%D0%B0-%D1%81-%D0%BA%D0%BE%D0%BD%D1%82%D1%80%D0%BE%D0%BB%D0%B0%D0%BC%D0%B8-%D0%B8%D0%B7-%D1%84%D0%BE%D0%BD%D0%BE%D0%B2%D0%BE%D0%B3%D0%BE-%D0%BF%D0%BE%D1%82%D0%BE%D0%BA%D0%B0
//async https://stackoverflow.com/questions/661561/how-do-i-update-the-gui-from-another-thread/18033198#18033198

namespace ScreenShoter
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class FNotification : System.Windows.Forms.Form
    {
        private NotifyIcon notifyIcon1;
        private readonly ContextMenu contextMenu1;
        private readonly MenuItem menuItemExit;
        private readonly MenuItem menuItemShow;
        private TextBox txtKeys;
        private System.ComponentModel.IContainer components;

        private static event EventHandler EventUpdateLog;
        private static event EventHandler EventShowCustomBallonTip;

        #region Fields
        private static string logText;
        private static string balloonText;
        public static string TextLine {
            get => logText;                
            set {
                logText = value;
                EventUpdateLog?.Invoke(null, null);
            }
        }

        public static string CustomBallonTip {
            get => balloonText;
            set {
                balloonText = value;
                EventShowCustomBallonTip?.Invoke(null, null);
            }
        }
        #endregion

        public FNotification()
        {
            this.InitializeComponent();
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new ContextMenu();
            this.menuItemExit = new MenuItem();
            this.menuItemShow = new MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new MenuItem[] { this.menuItemExit, this.menuItemShow });

            // Initialize menuItem1
            this.menuItemShow.Index = 0;
            this.menuItemShow.Text = "S&how";
            this.menuItemShow.Click += new EventHandler(this.MenuItem1_ClickShow);
            this.menuItemExit.Index = 1;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new EventHandler(this.MenuItem1_ClickExit);
            

            // Set up how the form should be displayed.
            this.ClientSize = new Size(this.Width, this.Height);
            this.WindowState = FormWindowState.Minimized;

            // Create the NotifyIcon.
            this.notifyIcon1 = new NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new Icon(SystemIcons.Exclamation, 40, 40);

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            //notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;

            this.Resize                     += this.HideForm;
            EventUpdateLog                  += this.UpdateLog;
            EventShowCustomBallonTip        += this.ShowCustomBalloonTip;

            Publisher.EventCaptured         += this.ScreenCaptured;
            Publisher.EventDeleted          += this.ScreenDeleted;
            Publisher.EventError            += this.ScreenError;
            Publisher.EventHideBallonTip    += this.HideBalloonTip;
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        private void ScreenCaptured(object Sender, EventArgs e) {
            ShowTrayNotificationBalloon("снимок сделан", ToolTipIcon.Info);
        }

        private void ScreenDeleted(object Sender, EventArgs e)
        {
            ShowTrayNotificationBalloon("снимок удалён", ToolTipIcon.Warning);
        }

        private void ScreenError(object Sender, EventArgs e)
        {
            ShowTrayNotificationBalloon("возникла ошибка", ToolTipIcon.Error);
        }

        private void ShowCustomBalloonTip(object Sender, EventArgs e) {
            ShowTrayNotificationBalloon(CustomBallonTip, ToolTipIcon.None);
            FNotification.TextLine = CustomBallonTip;

        }

        private void ShowTrayNotificationBalloon(string s, ToolTipIcon tti) {
            notifyIcon1.ShowBalloonTip(0, "", $"{s}{DateTime.Now.ToString(" [HH:mm:ss]")}", tti);
            TextLine = CustomBallonTip;
        }

        private void MenuItem1_ClickExit(object Sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuItem1_ClickShow(object Sender, EventArgs e)
        {
            this.Show();
        }

        private void UpdateLog(object sender, System.EventArgs e)
        {
            this.txtKeys.Invoke((MethodInvoker)delegate
           {
               this.txtKeys.AppendText(
                   DateTime.Now.ToString("[HH:mm:ss]  ") +
                   TextLine +
                   Environment.NewLine);
           });
        }

        private void HideForm(object Sender, EventArgs e) {
            if (FormWindowState.Normal != this.WindowState)
                this.Hide();
        }

        private void HideBalloonTip(object Sender, EventArgs e) {
            this.notifyIcon1.Visible = false;
            this.notifyIcon1.Visible = true;
        }

        private void InitializeComponent()
        {
            this.txtKeys = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtKeys
            // 
            this.txtKeys.AcceptsReturn = true;
            this.txtKeys.AcceptsTab = true;
            this.txtKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeys.Location = new System.Drawing.Point(0, 0);
            this.txtKeys.Multiline = true;
            this.txtKeys.Name = "txtKeys";
            this.txtKeys.ReadOnly = true;
            this.txtKeys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKeys.Size = new System.Drawing.Size(641, 352);
            this.txtKeys.TabIndex = 0;
            this.txtKeys.TabStop = false;
            // 
            // FNotification
            // 
            this.ClientSize = new System.Drawing.Size(641, 352);
            this.Controls.Add(this.txtKeys);
            this.Name = "FNotification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
