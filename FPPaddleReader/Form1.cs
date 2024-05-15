using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PaddleOCRSharp;
using PdfLibCore;
using PdfLibCore.Enums;
using WIA;
using System.Threading.Tasks;

namespace FPPaddleReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Font MyNewFont;
        public static string MyDataDirectory = "";
        public static string scannerDevice = "";
        private string scannedFiles = "";
        public static string scanLogsDirectory = "";
        private ConcurrentDictionary<string, int> dataCDCount = new ConcurrentDictionary<string, int>();
        private ArrayList FilesAL = new ArrayList();
        public static double MyFontScale = 1;
        public static float MyDpi = 96;
        private bool CPUbool = true;
        private static PaddleOCREngine OCREngine = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                OCREngine = new PaddleOCREngine(null, new OCRParameter());
            }
            catch (Exception ex)
            {
                CPUbool = false;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            label2.Text = "";
            label4.Text = "";
            using (Graphics gh = Graphics.FromHwnd(IntPtr.Zero))
            {
                MyDpi = gh.DpiX;
                if (MyDpi > 96)
                {
                    MyNewFont = new Font("Microsoft YaHei UI", 9F);
                    this.Font = MyNewFont;
                    MyFontScale = (this.FontHeight) / 16D;
                    dataGridView1.RowTemplate.Height = (int)(dataGridView1.RowTemplate.Height * MyFontScale);
                }
                else
                {
                    MyNewFont = new Font("SimSun", 9F);
                    数据ContextMenuStrip.Font = MyNewFont;
                    扫描设置ContextMenuStrip.Font = MyNewFont;
                    文件ContextMenuStrip.Font = MyNewFont;
                }
            }
            MyDataDirectory = Directory.GetCurrentDirectory();
            if (MyDataDirectory.Length > 3)
            {
                MyDataDirectory += "\\";
            }
            scanLogsDirectory = MyDataDirectory + "Logs\\";
            MyDataDirectory += "Data\\";
            if (!Directory.Exists(MyDataDirectory))
            {
                Directory.CreateDirectory(MyDataDirectory);
            }
            scannedFiles = MyDataDirectory + "ScannedFiles.dat";
            if (File.Exists(scannedFiles))
            {
                dataGridView1.DataSource = null;
                LoadScannedFiles(scannedFiles);
                dataGridView1.DataSource = dataSet1;
                dataGridView1.DataMember = dataTable1.TableName;
                label2.Text = dataTable1.Rows.Count.ToString();
            }
            string scannerDeviceSetup = MyDataDirectory + "ScannerDevice.ini";
            if (File.Exists(scannerDeviceSetup))
            {
                using (StreamReader sr = new StreamReader(scannerDeviceSetup, Encoding.Unicode))
                {
                    scannerDevice = sr.ReadToEnd().Trim();
                }
            }
        }

        private void LoadScannedFiles(string s1)
        {
            using (StreamReader sr = new StreamReader(s1, Encoding.Unicode))
            {
                string[] str1 = { "\r", "\n" };
                string[] str2 = { "\t" };
                string[] strTmp = sr.ReadToEnd().Split(str1, StringSplitOptions.RemoveEmptyEntries);
                int i1 = strTmp.Length;
                for (int x = 0; x < i1; x++)
                {
                    string[] strRead = strTmp[x].Split(str2, StringSplitOptions.None);
                    int i2 = strRead.Length;
                    if (i2 == 7)
                    {
                        object[] ob = new object[7];
                        ob[0] = strRead[0];
                        ob[1] = strRead[1];
                        ob[2] = strRead[2];
                        try
                        {
                            ob[3] = double.Parse(strRead[3]);
                        }
                        catch
                        { }
                        ob[4] = strRead[4];
                        ob[5] = strRead[5];
                        ob[6] = strRead[6];
                        if (dataCDCount.TryAdd(strRead[0] + strRead[1], 0))
                        {
                            dataTable1.Rows.Add(ob);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableButton();
            文件ContextMenuStrip.Show(button1, (int)(button1.Width / 2D), (int)(button1.Location.Y + button1.Height / 2D));
        }

        private void 拍照ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.Filter = "拍照(*.png;*.jpg)|*.png;*.jpg";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = null;
                button1.Enabled = false;
                DisableButton();
                button7.Enabled = false;
                button8.Enabled = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label2.Text = "";
                label4.Text = "";
                int i1 = OFD.FileNames.Length;
                progressBar1.Maximum = i1;
                progressBar1.Visible = true;
                FilesAL.Clear();
                for (int i = 0; i < i1; i++)
                {
                    FilesAL.Add(OFD.FileNames[i]);
                }
                using (BackgroundWorker PaiZhaoWorker = new BackgroundWorker())
                {
                    PaiZhaoWorker.RunWorkerCompleted += PaiZhaoWorker_RunWorkerCompleted;
                    PaiZhaoWorker.DoWork += PaiZhaoWorker_DoWork;
                    PaiZhaoWorker.RunWorkerAsync();
                }
            }
            else
            {
                this.TopMost = true;
            }
        }
        private void PaiZhaoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int fileCount = FilesAL.Count;
            object[][] ob = new object[fileCount][];
            for (int x = 0; x < fileCount; x++)
            {
                ob[x] = new object[7];
                ob[x][5] = "拍照";
                ob[x][6] = FilesAL[x].ToString();
                this.Invoke(new Action(delegate
                {
                    progressBar1.Value = x + 1;
                }));
                Bitmap bmpOrg = new Bitmap(FilesAL[x].ToString());
                int FPCode = 0;
                bool bl1 = true;
                if (bmpOrg.Width < bmpOrg.Height)
                {
                    bmpOrg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                Point p;
                Size s;
                Bitmap bmpTmp;
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
            AA: s = new Size((int)(bmpTmp.Width * 0.5), (int)(bmpTmp.Height * 0.5));
                Bitmap bmp1 = bmpTmp.Clone(new Rectangle(new Point(0, 0), s), PixelFormat.Format32bppArgb);
                OCRResult Re1 = OCREngine.DetectText(bmp1);
                if (Re1 != null)
                {
                    List<TextBlock> tb1 = Re1.TextBlocks;
                    int i1 = tb1.Count;
                    int y = 0;
                    for (; y < i1; y++)
                    {
                        string s1 = tb1[y].Text.Replace(" ", "");
                        if (s1.Length == 12)
                        {
                            try
                            {
                                long l = long.Parse(s1);
                                ob[x][0] = s1;
                                FPCode++;
                                break;
                            }
                            catch
                            { }
                        }
                        else if (s1.Length == 13)
                        {
                            s1 = s1.Substring(1, 12);
                            try
                            {
                                long l = long.Parse(s1);
                                ob[x][0] = s1;
                                FPCode++;
                                break;
                            }
                            catch
                            { }
                        }
                    }
                    if (ob[x][0] == null && bl1)
                    {
                        bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bl1 = false;
                        goto AA;
                    }
                    if (ob[x][0] != null)
                    {
                        int z = y;
                        for (; y < i1; y++)
                        {
                            string s1 = tb1[y].Text.Replace(" ", "");
                            if ((s1.Contains("校") && s1.Contains("码")) || s1.Contains("校验") || s1.Contains("验码"))
                            {
                                int xymInt = s1.Length;
                                int m = 0;
                                for (; m < xymInt; m++)
                                {
                                    try
                                    {
                                        int n = int.Parse(s1.Substring(m, 1));
                                        break;
                                    }
                                    catch
                                    { }
                                }
                                ob[x][4] = s1.Substring(m, xymInt - m);
                                break;
                            }
                        }
                        if (ob[x][4] == null)
                        {
                            y = z;
                        }
                        else
                        {
                            y++;
                        }
                        int i_nashuirenXHome = 0, i_nashuirenYHome = 0, i_kaihuahangXHome = 0, i_kaihuhangYHome = 0, i_kaihuahangXEnd = 0, i_kaihuahangYEnd = 0;
                        for (; y < i1; y++)
                        {
                            string s1 = tb1[y].Text.Replace(" ", "");
                            if (s1.Contains("纳税人") && i_nashuirenYHome == 0)
                            {
                                i_nashuirenXHome = tb1[y].BoxPoints[0].X;
                                i_nashuirenYHome = tb1[y].BoxPoints[0].Y;
                            }
                            else if (s1.Contains("开户行") && i_kaihuhangYHome == 0)
                            {
                                i_kaihuahangXHome = tb1[y].BoxPoints[0].X;
                                i_kaihuhangYHome = tb1[y].BoxPoints[0].Y;
                                i_kaihuahangXEnd = tb1[y].BoxPoints[1].X;
                                i_kaihuahangYEnd = tb1[y].BoxPoints[1].Y;
                                break;
                            }
                        }
                        if (i_nashuirenYHome > 0 && i_kaihuhangYHome > 0)
                        {
                            double d_zhengqie = (i_kaihuahangYEnd - i_kaihuhangYHome) / (double)(i_kaihuahangXEnd - i_kaihuahangXHome);
                            double d_bili = (i_kaihuhangYHome - i_nashuirenYHome) / 167D;
                            int pX = (int)(i_nashuirenXHome + d_bili * 1950);
                            int pY = (int)(i_nashuirenYHome - d_bili * 490 + d_bili * 1950 * d_zhengqie);
                            int sX = (int)(d_bili * 1230);
                            int sY = (int)(d_bili * 510);
                            if (pY < 0)
                            {
                                pY = 0;
                            }
                            if ((pX + sX) > bmpTmp.Width)
                            {
                                pX = bmpTmp.Width - sX;
                            }
                            p = new Point(pX, pY);
                            s = new Size(sX, sY);
                            Bitmap bmp2 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                            pX = (int)(i_nashuirenXHome + d_bili * 1850);
                            pY = (int)(i_nashuirenYHome + d_bili * 750 + d_bili * 1850 * d_zhengqie);
                            sX = (int)(d_bili * 1220);
                            sY = (int)(d_bili * 510);
                            if ((pY + sY) > bmpTmp.Height)
                            {
                                pY = bmpTmp.Height - sY;
                            }
                            if ((pX + sX) > bmpTmp.Width)
                            {
                                pX = bmpTmp.Width - sX;
                            }
                            p = new Point(pX, pY);
                            s = new Size(sX, sY);
                            Bitmap bmp3 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                            OCRResult Re2 = OCREngine.DetectText(bmp2);
                            OCRResult Re3 = OCREngine.DetectText(bmp3);
                            if (Re2 != null && Re3 != null)
                            {
                                List<TextBlock> tb2 = Re2.TextBlocks;
                                int i2 = tb2.Count - 1;
                                z = i2;
                                for (; i2 >= 0; i2--)
                                {
                                    string s1 = tb2[i2].Text.Replace(" ", "");
                                    if (s1.Length > 8)
                                    {
                                        if (s1.Contains("年") && s1.Contains("月"))
                                        {
                                            if (s1.Contains("日"))
                                            {
                                                int strLen = s1.Length;
                                                int m = 0;
                                                for (; m < strLen; m++)
                                                {
                                                    try
                                                    {
                                                        int n = int.Parse(s1.Substring(m, 1));
                                                        break;
                                                    }
                                                    catch
                                                    { }
                                                }
                                                if (strLen - m > 11)
                                                {
                                                    s1 = s1.Substring(m, 11);
                                                }
                                                else
                                                {
                                                    s1 = s1.Substring(m, strLen - m);
                                                }
                                                try
                                                {
                                                    ob[x][2] = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(s1));
                                                }
                                                catch
                                                { }
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (ob[x][2] == null)
                                {
                                    i2 = z;
                                }
                                else
                                {
                                    i2--;
                                }
                                for (; i2 >= 0; i2--)
                                {
                                    string s1 = tb2[i2].Text.Replace(" ", "");
                                    int NoInt = s1.Length;
                                    if (NoInt > 7)
                                    {
                                        if (NoInt == 8)
                                        {
                                            try
                                            {
                                                long l = long.Parse(s1);
                                                ob[x][1] = s1;
                                                FPCode++;
                                            }
                                            catch
                                            { }
                                        }
                                        else if (s1.Substring(0, 1) == "N")
                                        {
                                            if (ob[x][1] == null)
                                            {
                                                int m = 0;
                                                for (; m < NoInt; m++)
                                                {
                                                    try
                                                    {
                                                        int n = int.Parse(s1.Substring(m, 1));
                                                        break;
                                                    }
                                                    catch
                                                    { }
                                                }
                                                string NoStr = s1.Substring(m, NoInt - m);
                                                if (NoStr.Length <= 8)
                                                {
                                                    ob[x][1] = NoStr;
                                                }
                                                else
                                                {
                                                    ob[x][1] = NoStr.Substring(0, 8);
                                                }
                                                FPCode++;
                                            }
                                        }
                                    }
                                }
                                List<TextBlock> tb3 = Re3.TextBlocks;
                                int i3 = tb3.Count - 1;
                                int xiaoXieXS = 0;
                                int xiaoXieXE = 0;
                                for (; i3 >= 0; i3--)
                                {
                                    if (tb3[i3].Text.Replace(" ", "").Contains("小写"))
                                    {
                                        xiaoXieXS = tb3[i3].BoxPoints[0].X + (int)(d_bili * 160);
                                        xiaoXieXE = tb3[i3].BoxPoints[0].X + (int)(d_bili * 270);
                                        break;
                                    }
                                }
                                i3--;
                                bool bl3 = false;
                                for (; i3 >= 0; i3--)
                                {
                                    if (tb3[i3].BoxPoints[1].X > xiaoXieXS && tb3[i3].BoxPoints[1].X < xiaoXieXE)
                                    {
                                        string s3 = tb3[i3].Text.Replace(" ", "").Replace("，", ",");
                                        int jeInt = s3.Length;
                                        if (jeInt > 3)
                                        {
                                            s3 = s3.Substring(0, jeInt - 3) + "." + s3.Substring(jeInt - 2, 2);
                                            for (int m = 0; m < jeInt; m++)
                                            {
                                                try
                                                {
                                                    double d3 = double.Parse(s3.Substring(m, jeInt - m));
                                                    ob[x][3] = d3;
                                                    bl3 = true;
                                                    break;
                                                }
                                                catch
                                                { }
                                            }
                                        }
                                    }
                                    if (bl3)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.Invoke(new Action(delegate
            {
                if (!Directory.Exists(scanLogsDirectory))
                {
                    Directory.CreateDirectory(scanLogsDirectory);
                }
                ArrayList al = new ArrayList();
                for (int x = 0; x < fileCount; x++)
                {
                    if (ob[x][0] != null && ob[x][1] != null)
                    {
                        string s4 = ob[x][0].ToString() + ob[x][1].ToString();
                        if (dataCDCount.TryAdd(ob[x][0].ToString() + ob[x][1].ToString(), 0))
                        {
                            dataTable1.Rows.Add(ob[x]);
                        }
                        else
                        {
                            int searchIndex = -1;
                            int dtCount = dataTable1.Rows.Count;
                            Parallel.For(0, dtCount, (m, ParallelLoopState) =>
                            {
                                if (s4 == dataTable1.Rows[m][0].ToString() + dataTable1.Rows[m][1].ToString())
                                {
                                    searchIndex = m;
                                    ParallelLoopState.Break();
                                }
                            });
                            int z = 2;
                            bool updatebl = false;
                            for (; z < 5; z++)
                            {
                                if (dataTable1.Rows[searchIndex][z].ToString() == "" && ob[x][z] != null)
                                {
                                    dataTable1.Rows[searchIndex][z] = ob[x][z];
                                    updatebl = true;
                                }
                            }
                            if (updatebl)
                            {
                                dataTable1.Rows[searchIndex][5] = ob[x][5];
                                dataTable1.Rows[searchIndex][6] = ob[x][6];
                            }
                        }
                    }
                    else
                    {
                        al.Add(x);
                    }
                }
                SaveScannedFiles();
                int logCount = al.Count;
                if (logCount > 0)
                {
                    logFile = scanLogsDirectory + string.Format("{0:yyyy-MM-dd H.m.ss}", DateTime.Now) + ".txt";
                    using (StreamWriter sw = new StreamWriter(logFile, false, Encoding.Unicode))
                    {
                        for (int i = 0; i < logCount; i++)
                        {
                            sw.WriteLine(FilesAL[(int)al[i]].ToString() + "\t" + "拍照");
                        }
                    }
                    this.TopMost = false;
                    MessageBox.Show("发现有未识别的文件，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.TopMost = true;
                }
            }));
        }

        public static string logFile = "";

        private void PaiZhaoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            button1.Enabled = true;
            EnableButton();
            button7.Enabled = true;
            button8.Enabled = true;
            int i1 = dataTable1.Rows.Count;
            if (i1 > 0)
            {
                label2.Text = i1.ToString();
            }
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            dataGridView1.DataSource = dataSet1;
            dataGridView1.DataMember = dataTable1.TableName;
            this.TopMost = true;
        }

        private void 发票信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.Filter = "发票信息(*.png;*.jpg)|*.png;*.jpg";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = null;
                button1.Enabled = false;
                DisableButton();
                button7.Enabled = false;
                button8.Enabled = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label2.Text = "";
                label4.Text = "";
                int i1 = OFD.FileNames.Length;
                progressBar1.Maximum = i1;
                progressBar1.Visible = true;
                FilesAL.Clear();
                for (int i = 0; i < i1; i++)
                {
                    FilesAL.Add(OFD.FileNames[i]);
                }
                using (BackgroundWorker FaPiaoXinXiWorker = new BackgroundWorker())
                {
                    FaPiaoXinXiWorker.RunWorkerCompleted += FaPiaoXinXiWorker_RunWorkerCompleted;
                    FaPiaoXinXiWorker.DoWork += FaPiaoXinXiWorker_DoWork;
                    FaPiaoXinXiWorker.RunWorkerAsync();
                }
            }
            else
            {
                this.TopMost = true;
            }
        }

        private void FaPiaoXinXiWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int fileCount = FilesAL.Count;
            object[][] ob = new object[fileCount][];
            for (int x = 0; x < fileCount; x++)
            {
                ob[x] = new object[7];
                ob[x][5] = "发票信息";
                ob[x][6] = FilesAL[x].ToString();
                this.Invoke(new Action(delegate
                {
                    progressBar1.Value = x + 1;
                }));
                Bitmap bmpTmp = new Bitmap(FilesAL[x].ToString());
                Point p = new Point(0, (int)(bmpTmp.Height * 0.2));
                Size s = new Size(bmpTmp.Width, (int)(bmpTmp.Height * 0.4));
                Bitmap b1 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                OCRResult Re = OCREngine.DetectText(b1);
                if (Re != null)
                {
                    List<TextBlock> tb = Re.TextBlocks;
                    int i2 = tb.Count;
                    for (int y = 0; y < i2; y++)
                    {
                        string s1 = tb[y].Text.Replace(" ", "").Trim();
                        try
                        {
                            if (s1 == "发票代码")
                            {
                                y++;
                                ob[x][0] = tb[y].Text.Trim();
                            }
                            else if (s1 == "发票号码")
                            {
                                y++;
                                ob[x][1] = tb[y].Text.Trim();
                            }
                            else if (s1 == "开票日期")
                            {
                                y++;
                                ob[x][2] = tb[y].Text.Replace(".", "-").Trim();
                            }
                            else if (s1 == "合计金额")
                            {
                                y++;
                                string s3 = tb[y].Text.Replace("不含税", "").Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace(" ", "").Trim();
                                ob[x][3] = double.Parse(s3.Substring(1, s3.Length - 1).TrimStart());
                            }
                            else if (s1 == "发票校验码")
                            {
                                y++;
                                ob[x][4] = tb[y].Text.Trim();
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }
            this.Invoke(new Action(delegate
            {
                if (!Directory.Exists(scanLogsDirectory))
                {
                    Directory.CreateDirectory(scanLogsDirectory);
                }
                ArrayList al = new ArrayList();
                for (int x = 0; x < fileCount; x++)
                {
                    if (ob[x][0] != null && ob[x][1] != null)
                    {
                        string s4 = ob[x][0].ToString() + ob[x][1].ToString();
                        if (dataCDCount.TryAdd(ob[x][0].ToString() + ob[x][1].ToString(), 0))
                        {
                            dataTable1.Rows.Add(ob[x]);
                        }
                        else
                        {
                            int searchIndex = -1;
                            int dtCount = dataTable1.Rows.Count;
                            Parallel.For(0, dtCount, (m, ParallelLoopState) =>
                            {
                                if (s4 == dataTable1.Rows[m][0].ToString() + dataTable1.Rows[m][1].ToString())
                                {
                                    searchIndex = m;
                                    ParallelLoopState.Break();
                                }
                            });
                            int z = 2;
                            bool updatebl = false;
                            for (; z < 5; z++)
                            {
                                if (dataTable1.Rows[searchIndex][z].ToString() == "" && ob[x][z] != null)
                                {
                                    dataTable1.Rows[searchIndex][z] = ob[x][z];
                                    updatebl = true;
                                }
                            }
                            if (updatebl)
                            {
                                dataTable1.Rows[searchIndex][5] = ob[x][5];
                                dataTable1.Rows[searchIndex][6] = ob[x][6];
                            }
                        }
                    }
                    else
                    {
                        al.Add(x);
                    }
                }
                SaveScannedFiles();
                int logCount = al.Count;
                if (logCount > 0)
                {
                    logFile = scanLogsDirectory + string.Format("{0:yyyy-MM-dd H.m.ss}", DateTime.Now) + ".txt";
                    using (StreamWriter sw = new StreamWriter(logFile, false, Encoding.Unicode))
                    {
                        for (int i = 0; i < logCount; i++)
                        {
                            sw.WriteLine(FilesAL[(int)al[i]].ToString() + "\t" + "发票信息");
                        }
                    }
                    this.TopMost = false;
                    MessageBox.Show("发现有未识别的文件，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.TopMost = true;
                }
            }));
        }

        private void FaPiaoXinXiWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            button1.Enabled = true;
            EnableButton();
            button7.Enabled = true;
            button8.Enabled = true;
            int i1 = dataTable1.Rows.Count;
            if (i1 > 0)
            {
                label2.Text = i1.ToString();
            }
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            dataGridView1.DataSource = dataSet1;
            dataGridView1.DataMember = dataTable1.TableName;
            this.TopMost = true;
        }

        private void 电子发票ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.Filter = "电子发票(*.pdf)|*.pdf";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = null;
                button1.Enabled = false;
                DisableButton();
                button7.Enabled = false;
                button8.Enabled = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label2.Text = "";
                label4.Text = "";
                int i1 = OFD.FileNames.Length;
                progressBar1.Maximum = i1;
                progressBar1.Visible = true;
                FilesAL.Clear();
                for (int i = 0; i < i1; i++)
                {
                    FilesAL.Add(OFD.FileNames[i]);
                }
                using (BackgroundWorker DianZiFaPiaoWorker = new BackgroundWorker())
                {
                    DianZiFaPiaoWorker.RunWorkerCompleted += DianZiFaPiaoWorker_RunWorkerCompleted;
                    DianZiFaPiaoWorker.DoWork += DianZiFaPiaoWorker_DoWork;
                    DianZiFaPiaoWorker.RunWorkerAsync();
                }
            }
            else
            {
                this.TopMost = true;
            }
        }

        private void DianZiFaPiaoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int fileCount = FilesAL.Count;
            object[][] ob = new object[fileCount][];
            double dpiX = 300D;
            double dpiY = 300D;
            for (int x = 0; x < fileCount; x++)
            {
                ob[x] = new object[7];
                ob[x][5] = "电子发票";
                ob[x][6] = FilesAL[x].ToString();
                this.Invoke(new Action(delegate
                {
                    progressBar1.Value = x + 1;
                }));
                using (FileStream FS = File.Open(FilesAL[x].ToString(), FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (PdfDocument doc = new PdfDocument(FS))
                    {
                        if (doc.Pages.Count > 0)
                        {
                            PdfPage p1 = doc.Pages[0];
                            int pageWidth = (int)(dpiX * p1.Size.Width / 72);
                            int pageHeight = (int)(dpiY * p1.Size.Height / 72);
                            using (PdfiumBitmap pb = new PdfiumBitmap(pageWidth, pageHeight, true))
                            {
                                p1.Render(pb, PageOrientations.Normal, RenderingFlags.LcdText);
                                using (Stream bs = pb.AsBmpStream(dpiX, dpiY))
                                {
                                    pageHeight = (int)(pageWidth * 0.6463);
                                    Point pl = new Point((int)(pageWidth * 0.682), 0);
                                    Size si = new Size((int)(pageWidth * 0.318), (int)(pageHeight * 0.228));
                                    Bitmap bmp1 = new Bitmap(bs).Clone(new Rectangle(pl, si), PixelFormat.Format32bppArgb);
                                    OCRResult Re1 = OCREngine.DetectText(bmp1);
                                    pl = new Point((int)(pageWidth * 0.638), (int)(pageHeight * 0.58));
                                    si = new Size((int)(pageWidth * 0.36), (int)(pageHeight * 0.25));
                                    Bitmap bmp2 = new Bitmap(bs).Clone(new Rectangle(pl, si), PixelFormat.Format32bppArgb);
                                    OCRResult Re2 = OCREngine.DetectText(bmp2);
                                    if (Re1 != null)
                                    {
                                        List<TextBlock> tb = Re1.TextBlocks;
                                        int i2 = tb.Count;
                                        string[] str = { ":", "：" };
                                        for (int y = 0; y < i2; y++)
                                        {
                                            string s2 = tb[y].Text.Replace(" ", "");
                                            string[] strTmp = s2.Split(str, StringSplitOptions.RemoveEmptyEntries);
                                            try
                                            {
                                                if (strTmp[0] == "发票代码")
                                                {
                                                    if (strTmp.Length > 1)
                                                    {
                                                        ob[x][0] = strTmp[1].Trim();
                                                    }
                                                }
                                                else if (strTmp[0] == "发票号码")
                                                {
                                                    if (strTmp.Length > 1)
                                                    {
                                                        ob[x][1] = strTmp[1].Trim();
                                                    }
                                                }
                                                else if (strTmp[0] == "开票日期")
                                                {
                                                    if (strTmp.Length > 1)
                                                    {
                                                        ob[x][2] = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(strTmp[1].Trim()));
                                                    }
                                                }
                                                else if (strTmp[0] == "校验码")
                                                {
                                                    if (strTmp.Length > 1)
                                                    {
                                                        ob[x][4] = strTmp[1].Trim();
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                    if (Re2 != null)
                                    {
                                        int b2w = (int)(bmp2.Width * 0.46D);
                                        List<TextBlock> tb = Re2.TextBlocks;
                                        int i2 = tb.Count - 1;
                                        for (; i2 >= 0; i2--)
                                        {
                                            string s2 = tb[i2].Text.Replace(" ", "");
                                            if (s2.Contains("小写"))
                                            {
                                                break;
                                            }
                                        }
                                        i2--;
                                        for (; i2 >= 0; i2--)
                                        {
                                            if (tb[i2].BoxPoints[1].X < b2w)
                                            {
                                                string bhs = tb[i2].Text.Replace(" ", "");
                                                ob[x][3] = double.Parse(bhs.Substring(1, bhs.Length - 1));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.Invoke(new Action(delegate
            {
                if (!Directory.Exists(scanLogsDirectory))
                {
                    Directory.CreateDirectory(scanLogsDirectory);
                }
                ArrayList al = new ArrayList();
                for (int x = 0; x < fileCount; x++)
                {
                    if (ob[x][0] != null && ob[x][1] != null)
                    {
                        string s4 = ob[x][0].ToString() + ob[x][1].ToString();
                        if (dataCDCount.TryAdd(ob[x][0].ToString() + ob[x][1].ToString(), 0))
                        {
                            dataTable1.Rows.Add(ob[x]);
                        }
                        else
                        {
                            int searchIndex = -1;
                            int dtCount = dataTable1.Rows.Count;
                            Parallel.For(0, dtCount, (m, ParallelLoopState) =>
                            {
                                if (s4 == dataTable1.Rows[m][0].ToString() + dataTable1.Rows[m][1].ToString())
                                {
                                    searchIndex = m;
                                    ParallelLoopState.Break();
                                }
                            });
                            int z = 2;
                            bool updatebl = false;
                            for (; z < 5; z++)
                            {
                                if (dataTable1.Rows[searchIndex][z].ToString() == "" && ob[x][z] != null)
                                {
                                    dataTable1.Rows[searchIndex][z] = ob[x][z];
                                    updatebl = true;
                                }
                            }
                            if (updatebl)
                            {
                                dataTable1.Rows[searchIndex][5] = ob[x][5];
                                dataTable1.Rows[searchIndex][6] = ob[x][6];
                            }
                        }
                    }
                    else
                    {
                        al.Add(x);
                    }
                }
                SaveScannedFiles();
                int logCount = al.Count;
                if (logCount > 0)
                {
                    logFile = scanLogsDirectory + string.Format("{0:yyyy-MM-dd H.m.ss}", DateTime.Now) + ".txt";
                    using (StreamWriter sw = new StreamWriter(logFile, false, Encoding.Unicode))
                    {
                        for (int i = 0; i < logCount; i++)
                        {
                            sw.WriteLine(FilesAL[(int)al[i]].ToString() + "\t" + "电子发票");
                        }
                    }
                    this.TopMost = false;
                    MessageBox.Show("发现有未识别的文件，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.TopMost = true;
                }
            }));
        }

        private void DianZiFaPiaoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            button1.Enabled = true;
            EnableButton();
            button7.Enabled = true;
            button8.Enabled = true;
            int i1 = dataTable1.Rows.Count;
            if (i1 > 0)
            {
                label2.Text = i1.ToString();
            }
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            dataGridView1.DataSource = dataSet1;
            dataGridView1.DataMember = dataTable1.TableName;
            this.TopMost = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            if (scannerDevice == "")
            {
                MessageBox.Show("没有找到扫描仪。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DeviceManager dm = new DeviceManager();
                bool bl = false;
                int i1 = dm.DeviceInfos.Count;
                int i = 1;
                for (; i <= i1; i++)
                {
                    if (dm.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                    {
                        if (dm.DeviceInfos[i].Properties["Name"].get_Value().ToString() == scannerDevice)
                        {
                            bl = true;
                            break;
                        }
                    }
                }
                if (bl)
                {
                    button1.Enabled = false;
                    DisableButton();
                    button7.Enabled = false;
                    button8.Enabled = false;
                    var scannerItem = dm.DeviceInfos[i].Connect().Items[1];
                    foreach (Property p in scannerItem.Properties)
                    {
                        switch (p.PropertyID)
                        {
                            case 6147:
                                p.set_Value(300);
                                break;
                            case 6148:
                                p.set_Value(300);
                                break;
                        }
                    }
                    CommonDialogClass dlg = new CommonDialogClass();
                    object scanResult = null;
                    try
                    {
                        scanResult = dlg.ShowTransfer(scannerItem, FormatID.wiaFormatBMP, true);
                    }
                    catch (COMException ex)
                    {
                        uint errorCode = (uint)ex.ErrorCode;
                        if (errorCode == 0x80210001)
                        {
                            MessageBox.Show("设备发生未知错误。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210002)
                        {
                            MessageBox.Show("纸张被堵塞在扫描仪的文档馈送器中。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210003)
                        {
                            MessageBox.Show("文档馈送器中没有文档。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210004)
                        {
                            MessageBox.Show("扫描程序的文档馈送器出现了未指定的问题。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210005)
                        {
                            MessageBox.Show("设备处于脱机状态。确保设备已打开并连接到电脑。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210006)
                        {
                            MessageBox.Show("设备正忙。关闭使用此设备或等待它完成的任何应用，然后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210007)
                        {
                            MessageBox.Show("设备正在预热。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210008)
                        {
                            MessageBox.Show("设备出现问题。确保设备已打开、联机，并正确连接任何电缆。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210009)
                        {
                            MessageBox.Show("设备已删除，已不可用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000A)
                        {
                            MessageBox.Show("与设备的通信失败，确保设备已打开并连接到电脑。如果问题仍然存在，请断开连接并重新连接设备。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000B)
                        {
                            MessageBox.Show("设备不支持此命令。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000C)
                        {
                            MessageBox.Show("设备上的设置不正确。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000D)
                        {
                            MessageBox.Show("设备已锁定。关闭使用此设备或等待它完成的任何应用，然后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000E)
                        {
                            MessageBox.Show("设备驱动程序引发异常。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x8021000F)
                        {
                            MessageBox.Show("驱动程序的响应无效。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210015)
                        {
                            MessageBox.Show("未找到扫描程序设备。确保设备处于联机状态，连接到电脑，并在电脑上安装了正确的驱动程序。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210016)
                        {
                            MessageBox.Show("一个或多个设备的封面处于打开状态。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210017)
                        {
                            MessageBox.Show("扫描仪的灯已关闭。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210020)
                        {
                            MessageBox.Show("由于多页源条件，出现了扫描错误。此功能适用于Windows 8和更高版本的Windows。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (errorCode == 0x80210021)
                        {
                            MessageBox.Show("扫描作业中断，因为 Imprinter/Endorser 项达到 WIA_IPS_PRINTER_ENDORSER_COUNTER 的最大有效值，并且已重置为 0。此功能适用于Windows 8和更高版本的Windows。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (scanResult != null)
                    {
                        ImageFile image = (ImageFile)scanResult;
                        Bitmap bmp = new Bitmap(new MemoryStream((byte[])image.FileData.get_BinaryData()));
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        Point p = new Point((int)(bmp.Width * 0.2), 0);
                        Size s = new Size((int)(bmp.Width - bmp.Width * 0.2), (int)(bmp.Height - bmp.Height * 0.35));
                        Bitmap bmpTmp = bmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppRgb);
                        p = new Point((int)(bmpTmp.Width * 0.12), 0);
                        s = new Size((int)(bmpTmp.Width * 0.35), (int)(bmpTmp.Height * 0.3));
                        Bitmap bmp1 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                        p = new Point((int)(bmpTmp.Width * 0.55), 0);
                        s = new Size((int)(bmpTmp.Width * 0.45), (int)(bmpTmp.Height * 0.3));
                        Bitmap bmp2 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                        p = new Point((int)(bmpTmp.Width * 0.59), (int)(bmpTmp.Height * 0.58));
                        s = new Size((int)(bmpTmp.Width * 0.32), (int)(bmpTmp.Height * 0.31));
                        Bitmap bmp3 = bmpTmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppArgb);
                        OCRResult Re1 = OCREngine.DetectText(bmp1);
                        OCRResult Re2 = OCREngine.DetectText(bmp2);
                        OCRResult Re3 = OCREngine.DetectText(bmp3);
                        if (Re1 != null && Re2 != null && Re3 != null)
                        {
                            List<TextBlock> tb1 = Re1.TextBlocks;
                            object[] ob = new object[7];
                            i1 = tb1.Count;
                            for (int y = 0; y < i1; y++)
                            {
                                string s1 = tb1[y].Text.Replace(" ", "");
                                if (s1 != "")
                                {
                                    if (s1.Length == 12)
                                    {
                                        try
                                        {
                                            long l = long.Parse(s1);
                                            ob[0] = s1;
                                        }
                                        catch
                                        { }
                                    }
                                    else if (s1.Length == 13)
                                    {
                                        try
                                        {
                                            s1 = s1.Substring(1, 12);
                                            long l = long.Parse(s1);
                                            ob[0] = s1;
                                        }
                                        catch
                                        { }
                                    }
                                    else if ((s1.Contains("校") && s1.Contains("码")) || s1.Contains("校验") || s1.Contains("验码"))
                                    {
                                        int xymInt = s1.Length;
                                        int m = 0;
                                        for (; m < xymInt; m++)
                                        {
                                            try
                                            {
                                                int n = int.Parse(s1.Substring(m, 1));
                                                break;
                                            }
                                            catch
                                            { }
                                        }
                                        ob[4] = s1.Substring(m, xymInt - m);
                                        break;
                                    }
                                }
                            }
                            List<TextBlock> tb2 = Re2.TextBlocks;
                            int i2 = tb2.Count - 1;
                            for (; i2 >= 0; i2--)
                            {
                                string s1 = tb2[i2].Text.Replace(" ", "");
                                if (s1.Length > 8)
                                {
                                    if (s1.Contains("年") && s1.Contains("月"))
                                    {
                                        if (s1.Contains("日"))
                                        {
                                            int strLen = s1.Length;
                                            int m = 0;
                                            for (; m < strLen; m++)
                                            {
                                                try
                                                {
                                                    int n = int.Parse(s1.Substring(m, 1));
                                                    break;
                                                }
                                                catch
                                                { }
                                            }
                                            if (strLen - m > 11)
                                            {
                                                s1 = s1.Substring(m, 11);
                                            }
                                            else
                                            {
                                                s1 = s1.Substring(m, strLen - m);
                                            }
                                            try
                                            {
                                                ob[2] = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(s1));
                                            }
                                            catch
                                            { }
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            i2--;
                            for (; i2 >= 0; i2--)
                            {
                                string s1 = tb2[i2].Text.Replace(" ", "");
                                int NoInt = s1.Length;
                                if (NoInt > 7)
                                {
                                    if (NoInt == 8)
                                    {
                                        try
                                        {
                                            long l = long.Parse(s1);
                                            ob[1] = s1;
                                        }
                                        catch
                                        { }
                                    }
                                    else if (s1.Substring(0, 1) == "N")
                                    {
                                        if (ob[1] == null)
                                        {
                                            int m = 0;
                                            for (; m < NoInt; m++)
                                            {
                                                try
                                                {
                                                    int n = int.Parse(s1.Substring(m, 1));
                                                    break;
                                                }
                                                catch
                                                { }
                                            }
                                            string NoStr = s1.Substring(m, NoInt - m);
                                            if (NoStr.Length <= 8)
                                            {
                                                ob[1] = NoStr;
                                            }
                                            else
                                            {
                                                ob[1] = NoStr.Substring(0, 8);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            List<TextBlock> tb3 = Re3.TextBlocks;
                            int i3 = tb3.Count - 1;
                            int xiaoXieX = 0;
                            for (; i3 >= 0; i3--)
                            {
                                if (tb3[i3].Text.Replace(" ", "").Contains("小写"))
                                {
                                    xiaoXieX = tb3[i3].BoxPoints[1].X;
                                    break;
                                }
                            }
                            i3--;
                            bool bl3 = false;
                            double b2w = bmp3.Width * 0.6D;
                            for (; i3 >= 0; i3--)
                            {
                                if (tb3[i3].BoxPoints[1].X < b2w && xiaoXieX > tb3[i3].BoxPoints[0].X)
                                {
                                    string s3 = tb3[i3].Text.Replace(" ", "").Replace("，", ",");
                                    int jeInt = s3.Length;
                                    if (jeInt > 3)
                                    {
                                        s3 = s3.Substring(0, jeInt - 3) + "." + s3.Substring(jeInt - 2, 2);
                                        for (int m = 0; m < jeInt; m++)
                                        {
                                            try
                                            {
                                                double d3 = double.Parse(s3.Substring(m, jeInt - m));
                                                ob[3] = d3;
                                                bl3 = true;
                                                break;
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                }
                                if (bl3)
                                {
                                    break;
                                }
                            }
                            if (ob[0] != null && ob[1] != null)
                            {
                                string s4 = ob[0].ToString() + ob[1].ToString();
                                if (dataCDCount.TryAdd(s4, 0))
                                {
                                    dataTable1.Rows.Add(ob);
                                    SaveScannedFiles();
                                }
                                else
                                {
                                    int searchIndex = -1;
                                    int dtCount = dataTable1.Rows.Count;
                                    Parallel.For(0, dtCount, (x, ParallelLoopState) =>
                                    {
                                        if (s4 == dataTable1.Rows[x][0].ToString() + dataTable1.Rows[x][1].ToString())
                                        {
                                            searchIndex = x;
                                            ParallelLoopState.Break();
                                        }
                                    });
                                    int z = 2;
                                    bool updatebl = false;
                                    for (; z < 5; z++)
                                    {
                                        if (dataTable1.Rows[searchIndex][z].ToString() == "" && ob[z] != null)
                                        {
                                            dataTable1.Rows[searchIndex][z] = ob[z];
                                            updatebl = true;
                                        }
                                    }
                                    if (updatebl)
                                    {
                                        dataTable1.Rows[searchIndex][5] = "";
                                        dataTable1.Rows[searchIndex][6] = "";
                                        SaveScannedFiles();
                                    }
                                }
                            }
                        }
                    }
                    button1.Enabled = true;
                    EnableButton();
                    button7.Enabled = true;
                    button8.Enabled = true;
                }
                else
                {
                    MessageBox.Show("没有找到设定的扫描仪。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.TopMost = true;
        }

        private void SaveScannedFiles()
        {
            if (!Directory.Exists(MyDataDirectory))
            {
                Directory.CreateDirectory(MyDataDirectory);
            }
            using (StreamWriter sw = new StreamWriter(scannedFiles, false, Encoding.Unicode))
            {
                int i1 = dataTable1.Rows.Count;
                for (int i = 0; i < i1; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dataTable1.Rows[i][0].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][1].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][2].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][3].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][4].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][5].ToString() + "\t");
                    sb.Append(dataTable1.Rows[i][6].ToString());
                    sw.WriteLine(sb.ToString());
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataTable1.Rows.Count > 0)
            {
                int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                int i2 = i1 + 1;
                label4.Text = i2.ToString();
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    if (f4.IsDisposed == false)
                    {
                        string s1 = dataTable1.Rows[i1][6].ToString();
                        if (File.Exists(s1))
                        {
                            if (dataTable1.Rows[i1][5].ToString() == "拍照")
                            {
                                f4.Text = "查看 - " + s1;
                                Bitmap bmpOrg = new Bitmap(s1);
                                if (bmpOrg.Width < bmpOrg.Height)
                                {
                                    bmpOrg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                }
                                Point p;
                                Size s;
                                Bitmap bmpTmp;
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
                                if (Form4.f4Flip == 1)
                                {
                                    bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }
                                f4.pictureBox1.Image = bmpTmp;
                            }
                        }
                        else
                        {
                            f4.Text = "查看";
                            f4.pictureBox1.Image = null;
                        }
                    }
                }
            }
            else
            {
                label4.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i1 = dataTable1.Rows.Count;
            if (i1 > 0)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    int i2 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected) + 1;
                    if (i1 == i2)
                    {
                        DisableButton();
                        try
                        {
                            Clipboard.Clear();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        dataGridView1.CurrentCell = dataGridView1[0, i2];
                    }
                }
            }
        }

        private void EnableButton()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void DisableButton()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (i1 >= 0)
            {
                try
                {
                    Clipboard.SetText(dataTable1.Rows[i1][0].ToString());
                }
                catch
                {
                    DisableButton();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (i1 >= 0)
            {
                try
                {
                    Clipboard.SetText(dataTable1.Rows[i1][1].ToString());
                }
                catch
                {
                    DisableButton();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (i1 >= 0)
            {
                try
                {
                    Clipboard.SetText(dataTable1.Rows[i1][2].ToString());
                }
                catch
                {
                    DisableButton();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (i1 >= 0)
            {
                try
                {
                    Clipboard.SetText(dataTable1.Rows[i1][3].ToString());
                }
                catch
                {
                    DisableButton();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (i1 >= 0)
            {
                try
                {
                    Clipboard.SetText(dataTable1.Rows[i1][4].ToString());
                }
                catch
                {
                    DisableButton();
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i1 = dataTable1.Rows.Count;
            if (i1 > 0)
            {
                if (e.ColumnIndex == 3)
                {
                    if (dataGridView1.Columns[3].HeaderCell.SortGlyphDirection == SortOrder.None || dataGridView1.Columns[3].HeaderCell.SortGlyphDirection == SortOrder.Descending)
                    {
                        dataGridView1.DataSource = null;
                        DataView DV = dataTable1.DefaultView;
                        DV.Sort = "不含税金额 ASC";
                        DataTable dTmp = DV.ToTable();
                        dataTable1.Rows.Clear();
                        for (int i = 0; i < i1; i++)
                        {
                            dataTable1.Rows.Add(dTmp.Rows[i].ItemArray);
                        }
                        dataGridView1.DataSource = dataSet1;
                        dataGridView1.DataMember = dataTable1.TableName;
                        dataGridView1.Columns[3].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        DataView DV = dataTable1.DefaultView;
                        DV.Sort = "不含税金额 DESC";
                        DataTable dTmp = DV.ToTable();
                        dataTable1.Rows.Clear();
                        for (int i = 0; i < i1; i++)
                        {
                            dataTable1.Rows.Add(dTmp.Rows[i].ItemArray);
                        }
                        dataGridView1.DataSource = dataSet1;
                        dataGridView1.DataMember = dataTable1.TableName;
                        dataGridView1.Columns[3].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    }
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (dataTable1.Rows.Count > 0)
            {
                删除ToolStripMenuItem.Enabled = true;
                删除已检查ToolStripMenuItem.Enabled = true;
                导出数据ToolStripMenuItem.Enabled = true;
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    编辑ToolStripMenuItem.Enabled = true;
                    查看文件ToolStripMenuItem.Enabled = true;
                }
                else
                {
                    编辑ToolStripMenuItem.Enabled = false;
                    查看文件ToolStripMenuItem.Enabled = false;
                }
                检查ToolStripMenuItem.Enabled = true;
            }
            else
            {
                删除ToolStripMenuItem.Enabled = false;
                删除已检查ToolStripMenuItem.Enabled = false;
                导出数据ToolStripMenuItem.Enabled = false;
                编辑ToolStripMenuItem.Enabled = false;
                查看文件ToolStripMenuItem.Enabled = false;
                检查ToolStripMenuItem.Enabled = false;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            if (MessageBox.Show("确定要删除选定的数据吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                ArrayList al = new ArrayList();
                int intFirstRow = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                int intLastRow = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Selected);
                for (; intFirstRow <= intLastRow; intFirstRow++)
                {
                    if (((int)dataGridView1.Rows.GetRowState(intFirstRow) & 32) == 32)
                    {
                        al.Add(intFirstRow);
                    }
                }
                al.Sort();
                int i1 = al.Count - 1;
                for (; i1 >= 0; i1--)
                {
                    int i = (int)al[i1];
                    dataCDCount.TryRemove(dataTable1.Rows[i][0].ToString() + dataTable1.Rows[i][1].ToString(), out int x);
                    dataTable1.Rows.RemoveAt(i);
                }
                dataTable1.AcceptChanges();
                label2.Text = dataTable1.Rows.Count.ToString();
                SaveScannedFiles();
            }
            this.TopMost = true;
        }

        private void 删除已检查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            int i1 = dataTable1.Rows.Count - 1;
            for (; i1 >= 0; i1--)
            {
                if (dataGridView1[1, i1].Style.BackColor == System.Drawing.Color.Aquamarine)
                {
                    dataCDCount.TryRemove(dataTable1.Rows[i1][0].ToString() + dataTable1.Rows[i1][1].ToString(), out int x);
                    dataTable1.Rows.RemoveAt(i1);
                }
            }
            dataTable1.AcceptChanges();
            label2.Text = dataTable1.Rows.Count.ToString();
            SaveScannedFiles();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            this.Hide();
            Form2 f2 = new Form2();
            f2.Font = MyNewFont;
            f2.ShowDialog();
            this.Show();
            this.TopMost = true;
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = ("数据文件(*.dat)|*.dat");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = null;
                int i2 = ofd.FileNames.Length;
                for (int i = 0; i < i2; i++)
                {
                    string s1 = ofd.FileNames[i];
                    if (s1 != scannedFiles)
                    {
                        LoadScannedFiles(s1);
                    }
                }
                SaveScannedFiles();
                dataGridView1.DataSource = dataSet1;
                dataGridView1.DataMember = dataTable1.TableName;
                label2.Text = dataTable1.Rows.Count.ToString();
            }
            this.TopMost = true;
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "发票数据文件";
            sfd.Filter = ("数据文件(*.dat)|*.dat");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string s1 = sfd.FileName;
                if (s1 != scannedFiles)
                {
                    try
                    {
                        File.Copy(scannedFiles, s1, true);
                    }
                    catch
                    {
                        MessageBox.Show("导出失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            this.TopMost = true;
        }

        public static object[] EditFPData;

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            dataCDCount.Clear();
            int i2 = dataTable1.Rows.Count;
            Parallel.For(0, i2, (int i) =>
            {
                dataCDCount.TryAdd(dataTable1.Rows[i][0].ToString() + dataTable1.Rows[i][1].ToString(), i);
            });
            this.TopMost = false;
            this.Hide();
            Form3 f3 = new Form3();
            string s1 = dataTable1.Rows[i1][0].ToString() + dataTable1.Rows[i1][1].ToString();
            EditFPData = dataTable1.Rows[i1].ItemArray;
            f3.Font = MyNewFont;
            f3.ShowDialog();
            string s2 = EditFPData[0].ToString() + EditFPData[1].ToString();
            if (s1 == s2)
            {
                dataTable1.Rows[i1].ItemArray = EditFPData;
                SaveScannedFiles();
            }
            else
            {
                if (dataCDCount.TryGetValue(s2, out int m))
                {
                    MessageBox.Show("发票代码和发票号码已存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.CurrentCell = dataGridView1[0, m];
                    dataGridView1.CurrentCell.Selected = true;
                }
                else
                {
                    dataCDCount.TryRemove(s1, out m);
                    dataCDCount.TryAdd(s2, i1);
                    dataTable1.Rows[i1].ItemArray = EditFPData;
                    SaveScannedFiles();
                    if (dataTable1.Rows[i1][0].ToString().Length == 12)
                    {
                        dataGridView1[0, i1].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    }
                    else
                    {
                        dataGridView1[0, i1].Style.BackColor = System.Drawing.Color.Orchid;
                    }
                }
            }
            this.Show();
            this.TopMost = true;
        }

        Form4 f4 = new Form4();

        private void 查看文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i1 = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            string s1 = dataTable1.Rows[i1][6].ToString();
            if (File.Exists(s1))
            {
                if (dataTable1.Rows[i1][5].ToString() == "拍照")
                {
                    if (f4.IsDisposed)
                    {
                        f4 = new Form4();
                    }
                    f4.Font = MyNewFont;
                    f4.Text = "查看 - " + s1;
                    Bitmap bmpOrg = new Bitmap(s1);
                    if (bmpOrg.Width < bmpOrg.Height)
                    {
                        bmpOrg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    Point p;
                    Size s;
                    Bitmap bmpTmp;
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
                    if (Form4.f4Flip == 1)
                    {
                        bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    f4.pictureBox1.Image = bmpTmp;
                    if (f4.Visible == false)
                    {
                        f4.Visible = true;
                    }
                }
            }
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
            this.TopMost = false;
            Form5 f5 = new Form5();
            f5.Font = MyNewFont;
            this.Hide();
            f5.ShowDialog();
            this.Show();
            this.TopMost = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f4.IsDisposed == false)
            {
                f4.Dispose();
            }
        }

        private void 检查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataCDCount.Clear();
            int i1 = dataTable1.Rows.Count;
            Parallel.For(0, i1, (int i) =>
            {
                dataCDCount.TryAdd(dataTable1.Rows[i][0].ToString() + dataTable1.Rows[i][1].ToString(), i);
            });
            string[] str1 = { "\r", "\n" };
            string[] str2 = Clipboard.GetText().Split(str1, StringSplitOptions.RemoveEmptyEntries);
            int i2 = str2.Length;
            if (i2 > 0)
            {
                for (int x = 0; x < i2; x++)
                {
                    if (dataCDCount.TryGetValue(str2[x].Replace("\t", ""), out int y))
                    {
                        dataGridView1[1, y].Style.BackColor = System.Drawing.Color.Aquamarine;
                    }
                }
            }
        }
    }
}