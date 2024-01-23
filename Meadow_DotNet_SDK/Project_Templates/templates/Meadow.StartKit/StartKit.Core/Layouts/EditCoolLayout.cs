using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace StartKit.Core
{

    internal class EditCoolLayout : ThermostatLayout
    {
        private Label _titleLabel;
        private Label _setpointLabel;
        private Temperature _setpoint;

        public EditCoolLayout(DisplayScreen screen)
            : base(screen)
        {
            _titleLabel = new Label(0, 0, screen.Width, 30)
            {
                Font = MediumFont,
                Text = "Cool To:"
            };

            _setpointLabel = new Label(0, 40, screen.Width, 40)
            {
                Font = LargeFont,
                Text = "__"
            };

            this.Controls.Add(
                this._titleLabel,
                _setpointLabel);
        }

        public virtual Temperature SetPoint
        {
            get => _setpoint;
            set
            {
                _setpoint = value;

                _setpointLabel.Text = Units switch
                {
                    DisplayUnits.Fahrenheit => $"{_setpoint.Fahrenheit:n0}",
                    _ => $"{_setpoint.Celsius:n0}",
                };
            }
        }
    }
}