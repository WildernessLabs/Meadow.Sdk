using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace StartKit.Core;

internal class NormalLayout : ThermostatLayout
{
    private Label _headerLabel;
    private Label _tempLabel;
    private Label _modeLabel;
    private ThermostatMode _thermostatMode;

    public NormalLayout(DisplayScreen screen) : base(screen)
    {
        _headerLabel = new Label(0, 0, screen.Width, 30)
        {
            Font = MediumFont,
            Text = "Current Temp"
        };

        _tempLabel = new Label(0, 30, screen.Width, 50)
        {
            Font = LargeFont,
            Text = "__"
        };

        _modeLabel = new Label(0, 80, screen.Width, 50)
        {
            Font = MediumFont,
            Text = "Off"
        };

        Controls.Add(
            _headerLabel,
            _tempLabel,
            _modeLabel);
    }

    public ThermostatMode Mode
    {
        set
        {
            _modeLabel.Text = value switch
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

            _tempLabel.Text = Units switch
            {
                DisplayUnits.Fahrenheit => $"{DisplayTemperature.Fahrenheit:n0}",
                _ => $"{DisplayTemperature.Celsius:n1}",
            };
        }
    }
}
