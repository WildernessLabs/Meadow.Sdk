using Meadow;
using Meadow.Foundation.Displays;
using System.Windows.Forms;

namespace MeadowApplication.Template
{
    public class MeadowApp : App<Meadow.Windows>
    {
        WinFormsDisplay? display;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            display = new WinFormsDisplay(320, 240);
            var displayController = new DisplayController(display);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            Application.Run(display);
        }

        public static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            await MeadowOS.Start(args);
        }
    }
}