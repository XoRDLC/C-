using System;
using System.Windows.Forms;

namespace ScreenShoter
{
    public partial class FShowBitmap : Form
    {
        public FShowBitmap()
        {
            InitializeComponent();

            picboxBitmap.SizeMode = PictureBoxSizeMode.StretchImage;
            picboxBitmap.Image = FStart.BM;
            this.btnSave_Click(null,null);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                FStart.BM.Save(SFD.FileName);
            }
        }
    }

    
}
