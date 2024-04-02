using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.DT
{
    internal class ___safeprojectname___App : App<Desktop>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            // output log messages to the VS debug window
            Resolver.Log.AddProvider(new DebugLogProvider());

            var hardware = new ___safeprojectname___Hardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            // this must be spawned in a worker because the UI needs the main thread
            _ = mainController.Run();

            ExecutePlatformDisplayRunner();

            return base.Run();
        }

        private void ExecutePlatformDisplayRunner()
        {
/* TODO Uncomment before mergoing to develop #if (framework == net8.0-windows)
            if (Device.Display is System.Windows.Forms.Form display) {
                System.Windows.Forms.Application.Run(display);
            }
#else */
            if (Device.Display is GtkDisplay display)
            {
                display.Run();
            }
// TODO #endif
        }
    }
}