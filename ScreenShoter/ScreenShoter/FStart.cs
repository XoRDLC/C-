using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenShoter
{
    public partial class FStart : Form
    {


        public static Bitmap BM = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        public FStart()
        {
            InitializeComponent();
        }


        public void ClickStart(object sender, EventArgs e) {
            this.btnStart_Click( sender,  e);
        }

        private void FStart_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(0, 0, 0, 0, BM.Size);

            FShowBitmap SI = new FShowBitmap();

            SI.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
