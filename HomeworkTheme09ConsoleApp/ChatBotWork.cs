using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HomeworkTheme09ConsoleApp
{
    class ChatBotWork
    {
        static TelegramBotClient bot;

        static string token = File.ReadAllText(@"C:\SkillboxC#\tokenOhMyFirstBot2022_bot.txt");

        public static void StartWork()
        {
            bot = new TelegramBotClient(token);
            Console.WriteLine("Запущен бот: " + bot.GetMeAsync().Result.FirstName);
            bot.OnMessage += MessageListener;
            bot.StartReceiving();
            Console.ReadKey();
        }

        private static void MessageListener(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = $"{DateTime.Now.ToLongTimeString()}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

            Console.WriteLine($"{text} TypeMessage: {e.Message.Type.ToString()}");

            if (e.Message.Text.ToLower()=="/start")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id,$"Привет, {e.Message.Chat.FirstName}! \n Я бот \"без-забот\", вот {BotCommand()}");
            }
            else
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, $"Мне не удалось распознать ваш запрос,\n{BotCommand()}");
            }

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                Console.WriteLine(e.Message.Document.FileId);
                Console.WriteLine(e.Message.Document.FileName);
                Console.WriteLine(e.Message.Document.FileSize);

                //DownLoad(e.Message.Document.FileId, e.Message.Document.FileName);
            }

            if (e.Message.Text == null) return;

        }

        private static string BotCommand()
        {
            string stringCommand = $"список команд, которые я понимаю:" +
                $"\n \\sky - узнать прогноз погоды в вашем городе" +
                $"\n \\files - просмотреть список загруженных файлов" +
                $"\n также я сохраняю аудиосообщения, картинки и произвольные файлы" +
                $"\n и позволяю скачать выбранный файл";
            return stringCommand;
        }
    }
}
