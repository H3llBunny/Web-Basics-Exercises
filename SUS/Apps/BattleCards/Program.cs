
using SUS.MvcFramework;

namespace BattleCards
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateHostAsync(new Startup(), 80);
        }
    }
}
