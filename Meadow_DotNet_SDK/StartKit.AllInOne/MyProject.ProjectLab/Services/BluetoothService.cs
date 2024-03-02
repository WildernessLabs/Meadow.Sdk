using Meadow.Devices;
using MyProject.Core;

namespace MyProject.ProjectLab;

internal class BluetoothService : IBluetoothService
{
    private F7CoreComputeV2 device;

    public BluetoothService(F7CoreComputeV2 device)
    {
        this.device = device;
    }

    public void Start()
    {
        // TODO:
        /*
        device.BluetoothAdapter.StartBluetoothServer(
            new Definition("Meadow",
                new Service("MyProject", 
                ...
        */
    }
}
