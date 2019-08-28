using System;
using System.Threading.Tasks;
using MeadowCLI.DeviceManagement;

namespace CLICoreTestApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MeadowDeviceManager.FindAttachedMeadowDevices();

            var meadow = new MeadowDevice("//dev//tty.usbmodem01");
            MeadowDeviceManager.CurrentDevice = meadow;
            meadow.Initialize();

            FileWork(meadow);//fire and forget ... we'll just watch the console output

            Console.ReadKey();
        }

        static async Task FileWork (MeadowDevice meadow)
        {
            // await WriteFile(meadow, "App.exe");

            await meadow.DeployRequiredLibs(System.Reflection.Assembly.GetExecutingAssembly().Location);
            await GetListOfFiles(meadow);
        }

        static Task<bool> WriteFile(MeadowDevice meadow, string filename, string path)
        {
            return meadow.WriteFile(filename, path);
        }

        static async Task GetListOfFiles(MeadowDevice meadow)
        {
            var files = await meadow.GetFilesOnDevice();

            Console.WriteLine($"Found {files.Count} files");
        }
    }
}
