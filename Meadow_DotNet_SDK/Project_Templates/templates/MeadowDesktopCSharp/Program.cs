using Meadow;
using System;
using System.Threading.Tasks;

namespace MeadowApplication.Template;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
        }

        await MeadowOS.Start(args);
    }
}