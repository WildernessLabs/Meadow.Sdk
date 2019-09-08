using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            {
              //  UpdateTargetsList(); //fire and forget ... this will update via an Action 
                return _deployTargets;
            }
        }
        private static readonly List<MeadowDeviceExecutionTarget> _deployTargets = new List<MeadowDeviceExecutionTarget>();

        private static Timer devicePollTimer;
        private static object eventLock = new object();
        private static bool isPolling;

        public static event Action<object> DeviceListChanged;

        public static void StartPollingForDevices()
        {
            if (isPolling)
                return;

            isPolling = true;

            devicePollTimer = new Timer(new TimerCallback(UpdateTargetsFromTimer), null, 1000, 1000);
        }

        static object timerLock = new object();

        public static void StopPollingForDevices()
        {
            isPolling = false;
   
            lock (timerLock)
            {
                devicePollTimer?.Dispose();
                devicePollTimer = null;
            }
        }

        private static void UpdateTargetsFromTimer(object state)
        {
            UpdateTargetsList();//fire and forget 
        }

        static bool isUpdating;
        private static async Task UpdateTargetsList()
        {
            if (isUpdating)
                return;

            isUpdating = true;
            //  _deployTargets.Clear();

            var serialPorts = MeadowDeviceManager.FindSerialDevices();

            foreach(var port in serialPorts)
            {
                if (_deployTargets.Any(t => t.MeadowDevice.SerialPort.PortName == port))
                    continue;

                var timeout = Task<MeadowDevice>.Delay(1000);
                var meadowTask = MeadowDeviceManager.GetMeadowForSerialPort(port);

                await Task.WhenAny(timeout, meadowTask);

                var meadow = meadowTask.Result;

                if (meadow != null)
                {
                    //we should really just have the execution target own an instance of MeadowDevice 
                    _deployTargets.Add(new MeadowDeviceExecutionTarget(meadow));
                    meadow.SerialPort.Close();
                    DeviceListChanged?.Invoke(null);
                }
            }

            var removeList = new List<MeadowDeviceExecutionTarget>();
            foreach(var t in _deployTargets)
            {
                if(serialPorts.Any(p => p == t.MeadowDevice.SerialPort.PortName) == false)
                {
                    removeList.Add(t);
                }
            }

            foreach(var r in removeList)
            {
                _deployTargets.Remove(r);
                DeviceListChanged?.Invoke(null);
            }

            isUpdating = false;

          //  await MeadowDeviceManager.FindConnectedDevices();
          /*
            if (MeadowDeviceManager.AttachedDevices.Count < 1)
                return;

            foreach(var d in MeadowDeviceManager.AttachedDevices)
            {
                if (_deployTargets.Any(m => m.Name == d.Name))
                    continue;

                _deployTargets.Add(new MeadowDeviceExecutionTarget(d.Name, d.Id));
                _deviceListChanged?.Invoke(null);
            }

            foreach(var t in _deployTargets)
            {
                if(MeadowDeviceManager.AttachedDevices.Any(d => d.Name == t.Name) == false)
                {
                    _deployTargets.Remove(t);
                }
            } */
        }
    }
}