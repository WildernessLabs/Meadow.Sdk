using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Peripherals.Sensors.Moisture;
using Meadow.Peripherals.Speakers;
using SampleApp.Hardware;

namespace SampleApp.MeadowApp.Hardware
{
    public class ProjectLabHardware : ISampleAppHardware
    {
        protected IProjectLabHardware projectLab { get; set; }

        public ITemperatureSensor? TemperatureSensor => projectLab.EnvironmentalSensor;

        public IHumiditySensor? HumiditySensor => projectLab.EnvironmentalSensor;

        public IBarometricPressureSensor? PressureSensor => projectLab.EnvironmentalSensor;

        public IToneGenerator? Speaker => projectLab.Speaker;

        public RgbPwmLed? RgbLed => projectLab.RgbLed;

        public IButton? LeftButton => projectLab.LeftButton;

        public IButton? RightButton => projectLab.RightButton;

        public IButton? UpButton => projectLab.UpButton;

        public IButton? DownButton => projectLab.DownButton;

        public IGraphicsDisplay? Display => projectLab.Display;

        public ProjectLabHardware()
        {
            projectLab = ProjectLab.Create();

            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            Resolver.Log.Info($"Success!");
        }
    }
}

