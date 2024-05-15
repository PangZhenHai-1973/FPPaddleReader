using System;
using System.Drawing;
using System.Windows.Forms;

namespace FPPaddleReader
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetDefaultFont(new Font("SimSun", 9F));
            Application.Run(new Form1());
        }
    }
}