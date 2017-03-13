using System;
using System.IO;
using System.Linq;
using System.Threading;
using Discord;
using Discord.Commands;

namespace PeaceBot
{
    class MyBot
    {
        public readonly DiscordClient Discord;
        public readonly CommandService Commands;
        private Random rand;
        private int _lastNumber;
        public static bool OnceBool { get; set; }
        public static bool TurnOff { get; set; }
        private readonly string[] _freshestMemes;

        public MyBot()
        {
            rand = new Random();

            _freshestMemes = new[]
            {
                "http://i.imgur.com/qmvnneb.png",
                "http://i.imgur.com/4wZckhH.jpg",
                "http://i.imgur.com/8BzJSNM.jpg",
                "http://i.imgur.com/mt3PApY.gif",
                "http://i.imgur.com/rpUlDw2.jpg",
                "http://i.imgur.com/JAbJNU8.jpg",
                "http://i.imgur.com/STHLxsq.jpg",
                "http://i.imgur.com/kAgqBPQ.jpg",
                "https://i.redd.it/ue6uztfw36ly.jpg",
                "https://i.redd.it/vlufm6a768ly.jpg",
                "https://i.imgur.com/cej88C3.jpg",
                "http://i.imgur.com/s4EA4tD.jpg",
                "https://i.imgur.com/Hozfqi5.jpg",
                "https://i.redd.it/uf3nt10u43ly.jpg",
                "https://i.imgur.com/qVPf2sv.jpg"

            };

            Discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            Discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });


            Commands = Discord.GetService<CommandService>();

            RegisterCommands();

            Discord.ExecuteAndWait(async () =>
            {
                Log("Connecting to Discord API...");
                try
                {
                    await Discord.Connect("MjkwODAzOTI0NTM2MDAwNTEy.C6hk0Q.wcG1X7t8ZzbF0pKzC4tCYZSYcRE", TokenType.Bot);
                }
                catch (Exception ex)
                {
                    Log("Could not connect to Discord API.\n:Error Message: " + ex.Message);
                    Console.WriteLine("Could not connect to Discord API.\n" + ex.Message);
                }
                Log("Bot connected to Discord.");
            });


        }


        private void RegisterCommands()
        {
            RegisterMemeCommand();
            RegisterPurgeCommand();
            RegisterTrashCommand();
            RegisterPeaceCommandsCommand();
            RegisterShutDownCommand();
        }

        private void RegisterPeaceCommandsCommand()
        {
            Commands.CreateCommand("peacecommands")
    .Do(async (e) =>
    {
        Log(e.User.Name + " on " + e.Server.Name + " requested commands.");
        string commString = "";
        commString += "!peacecommands - Get commands.\n!meme - Posts a meme.";
        if (e.User.Name == "peacedude")
        {
            commString += "\n!quit - Turn off bot";
        }
        if (e.User.HasRole(e.Server.FindRoles("Mod").FirstOrDefault()))
        {
            commString += "\n!purge param - Delete 'param' amount of messages. Default = 5";
        }
        await e.User.SendMessage(commString);
    });
        }

        private void RegisterShutDownCommand()
        {
            Commands.CreateCommand("quit")
                .Do(async (e) =>
                {
                    if (e.User.Name == "peacedude")
                    {
                        Console.WriteLine(e.User.Name + " turned me off.");
                        TurnOff = true;
                        Log(e.User.Name + " turned me off.");
                        await e.Channel.SendMessage("Bye!");
                        Thread.Sleep(1000);
                        await Discord.Disconnect();
                    }
                    else
                    {
                        Console.WriteLine(e.User.Name + " tried to turn me off.");
                        await e.Channel.SendMessage(e.User.Name + " don't touch me there <:gachiGASM:231525548390744064>");
                    }
                });
        }

        private void RegisterPurgeCommand()
        {
            Commands.CreateCommand("purge")
                .Parameter("purgeAmount", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (e.User.HasRole(e.Server.FindRoles("Mod").FirstOrDefault()))
                    {
                        int amnt = int.TryParse(e.GetArg("purgeAmount"), out amnt) ? int.Parse(e.GetArg("purgeAmount")) : 5;

                        if (amnt <= 100)
                        {
                            var messagesToDelete = await e.Channel.DownloadMessages(amnt);
                            await e.Channel.DeleteMessages(messagesToDelete);
                            await e.Channel.SendMessage(amnt + " messages deleted.");
                            Log(e.User.Name + " on " + e.Server.Name + " purged " + amnt + " messages.");
                            Console.WriteLine(e.User.Name + " on " + e.Server.Name + " purged " + amnt + " messages.");
                        }
                        else if (amnt > 100)
                        {
                            await e.Channel.SendMessage("The maximum amount is 100.");
                        }
                        else
                        {
                            await e.Channel.SendMessage("Invalid amount.");
                        }
                    }

                });
        }

        private void RegisterMemeCommand()
        {
            Commands.CreateCommand("meme")
                .Do(async (e) =>
                {

                    var randomMemeIndex = rand.Next(_freshestMemes.Length);
                    while (randomMemeIndex == _lastNumber)
                    {
                        randomMemeIndex = rand.Next(_freshestMemes.Length);
                    }
                    var memeToPost = _freshestMemes[randomMemeIndex];
                    await e.Channel.SendMessage(memeToPost);
                    _lastNumber = randomMemeIndex;
                    Log(e.User.Name + " on " + e.Server.Name + " requested a meme and got " + memeToPost);
                    Console.WriteLine(e.User.Name + " on " + e.Server.Name + " requested a meme and got " + memeToPost);
                });
        }

        private void RegisterTrashCommand()
        {
            Commands.CreateCommand("trash")
                .Do(async (e) =>
                {
                    var userRole = e.Server.FindRoles("OG").FirstOrDefault();
                    await e.User.AddRoles(userRole);
                });
        }


        private static void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public static void Log(string logMessage)
        {
            using (StreamWriter w =
            new StreamWriter("Memory/log.txt", true))
            {

                if (OnceBool == false)
                {
                    w.WriteLine("-------------------|Starting bot|-----------------------");
                    OnceBool = true;
                }
                w.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
                if (TurnOff)
                {
                    w.WriteLine("-------------------|Stopping bot|-----------------------\n");
                }
            }
        }

        
    }

}
