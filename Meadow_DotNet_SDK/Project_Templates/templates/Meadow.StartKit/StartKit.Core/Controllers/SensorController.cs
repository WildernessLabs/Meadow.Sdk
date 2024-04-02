using System;
using System.Threading.Tasks;
using Meadow.Units;
using ___safeprojectname___.Core.Contracts;

namespace ___safeprojectname___.Core
{
    public class SensorController
    {
        private Temperature temperature;

        public event EventHandler<Temperature> CurrentTemperatureChanged = default!;

        public SensorController(I___safeprojectname___Hardware platform)
        {
            if (platform.TemperatureSensor is { } t)
            {
                t.Updated += OnTemperatureUpdated;
                t.StartUpdating(TimeSpan.FromSeconds(1));
            }
        }

        public Temperature CurrentTemperature
        {
            get => temperature;
            private set
            {
                if (value == CurrentTemperature) return;
                temperature = value;
                CurrentTemperatureChanged?.Invoke(this, CurrentTemperature);
            }
        }

        private void OnTemperatureUpdated(object sender, Meadow.IChangeResult<Meadow.Units.Temperature> e)
        {
            CurrentTemperature = e.New;
        }
    }
}