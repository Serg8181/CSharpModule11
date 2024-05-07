using System;
using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using UtilityBot.Controllers;
using UtilityBot.Configuration;
using UtilityBot.Services;
using UtilityBot.Models;


namespace UtilityBot
{
    class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");

            // Запускаем сервис
            await host.RunAsync();

            Console.WriteLine("Сервис остановлен");

        }
        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();

            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddSingleton<IMessageHandler, MessageService>();

            // Подключаем контроллеры сообщений и кнопок

            services.AddTransient<MessageController>();

            services.AddTransient<InlineKeyboardController>();

            services.AddTransient<DefaultMessageController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();

        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
               
                BotToken = "6430819643:AAEzCb3f36InHkDPEfgXhLgmX9vh1vN94aI",
               
            };
        }
    }
}
