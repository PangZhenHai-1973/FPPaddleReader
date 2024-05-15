using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WIA;
using System.IO;
using System.Data;
using System.Text;

namespace FPPaddleReader
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            double z = button1.Location.Y + button1.Height / 2D;
            comboBox1.Location = new Point(comboBox1.Location.X, (int)(z - comboBox1.Height / 2D));
            label1.Location = new Point(label1.Location.X, (int)(z - label1.Height / 2D));
            DeviceManager dm = new DeviceManager();
            int i1 = dm.DeviceInfos.Count;
            for (int i = 1; i <= i1; i++)
            {
                if (dm.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                {
                    comboBox1.Items.Add(dm.DeviceInfos[i].Properties["Name"].get_Value());
                }
            }
            if (comboBox1.Items.Count > 0)
            {
                if (Form1.scannerDevice != "")
                {
                    if (comboBox1.Items.Contains(Form1.scannerDevice))
                    {
                        comboBox1.Text = Form1.scannerDevice;
                    }
                    else
                    {
                        comboBox1.Text = comboBox1.Items[0].ToString();
                    }
                }
                else
                {
                    comboBox1.Text = comboBox1.Items[0].ToString();
                }
            }
            else
            {
                Form1.scannerDevice = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string comStr = comboBox1.Text;
            if (comStr != "")
            {
                label1.Enabled = false;
                comboBox1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                DeviceManager dm = new DeviceManager();
                int i1 = dm.DeviceInfos.Count;
                int i = 1;
                for (; i <= i1; i++)
                {
                    if (dm.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                    {
                        if (dm.DeviceInfos[i].Properties["Name"].get_Value().ToString() == comStr)
                        {
                            break;
                        }
                    }
                }
                var scannerItem = dm.DeviceInfos[i].Connect().Items[1];
                CommonDialogClass dlg = new CommonDialogClass();
                try
                {
                    object scanResult = dlg.ShowTransfer(scannerItem, FormatID.wiaFormatBMP, true);

                    if (scanResult != null)
                    {
                        ImageFile image = (ImageFile)scanResult;
                        Bitmap bmp = new Bitmap(new MemoryStream((byte[])image.FileData.get_BinaryData()));
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        Point p = new Point((int)(bmp.Width * 0.2), 0);
                        Size s = new Size((int)(bmp.Width - bmp.Width * 0.2), (int)(bmp.Height - bmp.Height * 0.35));
                        Bitmap bmpTmp = bmp.Clone(new Rectangle(p, s), PixelFormat.Format32bppRgb);
                        pictureBox1.Image = bmpTmp;
                    }
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
                label1.Enabled = true;
                comboBox1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string comStr = comboBox1.Text;
            if (!Directory.Exists(Form1.MyDataDirectory))
            {
                Directory.CreateDirectory(Form1.MyDataDirectory);
            }
            string scannerDeviceSetup = Form1.MyDataDirectory + "ScannerDevice.ini";
            using (StreamWriter sw = new StreamWriter(scannerDeviceSetup, false, Encoding.Unicode))
            {
                if (comStr != "")
                {
                    Form1.scannerDevice = comStr;
                    sw.WriteLine(comStr);
                }
            }
            this.Close();
        }
    }
}
