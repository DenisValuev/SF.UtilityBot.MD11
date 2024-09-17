using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

namespace SF.UtilityBot.Module11
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            //Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))//Задаем конфигурацию
                .UseConsoleLifetime() //Позволяет поддерживать приложение активным в консоли
                .Build(); //Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем объект TelegramBotClient с токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new
            TelegramBotClient("7477165269:AAHJjwtxhEVc6ZNXxaxYcYsT8XU7u6Zkz3E"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}
