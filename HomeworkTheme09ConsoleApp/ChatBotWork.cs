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

            switch (e.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    if (e.Message.Text.ToLower() == "/start")
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, $"Привет, {e.Message.Chat.FirstName}! \n Я бот \"без-забот\", вот {BotCommandTextMessage()}");
                    }
                    else
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, $"Мне не удалось распознать ваш запрос,\n{BotCommandTextMessage()}");
                    }
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Photo:
                    Console.WriteLine(e.Message.Photo.Last().FileId);
                    Console.WriteLine(e.Message.Photo.Last().FileSize);
                    string fileNamePhoto = e.Message.Photo.Last().FileId.ToString() + ".jpg";

                    DownLoad(e.Message.Photo.Last().FileId, e.Message.Chat.Id, fileNamePhoto);
                    break;

                case Telegram.Bot.Types.Enums.MessageType.Audio:
                    Console.WriteLine(e.Message.Audio.FileId);
                    Console.WriteLine(e.Message.Audio.FileName);
                    Console.WriteLine(e.Message.Audio.FileSize);

                    DownLoad(e.Message.Audio.FileId, e.Message.Chat.Id, e.Message.Audio.FileName);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    Console.WriteLine(e.Message.Video.FileId);
                    Console.WriteLine(e.Message.Video.FileName);
                    Console.WriteLine(e.Message.Video.FileSize);

                    DownLoad(e.Message.Video.FileId, e.Message.Chat.Id, e.Message.Video.FileName);
                    break;
              
                case Telegram.Bot.Types.Enums.MessageType.Voice:
                    Console.WriteLine(e.Message.Voice.FileId);
                    Console.WriteLine(e.Message.Voice.FileSize);

                    DownLoad(e.Message.Voice.FileId, e.Message.Chat.Id, e.Message.Voice.FileId);
                    break;
               
                case Telegram.Bot.Types.Enums.MessageType.Document:
                    Console.WriteLine(e.Message.Document.FileId);
                    Console.WriteLine(e.Message.Document.FileName);
                    Console.WriteLine(e.Message.Document.FileSize);
                    
                    DownLoad(e.Message.Document.FileId, e.Message.Chat.Id, e.Message.Document.FileName);
                    break;

                default:
                    break;
            }

        }

        private static string BotCommandTextMessage()
        {
            string stringCommand = $"список команд, которые я понимаю:" +
                $"\n /sky - узнать прогноз погоды в вашем городе" +
                $"\n /files - просмотреть список загруженных файлов" +
                $"\n также я сохраняю аудиосообщения, картинки и произвольные файлы" +
                $"\n и позволяю скачать выбранный файл";
            return stringCommand;
        }

        static async void DownLoad(string fileId, long userId, string fileName)
        {

            string pathDir = @"C:\FileUsers\";

            string pathSubDir = userId.ToString();

            DirectoryInfo dirInfo = new DirectoryInfo(pathDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();

            }

            Directory.CreateDirectory($"{pathDir}/{pathSubDir}");

            string pathFull = Path.Combine(pathDir, pathSubDir, fileName);

            try
            {
                var file = await bot.GetFileAsync(fileId);

                using (var fs = new FileStream(pathFull, FileMode.Create))
                {
                    await bot.DownloadFileAsync(file.FilePath, fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки: " + ex.Message);
            }
        }

       
    }
}
