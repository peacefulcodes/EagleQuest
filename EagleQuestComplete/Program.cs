using System;
using System.Windows.Forms;
using EagleQuest.Forms;

namespace EagleQuest
{
    // Entry point of the application.
    // Opens StartForm — all other forms open from there.

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm());
        }
    }
}
