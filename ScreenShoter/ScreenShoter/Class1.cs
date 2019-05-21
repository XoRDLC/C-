using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?redirectedfrom=MSDN&view=netframework-4.8
//https://stackoverflow.com/questions/14273718/how-do-i-create-a-popup-notification-form
namespace ScreenShoter
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components;

        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }

        public Form1()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new ContextMenu();
            this.menuItem1 = new MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new MenuItem[] { this.menuItem1 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);

            // Set up how the form should be displayed.
            this.ClientSize = new Size(10, 10);
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

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);

        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(1, "captured", "s_c", new ToolTipIcon());
         }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
    }
}
