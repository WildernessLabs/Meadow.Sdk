using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using StartKit.Core;
using StartKit.Core.Contracts;

namespace StartKit.ProjectLab;

internal class ProjectLabHardware : IStartKitHardware
{
    private F7CoreComputeV2 device;
    private IProjectLabHardware projLab;
    private IOutputController? outputService;
    private IBluetoothService? bluetoothService;

    public ProjectLabHardware(F7CoreComputeV2 device)
    {
        this.device = device;
        projLab = Meadow.Devices.ProjectLab.Create();
    }

    public IButton? UpButton => projLab.UpButton;
    public IButton? DownButton => projLab.DownButton;
    public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;
    public IPixelDisplay? Display => projLab.Display;
    public IOutputController OutputController => outputService ??= new OutputController(projLab);
    public IBluetoothService BluetoothService => bluetoothService ??= new BluetoothService(device);
}
