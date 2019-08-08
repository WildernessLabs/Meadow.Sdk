using System;
using System.Collections.Generic;
using System.Threading;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    /// <summary>
    /// Manages the deployment targets in the toolbar and other places.
    ///
    /// A lot of this code is based on very old stuff, so the multithreading is a
    /// bit antiquated.
    ///
    /// TODO: get this working and re-write this, this threading here is _really_
    /// beach bally.
    /// </summary>
    public class DeploymentTargetsManager
    {
        /// <summary>
        /// A collection of connected and ready Meadow devices
        ///
        /// TODO: this is terrible. beach ball city folks.
        /// </summary>
        public static List<MeadowDeviceExecutionTarget> Targets
        {
            get
            {
                if (!_listening)
                    UpdateTargetsList(null);
                return _deployTargets;
            }
        }
        private static List<MeadowDeviceExecutionTarget> _deployTargets = new List<MeadowDeviceExecutionTarget>();

        private static Timer _timer;
        private static bool _listening = false;
        private static object _lock = new object();

        private static event Action<object> deviceListChanged;

        // this is probably manually executed so that the `StartListening()`
        // method isn't called until the IDE needs the list.
        //
        // note: this pattern would likely fail in a multiple subscription use
        // case. perhaps there's only one sub; the IDE.
        public static event Action<object> DeviceListChanged
        {
            add
            {
                Console.WriteLine("WLABS: Adding device list changed handler");
                lock (_lock)
                {
                    if (deviceListChanged == null)
                    {
                        StartListening();
                    }
                    deviceListChanged += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    deviceListChanged -= value;
                    if (deviceListChanged == null)
                    {
                        StopListening();
                    }
                }
            }
        }

        // is this blocking?! (yes, but does it matter?)
        private static void StartListening()
        {
            UpdateTargetsList(null);
            _listening = true;
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(UpdateTargetsList), null, 1000, 1000);
        }

        static object locker = new object();

        private static void StopListening()
        {
            _listening = false;
            if (_timer != null)
            {
                lock (locker)
                {
                    _timer.Dispose();
                    _timer = null;
                    //LibUsb_AsyncUsbStream.Exit();
                }
            }
        }

        // TODO: this is _monstrously terrible code_; rewrite.
        private static void UpdateTargetsList(object state)
        {
            Console.WriteLine("WLABS: Updating Targets List");
            // copy target state
            var targetsToKeep = new List<MeadowDeviceExecutionTarget>();
            // stop [what?]
            bool changed = false;
            lock (locker)
            {
                //var devices = ImportDefinition.Enumerate(PortFilter.Usb);
                Console.WriteLine("Devices");
                // fake up some devices
                List<MeadowDeviceExecutionTarget> devices = new List<MeadowDeviceExecutionTarget>() {
                    new MeadowDeviceExecutionTarget("F7 Micro 1", "1"),
                    new MeadowDeviceExecutionTarget("F7 Micro 2", "2")
                };

                // TODO: Devices
                foreach (var device in devices)
                {
                    bool targetExist = false;
                    foreach (var target in _deployTargets)
                    {
                        if (target.Id == (device.Id))
                        {
                            targetsToKeep.Add(target);
                            targetExist = true;
                            break;
                        }
                    }
                    if (!targetExist)
                    {
                        changed = true;
                        var newTarget = new MeadowDeviceExecutionTarget(device.Name, device.Id);
                        _deployTargets.Add(newTarget);
                        targetsToKeep.Add(newTarget);
                    }
                }
                // remove all the ones that are not in the targets to keep list, and set
                // changed to true if there's anything left? (or it was already true, because of the `|=` operator)
                changed |= _deployTargets.RemoveAll((target) => !targetsToKeep.Contains(target)) > 0;
            }
            if (changed)
            { // if there is a change, call the deviceListChanged delegate (raise the event)
                deviceListChanged?.Invoke(null);
            }
        }
    }
}
