using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.F7
{
    public class ___safeprojectname___ProjectLabApp : App<F7CoreComputeV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new ___safeprojectname___ProjectLabHardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}