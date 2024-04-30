using Meadow;

namespace StartKit.Windows;

public static class Program
{
    private static void Main(string[] args)
    {
#if WINDOWS
        ApplicationConfiguration.Initialize();
#endif
        MeadowOS.Start(args);
    }
}
