using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotQuest
{
    class Program
    {
        static string token { get; set; } = "1817315659:AAHkiFiSVsyVoIO3v3ZwLf3j1mh-3DFt4uw";
        static Dictionary<long, User> usersIds;
        static List<string> faultMessages = new List<string>() { "Диана👁👄👁, ну йомаё, давай еще раз",
            "Это даже ребенок👶 решит... еще раз!!!",
            "Не правильно, давай включайся!💡",
            "🗺У нас еще целый квест впереди, давай угадывай!🗺",
            "Ди, ну ты канешна вапше...🕊🕊🕊",
            "Пока ты думаешь, я пойду ноготочки сделаю💅💅💅",
            "Вы тупите, хотите об этом поговорить?🧠🗿",
            "Форта уже покекала💩"};
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
                            await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/MonoRight/TelegramBotQuest/blob/master/953e4bb00c9e2267f8c1bf5078683e70.jpg?raw=true",
                                caption: "<b>Локация</b>. <i><a href=\"https://www.google.com.ua/maps/place/50%C2%B029'47.5%22N+30%C2%B031'38.7%22E/@50.496533,30.5268798,19z/data=!3m1!4b1!4m6!3m5!1s0x0:0x0!7e2!8m2!3d50.4965334!4d30.5274266?hl=ru\"> Google maps</a></i>",
                                parseMode: ParseMode.Html
                                );
                            await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку.", replyMarkup: GetButtons());
                            usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                        }
                        else
                        {
                            await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                        }
                        break;

                    case 1:
                        if(msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].SecondMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Ты наверное уже знаешь ответ😏...", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].SecondMessage = true;                 
                                return;
                            }
                        }
                        if (usersIds[chatId].SecondMessage == true)
                        {
                            if (msg.Text.ToLower() == "утконос")
                            {
                                await client.SendTextMessageAsync(chatId, "Карасава!");

                                //тут должна быть фотка дани какая то там всратая маска
                                await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку.", replyMarkup: GetButtons());
                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                            }
                        }
                        break;

                    case 2:
                        if (msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].ThirdMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Быстро ты нашла😎. Я жду ответа😏", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].ThirdMessage = true;
                                return;
                            }
                        }
                        if (usersIds[chatId].ThirdMessage == true)
                        {
                            if (msg.Text.ToLower() == "корабль")
                            {
                                await client.SendTextMessageAsync(chatId, "Ого ты молодец, ты все ближе к своим друзьям. Отправляйся к себе домой и найди йогуртницу.");
                                await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку.", replyMarkup: GetButtons());
                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                            }
                        }              
                        break;

                    case 3:
                        if (msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].FourthMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Ты что Флеш??? Ну ладно... (Подсказка: формат ответа \"XX.XXXXXX, XX.XXXXXX\")", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].FourthMessage = true;
                                return;
                            }
                        }
                        if (usersIds[chatId].FourthMessage == true)
                        {
                            if (msg.Text == "50.493638, 30.527765")
                            {
                                await client.SendTextMessageAsync(chatId, "Так чего ты ждешь? Ищи это место в гугл картах!🗺🗺🗺");
                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                            }
                        }
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

        private static IReplyMarkup ReplyKeyboardRemove()
        {
            return new ReplyKeyboardRemove();
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                {
                    new List<KeyboardButton>(){ new KeyboardButton {Text = "🌟🌟Нашла🌟🌟" } }
                }
            };
        }

        private static string RandomFaultMessage()
        {
            Random rnd = new Random();
            int i = rnd.Next(1, faultMessages.Count + 1);
            return faultMessages[i - 1];
        }
    }
}

