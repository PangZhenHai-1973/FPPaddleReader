using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FPPaddleReader
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.扫描设置ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.发票代码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票号码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.开票日期DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.不含税金额DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.校验码DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数据ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导入数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.查看文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.检查ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.发票代码DataColumn = new System.Data.DataColumn();
            this.发票号码DataColumn = new System.Data.DataColumn();
            this.开票日期DataColumn = new System.Data.DataColumn();
            this.不含税金额DataColumn = new System.Data.DataColumn();
            this.校验码DataColumn = new System.Data.DataColumn();
            this.来源DataColumn = new System.Data.DataColumn();
            this.文件DataColumn = new System.Data.DataColumn();
            this.文件ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.拍照ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发票信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.电子发票ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除已检查ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.扫描设置ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.数据ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.文件ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 98);
            this.panel1.TabIndex = 0;
            // 
            // button8
            // 
            this.button8.ContextMenuStrip = this.扫描设置ContextMenuStrip;
            this.button8.Location = new System.Drawing.Point(12, 53);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 32);
            this.button8.TabIndex = 12;
            this.button8.Text = "扫描";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // 扫描设置ContextMenuStrip
            // 
            this.扫描设置ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem});
            this.扫描设置ContextMenuStrip.Name = "扫描设置ContextMenuStrip";
            this.扫描设置ContextMenuStrip.Size = new System.Drawing.Size(101, 26);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Fuchsia;
            this.label4.Location = new System.Drawing.Point(258, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "9999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "当前发票：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "9999";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "发票数：";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(95, 53);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(480, 32);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Visible = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(500, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 32);
            this.button7.TabIndex = 6;
            this.button7.Text = "下一个";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(419, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 32);
            this.button6.TabIndex = 5;
            this.button6.Text = "校验码";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(338, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 32);
            this.button5.TabIndex = 4;
            this.button5.Text = "金额";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(257, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 32);
            this.button4.TabIndex = 3;
            this.button4.Text = "开票日期";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(176, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 2;
            this.button3.Text = "发票号码";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(95, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "发票代码";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "加载图片";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.发票代码DataGridViewTextBoxColumn,
            this.发票号码DataGridViewTextBoxColumn,
            this.开票日期DataGridViewTextBoxColumn,
            this.不含税金额DataGridViewTextBoxColumn,
            this.校验码DataGridViewTextBoxColumn});
            this.dataGridView1.ContextMenuStrip = this.数据ContextMenuStrip;
            this.dataGridView1.DataMember = "Table1";
            this.dataGridView1.DataSource = this.dataSet1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 98);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.Size = new System.Drawing.Size(587, 420);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // 发票代码DataGridViewTextBoxColumn
            // 
            this.发票代码DataGridViewTextBoxColumn.DataPropertyName = "发票代码";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.发票代码DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.发票代码DataGridViewTextBoxColumn.HeaderText = "发票代码";
            this.发票代码DataGridViewTextBoxColumn.Name = "发票代码DataGridViewTextBoxColumn";
            this.发票代码DataGridViewTextBoxColumn.ReadOnly = true;
            this.发票代码DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 发票号码DataGridViewTextBoxColumn
            // 
            this.发票号码DataGridViewTextBoxColumn.DataPropertyName = "发票号码";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.发票号码DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.发票号码DataGridViewTextBoxColumn.HeaderText = "发票号码";
            this.发票号码DataGridViewTextBoxColumn.Name = "发票号码DataGridViewTextBoxColumn";
            this.发票号码DataGridViewTextBoxColumn.ReadOnly = true;
            this.发票号码DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 开票日期DataGridViewTextBoxColumn
            // 
            this.开票日期DataGridViewTextBoxColumn.DataPropertyName = "开票日期";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.开票日期DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.开票日期DataGridViewTextBoxColumn.HeaderText = "开票日期";
            this.开票日期DataGridViewTextBoxColumn.Name = "开票日期DataGridViewTextBoxColumn";
            this.开票日期DataGridViewTextBoxColumn.ReadOnly = true;
            this.开票日期DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 不含税金额DataGridViewTextBoxColumn
            // 
            this.不含税金额DataGridViewTextBoxColumn.DataPropertyName = "不含税金额";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.不含税金额DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.不含税金额DataGridViewTextBoxColumn.HeaderText = "不含税金额";
            this.不含税金额DataGridViewTextBoxColumn.Name = "不含税金额DataGridViewTextBoxColumn";
            this.不含税金额DataGridViewTextBoxColumn.ReadOnly = true;
            this.不含税金额DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // 校验码DataGridViewTextBoxColumn
            // 
            this.校验码DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.校验码DataGridViewTextBoxColumn.DataPropertyName = "校验码";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.校验码DataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.校验码DataGridViewTextBoxColumn.HeaderText = "发票校验码";
            this.校验码DataGridViewTextBoxColumn.MinimumWidth = 150;
            this.校验码DataGridViewTextBoxColumn.Name = "校验码DataGridViewTextBoxColumn";
            this.校验码DataGridViewTextBoxColumn.ReadOnly = true;
            this.校验码DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 数据ContextMenuStrip
            // 
            this.数据ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入数据ToolStripMenuItem,
            this.导出数据ToolStripMenuItem,
            this.toolStripSeparator1,
            this.编辑ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.删除已检查ToolStripMenuItem,
            this.toolStripSeparator2,
            this.查看文件ToolStripMenuItem,
            this.查看日志ToolStripMenuItem,
            this.toolStripSeparator3,
            this.检查ToolStripMenuItem});
            this.数据ContextMenuStrip.Name = "contextMenuStrip1";
            this.数据ContextMenuStrip.Size = new System.Drawing.Size(181, 220);
            // 
            // 导入数据ToolStripMenuItem
            // 
            this.导入数据ToolStripMenuItem.Name = "导入数据ToolStripMenuItem";
            this.导入数据ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.导入数据ToolStripMenuItem.Text = "导入数据";
            this.导入数据ToolStripMenuItem.Click += new System.EventHandler(this.导入数据ToolStripMenuItem_Click);
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.导出数据ToolStripMenuItem.Text = "导出数据";
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.编辑ToolStripMenuItem.Text = "编辑";
            this.编辑ToolStripMenuItem.Click += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // 查看文件ToolStripMenuItem
            // 
            this.查看文件ToolStripMenuItem.Name = "查看文件ToolStripMenuItem";
            this.查看文件ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.查看文件ToolStripMenuItem.Text = "查看文件";
            this.查看文件ToolStripMenuItem.Click += new System.EventHandler(this.查看文件ToolStripMenuItem_Click);
            // 
            // 查看日志ToolStripMenuItem
            // 
            this.查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            this.查看日志ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.查看日志ToolStripMenuItem.Text = "查看日志";
            this.查看日志ToolStripMenuItem.Click += new System.EventHandler(this.查看日志ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // 检查ToolStripMenuItem
            // 
            this.检查ToolStripMenuItem.Name = "检查ToolStripMenuItem";
            this.检查ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.检查ToolStripMenuItem.Text = "检查";
            this.检查ToolStripMenuItem.Click += new System.EventHandler(this.检查ToolStripMenuItem_Click);
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.发票代码DataColumn,
            this.发票号码DataColumn,
            this.开票日期DataColumn,
            this.不含税金额DataColumn,
            this.校验码DataColumn,
            this.来源DataColumn,
            this.文件DataColumn});
            this.dataTable1.TableName = "Table1";
            // 
            // 发票代码DataColumn
            // 
            this.发票代码DataColumn.ColumnName = "发票代码";
            // 
            // 发票号码DataColumn
            // 
            this.发票号码DataColumn.ColumnName = "发票号码";
            // 
            // 开票日期DataColumn
            // 
            this.开票日期DataColumn.ColumnName = "开票日期";
            // 
            // 不含税金额DataColumn
            // 
            this.不含税金额DataColumn.ColumnName = "不含税金额";
            this.不含税金额DataColumn.DataType = typeof(double);
            // 
            // 校验码DataColumn
            // 
            this.校验码DataColumn.ColumnName = "校验码";
            // 
            // 来源DataColumn
            // 
            this.来源DataColumn.ColumnName = "来源";
            // 
            // 文件DataColumn
            // 
            this.文件DataColumn.ColumnName = "文件";
            // 
            // 文件ContextMenuStrip
            // 
            this.文件ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.拍照ToolStripMenuItem,
            this.发票信息ToolStripMenuItem,
            this.电子发票ToolStripMenuItem});
            this.文件ContextMenuStrip.Name = "文件ContextMenuStrip";
            this.文件ContextMenuStrip.Size = new System.Drawing.Size(125, 70);
            // 
            // 拍照ToolStripMenuItem
            // 
            this.拍照ToolStripMenuItem.Name = "拍照ToolStripMenuItem";
            this.拍照ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.拍照ToolStripMenuItem.Text = "拍照";
            this.拍照ToolStripMenuItem.Click += new System.EventHandler(this.拍照ToolStripMenuItem_Click);
            // 
            // 发票信息ToolStripMenuItem
            // 
            this.发票信息ToolStripMenuItem.Name = "发票信息ToolStripMenuItem";
            this.发票信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.发票信息ToolStripMenuItem.Text = "发票信息";
            this.发票信息ToolStripMenuItem.Click += new System.EventHandler(this.发票信息ToolStripMenuItem_Click);
            // 
            // 电子发票ToolStripMenuItem
            // 
            this.电子发票ToolStripMenuItem.Name = "电子发票ToolStripMenuItem";
            this.电子发票ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.电子发票ToolStripMenuItem.Text = "电子发票";
            this.电子发票ToolStripMenuItem.Click += new System.EventHandler(this.电子发票ToolStripMenuItem_Click);
            // 
            // 删除已检查ToolStripMenuItem
            // 
            this.删除已检查ToolStripMenuItem.Name = "删除已检查ToolStripMenuItem";
            this.删除已检查ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.删除已检查ToolStripMenuItem.Text = "删除已检查";
            this.删除已检查ToolStripMenuItem.Click += new System.EventHandler(this.删除已检查ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(587, 518);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发票信息读取工具 Build 20240515";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.扫描设置ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.数据ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.文件ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView1;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Button button3;
        private Button button2;
        private Button button1;
        private ProgressBar progressBar1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DataSet dataSet1;
        private DataTable dataTable1;
        private DataColumn 发票代码DataColumn;
        private DataColumn 发票号码DataColumn;
        private DataColumn 开票日期DataColumn;
        private DataColumn 不含税金额DataColumn;
        private DataColumn 校验码DataColumn;
        private DataGridViewTextBoxColumn 发票代码DataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn 发票号码DataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn 开票日期DataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn 不含税金额DataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn 校验码DataGridViewTextBoxColumn;
        private Button button8;
        private ContextMenuStrip 数据ContextMenuStrip;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ContextMenuStrip 扫描设置ContextMenuStrip;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem 导入数据ToolStripMenuItem;
        private ToolStripMenuItem 导出数据ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem 编辑ToolStripMenuItem;
        private ContextMenuStrip 文件ContextMenuStrip;
        private ToolStripMenuItem 拍照ToolStripMenuItem;
        private ToolStripMenuItem 发票信息ToolStripMenuItem;
        private ToolStripMenuItem 电子发票ToolStripMenuItem;
        private DataColumn 来源DataColumn;
        private DataColumn 文件DataColumn;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem 查看文件ToolStripMenuItem;
        private ToolStripMenuItem 查看日志ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem 检查ToolStripMenuItem;
        private ToolStripMenuItem 删除已检查ToolStripMenuItem;
    }
}

