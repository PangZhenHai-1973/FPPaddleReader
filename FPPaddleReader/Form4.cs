using System;
using System.Drawing;
using System.Windows.Forms;

namespace FPPaddleReader
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public static int f4Flip = 0;

        private void 旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Image myImage = pictureBox1.Image;
                myImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pictureBox1.Image = myImage;
                if (f4Flip == 0)
                {
                    f4Flip = 1;
                }
                else
                {
                    f4Flip = 0;
                }
            }
        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            contextMenuStrip1.Font = this.Font;
        }
    }
}
