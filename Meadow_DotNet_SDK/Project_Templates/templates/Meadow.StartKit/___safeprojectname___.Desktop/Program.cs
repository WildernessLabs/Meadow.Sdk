using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Logging;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.DT;

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