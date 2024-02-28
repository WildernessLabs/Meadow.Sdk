using System;
using System.Threading.Tasks;
using Meadow;
using $safeprojectname$.Core;

namespace $safeprojectname$.Desktop
{
    internal class $safeprojectname$App : App<Meadow.Desktop>
    {
        private $safeprojectname$Platform _platform;

        private static void Main(string[] args)
        {
#if (Framework == net8.0-windows)
	        System.Windows.Forms.Application.EnableVisualStyles();
	        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
	        ApplicationConfiguration.Initialize();
#endif

	        MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            _platform = new $safeprojectname$Platform(Device);
            var c = new MainController();
            await c.Initialize(_platform);
            _ = c.Run();
        }

        public override Task Run()
        {
#if (Framework == net8.0-windows)
	        System.Windows.Forms.Application.Run(Device.Display as System.Windows.Forms.Form);
#endif

	        Application.Run(_platform.GetDisplay() as Form);

	        return base.Run();
        }
    }
}