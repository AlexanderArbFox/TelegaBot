using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ServiceBot.HostExtentions
{
    public static class WindowsHostExtentions
    {
        public static async Task RunService(this IHostBuilder hostBuilder)
        {
            if (!Environment.UserInteractive) // Запускаем сервисом, если это джоб
            {
                await hostBuilder.RunConsoleAsync();
            }
            else
            {
                await hostBuilder.RunConsoleAsync(); // запускаем консолью, если это дебаг
            }
        }
    }
}
