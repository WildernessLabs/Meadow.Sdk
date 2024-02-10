using StartKit.Core.Contracts;

namespace StartKit.Core;

public class InputService
{
    public InputService(IStartKitHardware platform)
    {
        if (platform.UpButton is { } ub)
        {
            ub.Clicked += OnUpButtonClicked;
        }
        if (platform.DownButton is { } db)
        {
            db.Clicked += OnDownButtonClicked;
        }
    }

    private void OnUpButtonClicked(object sender, EventArgs e)
    {

    }

    private void OnDownButtonClicked(object sender, EventArgs e)
    {
    }
}
