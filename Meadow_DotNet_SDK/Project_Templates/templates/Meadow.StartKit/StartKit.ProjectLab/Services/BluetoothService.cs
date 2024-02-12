using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.ProjectLab;

internal class BluetoothService : IBluetoothService
{
    private F7CoreComputeV2 _device;

    public BluetoothService(F7CoreComputeV2 device)
    {
        _device = device;
    }

    public void Start()
    {
        // TODO:
        /*
        _device.BluetoothAdapter.StartBluetoothServer(
            new Definition("Meadow",
                new Service("$safeprojectname$", 
                ...
        */
    }
}
