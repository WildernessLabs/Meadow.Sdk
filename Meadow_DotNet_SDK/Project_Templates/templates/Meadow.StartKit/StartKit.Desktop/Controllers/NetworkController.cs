using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using $safeprojectname$.Core;

namespace $safeprojectname$.DT
{
    internal class NetworkController : INetworkController
    {
        private bool isConnected;

        public event EventHandler NetworkStatusChanged;

        public NetworkController(Keyboard? keyboard)
        {
            if (keyboard != null)
            {
                // allow the app to simulate network up/down with the keyboard
                keyboard.Pins.Plus.CreateDigitalInterruptPort(InterruptMode.EdgeRising).Changed += (s, e) => { _ = Connect(); };
                keyboard.Pins.Minus.CreateDigitalInterruptPort(InterruptMode.EdgeRising).Changed += (s, e) => { IsConnected = false; };
            }
        }

        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                if (value == IsConnected) { return; }
                isConnected = value;
                NetworkStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task Connect()
        {
            // simulate connection delay
            await Task.Delay(1000);

            IsConnected = true;
        }
    }
}