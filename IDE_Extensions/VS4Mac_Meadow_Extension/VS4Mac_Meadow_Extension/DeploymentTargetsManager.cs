using System;
using System.Collections.Generic;
using System.Threading;
using MeadowCLI.DeviceManagement;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    /// <summary>
    /// Manages the deployment targets in the toolbar and other places.
    /// </summary>
    public class DeploymentTargetsManager
    {
        /// <summary>
        /// A collection of connected and ready Meadow devices
        /// </summary>
        public static List<MeadowDeviceExecutionTarget> Targets 
        {
            get
            {   //very blocking still
                if(_deployTargets.Count == 0)
                    UpdateTargetsList();
                return _deployTargets;
            }
        }
        private static List<MeadowDeviceExecutionTarget> _deployTargets = new List<MeadowDeviceExecutionTarget>();

        public static event Action<object> DeviceListChanged;

        private static void UpdateTargetsList(object state = null)
        {
            //quick hack for now - only load the device once
            if (_deployTargets.Count == 0)
            {
                MeadowDeviceManager.FindConnectedDevices();

                if (MeadowDeviceManager.AttachedDevices.Count < 1)
                    return;

                foreach(var d in MeadowDeviceManager.AttachedDevices)
                {
                    _deployTargets.Add(new MeadowDeviceExecutionTarget(d.Name, d.Id));
                    DeviceListChanged?.Invoke(null);
                }
            }
        }
    }
}