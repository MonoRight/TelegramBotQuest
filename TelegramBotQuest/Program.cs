using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        static List<string> faultPhotos = new List<string>()
        {
            "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_1.jpg?raw=true",
            "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_7.jpg?raw=true",
            "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_8.jpg?raw=true",
            "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_9.jpg?raw=true",
            "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_13.jpg?raw=true",
        };

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
                if (msg.Text == "/restart")
                {
                    usersIds.Remove(chatId);
                }
                if (!usersIds.ContainsKey(chatId))
                {
                    usersIds.Add(chatId, new User() { UsersIds = chatId });
                }
        
                switch (usersIds[chatId].ProgressLevel)
                {
                    //Диана дома -> идет на вход/выход из наталки
                    case 0:
                        if (!usersIds[chatId].FirstMessage)
                        {
                            await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_14.jpg?raw=true");
                            await client.SendTextMessageAsync(chatId, "Правила такие: на бумаге есть загадка. Подставив правильные слова, получаем разгадку - слово, что есть ключем для следующего шага. Его нужно ввести сюда. (Сперва найди помощника за лифтом на первом этаже и обращай внимание на QR коды). Удачи!");
                            await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_4.jpg?raw=true",
                                caption: "А вот этот инопришленец украл твоих друзей, освободи их."
                                );
                            await client.SendTextMessageAsync(chatId, "Итак, у тебя есть первое задание. Каков же ответ?");
                            usersIds[chatId].FirstMessage = true;

                            return;
                        }
                        if(msg.Text.ToLower() == "молоток")
                        {
                            if (usersIds[chatId].FirstMessageFirstTryTrue == 0)
                            {
                                await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_2.jpg?raw=true");
                            }
                            await client.SendTextMessageAsync(chatId, "Молодец, ты продвинулась дальше!");
                            await client.SendTextMessageAsync(chatId, "Следующая загадка находится в указаном месте:");
                            await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/MonoRight/TelegramBotQuest/blob/master/photo_2021-08-14_23-59-16.jpg?raw=true",
                                caption: "<b>Локация</b>. <i><a href=\"https://www.google.com.ua/maps/place/50%C2%B030'02.3%22N+30%C2%B031'25.9%22E/@50.5006459,30.5235655,83m/data=!3m1!1e3!4m14!1m7!3m6!1s0x0:0x0!2zNTDCsDMwJzAxLjkiTiAzMMKwMzEnMjUuNiJF!3b1!8m2!3d50.5005244!4d30.5237753!3m5!1s0x0:0x0!7e2!8m2!3d50.5006445!4d30.5238601?hl=ru\"> Google maps</a></i>",
                                parseMode: ParseMode.Html
                                );
                            await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку. (Раньше времени не нажимай, а то получишь по жопи. 3 минуты минимум бежать⏳)", replyMarkup: GetButtons());

                            ThreadSleep();

                            usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                        }
                        else
                        {
                            await client.SendPhotoAsync(chatId, RandomFaultPhoto());
                            await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                            usersIds[chatId].FirstMessageFirstTryTrue++;
                        }
                        break;

                    //На выходе из Наталки -> идет на центральную клумбу искать подсказку на лавочках
                    case 1:
                        if(msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].SecondMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Раз ты нашла загадку, то наверное уже знаешь ответ😏...", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].SecondMessage = true;                 
                                return;
                            }
                        }
                        if (usersIds[chatId].SecondMessage == true)
                        {
                            if (msg.Text.ToLower() == "утконос")
                            {                              
                                await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_3.jpg?raw=true");
                                await client.SendTextMessageAsync(chatId, "Красава!");
                                await client.SendTextMessageAsync(chatId, "Следующая загадка находится в указаном месте:");
                                await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/MonoRight/TelegramBotQuest/blob/master/photo_2021-08-15_00-09-46.jpg?raw=true",
                                caption: "<b>Ты точно знаешь это место!</b> <i><a href=\"https://www.google.com.ua/maps/place/50%C2%B029'46.1%22N+30%C2%B031'26.1%22E/@50.49614,30.5233548,132m/data=!3m2!1e3!4b1!4m14!1m7!3m6!1s0x0:0x0!2zNTDCsDI5JzM3LjEiTiAzMMKwMzEnNDAuMCJF!3b1!8m2!3d50.493638!4d30.527765!3m5!1s0x0:0x0!7e2!8m2!3d50.4961402!4d30.5239021?hl=ru\"> Google maps</a></i>",
                                parseMode: ParseMode.Html
                                );
                                await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку. (Раньше времени не нажимай, а то получишь по жопи. 3 минуты минимум бежать⏳)", replyMarkup: GetButtons());

                                ThreadSleep();
                                ShowJdunPhoto(chatId);

                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendPhotoAsync(chatId, RandomFaultPhoto());
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                                usersIds[chatId].SecondMessageFirstTryTrue++;
                            }
                        }
                        break;

                    //На скамейке в центре Наталки -> идет домой искать йогуртницу
                    case 2:
                        if (msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].ThirdMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Быстро ты нашла😎. А теперь я жду ответа😏", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].ThirdMessage = true;
                                return;
                            }
                        }
                        if (usersIds[chatId].ThirdMessage == true)
                        {
                            if (msg.Text.ToLower() == "корабль")
                            {
                                if (usersIds[chatId].ThirdMessageFirstTryTrue == 0)
                                {
                                    await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_10.jpg?raw=true");
                                }
                                await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_11.jpg?raw=true");
                                await client.SendTextMessageAsync(chatId, "Ого ты молодец, ты все ближе к своим друзьям. Отправляйся к себе домой и найди йогуртницу.");
                                await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только найдешь следующую загадку. (Раньше времени не нажимай, а то получишь по жопи. 4 минуты минимум бежать⏳)", replyMarkup: GetButtons());

                                ThreadSleep();

                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendPhotoAsync(chatId, RandomFaultPhoto());
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                                usersIds[chatId].ThirdMessageFirstTryTrue++;
                            }
                        }              
                        break;

                    //Диана дома нашла йогуртницу и подсказку в ней -> идет на камни на берегу Днепра
                    case 3:
                        if (msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            if (!usersIds[chatId].FourthMessage)
                            {
                                await client.SendTextMessageAsync(chatId, "Ты что Флеш??? Ну ладно... (Подсказка: формат ответа: XX.XXXXXX, XX.XXXXXX)", replyMarkup: ReplyKeyboardRemove());
                                usersIds[chatId].FourthMessage = true;
                                return;
                            }
                        }
                        if (usersIds[chatId].FourthMessage == true)
                        {
                            if (msg.Text == "50.493638, 30.527765")
                            {
                                await client.SendPhotoAsync(chatId, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_5.jpg?raw=true");
                                await client.SendTextMessageAsync(chatId, "Так чего ты ждешь? Ищи это место в гугл картах!🗺🗺🗺");
                                await client.SendPhotoAsync(
                                chatId: e.Message.Chat,
                                photo: "https://github.com/MonoRight/TelegramBotQuest/blob/master/photo_2021-08-15_00-13-40.jpg?raw=true",
                                caption: "<b>Где-то тут</b>. <i><a href=\"https://www.google.com.ua/maps/place/50%C2%B029'37.1%22N+30%C2%B031'40.0%22E/@50.4936319,30.5275616,61m/data=!3m1!1e3!4m5!3m4!1s0x0:0x0!8m2!3d50.493638!4d30.527765?hl=ru\"> Google maps</a></i>",
                                parseMode: ParseMode.Html
                                );
                                await client.SendTextMessageAsync(chatId, "Нажми кнопку, как только дойдешь до указанного места в загадке. (Раньше времени не нажимай, а то получишь по жопи)", replyMarkup: GetButtons());
                                usersIds[chatId].ProgressLevel = usersIds[chatId].ProgressLevel + 1;
                            }
                            else
                            {
                                await client.SendPhotoAsync(chatId, RandomFaultPhoto());
                                await client.SendTextMessageAsync(chatId, RandomFaultMessage());
                                usersIds[chatId].FourthMessageFirstTryTrue++;
                            }
                        }
                        break;

                    default:
                        if (msg.Text == "🌟🌟Нашла🌟🌟")
                        {
                            await client.SendTextMessageAsync(chatId, "Ура! Ты закончила квест!", replyMarkup: ReplyKeyboardRemove());
                            await client.SendTextMessageAsync(chatId, "С Днем рождения!🎂🎂🎂");
                            await client.SendTextMessageAsync(chatId, "(Если хочешь заново пройти путь, введи: /restart)");
                        }
                        
                        break;
                }
            }
        }

        private static void ThreadSleep()
        {
            Thread.Sleep(180000);
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

        private static string RandomFaultPhoto()
        {
            Random rnd = new Random();
            int i = rnd.Next(1, faultPhotos.Count + 1);
            return faultPhotos[i - 1];
        }

        private static async void ShowJdunPhoto(object chatId)
        {
            long chatId1 = (long)chatId;
            await client.SendPhotoAsync(chatId1, "https://github.com/MonoRight/TelegramBotQuest/blob/master/photos/meme_6.jpg?raw=true");
        }
    }
}

