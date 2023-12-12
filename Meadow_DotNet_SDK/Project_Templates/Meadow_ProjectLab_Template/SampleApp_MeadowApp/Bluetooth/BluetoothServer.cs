using Meadow.Gateways.Bluetooth;
using SampleApp.Controllers;

namespace SampleApp.MeadowApp.Bluetooth
{
    public class BluetoothServer
    {
        public static BluetoothServer Current { get; protected set; }

        Definition bleTreeDefinition;
        ICharacteristic temperatureCharacteristic;
        ICharacteristic humitidyCharacteristic;
        ICharacteristic pressureCharacteristic;

        MainAppController mainAppController;

        public BluetoothServer(MainAppController appController)
        {
            bleTreeDefinition = GetDefinition();

            mainAppController = appController;
            mainAppController.ConditionsUpdated += MainAppControllerConditionsUpdated;

            MeadowApp.Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);
        }

        private void MainAppControllerConditionsUpdated(object sender, Models.AtmosphericConditionsModel e)
        {
            temperatureCharacteristic.SetValue(e.Temperature.Value.Celsius);
            humitidyCharacteristic.SetValue(e.Humidity.Value.Percent);
            pressureCharacteristic.SetValue(e.Pressure.Value.StandardAtmosphere);
        }

        Definition GetDefinition()
        {
            temperatureCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.TEMPERATURE),
                uuid: BluetoothCharacteristics.TEMPERATURE,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            humitidyCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.HUMIDITY),
                uuid: BluetoothCharacteristics.HUMIDITY,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            pressureCharacteristic = new CharacteristicString(
                name: nameof(BluetoothCharacteristics.PRESSURE),
                uuid: BluetoothCharacteristics.PRESSURE,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            var service = new Service(
                name: "Service",
                uuid: 253,
                temperatureCharacteristic,
                humitidyCharacteristic,
                pressureCharacteristic
            );

            return new Definition("ProjectLab", service);
        }
    }
}