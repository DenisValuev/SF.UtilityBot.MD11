using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SF.UtilityBot.Module11.Controllers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SF.UtilityBot.Module11
{
    internal class Bot : BackgroundService
    {
        // Клиент к Telegram Bot API
        private ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        private InlineKeyboardController _inlineKeyboardController;
        private TextMessageController _textMessageController;
        private DefaultMessageController _defaultMessageController;
        public Bot(ITelegramBotClient telegramClient,
            InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController,
            DefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, //Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }

        /// <summary>
        /// Метод служит для обротки событий
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Обрабатываем нажатия на кнопки из Telegram Bot API
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            //Обрабатываем входящие сообщения из Telegram Bot API
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        break;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        break;

                    //case MessageType.Text:
                    //    await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken : cancellationToken);
                    //    return;
                    //default:
                    //    await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Данный тип сообщения не поддерживается. Пожалуйста отправьте текст.", cancellationToken: cancellationToken);
                    //    return;
                }
            }
        }
        /// <summary>
        /// Метод служит для обработки ошибок
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //Задаем сообщение об ошибки в зависимости от того. какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error: \n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            //Выводим на консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            //Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}
