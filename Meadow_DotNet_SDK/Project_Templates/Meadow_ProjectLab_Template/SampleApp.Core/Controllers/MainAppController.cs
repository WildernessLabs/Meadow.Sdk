using System;
using System.Threading.Tasks;
using SampleApp.Hardware;

namespace SampleApp.Controllers
{
	public class MainAppController
	{
		public MainAppController(ISampleAppHardware hardware)
		{
		}


        public Task Run()
        {
            //_ = StartUpdating(UpdateInterval);

            return Task.CompletedTask;
        }
    }
}

