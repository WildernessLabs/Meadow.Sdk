using System;
using System.Threading.Tasks;

namespace ___safeprojectname___.Core
{
    public interface INetworkController
    {
        event EventHandler NetworkStatusChanged;

        Task Connect();
        bool IsConnected { get; }
    }
}