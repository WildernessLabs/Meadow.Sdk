using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Pinouts;
using $safeprojectname$.Core;

namespace $safeprojectname$.RasPi
{
    internal class $safeprojectname$App : App<RaspberryPi>
    {
        private $safeprojectname$Hardware hardware;
        private MainController mainController;

        public bool SupportDisplay { get; set; } = false;

        public override Task Initialize()
        {
            hardware = new $safeprojectname$Hardware(Device, SupportDisplay);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            if (hardware.Display is GtkDisplay gtk)
            {
                _ = mainController.Run();
                gtk.Run();
            }

            return mainController.Run();
        }
    }
}