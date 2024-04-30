using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.F7Feather
{
    public class $safeprojectname$App : App<F7FeatherV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new $safeprojectname$Hardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}