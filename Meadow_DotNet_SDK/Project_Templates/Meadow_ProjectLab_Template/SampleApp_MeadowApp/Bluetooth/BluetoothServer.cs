using Meadow.Gateways.Bluetooth;
using Meadow.Units;
using SampleApp.Controllers;
using System;

namespace SampleApp.MeadowApp.Bluetooth
{
    public class BluetoothServer
    {
        private static readonly Lazy<BluetoothServer> instance =
            new Lazy<BluetoothServer>(() => new BluetoothServer());
        public static BluetoothServer Current => instance.Value;

        Definition bleTreeDefinition;
        ICharacteristic TemperatureCharacteristic;
        ICharacteristic HumitidyCharacteristic;
        ICharacteristic PressureCharacteristic;

        MainAppController mainAppController;

        public bool IsInitialized { get; private set; }

        private BluetoothServer() { }

        public void Initialize(MainAppController appController)
        {
            mainAppController = appController;

            bleTreeDefinition = GetDefinition();

            mainAppController.ConditionsUpdated += MainAppController_ConditionsUpdated;

            MeadowApp.Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            IsInitialized = true;
        }

        private void MainAppController_ConditionsUpdated(object sender, Models.AtmosphericConditionsModel e)
        {
            // update your characteristic values.
        }

        public void SetEnvironmentalCharacteristicValue((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) value)
        {
            TemperatureCharacteristic.SetValue((int)value.Temperature?.Celsius);
            HumitidyCharacteristic.SetValue((int)value.Humidity?.Percent);
            PressureCharacteristic.SetValue((int)value.Pressure?.Millibar);
        }

        Definition GetDefinition()
        {
            TemperatureCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.TEMPERATURE),
                uuid: BluetoothCharacteristics.TEMPERATURE,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            HumitidyCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.HUMIDITY),
                uuid: BluetoothCharacteristics.HUMIDITY,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            PressureCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.PRESSURE),
                uuid: BluetoothCharacteristics.PRESSURE,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            var service = new Service(
                name: "Service",
                uuid: 253,
                TemperatureCharacteristic,
                HumitidyCharacteristic,
                PressureCharacteristic
            );

            return new Definition("ProjectLab", service);
        }
    }
}