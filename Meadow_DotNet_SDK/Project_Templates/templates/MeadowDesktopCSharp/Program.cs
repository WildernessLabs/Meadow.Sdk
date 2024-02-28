using Meadow;
using System;
using System.Threading.Tasks;

namespace MeadowApplication.Template;

public class Program
{
    public static async Task Main(string[] args)
    {
#if (Framework == net8.0-windows)
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
#endif

        await MeadowOS.Start(args);
    }
}