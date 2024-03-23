using SUS.MvcFramework;

namespace Suls
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateHostAsync(new StartUp());
        }
    }
}
