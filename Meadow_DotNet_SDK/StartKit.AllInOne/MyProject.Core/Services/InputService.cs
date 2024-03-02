using System;
using MyProject.Core.Contracts;

namespace MyProject.Core
{

    public class InputService
    {
        private DisplayMode displayMode;
        private ThermostatMode thermostatMode;

        public event EventHandler<DisplayMode> DisplayModeChanged;
        public event EventHandler<ThermostatMode> ThermostatModeChanged;
        public event EventHandler HeatToIncremented;
        public event EventHandler HeatToDecremented;
        public event EventHandler CoolToIncremented;
        public event EventHandler CoolToDecremented;

        public InputService(IStartKitHardware platform)
        {
            if (platform.RightButton is { } rb)
            {
                rb.Clicked += OnRightButtonClicked;
            }
            if (platform.LeftButton is { } lb)
            {
                lb.Clicked += OnLeftButtonClicked;
            }
            if (platform.UpButton is { } ub)
            {
                ub.Clicked += OnUpButtonClicked;
            }
            if (platform.DownButton is { } db)
            {
                db.Clicked += OnDownButtonClicked;
            }
        }

        public DisplayMode DisplayMode
        {
            get => displayMode;
            private set
            {
                if (value == DisplayMode) return;
                displayMode = value;

                DisplayModeChanged?.Invoke(this, DisplayMode);
            }
        }

        public ThermostatMode ThermostatMode
        {
            get => thermostatMode;
            private set
            {
                if (value == ThermostatMode) return;
                thermostatMode = value;

                ThermostatModeChanged?.Invoke(this, ThermostatMode);
            }
        }

        private void OnUpButtonClicked(object sender, EventArgs e)
        {
            switch (DisplayMode)
            {
                case DisplayMode.EditCoolTo:
                    CoolToIncremented?.Invoke(this, EventArgs.Empty);
                    break;
                case DisplayMode.EditHeatTo:
                    HeatToIncremented?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void OnDownButtonClicked(object sender, EventArgs e)
        {
            switch (DisplayMode)
            {
                case DisplayMode.EditCoolTo:
                    CoolToDecremented?.Invoke(this, EventArgs.Empty);
                    break;
                case DisplayMode.EditHeatTo:
                    HeatToDecremented?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void OnRightButtonClicked(object sender, EventArgs e)
        {
            var newMode = DisplayMode switch
            {
                DisplayMode.None => DisplayMode.EditCoolTo,
                _ => DisplayMode.EditHeatTo
            };

            DisplayMode = newMode;
        }

        private void OnLeftButtonClicked(object sender, EventArgs e)
        {
            var newMode = DisplayMode switch
            {
                DisplayMode.EditHeatTo => DisplayMode.EditCoolTo,
                _ => DisplayMode.None,
            };

            DisplayMode = newMode;
        }
    }
}
