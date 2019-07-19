using System;
using System.Collections.Generic;
using System.Threading;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{

    public class DeploymentTargetsManager
    {
        /// <summary>
        /// A collection of connected and ready Meadow devices
        /// </summary>
        public static List<MeadowDevice> Targets {
            get {
                if (!_listening)
                    updateTargetsList(null);
                return _deployTargets;
            }
        }
        private static List<MeadowDevice> _deployTargets = new List<MeadowDevice>();

        private static Timer _timer;
        private static bool _listening = false;
        private static object _lock = new object();

        private static event Action<object> deviceListChanged;

        public static event Action<object> DeviceListChanged {
            add {
                lock (_lock) {
                    if (deviceListChanged == null) {
                        StartListening();
                    }
                    deviceListChanged += value;
                }
            }
            remove {
                lock (_lock) {
                    deviceListChanged -= value;
                    if (deviceListChanged == null) {
                        StopListening();
                    }
                }
            }
        }

        private static void StartListening()
        {
            updateTargetsList(null);
            _listening = true;
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(updateTargetsList), null, 1000, 1000);
        }

        static object locker = new object();

        private static void StopListening()
        {
            _listening = false;
            if (_timer != null) {
                lock (locker) {
                    _timer.Dispose();
                    _timer = null;
                    //LibUsb_AsyncUsbStream.Exit();
                }
            }
        }

        private static void updateTargetsList(object state)
        {


            //var targetsToKeep = new List<MeadowDevice>();
            //bool changed = false;
            //lock (locker) {
            //    var devices = PortDefinition.Enumerate(PortFilter.Usb);
            //    foreach (var device in devices) {
            //        bool targetExist = false;
            //        foreach (var target in _deployTargets) {
            //            if (target.PortDefinition.Port == (device as PortDefinition).Port) {
            //                targetsToKeep.Add(target);
            //                targetExist = true;
            //                break;
            //            }
            //        }
            //        if (!targetExist) {
            //            changed = true;
            //            var newTarget = new MeadowDevice(device as PortDefinition);
            //            _deployTargets.Add(newTarget);
            //            targetsToKeep.Add(newTarget);
            //        }
            //    }
            //    changed |= _deployTargets.RemoveAll((target) => !targetsToKeep.Contains(target)) > 0;
            //}
            //if (changed)
            //    deviceListChanged?.Invoke(null);
        }
    }
}
