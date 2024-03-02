using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace MyProject.Core
{

    public class DisplayService
    {
        private readonly DisplayScreen? screen;

        private Temperature currentTemp;
        private SetPoints setpoints;

        private DisplayMode displayMode = DisplayMode.None;
        private ThermostatMode thermostatMode = ThermostatMode.Off;

        private NormalLayout normalLayout;
        private EditHeatLayout editHeatLayout;
        private EditCoolLayout editCoolLayout;

        public DisplayService(
            IPixelDisplay? display,
            Temperature currentTemp,
            SetPoints setPoints)
        {
            this.currentTemp = currentTemp;
            setpoints = setPoints;
            if (display != null)
            {
                var theme = new DisplayTheme
                {
                    Font = new Font12x20()
                };

                screen = new DisplayScreen(
                    display,
                    theme: theme);

                GenerateLayouts(screen);
            }
        }

        private void GenerateLayouts(DisplayScreen screen)
        {
            normalLayout = new NormalLayout(screen)
            {
                IsVisible = true,
            };

            editHeatLayout = new EditHeatLayout(screen)
            {
                IsVisible = false
            };

            editCoolLayout = new EditCoolLayout(screen)
            {
                IsVisible = false
            };

            screen.Controls.Add(
                normalLayout,
                editHeatLayout,
                editCoolLayout);
        }

        public Task UpdateCurrentTemperature(Temperature temperature)
        {
            currentTemp = temperature;
            UpdateDisplay();

            return Task.CompletedTask;
        }

        public Task UpdateHeatTo(Temperature temperature)
        {
            setpoints.HeatTo = temperature;
            UpdateDisplay();

            return Task.CompletedTask;
        }

        public Task UpdateCoolTo(Temperature temperature)
        {
            setpoints.CoolTo = temperature;
            UpdateDisplay();

            return Task.CompletedTask;
        }

        public Task UpdateDisplayMode(DisplayMode mode)
        {
            displayMode = mode;
            UpdateDisplay();

            return Task.CompletedTask;
        }

        public Task UpdateThermostatMode(ThermostatMode mode)
        {
            thermostatMode = mode;
            UpdateDisplay();

            return Task.CompletedTask;
        }

        private void UpdateDisplay()
        {
            if (screen == null)
            {
                return;
            }

            switch (displayMode)
            {
                case DisplayMode.None:
                    // just running
                    normalLayout.IsVisible = true;
                    editCoolLayout.IsVisible = false;
                    editHeatLayout.IsVisible = false;
                    normalLayout.DisplayTemperature = currentTemp;
                    normalLayout.Mode = thermostatMode;
                    break;
                case DisplayMode.EditHeatTo:
                    // editing heat to setpoint
                    normalLayout.IsVisible = false;
                    editCoolLayout.IsVisible = false;
                    editHeatLayout.IsVisible = true;
                    editHeatLayout.SetPoint = setpoints.HeatTo!.Value;
                    break;
                case DisplayMode.EditCoolTo:
                    // editing cool to setpoint
                    normalLayout.IsVisible = false;
                    editCoolLayout.IsVisible = true;
                    editHeatLayout.IsVisible = false;
                    editCoolLayout.SetPoint = setpoints.CoolTo!.Value;
                    break;
            }

            /*
            _graphics.DrawText(0, 0, "Hello Start Kit", Color.White);

            _graphics.DrawText(0, 30, $"Current: {_currentTemp.Celsius:N1}°C", Color.White);

            _graphics.DrawText(0, 60, $"Target: {_targetTemp.Celsius:N1}°C", Color.White);

            _graphics.DrawText(0, 90, $"Thermostat Mode: {_thermostatMode}", Color.White);

            _graphics.DrawText(0, 120, $"Display Mode: {_displayMode}", Color.White);

            if (_thermostatMode == ThermostatMode.Off)
            {
                _graphics.DrawText(0, 150, "Off", Color.LightGray);
            }
            else if (_currentTemp == _targetTemp)
            {
                _graphics.DrawText(0, 150, "At Target", Color.Green);
            }
            else if (_currentTemp < _targetTemp && _thermostatMode == ThermostatMode.Heat)
            {
                _graphics.DrawText(0, 150, "Heating", Color.Red);
            }
            else if (_currentTemp > _targetTemp && _thermostatMode == ThermostatMode.Cool)
            {
                _graphics.DrawText(0, 150, "Cooling", Color.Blue);
            }

            _graphics.Show();
            */
        }
    }
}