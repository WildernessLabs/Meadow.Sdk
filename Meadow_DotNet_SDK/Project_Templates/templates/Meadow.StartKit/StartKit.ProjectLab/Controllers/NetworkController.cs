using System;
using System.Threading.Tasks;
using Meadow.Devices;
using Meadow.Hardware;
using $safeprojectname$.Core;

namespace $safeprojectname$.ProjectLab
{
    internal class NetworkController : INetworkController
    {
        public event EventHandler NetworkStatusChanged;

        private bool isNetworkConnected;

        public NetworkController(F7MicroBase device)
        {
            var wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

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

        public bool IsConnected
        {
            get => isNetworkConnected;
            set
            {
                if (value == IsConnected) return;
                isNetworkConnected = value;
                NetworkStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Task Connect()
        {
            throw new NotImplementedException();
        }
    }
}