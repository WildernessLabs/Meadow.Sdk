using System;
using System.Threading.Tasks;
using Meadow.Devices;
using Meadow.Hardware;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.F7;

internal class NetworkController : INetworkController
{
    private const string WIFI_NAME = "[SOME_NAME]";
    private const string WIFI_PASSWORD = "[SOME_SECRET]";

    public event EventHandler? NetworkStatusChanged;

    private bool isConnected;

    public NetworkController(F7MicroBase device)
    {
        wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        IsConnected = wifi.IsConnected;
        wifi.NetworkConnected += OnNetworkConnected;
        wifi.NetworkDisconnected += OnNetworkDisconnected;
    }

    private void OnNetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
    {
        IsConnected = false;
    }

    private void OnNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        IsConnected = true;
    }

    private IWiFiNetworkAdapter? wifi;

    public bool IsConnected
    {
        get => isConnected;
        set
        {
            if (value == IsConnected) return;
            isConnected = value;
            NetworkStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task Connect()
    {
        await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));
    }
}