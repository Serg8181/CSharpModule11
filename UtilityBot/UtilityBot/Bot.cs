using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Controllers;


namespace UtilityBot
{
    internal class Bot : BackgroundService
    {

        private ITelegramBotClient _telegramClient;
        private InlineKeyboardController _inlineKeyboardController;
        private MessageController _messageController;
        private DefaultMessageController _defaultMessageController;

        public Bot(ITelegramBotClient telegramClient, InlineKeyboardController inlineKeyboardController,MessageController messageControlle, DefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _messageController = messageControlle;
            _defaultMessageController = defaultMessageController;
            
        }

        async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;

            }

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {

                    case MessageType.Text:
                        await _messageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }

        Task HandleError(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
        {
                var errorMessage = ex switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => ex.ToString()
                };

                // Выводим в консоль информацию об ошибке
                Console.WriteLine(errorMessage);

                // Задержка перед повторным подключением
                Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
                Thread.Sleep(10000);

                return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdate,
                HandleError,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }
    }
}
