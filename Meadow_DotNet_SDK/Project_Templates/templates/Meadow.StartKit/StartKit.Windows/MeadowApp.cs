using Meadow;
using $safeprojectname$.Core;
using System.Windows.Forms;

namespace $safeprojectname$.Windows
{

    internal class MeadowApp : App<Meadow.Windows>
    {
        private $safeprojectname$Platform _platform;

        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

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
            Application.Run(_platform.GetDisplay() as Form);

            return base.Run();
        }
    }
}