using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using $safeprojectname$.Core;

namespace $safeprojectname$.DT
{
    public static class Program
    {
        private static void Main(string[] args)
        {
#if (Framework == net8.0-windows)
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
#endif
            MeadowOS.Start(args);
        }
    }
}