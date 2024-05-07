using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient botClient, IStorage memoryStorage)
        {
            _botClient = botClient;

            _memoryStorage = memoryStorage;
        }
       public async Task Handle(CallbackQuery? callbackQuery, CancellationToken cancellationToken)
       {
            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).SelectFunction = callbackQuery.Data;

            string SelectFunction = callbackQuery.Data switch
            {
                "count" => "Подсчет букв",
                "sum" => "Сложение чисел",
                _ => String.Empty
            };

            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            // Отправляем в ответ уведомление о выборе
            await _botClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"Выбрана функция - {SelectFunction}.{Environment.NewLine}", cancellationToken: cancellationToken, parseMode: ParseMode.Html);


        }
    }
}
