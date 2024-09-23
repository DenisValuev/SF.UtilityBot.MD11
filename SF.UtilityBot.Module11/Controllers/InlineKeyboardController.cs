using SF.UtilityBot.Module11.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SF.UtilityBot.Module11.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
            {
                return;
            }
            //Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).taskCode = callbackQuery.Data;

            //Генерируем информационное сообщение
            string numberTask = callbackQuery.Data switch
            {
                "tsk1" => " Кол-во символов",
                "tsk2" => " Сумма чисел",
                _ => string.Empty,
            };

            //Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>{numberTask}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
