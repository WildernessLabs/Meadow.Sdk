using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace MyProject.Core
{

    internal class NormalLayout : ThermostatLayout
    {
        private Label headerLabel;
        private Label tempLabel;
        private Label modeLabel;
        private ThermostatMode thermostatMode;

        public NormalLayout(DisplayScreen screen) : base(screen)
        {
            headerLabel = new Label(0, 0, screen.Width, 30)
            {
                Font = MediumFont,
                Text = "Current Temp"
            };

            tempLabel = new Label(0, 30, screen.Width, 50)
            {
                Font = LargeFont,
                Text = "__"
            };

            modeLabel = new Label(0, 80, screen.Width, 50)
            {
                Font = MediumFont,
                Text = "Off"
            };

            Controls.Add(
                headerLabel,
                tempLabel,
                modeLabel);
        }

        public ThermostatMode Mode
        {
            set
            {
                modeLabel.Text = value switch
                {
                    ThermostatMode.Heat => "Heating",
                    ThermostatMode.Cool => "Cooling",
                    _ => "Off",
                };
            }
        }

        public override Temperature DisplayTemperature
        {
            get => base.DisplayTemperature;
            set
            {
                base.DisplayTemperature = value;

                tempLabel.Text = Units switch
                {
                    DisplayUnits.Fahrenheit => $"{DisplayTemperature.Fahrenheit:n0}",
                    _ => $"{DisplayTemperature.Celsius:n1}",
                };
            }
        }
    }
}