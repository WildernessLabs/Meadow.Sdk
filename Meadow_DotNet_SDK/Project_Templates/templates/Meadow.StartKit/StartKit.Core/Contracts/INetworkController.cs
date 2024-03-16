using System;
using System.Threading.Tasks;

namespace $safeprojectname$.Core
{
    public interface INetworkController
    {
        event EventHandler NetworkStatusChanged;

        Task Connect();
        bool IsConnected { get; }
    }
}