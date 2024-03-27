using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.F7
{
    public class $safeprojectname$ProjectLabApp : App<F7CoreComputeV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new $safeprojectname$ProjectLabHardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}