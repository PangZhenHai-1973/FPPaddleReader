using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPPaddleReader
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            dateTimePicker1.Font = this.Font;
            textBox0.Text = Form1.EditFPData[0].ToString();
            textBox1.Text = Form1.EditFPData[1].ToString();
            try
            {
                dateTimePicker1.Value = DateTime.Parse(Form1.EditFPData[2].ToString());
            }
            catch
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            textBox3.Text = Form1.EditFPData[3].ToString();
            textBox4.Text = Form1.EditFPData[4].ToString();
            textBox0.Location = new Point(textBox0.Location.X, (int)(label1.Location.Y + label1.Height / 2D - textBox0.Height / 2D));
            textBox1.Location = new Point(textBox1.Location.X, (int)(label2.Location.Y + label2.Height / 2D - textBox1.Height / 2D));
            dateTimePicker1.Location = new Point(dateTimePicker1.Location.X, (int)(label3.Location.Y + label3.Height / 2D - dateTimePicker1.Height / 2D));
            textBox3.Location = new Point(textBox3.Location.X, (int)(label4.Location.Y + label4.Height / 2D - textBox3.Height / 2D));
            textBox4.Location = new Point(textBox4.Location.X, (int)(label5.Location.Y + label5.Height / 2D - textBox4.Height / 2D));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool bl = true;
            string s3 = textBox3.Text.Trim();
            if (s3 == "")
            {
                MessageBox.Show("请填写金额。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                double d = 0;
                try
                {
                    d = double.Parse(s3);
                }
                catch
                {
                    bl = false;
                }
                if (bl)
                {
                    Form1.EditFPData[0] = textBox0.Text;
                    Form1.EditFPData[1] = textBox1.Text;
                    Form1.EditFPData[2] = dateTimePicker1.Text;
                    Form1.EditFPData[3] = d;
                    Form1.EditFPData[4] = textBox4.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("金额不是有效的数值！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
