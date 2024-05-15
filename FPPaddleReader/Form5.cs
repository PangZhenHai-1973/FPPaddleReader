using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace FPPaddleReader
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Shown(object sender, EventArgs e)
        {
            if (Form1.MyDpi > 96)
            {
                dataGridView1.RowTemplate.Height = (int)(dataGridView1.RowTemplate.Height * Form1.MyFontScale);
                splitContainer1.SplitterDistance = (int)(splitContainer1.SplitterDistance * Form1.MyFontScale);
                double pd = panel1.Height / 2D;
                label1.Location = new Point(label1.Location.X, (int)(pd - label1.Height / 2D));
                textBox1.Location = new Point(textBox1.Location.X, (int)(pd - textBox1.Height / 2D));
                button1.Location = new Point(button1.Location.X, (int)(pd - button1.Height / 2D));
            }
            contextMenuStrip1.Font = this.Font;
            if (File.Exists(Form1.logFile))
            {
                openLogbl = false;
                dataGridView1.DataSource = null;
                using (StreamReader sr = new StreamReader(Form1.logFile, Encoding.Unicode))
                {
                    string s1 = "";
                    while ((s1 = sr.ReadLine()) != null)
                    {
                        string[] strTmp = s1.Split('\t');
                        if (strTmp.Length == 2)
                        {
                            if (strTmp[1] == "拍照")
                            {
                                FileInfo fi = new FileInfo(strTmp[0]);
                                object[] ob = new object[2];
                                ob[0] = fi.Name;
                                ob[1] = strTmp[0];
                                dataTable1.Rows.Add(ob);
                            }
                        }
                    }
                }
                dataGridView1.DataSource = dataSet1;
                openLogbl = true;
                dataGridView1.DataMember = dataTable1.TableName;
            }
            if (dataTable1.Rows.Count > 0)
            {
                textBox1.Text = Form1.logFile;
            }
            this.Opacity = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "日志文件(*.txt)|*.txt";
            ofd.Multiselect = false;
            ofd.InitialDirectory = Form1.scanLogsDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                openLogbl = false;
                dataTable1.Rows.Clear();
                pictureBox1.Image = null;
                dataGridView1.DataSource = null;
                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.Unicode))
                {
                    string s1 = "";
                    while ((s1 = sr.ReadLine()) != null)
                    {
                        string[] strTmp = s1.Split('\t');
                        if (strTmp.Length == 2)
                        {
                            if (strTmp[1] == "拍照")
                            {
                                FileInfo fi = new FileInfo(strTmp[0]);
                                object[] ob = new object[2];
                                ob[0] = fi.Name;
                                ob[1] = strTmp[0];
                                dataTable1.Rows.Add(ob);
                            }
                        }
                    }
                }
                if (dataTable1.Rows.Count > 0)
                {
                    textBox1.Text = ofd.FileName;
                }
                dataGridView1.DataSource = dataSet1;
                openLogbl = true;
                dataGridView1.DataMember = dataTable1.TableName;
            }
        }

        private bool openLogbl = true;
        private int f5Flip = 0;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (openLogbl)
            {
                int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                if (i1 > -1)
                {
                    string s1 = dataTable1.Rows[i1][1].ToString();
                    if (File.Exists(s1))
                    {
                        Bitmap bmpOrg = new Bitmap(s1);
                        if (bmpOrg.Width < bmpOrg.Height)
                        {
                            bmpOrg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        Point p;
                        Size s;
                        Bitmap bmpTmp = null;
                        double whDouble = (double)bmpOrg.Width / (double)bmpOrg.Height;
                        double yeqDouble = 12D / 7D;
                        double qyeDouble = 7D / 12D;
                        if (whDouble >= yeqDouble)
                        {
                            double newWidthDouble = (double)bmpOrg.Height * yeqDouble;
                            double widthDouble = ((double)bmpOrg.Width - newWidthDouble) / 2D;
                            p = new Point((int)widthDouble, 0);
                            s = new Size((int)newWidthDouble, bmpOrg.Height);
                            bmpTmp = bmpOrg.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                        }
                        else
                        {
                            double newHeightDouble = (double)bmpOrg.Width * qyeDouble;
                            double heightDouble = ((double)bmpOrg.Height - newHeightDouble) / 2D;
                            p = new Point(0, (int)heightDouble);
                            s = new Size(bmpOrg.Width, (int)newHeightDouble);
                            bmpTmp = bmpOrg.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                        }
                        if (f5Flip == 1)
                        {
                            bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        pictureBox1.Image = bmpTmp;
                    }
                }
            }
        }

        private void 旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Image myImage = pictureBox1.Image;
                myImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pictureBox1.Image = myImage;
                if (f5Flip == 0)
                {
                    f5Flip = 1;
                }
                else
                {
                    f5Flip = 0;
                }
            }
        }
    }
}
