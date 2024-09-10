using System;
using System.IO;
using System.Threading.Tasks;
using Meadow.Foundation.Serialization;
using Meadow.Units;

namespace ___SafeProjectName___.Core;

public class AppConfigSettings
{
    public Temperature.UnitType Units { get; set; }
    public double ThresholdC { get; set; }
}

public class ConfigurationController
{
    private const string SettingsFileName = "settings.json";

    public Temperature.UnitType Units { get; set; }
    public Temperature ThresholdTemp { get; set; }

    public ConfigurationController()
    {
        Units = Temperature.UnitType.Celsius;
        ThresholdTemp = 22.5.Celsius();

        Load();
    }

    public void Load()
    {
        if (File.Exists(SettingsFileName))
        {
            var json = File.ReadAllText(SettingsFileName);
            var s = MicroJson.Deserialize<AppConfigSettings>(json);
            Units = s.Units;
            ThresholdTemp = s.ThresholdC.Celsius();
        }
    }

    public Task Save()
    {
        return Task.Run(() =>
        {
            var cfg = new AppConfigSettings
            {
                Units = Units,
                ThresholdC = ThresholdTemp.Celsius
            };
            var json = MicroJson.Serialize(cfg);
            if (File.Exists(SettingsFileName))
            {
                File.Delete(SettingsFileName);
            }
            File.WriteAllText(SettingsFileName, json);
        });
    }
}