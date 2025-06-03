using System;
using System.Threading.Tasks;

namespace ___SafeProjectName___.Core;

public interface INetworkController
{
    event EventHandler NetworkStatusChanged;

    Task Connect();
    bool IsConnected { get; }
}