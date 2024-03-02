using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace MyProject.Core
{

    internal class EditCoolLayout : ThermostatLayout
    {
        private Label titleLabel;
        private Label setpointLabel;
        private Temperature setpoint;

        public EditCoolLayout(DisplayScreen screen)
            : base(screen)
        {
            titleLabel = new Label(0, 0, screen.Width, 30)
            {
                Font = MediumFont,
                Text = "Cool To:"
            };

            setpointLabel = new Label(0, 40, screen.Width, 40)
            {
                Font = LargeFont,
                Text = "__"
            };

            this.Controls.Add(
                this.titleLabel,
                setpointLabel);
        }

        public virtual Temperature SetPoint
        {
            get => setpoint;
            set
            {
                setpoint = value;

                setpointLabel.Text = Units switch
                {
                    DisplayUnits.Fahrenheit => $"{setpoint.Fahrenheit:n0}",
                    _ => $"{setpoint.Celsius:n0}",
                };
            }
        }
    }
}