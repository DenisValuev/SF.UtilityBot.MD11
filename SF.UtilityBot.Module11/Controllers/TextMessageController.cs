using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using SF.UtilityBot.Module11.Services;
//using SF.UtilityBot.Module11.Services;

namespace SF.UtilityBot.Module11.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    //Объект, представляющий кнопки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Кол-во символов", $"tsk1"),
                        InlineKeyboardButton.WithCallbackData($"Сумма чисел", $"tsk2")
                        
                    });
                    //Передаем кнопки вместе с сообщением
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Наш бот выполняет подсчет количества символов в тексте. а также находит сумму введенных чисел</b> {Environment.NewLine}" +
                        $"Выберите действие. {Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    string userTaskCode = _memoryStorage.GetSession(message.Chat.Id).taskCode;
                    switch (userTaskCode)
                    {
                        case "tsk1":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Количество символов равно: {message.Text.Length}", cancellationToken: ct);
                            break;
                        case "tsk2":
                            try
                            {
                                int sum = 0;
                                var str = message.Text;
                                string[] number = str.Split(' ');
                                foreach (var numberItem in number)
                                {
                                    sum += int.Parse(numberItem);
                                }
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел равна: {sum}", cancellationToken: ct);
                            }
                            catch (Exception)
                            {
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Неверный формат сообщения для выбранного задания", cancellationToken: ct);
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
