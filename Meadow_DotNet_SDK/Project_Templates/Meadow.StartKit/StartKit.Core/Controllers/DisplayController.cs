using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayController
{
    private readonly DisplayScreen? screen;

    private Label displayTempLabel;

    private Temperature displayTemp;
    private Temperature.UnitType displayUnits;

    public DisplayController(
        IPixelDisplay? display,
        Temperature.UnitType unit)
    {
        if (display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20(),
                BackgroundColor = Color.Black,
                TextColor = Color.White
            };

            screen = new DisplayScreen(
                display,
                theme: theme);

            GenerateLayout(screen);
        }

        UpdateDisplay();
    }

    private void GenerateLayout(DisplayScreen screen)
    {

        displayTempLabel = new Label(0, 0, screen.Width, screen.Height)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        screen.Controls.Add(displayTempLabel);
    }

    public Task UpdateCurrentTemperature(Temperature temperature)
    {
        displayTemp = temperature;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    public Task UpdateDisplayUnits(Temperature.UnitType units)
    {
        displayUnits = units;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    private void UpdateDisplay()
    {
        var unitLabel = displayUnits switch
        {
            Temperature.UnitType.Celsius => "C",
            Temperature.UnitType.Fahrenheit => "F",
            _ => "K"
        };

        var text = $"{displayTemp.From(displayUnits):N1}°{unitLabel}";

        if (screen != null)
        {
            displayTempLabel.Text = text;
        }
        else
        {
            Resolver.Log.Info(text);
        }
    }
}
