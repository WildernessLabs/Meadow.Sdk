using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.DT
{
    public static class Program
    {
        private static void Main(string[] args)
        {
/* TODO Uncomment before mergoing to develop #if (framework == net8.0-windows)
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
#endif */
            MeadowOS.Start(args);
        }
    }
}