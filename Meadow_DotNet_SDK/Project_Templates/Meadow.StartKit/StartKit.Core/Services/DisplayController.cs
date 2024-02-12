using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayController
{
    private readonly DisplayScreen? screen;

    private Temperature currentTemp;
    private Temperature.UnitType displayUnits;
    private HomeLayout homeLayout;

    public DisplayController(
        IPixelDisplay? display,
        Temperature.UnitType unit)
    {
        if (display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20()
            };

            screen = new DisplayScreen(
                display,
                theme: theme);

            GenerateLayouts(screen);
        }
    }

    private void GenerateLayouts(DisplayScreen screen)
    {
        homeLayout = new HomeLayout(screen)
        {
            IsVisible = true,
        };

        screen.Controls.Add(
            homeLayout);
    }

    public Task UpdateCurrentTemperature(Temperature temperature)
    {
        currentTemp = temperature;
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
        if (screen == null)
        {
            return;
        }

        //        DisplayTemperature = currentTemp;
    }
}
