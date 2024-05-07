using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    internal class MessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IMessageHandler _handler;
        private readonly IStorage _memoryStorage;

        public MessageController(ITelegramBotClient telegramBotClient, IMessageHandler handler, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _handler = handler;
            _memoryStorage = memoryStorage;


        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            if(message.Text == "/start")
            {
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($"Подсчет букв" , $"count"),
                        InlineKeyboardButton.WithCallbackData($"Сложение чисел" , $"sum")
                });

                string text = "Этот бот умеет:\n1. Подсчитывать количество символов в сообщении.\n2. Считать сумму чисел,переданных сообщением.\n" +
                    "Выбирите нужную функцию.";
                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, text, cancellationToken: ct,
                    parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            else
            {
                try
                {
                    string selectFunction = _memoryStorage.GetSession(message.Chat.Id).SelectFunction;
                    int sum = _handler.Counting(message.Text, selectFunction);
                    if (selectFunction == "count") await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении: {sum} символов.", cancellationToken: ct);
                    else if (selectFunction == "sum") await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
                    else throw new Exception();
                }
                catch (Exception)
                {

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Для корректного подсчета суммы, введите в сообщении только числа.", cancellationToken: ct);
                }              
            }
           
        }

    }
}
