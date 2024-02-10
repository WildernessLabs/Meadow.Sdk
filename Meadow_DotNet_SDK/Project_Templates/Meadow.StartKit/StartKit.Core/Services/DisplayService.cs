using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace StartKit.Core;

public class DisplayService
{
    private readonly DisplayScreen? _screen;

    private Temperature currentTemp;
    private DisplayUnits displayUnits;
    private HomeLayout homeLayout;

    public DisplayService(
        IPixelDisplay? display,
        Temperature currentTemp,
        SetPoints setPoints)
    {
        if (display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20()
            };

            _screen = new DisplayScreen(
                display,
                theme: theme);

            GenerateLayouts(_screen);
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

    public Task UpdateDisplayUnits(DisplayUnits units)
    {
        displayUnits = units;
        UpdateDisplay();

        return Task.CompletedTask;
    }

    private void UpdateDisplay()
    {
        if (_screen == null)
        {
            return;
        }

        DisplayTemperature = currentTemp;
    }
}
