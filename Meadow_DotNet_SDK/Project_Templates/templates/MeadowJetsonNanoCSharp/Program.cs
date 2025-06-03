using Meadow;
using System.Threading.Tasks;

namespace MeadowApplication.Template;

public class Program
{
    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}