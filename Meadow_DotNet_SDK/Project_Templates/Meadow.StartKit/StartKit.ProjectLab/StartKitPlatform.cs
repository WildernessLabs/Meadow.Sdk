using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.ProjectLab;

internal class StartKitPlatform : IStartKitPlatform
{
    private F7CoreComputeV2 _device;
    private IProjectLabHardware _projLab;
    private IOutputService? _outputService;
    private IBluetoothService? _bluetoothService;

    public StartKitPlatform(F7CoreComputeV2 device)
    {
        _device = device;
        _projLab = Meadow.Devices.ProjectLab.Create();
    }

    public ITemperatureSensor? GetTemperatureSensor()
    {
        return _projLab.EnvironmentalSensor;
    }

    public IHumiditySensor? GetHumiditySensor()
    {
        return _projLab.EnvironmentalSensor;
    }

    public IGraphicsDisplay? GetDisplay()
    {
        return _projLab.Display;
    }

    public IOutputService GetOutputService()
    {
        return _outputService ??= new OutputService(_projLab);
    }

    public IBluetoothService GetBluetoothService()
    {
        return _bluetoothService ??= new BluetoothService(_device);
    }
}
