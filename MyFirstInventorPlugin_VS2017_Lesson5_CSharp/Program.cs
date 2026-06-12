using System;
using System.Windows.Forms;

namespace MyFirstInventorPlugin_VS2017_Lesson5_CSharp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
