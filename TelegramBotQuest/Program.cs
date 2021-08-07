using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramBotQuest
{
    class Program
    {
        static string token { get; set; } = "1817315659:AAHkiFiSVsyVoIO3v3ZwLf3j1mh-3DFt4uw";
        static Dictionary<long, User> usersIds;
        static List<string> faultMessages = new List<string>() { "Диана, ну йомаё, давай еще раз", 
            "Это даже ребенок решит... еще раз!!!", 
            "Не правильно, давай включайся!", 
            "У нас еще целый квест впереди, давай угадывай!!!", 
            "Ди, ну ты канешна вапше"  };
        static TelegramBotClient client;

        [Obsolete]
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            usersIds = new Dictionary<long, User>();
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.WriteLine("Enter key to close the bot...");
            Console.ReadKey();
            client.StopReceiving();
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            long chatId = msg.Chat.Id;
            if (msg.Text != null)
            {
                if (!usersIds.ContainsKey(chatId))
                {
                    usersIds.Add(chatId, new User() { UsersIds = chatId });
                }

                switch (usersIds[chatId].ProgressLevel)
                {
                    case 0:
                        if (!usersIds[chatId].FirstMessage)
                        {
                            await client.SendTextMessageAsync(chatId, "Правила такие: на бумаге есть загадка. Подставив правильные слова, получаем разгадку - слово, что есть ключем для следующего шага. Его нужно ввести сюда. Удачи!");
                            await client.SendTextMessageAsync(chatId, "Итак, у тебя есть первое задание. Каков же ответ?");
                            usersIds[chatId].FirstMessage = true;
                            return;
                        }
                        if(msg.Text.ToLower() == "молоток")
                        {
                            await client.SendTextMessageAsync(chatId, "Молодец, ты продвинулась дальше!");
                            await client.SendTextMessageAsync(chatId, "Следующая загадка находится в указаном месте:");
                            //тут прислать фотографию места и координаты
                           await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
                                caption: "<b>Локация</b>. <i>Google maps</i>: <a href=\"https://www.google.com.ua/maps/place/50%C2%B029'47.5%22N+30%C2%B031'38.7%22E/@50.496533,30.5268798,19z/data=!3m1!4b1!4m6!3m5!1s0x0:0x0!7e2!8m2!3d50.4965334!4d30.5274266?hl=ru\"> location</a>",
                                parseMode: ParseMode.Html
                                );
                            usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                        }
                        else
                        {
                            await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                        }
                        break;
                    case 1:
                        await client.SendTextMessageAsync(chatId, "Вы продвинулись");

                        break;
                    default:
                        await client.SendTextMessageAsync(chatId, "Ура! Ты закончила квест!");
                        break;
                }
                //await client.SendLocationAsync(chatId, (float)49.4409860, (float)32.0478000);
                
                //Console.WriteLine($"Message: {msg.Text}");
                //await client.SendTextMessageAsync(chatId, msg.Text);
            }
        }

        static string RandomFaultMessage()
        {
            Random rnd = new Random();
            int i = rnd.Next(1, faultMessages.Count + 1);
            return faultMessages[i - 1];
        }
    }
}
