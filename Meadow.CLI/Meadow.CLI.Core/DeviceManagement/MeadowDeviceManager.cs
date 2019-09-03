﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using MeadowCLI.Hcom;
using static MeadowCLI.DeviceManagement.MeadowFileManager;

namespace MeadowCLI.DeviceManagement
{
    /// <summary>
    /// TODO: put device enumeration and such stuff here.
    /// </summary>
    public static class MeadowDeviceManager
    {
        public static ObservableCollection<MeadowDevice> AttachedDevices = new ObservableCollection<MeadowDevice>();

        public static MeadowDevice CurrentDevice { get; set; } //short cut for now but may be useful

        static HcomMeadowRequestType _meadowRequestType;

        static MeadowDeviceManager()
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

        public static void FindAttachedMeadowDevices()
        {
            foreach (var d in AttachedDevices)
            {
                d?.SerialPort?.Close();
            }
            AttachedDevices.Clear();

            foreach (var s in SerialPort.GetPortNames())
            {
                //limit Mac searches to tty.usb*, Windows, try all COM ports
                if(Environment.OSVersion.Platform != PlatformID.Unix ||
                    s.Contains("tty.usb"))
                {
                    var meadow = new MeadowDevice(s, $"Meadow F7 ({s})");
                    AttachedDevices.Add(meadow);
                }
            }
        }

        //we'll delete this soon
        public static ObservableCollection<MeadowDevice> FindConnectedDevices()
        {
            FindAttachedMeadowDevices();

            return AttachedDevices;
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

        public static void NshEnable(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_ENABLE_DISABLE_NSH;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint) 1);
        }

        public static void MonoDisable(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_MONO_DISABLE;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void MonoEnable(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_MONO_ENABLE;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void MonoRunState(MeadowDevice meadow)
        {
             _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_MONO_RUN_STATE;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void GetDeviceInfo(MeadowDevice meadow)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_GET_DEVICE_INFORMATION;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType);
        }

        public static void SetDeveloper1(MeadowDevice meadow, int userData)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_1;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)userData);
        }
        public static void SetDeveloper2(MeadowDevice meadow, int userData)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_2;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)userData);
        }
        public static void SetDeveloper3(MeadowDevice meadow, int userData)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_3;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)userData);
        }

        public static void SetDeveloper4(MeadowDevice meadow, int userData)
        {
            _meadowRequestType = HcomMeadowRequestType.HCOM_MDOW_REQUEST_DEVELOPER_4;

            new SendTargetData(meadow.SerialPort).SendSimpleCommand(_meadowRequestType, (uint)userData);
        }

        public static void EnterEchoMode(MeadowDevice meadow)
        {
            if (meadow == null)
            {
                Console.WriteLine("No current device");
                return;
            }

            if (meadow.SerialPort == null)
            {
                Console.WriteLine("No current serial port");
                return;
            }

            meadow.Initialize(true);
        }
    }
}