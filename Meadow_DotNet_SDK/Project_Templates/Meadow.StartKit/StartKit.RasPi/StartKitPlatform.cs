using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace Meadow.RasPi;

internal class StartKitPlatform<T> : IStartKitPlatform
    where T : IPinDefinitions, new()
{
    private readonly Meadow.Linux<T> _device;
    private readonly IGraphicsDisplay? _graphicsDisplay = null;
    private readonly ITemperatureSensor _temperatureSimulator;
    private readonly IOutputService _outputService;

    public StartKitPlatform(Meadow.Linux<T> device, bool supportDisplay)
    {
        _device = device;

        if (supportDisplay)
        { // only if we have a display attached
            _graphicsDisplay = new GtkDisplay(ColorMode.Format16bppRgb565);
        }
    }

    public IBluetoothService? GetBluetoothService()
    {
        return null;
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return _graphicsDisplay;
    }

    public IHumiditySensor? GetHumiditySensor()
    {
        return null;
    }

    public IOutputService GetOutputService()
    {
        return _outputService;
    }

    public ITemperatureSensor? GetTemperatureSensor()
    {
        return _temperatureSimulator;
    }

    public IButton? GetDownButton()
    {
        return null;
    }

    public IButton? GetLeftButton()
    {
        return null;
    }

    public IButton? GetRightButton()
    {
        return null;
    }

    public IButton? GetUpButton()
    {
        return null;
    }
}
