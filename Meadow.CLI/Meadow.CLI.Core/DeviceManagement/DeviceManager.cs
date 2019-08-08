using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MeadowCLI.Hcom;
using static MeadowCLI.DeviceManagement.MeadowFileManager;

namespace MeadowCLI.DeviceManagement
{
    /// <summary>
    /// TODO: put device enumeration and such stuff here.
    /// </summary>
    public static class DeviceManager
    {
        public static ObservableCollection<MeadowDevice> AttachedDevices = new ObservableCollection<MeadowDevice>();

        public static MeadowDevice CurrentDevice { get; set; } //short cut for now but may be useful

        static HcomMeadowRequestType _meadowRequestType;

        static DeviceManager()
        {
            // TODO: populate the list of attached devices

            // TODO: wire up listeners for device plug and unplug
        }

        private static void Handle_DeviceAdded()
        {
            // add device to AttachedDevices using lib usb
        }

        private static void Handle_DeviceRemoved()
        {
            // remove device from AttachedDevices using lib usb
        }

        private static async Task FindConnectedDevices ()
        {
            var device = new MeadowDevice("/dev/tty.usbserial01", "Meadow Micro F7");
            AttachedDevices.Add(device);

            CurrentDevice = device;
        }

        //providing a numeric (0 = none, 1 = info and 2 = debug)
        public static void SetTraceLevel(MeadowDevice meadow, int level)
        {
            if (level < 1 || level > 4)
                throw new System.ArgumentOutOfRangeException(nameof(level), "Trace level must be between 0 & 3 inclusive");


            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_CHANGE_TRACE_LEVEL;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)level);
        }

        public static void ResetTargetMcu(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_RESET_PRIMARY_MCU;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void EnterDfuMode(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_ENTER_DFU_MODE;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void ToggleNsh(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_ENABLE_DISABLE_NSH;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        //ToDo - look these up - I assume their developer modes? Should be SetDev1, etc. ?
        public static void SetDeveloperLevel(MeadowDevice meadow, int level)
        {
            if (level < 1 || level > 4)
                throw new System.ArgumentOutOfRangeException(nameof(level), "Developer level must be between 1 & 4 inclusive");

            if(level == 1)
                _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_1;
            else if (level == 2)
                _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_2;
            else if (level == 3)
                _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_3;
            else
                _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_4;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }
    }
}