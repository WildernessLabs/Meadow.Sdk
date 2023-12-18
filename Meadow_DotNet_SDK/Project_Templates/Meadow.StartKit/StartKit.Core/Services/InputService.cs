using StartKit.Core.Contracts;

namespace StartKit.Core;

public class InputService
{
    private DisplayMode _displayMode;
    private ThermostatMode _thermostatMode;

    public event EventHandler<DisplayMode> DisplayModeChanged;
    public event EventHandler<ThermostatMode> ThermostatModeChanged;
    public event EventHandler HeatToIncremented;
    public event EventHandler HeatToDecremented;
    public event EventHandler CoolToIncremented;
    public event EventHandler CoolToDecremented;

    public InputService(IStartKitPlatform platform)
    {
        if (platform.GetRightButton() is { } rb)
        {
            rb.Clicked += OnRightButtonClicked;
        }
        if (platform.GetLeftButton() is { } lb)
        {
            lb.Clicked += OnLeftButtonClicked;
        }
        if (platform.GetUpButton() is { } ub)
        {
            ub.Clicked += OnUpButtonClicked;
        }
        if (platform.GetDownButton() is { } db)
        {
            db.Clicked += OnDownButtonClicked;
        }
    }

    public DisplayMode DisplayMode
    {
        get => _displayMode;
        private set
        {
            if (value == DisplayMode) return;
            _displayMode = value;

            DisplayModeChanged?.Invoke(this, DisplayMode);
        }
    }

    public ThermostatMode ThermostatMode
    {
        get => _thermostatMode;
        private set
        {
            if (value == ThermostatMode) return;
            _thermostatMode = value;

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
