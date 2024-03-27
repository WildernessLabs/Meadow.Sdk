using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.F7
{
    public class $safeprojectname$F7FeatherApp : App<F7FeatherV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new $safeprojectname$F7FeatherHardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}