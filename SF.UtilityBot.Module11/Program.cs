using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using SF.UtilityBot.Module11.Configuration;
using SF.UtilityBot.Module11.Controllers;
using SF.UtilityBot.Module11.Services;

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
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            //Подключаем хранилище пользовательских данных в памяти
            services.AddSingleton<IStorage, MemoryStorage>();

            //Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient с токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new
            TelegramBotClient(appSettings.BotToken));

            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "7258438892:AAEH7Tj1-RPz17B6JUsToJORqBOY6aa3rcE",
            };  
        }
    }
}
