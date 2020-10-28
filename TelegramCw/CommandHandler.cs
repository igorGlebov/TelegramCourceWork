using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramCw.Extensions;
using TelegramCw.Tools;

namespace TelegramCw
{
    /// <summary>
    /// Класс, обрабатывающий поступающие через Telegram команды.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Собсна, бот.
        /// </summary>
        private readonly TelegramBotClient _bot;

        /// <summary>
        /// Конфиг.
        /// </summary>
        private readonly Infrastructure.Config _config;

        /// <summary>
        /// Список заблокированных процессов.
        /// </summary>
        public List<string> BlockedProcesses => _config.BlockedProcesses;

        /// <summary>
        /// Вызывается при добавлении нового процесса в список.
        /// </summary>
        public EventHandler<string> OnProcessAdded;
        
        public CommandHandler()
        {
            _config = DataSerializer.Deserialize<Infrastructure.Config>();

            if (_config.Token == null)
            {
                Console.Write("Введите id бота: ");
                _config.Token = Console.ReadLine();
                DataSerializer.Serialize(_config);
            }

            _bot = new TelegramBotClient(_config.Token);
            _bot.OnUpdate += OnUpdate;
            _bot.StartReceiving();

            Console.WriteLine("Bot start listening...");
        }

        /// <summary>
        /// Останавливает прием сообщений ботом.
        /// </summary>
        public void StopReceiving() => _bot.StopReceiving();
        
        /// <summary>
        /// Обработчик события обновления чата.
        /// </summary>
        private async void OnUpdate(object? sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;

            if (message.Type == MessageType.Text)
            {
                var text = message.Text;
                var chatId = message.Chat.Id;

                switch (text)
                {
                    case Infrastructure.Commands.GET_PROCESSES:
                        await _bot.SendTextMessageAsync(chatId, "Высылаю список процессов...");
                        var processes = ProcessesWorker.GetProcesses();
                        await _bot.SendStrings(chatId, processes);
                        break;

                    case Infrastructure.Commands.UNLOG:
                        SystemWorker.Unlog();
                        await _bot.SendTextMessageAsync(chatId, "Произведен выход из системы!");
                        break;

                    case Infrastructure.Commands.GET_SCREEN:
                        await _bot.SendTextMessageAsync(chatId, "Высылаю снимок экрана...");
                        var path = ImagesWorker.GetScreenshot();
                        await _bot.SendImage(chatId, path);
                        break;

                    case Infrastructure.Commands.GET_USB:
                        await _bot.SendTextMessageAsync(chatId, "Высылаю список USB-устройств...");
                        var devices = UsbWorker.GetDevices();
                        await _bot.SendStrings(chatId, devices);
                        break;
                    
                    case Infrastructure.Commands.GET_CAM:
                        if (CamWorker.IsCameraExist)
                        {
                            await _bot.SendTextMessageAsync(chatId, "Высылаю снимок с вебкамеры...");
                            var camPath = CamWorker.GetCam();
                            await _bot.SendImage(chatId, camPath);
                        }
                        else
                        {
                            await _bot.SendTextMessageAsync(chatId, "Нет камеры.");
                        }
                        break;

                    default:
                        if (text.Contains(Infrastructure.Commands.ADD_BLOCK))
                        {
                            var name = text.Replace($"{Infrastructure.Commands.ADD_BLOCK} ", string.Empty);
                            BlockedProcesses.Add(name);
                            DataSerializer.Serialize(_config);
                            OnProcessAdded?.Invoke(this, name);
                            await _bot.SendTextMessageAsync(chatId, "Процесс успешно заблокирован.");
                        }
                        
                        else if (text.Contains(Infrastructure.Commands.REMOVE_BLOCK))
                        {
                            var name = text.Replace($"{Infrastructure.Commands.REMOVE_BLOCK} ", string.Empty);
                            BlockedProcesses.Remove(name);
                            DataSerializer.Serialize(_config);
                            await _bot.SendTextMessageAsync(chatId, "Процесс успешно разблокирован.");
                        }

                        else
                        {
                            await _bot.SendTextMessageAsync(chatId, "Неизвестная команда. Попробуйте еще раз.");
                        }
                        break;
                }
            }
        }
    }
}