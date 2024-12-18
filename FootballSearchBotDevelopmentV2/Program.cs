using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Timers; //не знаю, надо ли использовать это
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Org.BouncyCastle.Crypto.Tls;
//using FootballSearchBotDevelopmentV2.Parsers;

namespace Program
{
    class Program
    {
        static public TelegramBotClient botClient =
                new TelegramBotClient("nuh uh");


        static DataBase userDataBase = new DataBase();
        static Parser objectParser = new Parser();
        public static List<string> tmTbl = new List<string>(); //эта переменная все портит
        //public static Dictionary<long, string>
        public static List<List<string>> teamsTable;
        static public Dictionary<long, int> indexOfSearch = new Dictionary<long, int>();
        static public Dictionary<long, int> replyMessageType = new Dictionary<long, int>();
        static public Dictionary<long, string> confirmationСode = new Dictionary<long, string>();
        static public Dictionary<long, List<string>> registrationMessageTextDictionary = new Dictionary<long, List<string>>();
        static public List<League> leagues = new List<League>();
        static public Dictionary<LeaguesEnum, League> LeaguesDic = new Dictionary<LeaguesEnum, League>();
        static public Dictionary<CountriesEnum, Country> CountriesDic = new Dictionary<CountriesEnum, Country>();
        static public Dictionary<long, int> indexOfInlineMarkup = new Dictionary<long, int>();
        static public Dictionary<long, string> teamsListAnswer = new Dictionary<long, string>();
        static public Dictionary<long, int> numOfPlayerList = new Dictionary<long, int>();
        static public Dictionary<long, Message> lastMessage = new Dictionary<long, Message>();
        static public Dictionary<long, int> count = new Dictionary<long, int>();
        static public Dictionary<long, League> selectedLeague = new Dictionary<long, League>();
        static private Dictionary<long, bool> adminMode = new Dictionary<long, bool>();
        static public Dictionary<string, string> teamLink = new Dictionary<string, string>();
        static public Dictionary<long, List<List<string>>> parsedTeam = new Dictionary<long, List<List<string>>>();
        static public Dictionary<long, User> enterance = new Dictionary<long, User>();
        static public long addres;
        static public string mmssgg;
        static public Dictionary<long, User> RegistrationDictionary = new Dictionary<long, User>();
        static public List<User> authorizedUser = new List<User>();
        static public Buttons but = new Buttons();
        #region Buttons
        static InlineKeyboardMarkup fourRows16 = new(new[]
       {
                  new []
                  {
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[1], callbackData: "1"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[2], callbackData: "2"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[3], callbackData: "3"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[4], callbackData: "4"),

                  },
                  new []
                  {
                      InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                  },
                  });
        static InlineKeyboardMarkup eightRows16 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[5], callbackData: "5"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[6], callbackData: "6"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[7], callbackData: "7"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[8], callbackData: "8")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                   },
                   });
        static InlineKeyboardMarkup twelveRows16 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[9], callbackData: "9"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[10], callbackData: "10"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[11], callbackData: "11"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[12], callbackData: "12")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                   },
                });
        static InlineKeyboardMarkup sixteenRows16 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[13], callbackData: "13"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[14], callbackData: "14"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[15], callbackData: "15"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[16], callbackData: "16")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),

                   },
                });

        static Dictionary<int, InlineKeyboardMarkup> teams16qbmk = new Dictionary<int, InlineKeyboardMarkup>()
                {
                    { 0, fourRows16},
                    { 1, eightRows16 },
                    { 2, twelveRows16},
                    { 3, sixteenRows16},
                };
        static InlineKeyboardMarkup fiveRows20 = new(new[]
        {
                  new []
                  {
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[1], callbackData: "1"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[2], callbackData: "2"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[3], callbackData: "3"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[4], callbackData: "4"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[5], callbackData: "5"),

                  },
                  new []
                  {
                      InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                  },
                  });
        static InlineKeyboardMarkup tenRows20 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[6], callbackData: "6"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[7], callbackData: "7"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[8], callbackData: "8"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[9], callbackData: "9"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[10], callbackData: "10")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                   },
                });
        static InlineKeyboardMarkup fifteenRows20 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[11], callbackData: "11"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[12], callbackData: "12"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[13], callbackData: "13"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[14], callbackData: "14"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[15], callbackData: "15")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                   },
                });
        static InlineKeyboardMarkup twentyRows20 = new(new[]
        {
                   new []
                   {
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[16], callbackData: "16"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[17], callbackData: "17"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[18], callbackData: "18"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[19], callbackData: "19"),
                       InlineKeyboardButton.WithCallbackData(text: but.Pins[20], callbackData: "20")
                   },
                   new []
                   {
                     InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),

                   },
                });

        static Dictionary<int, InlineKeyboardMarkup> teams20qbmk = new Dictionary<int, InlineKeyboardMarkup>()
                {
                    { 0, fiveRows20},
                    { 1, tenRows20 },
                    { 2, fifteenRows20},
                    { 3, twentyRows20},
                };
        static InlineKeyboardMarkup sixRows18 = new(new[]
        {
                  new []
                  {
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[1], callbackData: "1"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[2], callbackData: "2"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[3], callbackData: "3"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[4], callbackData: "4"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[5], callbackData: "5"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[6], callbackData: "6"),

                  },
                  new []
                  {

                      InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                  },
                  });
        static InlineKeyboardMarkup twelveRows18 = new(new[]
        {
                  new []
                  {
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[7], callbackData: "7"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[8], callbackData: "8"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[9], callbackData: "9"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[10], callbackData: "10"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[11], callbackData: "11"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[12], callbackData: "12"),

                  },
                  new []
                  {
                      InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),

                      InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

                  },
                  });
        static InlineKeyboardMarkup eighteenRows18 = new(new[]
        {
                  new []
                  {
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[13], callbackData: "13"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[14], callbackData: "14"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[15], callbackData: "15"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[16], callbackData: "16"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[17], callbackData: "17"),
                    InlineKeyboardButton.WithCallbackData(text: but.Pins[18], callbackData: "18"),

                  },
                  new []
                  {
                      InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),


                  },
                  });
        static Dictionary<int, InlineKeyboardMarkup> teams18qbmk = new Dictionary<int, InlineKeyboardMarkup>()
                {
                    { 0, sixRows18},
                    { 1, twelveRows18 },
                    { 2, eighteenRows18},
                };

        static InlineKeyboardMarkup nxt_21 = new(new[]
        {
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),

        },
        });
        static InlineKeyboardMarkup nxt_021 = new(new[]
        {
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),
            InlineKeyboardButton.WithCallbackData(text: but.Pins[21], callbackData: "but_21"),
        },
        });
        static InlineKeyboardMarkup nxt_0 = new(new[]
        {
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: but.Pins[0], callbackData: "but_0"),

        },
        });
        static Dictionary<int, InlineKeyboardMarkup> nxt = new Dictionary<int, InlineKeyboardMarkup>()
        {
            {0, nxt_21 },
            {1, nxt_021 },
            {2, nxt_0 },
        };
        #endregion
        static ReplyKeyboardMarkup howToSearchMarkup = new(new[]
        {
                    new KeyboardButton[] { "Выбрать📄", "Ввести⌨" },
                })
        {
            //OneTimeKeyboard = true,
            ResizeKeyboard = true
        };
        public void GetTable(List<List<string>> lstTable, Dictionary<LeaguesEnum, League> lgsdc, Dictionary<CountriesEnum, Country> CntrDic, Dictionary<string, string> linksOfTeams)
        {
            teamsTable = lstTable;
            LeaguesDic = lgsdc;
            CountriesDic = CntrDic;
            teamLink = linksOfTeams;
        }
        public static List<PageInfo> nwsListSpR = new List<PageInfo>();
        public static int iter = 0;
        public static async Task GetNews(List<PageInfo> artspr, TimeSpan lastParseTime)
        {
            MatchCollection matches;
            nwsListSpR = artspr;
            //string a = "Динамо";
            //long chatId = 956430886;
            
            foreach (var time in artspr)
            {
                if(lastParseTime < Convert.ToDateTime(time.Time).TimeOfDay) //тут было меньше или равно
                {
                    using(StreamWriter streamWriter = new StreamWriter("Новости.txt", true))
                    {
                        streamWriter.WriteLine(time.Link);
                    }
                    Console.WriteLine("Проверку прошел: " + lastParseTime.ToString() + " " + time.Time.TimeOfDay.ToString());
                    
                    foreach(var usr in authorizedUser)
                    {
                        foreach (var itemM in time.Tag)
                        {

                            //foreach (var usr in authorizedUser)
                            //{
                            if (iter == 0)
                            {
                                foreach (var usssr in authorizedUser)
                                {
                                    await botClient.SendTextMessageAsync(
                                               chatId: usssr.ChatId, text: "Бот работает. ");
                                }
                                iter++;
                                return;
                            }
                            Regex regex = new Regex(@"\w*" + usr.Team.ToString() + @"\w*", RegexOptions.IgnoreCase);
                            matches = regex.Matches(itemM);


                            if (matches.Count > 0)
                            {

                                await botClient.SendTextMessageAsync(
                                chatId: usr.ChatId, text: time.Link);
                                Console.WriteLine("Новость отправлена: " + usr.ChatId.ToString() + " Время: " + DateTime.Now.TimeOfDay);
                                break;
                            }
                            //}

                        }
                    }
                    
                    
                }
                else
                {
                    Console.WriteLine(time.Time.ToString() + " " + lastParseTime);
                    Console.WriteLine("Новости отсеялись. Время: ");
                    break;
                }
            }
            //Thread.Sleep(5000);
            //Console.WriteLine(nwsListSpR.Count);
            #region найти новость
            //foreach (var item in nwsListSpR)
            //{
            //    //Console.WriteLine(item.Link);
            //    //Console.WriteLine(item.Tag.Count);

            //    foreach (var itemM in item.Tag)
            //    {
            //        string a = "Динамо";
            //        long chatId = 956430886;
            //        Regex regex = new Regex(@"\w*" + a + @"\w*", RegexOptions.IgnoreCase);
            //        matches = regex.Matches(itemM);
            //        //Console.WriteLine("Новость проверена");

            //        if (matches.Count > 0)
            //        {
            //            //Console.WriteLine("Новость отправлена");
            //            await botClient.SendTextMessageAsync(
            //            chatId: chatId, text: item.Link);
            //        }
            //        //else
            //        //{
            //        //    Console.WriteLine(matches.Count);
            //        //}
            //    }
            //}
            #endregion
            //await botClient.SendTextMessageAsync(
            //        chatId: 956430886, text: nwsListSpR[0].Link);
           Console.WriteLine("Новости отправлены пользователям. Время: " + DateTime.Now);
           if(artspr.Count> 0)
           {
                Console.WriteLine("Время последней новости " + artspr[0].Time);
           }
           
        }
       
       
        public static async Task HandleUpdateAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                BotOnMessageReceived(botClient, update.Message, cancellationToken);
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery, cancellationToken);

                //await BotOnMessageReceived(botClient, message: "no", cancellationToken);
            }
        }
        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {

            string teamName = "";
            //foreach (var item in authorizedUser)
            //{
            //    //Console.WriteLine(item.Name.ToString(), item.Email.ToString(), item.Team.ToString(), item.ChatId.ToString());
            //}
            
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            if (!replyMessageType.ContainsKey(chatId))
            {
                replyMessageType.Add(chatId, -1);
            }
            if (!count.ContainsKey(chatId))
            {
                count.Add(chatId, -1);
            }

            if (!indexOfInlineMarkup.ContainsKey(chatId))
            {
                indexOfInlineMarkup.Add(chatId, -1);
            }
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                new KeyboardButton[] { "Вход", "Регистрация" },
                new KeyboardButton[] { "Настройки", "Информация❔" },
                new KeyboardButton[] {"Тык"},
            })
            {
                ResizeKeyboard = true
            };
            ReplyKeyboardMarkup cancelAction = new(new[]
                 {
                    new KeyboardButton[] { "Отмена" },
                    new KeyboardButton[] { "Назад" },
                  })
            {
                OneTimeKeyboard = true,
                ResizeKeyboard = true
            };
            ReplyKeyboardMarkup cancelActionSign = new(new[]
                {
                    new KeyboardButton[] {"Отмена"},
                })
            {
                OneTimeKeyboard = true,
                ResizeKeyboard = true
            };
            while (true)
            {
                if (messageText == "/start")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId, text: "Привет! Я футбольный бот. " +
                    "Пока я нахожусь в разработке и не могу исполнять большинство функций:( " +
                    "Но со временем я буду становиться лучше! Напишите /help, чтобы получить список работающих функций бота.",
                    cancellationToken: cancellationToken);
                    break;
                    
                }
                if(messageText == "Настройки")
                {
                    replyMessageType[chatId] = -2000;
                    ReplyKeyboardMarkup settingsMarkup = new(new[]
                {
                new KeyboardButton[] { "Выбор любимой команды", "Выход из аккаунта" },
                new KeyboardButton[] { "Информация обо мне", "Информация о разработчике"},
                new KeyboardButton[] {"Отмена"}
            })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(chatId: chatId, text: "Что вас интересует?", replyMarkup: settingsMarkup, cancellationToken: cancellationToken);
                    break;
                }
                if (replyMessageType[chatId] <= -2000 && replyMessageType[chatId] >= -2006)
                {
                    if(messageText == "Отмена")
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        return;
                    }
                    if(messageText == "Информация обо мне")
                    {
                        bool flag = true;
                        foreach (var item in authorizedUser)
                        {
                            if(item.ChatId == chatId)
                            {
                                flag = false;
                                await botClient.SendTextMessageAsync(chatId: chatId, text: $"Имя: '{item.Name}'\n" +
                                    $"Любимая команда: '{item.Team}'\n", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                                
                                replyMessageType[chatId] = -1;
                                return;
                            }
                        }
                        if (flag == true)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatId, text: "Вы не авторизированы", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                            return;
                        }
                    }
                    if(messageText == "Выход из аккаунта")
                    {
                        bool flag = false;
                        foreach (var user in authorizedUser)
                        {
                            if(user.ChatId == chatId)
                            {
                                authorizedUser.Remove(user);
                                flag = true;
                                break;
                            }
                        }
                        if(flag == true)
                        {
                            var res = DataBase.ExitFromDb(chatId);
                            await botClient.SendTextMessageAsync(chatId: chatId, text: "Выход совершён. Что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                            replyMessageType[chatId] = -1;
                            return;
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId: chatId, text: "Чтобы выйти, нужно авторизироваться.", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                            replyMessageType[chatId] = -1;
                            return;
                        }
                    }
                    if(messageText == "Информация о разработчике")
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "    Привет! Меня зовут @Gaalexander, я разработчик этого бота. Если получится найти недоработку или ошибку, мои личные сообщения открыты. \n    Бот держится на энтузиазме разработчика и по сути является его школьным проектом, поэтому за баги простите :)", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        
                        return;
                    }
                    if (messageText == "Выбор любимой команды" && replyMessageType[chatId] == -2000)
                    {
                        foreach (var resUsr in authorizedUser)
                        {
                            if (resUsr.ChatId == chatId)
                            {
                                await botClient.SendTextMessageAsync(chatId: chatId, text: "Напишите название команды, за которой вы хотите следить. (Функция находится в разработке)", replyMarkup: cancelActionSign, cancellationToken: cancellationToken);
                                replyMessageType[chatId] = -2001;
                                return;
                            }
                        }
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Необходимо зарегистрироваться и зайти в аккаунт.", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -1;
                        return;
                    }
                    if (replyMessageType[chatId] == -2001)
                    {
                        if (messageText == "Отмена")
                        {
                            replyMessageType[chatId] = -1;
                            await botClient.SendTextMessageAsync(chatId: chatId, text: "Что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                            return;
                        }
                        DataBase.AddFavouriteteam(chatId, messageText);
                        foreach (var usr in authorizedUser)
                        {
                            if (usr.ChatId == chatId)
                            {
                                usr.Team = messageText;
                                await botClient.SendTextMessageAsync(chatId: chatId, text: "Команда добавлена, что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                                replyMessageType[chatId] = -1;
                            }


                        }
                        Console.WriteLine(authorizedUser.Count);
                        foreach (var item in authorizedUser)
                        {
                            Console.WriteLine(item.Team);
                        }
                        return;
                    }
                }
                if (messageText == "Вход")
                {
                    foreach(var item in authorizedUser)
                    {
                        if(item.ChatId == chatId)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatId, text: "Вы уже авторизировались", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                            replyMessageType[chatId] = -1;
                            return;
                        }
                    }
                    await botClient.SendTextMessageAsync(chatId: chatId, text: "Введите имя...", replyMarkup: cancelActionSign, cancellationToken: cancellationToken);
                    replyMessageType[chatId] = -10000;
                    
                    break;
                }
                
                if (replyMessageType[chatId] == -10000)
                {
                    
                    if(messageText == "Отмена")
                    {
                        replyMessageType[chatId] = -1;
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        if(enterance.Count != null)
                        {
                            enterance.Clear();
                        }
                        return;
                    }
                    
                    User user = new User();
                    user.Name = messageText;
                    enterance.Add(chatId, user);
                    bool cons = DataBase.ConsostsInDBName(enterance[chatId]);
                    if(cons == true)
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Введите пароль...", replyMarkup: cancelActionSign, cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -10001;
                        break;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Пользователя с таким именем не найдено. Введите имя еще раз.",replyMarkup: cancelActionSign,cancellationToken: cancellationToken);
                        enterance.Remove(chatId);
                        break;
                    }
                }
                if (replyMessageType[chatId] == -10001)
                {
                    if (messageText == "Отмена")
                    {
                        replyMessageType[chatId] = -1;
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Что вас интересует?", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        if(enterance.Count != null)
                        {
                            enterance.Clear();
                        }
                        return;
                    }
                    if (messageText != null)
                    {
                        enterance[chatId].Password = await User.HeshPassword(messageText);
                    }
                    bool con = DataBase.ConsostsInDBPassword(enterance[chatId]);
                    if(con == true)
                    {
                        
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Вход выполнен.",replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
                        enterance[chatId].ChatId = chatId;
                        enterance[chatId].Team = DataBase.TeamSrch(chatId);
                        //Console.WriteLine(enterance[chatId].Team);
                        authorizedUser.Add(enterance[chatId]);
                        replyMessageType[chatId] = -1;
                        break;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: "Пароль неверный. Введите его еще раз...", cancellationToken: cancellationToken);
                        enterance[chatId].Password = null;
                        break;
                    }
                }
                Buttons but = new Buttons();
                #region AdminMode
                if (messageText == "/admin" && chatId == 956430886)
                {
                    
                    replyMessageType[chatId] = -999;
                    
                    if (!adminMode.ContainsKey(chatId))
                    {
                        adminMode.Add(chatId, true);
                    }
                }
                if (adminMode.ContainsKey(chatId) && adminMode[chatId] == true || replyMessageType[chatId] == -999)
                {
                    if (replyMessageType[chatId] == -999)
                    {
                        ReplyKeyboardMarkup adminKeyboardMarkup = new(new[]
                        {
                            new KeyboardButton[] { "Сообщение"},
                            new KeyboardButton[] { "Выйти" },
                        })
                        {
                            ResizeKeyboard = true
                        };
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Что интересует, админ?",
                        replyMarkup: adminKeyboardMarkup,
                        cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -1000;
                        break;
                    }

                    if(messageText == "Сообщение")
                    {
                        Dictionary<long, string> addressee = new Dictionary<long, string>()
                        {
                            {1819545054, "TEST" },
                        };

                        ReplyKeyboardMarkup adresseeMarkUp = new(new[]
                        {
                            new KeyboardButton[] { addressee[1819545054] },
                        })
                        {
                            ResizeKeyboard = true
                        };
                        replyMessageType[chatId] = -1001;
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Кому надо написать сообщение?",
                        replyMarkup: adresseeMarkUp,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (messageText == "Выйти")
                    {
                        replyMessageType[chatId] = -1;
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Что вас интересует?",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (replyMessageType[chatId] == -1001)
                    {
                        Dictionary<long, string> addressee = new Dictionary<long, string>()
                        {
                            {1819545054, "Test" },
                            
                        };
                        //здесь нужен цикл
                        foreach (var item in addressee)
                        {
                            if(messageText == item.Value)
                            {
                                addres = item.Key;
                            }
                        }
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Напишите сообщение, которое надо отправить.",
                        cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -1002;
                        break;
                    }
                    if (replyMessageType[chatId] == -1002 && messageText != null)
                    {
                        ReplyKeyboardMarkup yesOrNo = new(new[]
                        {
                            new KeyboardButton[] { "Да", "Нет" },
                            
                        })
                        {
                            ResizeKeyboard = true
                        };
                        mmssgg = messageText;
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Отправляю сообщение?",
                        replyMarkup: yesOrNo,
                        cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -1003;
                        break;
                    }
                    if (replyMessageType[chatId] == -1003)
                    {
                        if(messageText == "Да")
                        {
                            
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: addres, text: mmssgg,
                            cancellationToken: cancellationToken);

                            Message sentMessageOtv = await botClient.SendTextMessageAsync(
                            chatId: chatId, text: "Сообщение отправлено.",
                            cancellationToken: cancellationToken);
                        }
                        if (messageText == "Нет")
                        {
                            mmssgg = null;
                            addres = -1;
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId, text: "Сообщение не отправлено.",
                            cancellationToken: cancellationToken);
                        }

                        ReplyKeyboardMarkup adminKeyboardMarkup = new(new[]
                        {
                            new KeyboardButton[] { "Сообщение"},
                            new KeyboardButton[] { "Выйти" },
                        })
                        {
                            ResizeKeyboard = true
                        };
                        Message sentMessageee = await botClient.SendTextMessageAsync(
                        chatId: chatId, text: "Что интересует, админ?",
                        replyMarkup: adminKeyboardMarkup,
                        cancellationToken: cancellationToken);
                        replyMessageType[chatId] = -1000;

                        break;
                    }
                }
                #endregion
                
               


                ReplyKeyboardMarkup pickCountryMarkup = new(new[]
                {
                    new KeyboardButton[] { CountriesDic[CountriesEnum.Russia].Pin, CountriesDic[CountriesEnum.Italy].Pin, CountriesDic[CountriesEnum.France].Pin },
                    new KeyboardButton[] { CountriesDic[CountriesEnum.Germany].Pin, CountriesDic[CountriesEnum.Spain].Pin, CountriesDic[CountriesEnum.Ukraine].Pin },
                    new KeyboardButton[] { CountriesDic[CountriesEnum.England].Pin },
                    new KeyboardButton[] { "Отмена" },
                })
                {
                    ResizeKeyboard = true
                };
                
                
                if (messageText == "/help")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId, text: "/reg - Регистрация, /info - Информация о футбольных командах", 
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);

                    break;
                }
                
                ReplyKeyboardMarkup menuOfSearch = new(new[]
                {
                  new KeyboardButton[] { "Количество игроков", "Средний возраст" },
                  new KeyboardButton[] { "Кол-во легионеров", "Средняя стоимость" },
                  new KeyboardButton[] { "Общая стоимость" },
                  new KeyboardButton[] { "Состав команды" },
                  new KeyboardButton[] { "Спасибо! Выйти." },
                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
                //ReplyKeyboardMarkup cancelAction = new(new[]
                //  {
                //    new KeyboardButton[] { "Отмена" },
                //    new KeyboardButton[] { "Назад" },
                //  })
                //{
                //    OneTimeKeyboard = true,
                //    ResizeKeyboard = true
                //};
                ReplyKeyboardMarkup cancelSearch = new(new[]
                  {
                    new KeyboardButton[] { "Отмена" },
                  })
                {
                    OneTimeKeyboard = true,
                    ResizeKeyboard = true
                };
                
                if (messageText == "Вход")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Функция пока недоступна :(",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
                }
                if(messageText == "Тык")
                {
                    Random r = new Random();
                    int rnd = r.Next(0, 4);
                    if(rnd == 0)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Чпоньк...",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    }
                    if (rnd == 1)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "ха-ха-ха, что ты делаешь? эта кнопка же бесполезная)",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    }
                    if (rnd == 2)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "❤️ тестерам",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    }
                    if (rnd == 3)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Следущий тост за тебя, тестер.",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    }

                }
                if(messageText == "Информация❔" || messageText == "/info")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Как хотите найти команду?",
                    replyMarkup: howToSearchMarkup,
                    cancellationToken: cancellationToken);
                    replyMessageType[chatId] = -1;
                    if (lastMessage.ContainsKey(chatId))
                    {
                        await botClient.DeleteMessageAsync(chatId, lastMessage[chatId].MessageId);
                        lastMessage.Remove(chatId);
                    }
                    if (selectedLeague.ContainsKey(chatId))
                    {
                       selectedLeague.Remove(chatId);
                    }
                    if (indexOfSearch.ContainsKey(chatId))
                    {
                        indexOfSearch.Remove(chatId);
                    }

                    break;
                } 
                if(messageText == "Выбрать📄")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Какая из этих стран вас интересует?",
                    replyMarkup: pickCountryMarkup,
                    cancellationToken: cancellationToken);

                    string LeaguesListAnswer = "";
                    foreach (var item in CountriesDic)
                    {
                        LeaguesListAnswer += $"{item.Value.Pin}  {item.Value.Name}" + "\n";
                    }
                    Message sentMessageLeaguesList = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: LeaguesListAnswer,
                    replyMarkup: pickCountryMarkup,
                    cancellationToken: cancellationToken);

                    replyMessageType[chatId] = 10;

                    break;
                }
                if (messageText == "Отмена" && replyMessageType[chatId] == 10)
                {
                    replyMessageType[chatId] = -1;
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Что вас интересует?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
                    indexOfSearch.Remove(chatId);
                    break;

                }
                if (replyMessageType[chatId] == 10 && messageText != "Отмена")
                {

                    foreach (var item in CountriesDic)
                    {
                        if(messageText == item.Value.Pin)
                        {
                            ReplyKeyboardMarkup pickLeagueMarkup;
                            
                            if (item.Value.Divisions.Count == 1)
                            {
                               pickLeagueMarkup = new(new[]
                                {
                                    new KeyboardButton[] { item.Value.Divisions[0].Name},
                                })
                                {
                                    ResizeKeyboard = true
                                };
                                 Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Какая по силе лига вас интересует?",
                                replyMarkup: pickLeagueMarkup,
                                cancellationToken: cancellationToken);
                                replyMessageType[chatId] = 11;
                            }
                            if(item.Value.Divisions.Count == 2)
                            {
                                pickLeagueMarkup = new(new[]
                                {
                                    new KeyboardButton[] { item.Value.Divisions[0].Name},
                                    new KeyboardButton[] { item.Value.Divisions[1].Name},
                                })
                                {
                                    ResizeKeyboard = true
                                };
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Какая по силе лига вас интересует?",
                                replyMarkup: pickLeagueMarkup,
                                cancellationToken: cancellationToken);
                            }
                            if (item.Value.Divisions.Count == 3)
                            {
                                pickLeagueMarkup = new(new[]
                                {
                                    new KeyboardButton[] { item.Value.Divisions[0].Name},
                                    new KeyboardButton[] { item.Value.Divisions[1].Name},
                                    new KeyboardButton[] { item.Value.Divisions[2].Name},
                                })
                                {
                                    ResizeKeyboard = true
                                };
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Какая по силе лига вас интересует?",
                                replyMarkup: pickLeagueMarkup,
                                cancellationToken: cancellationToken);
                            }

                            replyMessageType[chatId] = 11;
                            return;
                            //break;
                        }
                    }
                }
                
                if (replyMessageType[chatId] == 11)
                {
                    foreach (var item in CountriesDic)
                    {
                        foreach (var item1 in item.Value.Divisions.Values)
                        {
                            if(item1.Name == messageText)
                            {
                                //indexOfInlineMarkup.Add(chatId, 0);
                                teamsListAnswer.Add(chatId, "");
                                for (int i = 1; i < item1.TeamDic.Count+1; i++)
                                {
                                    teamsListAnswer[chatId] +=   $"{but.Pins[i]}    {item1.TeamDic[i]} " + "\n";
                                }
                                //count.Add(chatId, item1.TeamDic.Count);
                                count[chatId] = item1.TeamDic.Count;
                                indexOfInlineMarkup[chatId] = 0;

                                selectedLeague.Add(chatId, item1);
                                replyMessageType[chatId] = 12;
                                //возможно нужен break
                                break;
                            }
                        }
                        if (replyMessageType[chatId] == 12)
                        {
                            break;
                        }
                    }
                }
                if (replyMessageType[chatId] == 12)
                {
                    if (count[chatId] == 16)
                    {

                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Выберите команду. \n" + teamsListAnswer[chatId],
                        replyMarkup: teams16qbmk[indexOfInlineMarkup[chatId]],
                        cancellationToken: cancellationToken);


                        if (!lastMessage.ContainsKey(chatId))
                        {
                            lastMessage.Add(chatId, sentMessage);
                        }
                        else
                        {
                            lastMessage[chatId] = sentMessage;
                        }

                    }
                    if (count[chatId] == 18)
                    {
                        
                        
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Выберите команду. \n" + teamsListAnswer[chatId],
                        replyMarkup: teams18qbmk[indexOfInlineMarkup[chatId]],
                        cancellationToken: cancellationToken);

                        if (!lastMessage.ContainsKey(chatId))
                        {
                            lastMessage.Add(chatId, sentMessage);
                        }
                        else
                        {
                            lastMessage[chatId] = sentMessage;
                        }


                    }
                    if (count[chatId] == 20)
                    {

                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Выберите команду. \n" + teamsListAnswer[chatId],
                        replyMarkup: teams20qbmk[indexOfInlineMarkup[chatId]],
                        cancellationToken: cancellationToken);

                        if (!lastMessage.ContainsKey(chatId))
                        {
                            lastMessage.Add(chatId, sentMessage);
                        }
                        else
                        {
                            lastMessage[chatId] = sentMessage;
                        }


                    }
                    teamsListAnswer.Remove(chatId);
                    break;
                }
                if (messageText == "Ввести⌨")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Укажите команду...",
                    replyMarkup: cancelSearch,
                    cancellationToken: cancellationToken) ;
                    replyMessageType[chatId] = replyMessageType[chatId] - 1;
                    break;
                }
                if(messageText == "Отмена" && replyMessageType[chatId] == -2)
                {
                    replyMessageType[chatId]++;
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Что вас интересует?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
                    indexOfSearch.Remove(chatId);
                    break;

                }
                if (replyMessageType[chatId] == -2)
                {
                    if (messageText.ToLower() == "псж")
                    {
                        messageText = "Пари Сен-Жермен";
                    }
                    if (messageText.Length <= 2)
                    {
                        Message entMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Не удалось распознать команду. Напишите полное название.",
                        replyMarkup: cancelSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    MatchCollection matches;
                    foreach (List<string> info in teamsTable)
                    {
                        string messageTextRemoved = messageText;
                        Dictionary<int, string> dictionary = new Dictionary<int, string>();
                        Dictionary<int, int> coincidence = new Dictionary<int, int>();

                        int s;
                        int iter = 0;
                        int jiter = 0;

                        do
                        {
                            Regex regex = new Regex(@"\w*" + messageTextRemoved + @"\w*", RegexOptions.IgnoreCase);
                            foreach (var item in info)
                            {
                                matches = regex.Matches(item);
                                s = matches.Count;
                                if (s > 0)
                                {

                                    if (!dictionary.ContainsValue(item))
                                    {
                                        dictionary.Add(iter, item);
                                        coincidence.Add(iter, jiter);
                                        Console.WriteLine(dictionary.Keys);
                                        iter++;
                                    }
                                    else
                                    {
                                        foreach (var searchInDictionary in dictionary)
                                        {
                                            if (searchInDictionary.Value == item)
                                            {
                                                coincidence[searchInDictionary.Key]++;
                                            }
                                        }
                                    }
                                }
                                tmTbl = info;
                            }
                            messageTextRemoved = messageText.Remove(messageTextRemoved.Length - 1);
                        } while (messageTextRemoved != null && messageTextRemoved.Length > 3);

                        messageTextRemoved = messageText;
                        for (int i = 0; i < messageText.Length - 3; i++)
                        {
                            Regex regex = new Regex(@"\w*" + messageTextRemoved + @"\w*", RegexOptions.IgnoreCase);
                            foreach (var item in info)
                            {
                                matches = regex.Matches(item);
                                s = matches.Count;

                                if (s > 0)
                                {
                                    if (!dictionary.ContainsValue(item))
                                    {
                                        dictionary.Add(iter, item);
                                        coincidence.Add(iter, jiter);
                                        iter++;
                                    }
                                    else
                                    {
                                        foreach (var searchInDictionary in dictionary)
                                        {
                                            if (searchInDictionary.Value == item)
                                            {
                                                coincidence[searchInDictionary.Key]++;
                                            }
                                        }
                                    }
                                }
                            }
                            messageTextRemoved = messageTextRemoved.Remove(0, 1);
                        }
                        if (dictionary.Count > 0)
                        {
                            foreach (var item in dictionary)
                            {
                                Console.WriteLine(item);
                            }
                            foreach (var item in coincidence)
                            {
                                Console.WriteLine(item);
                            }

                        }

                        string result = "";

                        foreach (var item in dictionary)
                        {
                            result += $"{item.Value}" + "\n";
                        }
                        if (dictionary.Count > 1)
                        {
                            int check = coincidence[0];
                            int itter = 0;

                            foreach (var item in coincidence)
                            {
                                if (item.Value == check)
                                {
                                    itter++;
                                }
                                else
                                {

                                    Console.WriteLine(dictionary[0]);
                                    teamName = dictionary[0];
                                    tmTbl = info;
                                    for (int i = 0; i < info.Count; i++)
                                    {
                                        if (dictionary[0] == info[i])
                                        {
                                            indexOfSearch.Add(chatId, i);
                                            Message sentMessage = await botClient.SendTextMessageAsync(
                                            chatId: chatId,
                                            text: $"Выбрана команда {teamName}. Что вас интересует?",
                                            replyMarkup: menuOfSearch,
                                            cancellationToken: cancellationToken);
                                            replyMessageType[chatId] = -1;
                                            //replyMessageType[chatId] = -3;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            if (itter == dictionary.Count)
                            {
                                Message sentMessageQstn = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Какая из этих команд вас интересует? Напишите точнее.",
                                cancellationToken: cancellationToken);
                                Message ntMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: result,
                                cancellationToken: cancellationToken);
                                //itter = 0;
                                //replyMessageType[chatId] = -2; //эти две строчки возможно надо удалить
                                //break;
                            }
                            
                        }
                        //break;// эта хуйня не нужна
                        if (dictionary.Count == 1)
                        {
                            Console.WriteLine(dictionary[0]);
                            teamName = dictionary[0];
                            tmTbl = info;
                            for (int i = 0; i < info.Count; i++)
                            {
                                if (dictionary[0] == info[i])
                                {
                                    indexOfSearch.Add(chatId, i);
                                    Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: $"Выбрана команда {teamName}. Что вас интересует?",
                                    replyMarkup: menuOfSearch,
                                    cancellationToken: cancellationToken);
                                    replyMessageType[chatId] = -1;
                                    break;
                                }
                            }
                        }
                        if (dictionary.Count == 0)
                        {
                            Message senMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Клуб не найден.",
                            cancellationToken: cancellationToken);
                        }
                        tmTbl = info;
                    }
                    
                    break;
                }
                
                if (indexOfSearch.ContainsKey(chatId))
                {
                    if (messageText == "Количество игроков")
                    {
                        string answr = $"Количество игроков в команде {teamName}: {tmTbl[indexOfSearch[chatId] + 1]}";
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: answr,
                        replyMarkup: menuOfSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (messageText == "Средний возраст")
                    {
                        string answr = $"Средний возраст: {tmTbl[indexOfSearch[chatId] + 2]}";
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: answr,
                        replyMarkup: menuOfSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (messageText == "Кол-во легионеров")
                    {
                        string answr = $"Кол-во легионеров: {tmTbl[indexOfSearch[chatId] + 3]}";
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: answr,
                        replyMarkup: menuOfSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (messageText == "Средняя стоимость")
                    {
                        string answr = $"Средняя стоимость: {tmTbl[indexOfSearch[chatId] + 4]}";
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: answr,
                        replyMarkup: menuOfSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                    if (messageText == "Общая стоимость")
                    {
                        string answr = $"Общая стоимость: {tmTbl[indexOfSearch[chatId] + 5]}";
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: answr,
                        replyMarkup: menuOfSearch,
                        cancellationToken: cancellationToken);
                        break;
                    }
                     if(messageText == "Состав команды")
                    {
                        foreach (var item in teamLink)
                        {
                            if(item.Key == tmTbl[indexOfSearch[chatId]])
                            {
                                List<List<string>> a = new List<List<string>>();
                                var data = await Task.Run(() => objectParser.runLoadpg("https://www.transfermarkt.world" + item.Value));
                                a = await Task.Run(()=> objectParser.prsPg(data, a));

                                
                                parsedTeam.Add(chatId, a);
                                
                                
                                string tmMsg = "";
                                
                                if (numOfPlayerList.ContainsKey(chatId) == false)
                                {
                                    numOfPlayerList.Add(chatId, 5);
                                }
                                    for (int g = 0; g < numOfPlayerList[chatId]; g++)
                                    {
                                        for (int i = 0; i < a[g].Count; i++)
                                        {

                                            tmMsg += a[g][i] + "\n";

                                        }
                                        tmMsg += "______" + "\n";
                                    }

                                //короче надо сделать следущий реплаймесджтайп и путешествовать оттуда
                                //и там же исправлять сообщение
                                //всё получится :)



                                
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: tmMsg,
                                replyMarkup: nxt[0],
                                cancellationToken: cancellationToken);
                                if(lastMessage.ContainsKey(chatId) == true)
                                {
                                    lastMessage[chatId] = sentMessage;
                                }
                                else
                                {
                                    lastMessage.Add(chatId, sentMessage);
                                }
                                    
                                
                                

                                Message smsg = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: null,
                                replyMarkup: menuOfSearch,
                                cancellationToken: cancellationToken);
                                //replyMessageType[chatId] = -4;
                                break;
                               //var rdata = await Task.Run(() => objectParser.LoadPage("https://www.transfermarkt.world" + item));
                            }
                        }
                       
                    }
                    
                }
                
                if (messageText == "Спасибо! Выйти.")
                {
                    selectedLeague.Remove(chatId);
                    if (parsedTeam.ContainsKey(chatId) == true)
                    {
                        parsedTeam.Remove(chatId);
                    }
                    indexOfSearch.Remove(chatId);
                    replyMessageType[chatId] = -1;
                    teamsListAnswer.Remove(chatId);
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Что вас интересует?",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);


                    await botClient.DeleteMessageAsync(chatId, lastMessage[chatId].MessageId);
                    if(lastMessage.ContainsKey(chatId))
                    {
                        lastMessage.Remove(chatId);
                    }
                    
                    //tmTbl = null;       // возможная причина ошибки
                    
                    break;
                }
                
                if (messageText == "Регистрация" || messageText == "/reg" || 
                    replyMessageType[chatId] == 0 || replyMessageType[chatId] == 1 || 
                    replyMessageType[chatId] == 2 || replyMessageType[chatId] == 3 ||
                    replyMessageType[chatId] == 4 || replyMessageType[chatId] == 5)
                {
                    User usr = new User();
                    if (messageText == "Регистрация" || messageText == "/reg")
                    {
                        if(DataBase.IsRg(chatId) == true)
                        {
                            Message sntMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Нельзя регистрироваться более одного раза!",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);
                            replyMessageType[chatId] = -1;
                            return;
                        }
                        RegistrationDictionary.Add(chatId, usr);

                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Введите Имя...",
                        replyMarkup: cancelAction,
                        cancellationToken: cancellationToken);
                        //List<string> registrationMessageText = new List<string>();
                        //registrationMessageTextDictionary.Add(chatId, registrationMessageText);

                        replyMessageType[chatId] = 0;
                        break;
                    }
                    if (messageText == "Отмена" || messageText == "Назад")
                    {
                        replyMessageType[chatId] = -1;

                        Message sentMessagecnsl = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Регистрация прервана",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                        RegistrationDictionary.Remove(chatId);
                        //registrationMessageTextDictionary[chatId].Clear();
                        break;
                    }
                    if (replyMessageType[chatId] == 0)
                    {
                       // MatchCollection matches;

                       // string a = "'";
                        //registrationMessageTextDictionary[chatId].Add(messageText);
                        //Regex regex = new Regex(@"\w*" + a + @"\w*", RegexOptions.IgnoreCase);
                        //matches = regex.Matches(messageText);



                        //if (matches.Count > 0)
                        //{
                        //    await botClient.SendTextMessageAsync(
                        //    chatId: chatId, text: "Использован запрещенный символ ' ");
                        //    Message sendMessage = await botClient.SendTextMessageAsync(
                        //    chatId: chatId,
                        //    text: "Введите Имя...",
                        //    replyMarkup: cancelRegistration,
                        //    cancellationToken: cancellationToken);
                        //    break;
                        //}
                         

                        RegistrationDictionary[chatId].Name = messageText;
                        RegistrationDictionary[chatId].ChatId = chatId;
                        //usr.Name = messageText;
                        //usr.ChatId = chatId;
                        //registrationMessageText.Add(messageText);
                        Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Введите почту...",
                        replyMarkup: cancelAction,
                        cancellationToken: cancellationToken);
                        replyMessageType[chatId]++;
                        break;
                    }
                    if (replyMessageType[chatId] == 1)
                    {
                        if (messageText == "Отмена")
                        {
                            Message sentMessagecnsl = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Регистрация прервана",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);

                            replyMessageType[chatId] = -1;
                            //registrationMessageTextDictionary[chatId].Clear();
                            RegistrationDictionary.Remove(chatId);
                            break;
                        }
                        if (messageText == "Назад")
                        {
                            RegistrationDictionary[chatId].Email = null;
                            // registrationMessageTextDictionary[chatId].RemoveAt(0);
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите имя...",
                            replyMarkup: cancelAction,
                            cancellationToken: cancellationToken);

                            replyMessageType[chatId] = 0;
                            break;
                        }
                        if (replyMessageType[chatId] == 1)
                        {
                            try
                            {
                                RegistrationDictionary[chatId].Email = messageText;
                                //registrationMessageTextDictionary[chatId].Add(messageText);
                                MailAddress m = new MailAddress(RegistrationDictionary[chatId].Email);

                                Random rnd = new Random();
                                RegistrationDictionary[chatId].ConfirmationСode = Convert.ToString(rnd.Next(1000, 9999));
                                Console.WriteLine("Код: " + RegistrationDictionary[chatId].ConfirmationСode.ToString());

                                //confirmationСode.Add(chatId, msg);
                                try
                                {
                                    await SendEmailAsync(/*registrationMessageTextDictionary[chatId].Last()*/RegistrationDictionary[chatId].Email,
                                        "Подтверждение почты.", $"Ваш код подтверждения {/*confirmationСode[chatId]*/RegistrationDictionary[chatId].ConfirmationСode}");


                                    Message sentMessageEx = await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: "Сейчас вам на указанную почту придет код подтверждения, состоящий из 4 цифр. Пожалуйста, введите его...",
                                    cancellationToken: cancellationToken);

                                    replyMessageType[chatId]++;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Message sentMessageEx = await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: "Ой, похоже вы ввели почту неправильно. Попробуйте еще раз...",
                                    replyMarkup: cancelAction,
                                    cancellationToken: cancellationToken);
                                    Console.WriteLine(ex.Message);

                                    RegistrationDictionary[chatId].Email = null;
                                    //usr.Email = null;
                                    //registrationMessageTextDictionary[chatId].RemoveAt(1);
                                    break;
                                }

                            }
                            catch (FormatException)
                            {
                                Message sentMessageEx = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Эй! Это даже не похоже на пучту!) Введи реальную почту...",
                                replyMarkup: cancelAction,
                                cancellationToken: cancellationToken);
                                RegistrationDictionary[chatId].Email = null;
                                //registrationMessageTextDictionary[chatId].RemoveAt(1);
                            }
                            break;
                        }
                    }
                    if (replyMessageType[chatId] == 2)
                    {

                        if (messageText == "Отмена")
                        {
                            Message sentMessagecnsl = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Регистрация прервана",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);

                            RegistrationDictionary[chatId].ConfirmationСode = null;
                            //confirmationСode.Remove(chatId);
                            replyMessageType[chatId] = -1;
                            //registrationMessageTextDictionary[chatId].Clear();
                            RegistrationDictionary.Remove(chatId);
                            break;
                        }
                        if (messageText == "Назад")
                        {
                            RegistrationDictionary[chatId].Email = null;
                            //registrationMessageTextDictionary[chatId].RemoveAt(1);
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите почту...",
                            replyMarkup: cancelAction,
                            cancellationToken: cancellationToken);

                            RegistrationDictionary[chatId].ConfirmationСode = null;
                            //confirmationСode.Remove(chatId);
                            replyMessageType[chatId] = 1;
                            break;
                        }
                        if(replyMessageType[chatId] == 2)
                        {
                            if (messageText == /*confirmationСode[chatId]*/RegistrationDictionary[chatId].ConfirmationСode)
                            {
                                RegistrationDictionary[chatId].ConfirmationСode = null;
                                //confirmationСode.Remove(chatId);
                                replyMessageType[chatId]++;
                            }
                            else
                            {
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Код подтверждения неверный! Отправляю его повторно. Введите код еще раз...",
                                replyMarkup: cancelAction,
                                cancellationToken: cancellationToken);

                                //confirmationСode.Remove(chatId);
                                RegistrationDictionary[chatId].ConfirmationСode = null;

                                Random rnd = new Random();
                                string msg = Convert.ToString(rnd.Next(1000, 9999));

                                RegistrationDictionary[chatId].ConfirmationСode = msg;
                                //confirmationСode.Add(chatId, msg);
                                try
                                {
                                    await SendEmailAsync(/*registrationMessageTextDictionary[chatId].Last()*/RegistrationDictionary[chatId].Email,
                                        "Подтверждение почты.", $"Ваш код подтверждения {RegistrationDictionary[chatId].ConfirmationСode/*confirmationСode[chatId]*/}");


                                    replyMessageType[chatId] = 2;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Message sentMessageEx = await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: "Произошла ошибка в отправке. Пройдите регистрацию заново.",
                                    replyMarkup: cancelAction,
                                    cancellationToken: cancellationToken);
                                    Console.WriteLine(ex.Message);

                                    replyMessageType[chatId] = 0;
                                    //registrationMessageTextDictionary[chatId].Clear();
                                    RegistrationDictionary.Remove(chatId);

                                    Message sentMessage1 = await botClient.SendTextMessageAsync(
                                    chatId: chatId,
                                    text: "Введите имя...",
                                    replyMarkup: cancelAction,
                                    cancellationToken: cancellationToken);

                                    break;
                                }
                            }
                        }
                    }
                    if (replyMessageType[chatId] == 3)
                    {
                        if (messageText == "Отмена")
                        {
                            Message sentMessagecnsl = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Регистрация прервана",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);

                            replyMessageType[chatId] = -1;
                            //registrationMessageTextDictionary[chatId].Clear();
                            RegistrationDictionary.Remove(chatId);
                            break;
                        }
                        if (messageText == "Назад")
                        {
                            //registrationMessageTextDictionary[chatId].Remove(registrationMessageTextDictionary[chatId].Last());
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Введите почту...",
                            replyMarkup: cancelAction,
                            cancellationToken: cancellationToken);
                            replyMessageType[chatId] = 1;
                            break;
                        }
                        else
                        {
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Придумайте пароль...",
                            replyMarkup: cancelAction,
                            cancellationToken: cancellationToken);
                            replyMessageType[chatId]++;
                            break;
                        }
                    }
                    if (replyMessageType[chatId] == 4)
                    {
                        if (messageText == "Отмена")
                        {
                            Message sentMessagecnsl = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Регистрация прервана",
                            replyMarkup: replyKeyboardMarkup,
                            cancellationToken: cancellationToken);

                            replyMessageType[chatId] = -1;
                            //registrationMessageTextDictionary[chatId].Clear();
                            RegistrationDictionary.Remove(chatId);
                            break;
                        }
                        if (messageText == "Назад")
                        {
                            //registrationMessageTextDictionary[chatId].RemoveAt(2);
                            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Придумайте пароль...",
                            replyMarkup: cancelAction,
                            cancellationToken: cancellationToken);
                            replyMessageType[chatId] = 1;
                            break;
                        }
                        else
                        {
                            RegistrationDictionary[chatId].Password = await User.HeshPassword(messageText);
                            //RegistrationDictionary[chatId].Password = messageText;
                            if (await userDataBase.addUser(RegistrationDictionary[chatId]) == true)
                            {
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Пользователь зарегистрирован!",
                                replyMarkup: replyKeyboardMarkup,
                                cancellationToken: cancellationToken);
                                replyMessageType[chatId] = -1;
                                //registrationMessageTextDictionary[chatId].Clear();
                                //registrationMessageTextDictionary.Remove(chatId);
                                RegistrationDictionary.Remove(chatId); //это пробно
                            }
                            else
                            {
                                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Почта или имя уже используются! Введите другие данные.",
                                cancellationToken: cancellationToken);
                                RegistrationDictionary.Remove(chatId);
                                //registrationMessageTextDictionary[chatId].Clear();
                                replyMessageType[chatId] = 0;

                                Message sentMessage1 = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Введите имя...",
                                replyMarkup: cancelAction,
                                cancellationToken: cancellationToken);
                                break;
                            }
                        }
                        //string strChatId = Convert.ToString(chatId);
                        
                        //registrationMessageTextDictionary[chatId].Add(messageText);
                        //registrationMessageTextDictionary[chatId].Add(strChatId);


                        

                    }
                    break;
                }
                break;
            }
                
            using (var sw = new StreamWriter("СобщенияПользователей.txt", true))
            {
                if( chatId != 956430886)
                {
                    Message sentMessage1 = await botClient.SendTextMessageAsync(
                    chatId: 956430886,
                    text: $"ПОЛЬЗОВАТЕЛЬ: '{chatId}' НАПИСАЛ СОБЩЕНИЕ:" + messageText,
                    cancellationToken: cancellationToken);
                }
                sw.WriteLine($"ПОЛЬЗОВАТЕЛЬ: '{chatId}' НАПИСАЛ СОБЩЕНИЕ:" + messageText);
            }
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
                Buttons but = new Buttons();
                var chatId = callbackQuery.Message.Chat.Id;
            if (replyMessageType.ContainsKey(chatId))
            {
                if (callbackQuery.Data == "but_0" && replyMessageType[chatId] != -1/*&& callbackQuery.Message != null*/)
                {
                    chatId = callbackQuery.Message.Chat.Id;
                    if (!indexOfInlineMarkup.ContainsKey(chatId))
                    {
                        return;
                    }
                    indexOfInlineMarkup[callbackQuery.Message.Chat.Id]--;
                    if (count[chatId] == 16)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams16qbmk[indexOfInlineMarkup[chatId]]);
                    }
                    if (count[chatId] == 18)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams18qbmk[indexOfInlineMarkup[chatId]]);
                    }
                    if (count[chatId] == 20)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams20qbmk[indexOfInlineMarkup[chatId]]);
                    }

                }
                if (callbackQuery.Data == "but_21" && replyMessageType[chatId] != -1/*&& callbackQuery.Message != null*/)
                {
                    chatId = callbackQuery.Message.Chat.Id;
                    if (!indexOfInlineMarkup.ContainsKey(chatId))
                    {
                        return;
                    }
                    indexOfInlineMarkup[callbackQuery.Message.Chat.Id]++;
                    if (count[chatId] == 16)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams16qbmk[indexOfInlineMarkup[chatId]]);
                    }
                    if (count[chatId] == 18)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams18qbmk[indexOfInlineMarkup[chatId]]);
                    }
                    if (count[chatId] == 20)
                    {
                        await botClient.EditMessageReplyMarkupAsync(chatId,
                        lastMessage[chatId].MessageId, teams20qbmk[indexOfInlineMarkup[chatId]]);
                    }

                }
                if (callbackQuery.Data != "but_0" && callbackQuery.Data != "but_21" && selectedLeague.ContainsKey(chatId) && replyMessageType[chatId] != -1)
                {
                    foreach (var item in selectedLeague[chatId].TeamDic)
                    {
                        if (callbackQuery.Data == item.Key.ToString())
                        {
                            //chatId = callbackQuery.Message.Chat.Id;
                            if (indexOfSearch.ContainsKey(chatId))
                            {
                                indexOfSearch.Remove(chatId);
                            }
                            Message msg = new Message();
                            Chat c = new Chat();
                            msg.Text = item.Value;
                            c.Id = chatId;
                            msg.Chat = c;
                            replyMessageType[chatId] = -2;

                            await BotOnMessageReceived(botClient, msg, cancellationToken: cancellationToken);
                            if (lastMessage[chatId] != null)
                            {
                                botClient.DeleteMessageAsync(chatId: chatId, lastMessage[chatId].MessageId);
                            }

                        }


                    }
                }
                if (callbackQuery.Data == "but_21" && replyMessageType[chatId] == -1)
                {
                    string txt = "";
                    int difference = parsedTeam[chatId].Count - numOfPlayerList[chatId];
                    if (difference < 5)
                    {
                        for (int i = numOfPlayerList[chatId]; i < numOfPlayerList[chatId] + (parsedTeam[chatId].Count - numOfPlayerList[chatId]); i++)
                        {
                            for (int j = 0; j < parsedTeam[chatId][i].Count; j++)
                            {
                                txt += parsedTeam[chatId][i][j] + "\n";

                            }
                            txt += "_____" + "\n";
                        }
                        lastMessage[chatId] = await botClient.EditMessageTextAsync(chatId: chatId, lastMessage[chatId].MessageId, txt, replyMarkup: nxt_0, cancellationToken: cancellationToken);

                    }
                    else
                    {
                        for (int i = numOfPlayerList[chatId]; i < numOfPlayerList[chatId] + 5; i++)
                        {
                            for (int j = 0; j < parsedTeam[chatId][i].Count; j++)
                            {
                                txt += parsedTeam[chatId][i][j] + "\n";

                            }
                            txt += "_____" + "\n";
                        }
                        numOfPlayerList[chatId] = numOfPlayerList[chatId] + 5;
                        lastMessage[chatId] = await botClient.EditMessageTextAsync(chatId: chatId, lastMessage[chatId].MessageId, txt, replyMarkup: nxt_021, cancellationToken: cancellationToken);
                    }
                    
                    
                    
                }

                if (callbackQuery.Data == "but_0" && replyMessageType[chatId] == -1)
                {
                    string txt = "";
                    int difference = parsedTeam[chatId].Count - numOfPlayerList[chatId];
                    
                        for (int i = numOfPlayerList[chatId]-10; i < numOfPlayerList[chatId]-5; i++)
                        {
                            for (int j = 0; j < parsedTeam[chatId][i].Count; j++)
                            {
                                txt += parsedTeam[chatId][i][j] + "\n";
                            }
                            txt += "_____" + "\n";
                        }
                        
                        numOfPlayerList[chatId] = numOfPlayerList[chatId] - 5;
                        if (numOfPlayerList[chatId] == 5)
                        {
                            lastMessage[chatId] = await botClient.EditMessageTextAsync(chatId: chatId, lastMessage[chatId].MessageId, txt, replyMarkup: nxt_21, cancellationToken: cancellationToken);
                        }
                        else
                        {
                            lastMessage[chatId] = await botClient.EditMessageTextAsync(chatId: chatId, lastMessage[chatId].MessageId, txt, replyMarkup: nxt_021, cancellationToken: cancellationToken);
                        }
                    
                    
                }
            }
            
        }
       
        static async Task Main(string[] args)
        {
            #region Bot
            

            var me = botClient.GetMeAsync();
            using var cts = new CancellationTokenSource();
            
            var reseiverOptions = new ReceiverOptions
            {
               AllowedUpdates = { }
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                reseiverOptions,
                cancellationToken: cts.Token);
           authorizedUser = DataBase.WhoIsRegistrated();
           await objectParser.prstst();
           
           //await NewsLoader.MainPr(); 
            
            
            
            Console.ReadLine();
            cts.Cancel();
            #endregion
            
            
        }
        static Task HandleErrorAsync(
            ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        static public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("FSBot", "FootballSearchBot@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("Пользователь", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, true);
                await client.AuthenticateAsync("FootballSearchBot@yandex.ru", "uvaokejgtezboldt");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
    
}
