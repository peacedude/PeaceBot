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
        private readonly Random _rand;
        private int _lastNumber;
        public static bool OnceBool { get; set; }
        public string LogMessage { get; set; }

        private readonly string _discordToken = Utilities.Token.GetToken("DiscordToken");

        public MyBot()
        {
            _rand = new Random();

            Discord = new DiscordClient(x =>
            {
                x.AppName = "Peace Bot";
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

            Discord.UserJoined += async (object sender, UserEventArgs e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                LogMessage = user.Name + " has joined the server!";

                Console.WriteLine(LogMessage);

                Log(LogMessage);

                AddDefaultRoleToUser(e);

                await channel.SendMessage(LogMessage);
            };

            Discord.UserLeft += async (object sender, UserEventArgs e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                LogMessage = user.Name + " has left the server!";

                Console.WriteLine(LogMessage);

                Log(LogMessage);

                await channel.SendMessage(LogMessage);
            };

            Discord.UserBanned += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();
                Log(e.User.Name + " was banned.");
                await channel.SendMessage(e.User.Name + " was banned.");
            };

            Discord.ExecuteAndWait(async () =>
            {
                FixIfForceClosed();
                Log("Connecting to Discord API...");
                try
                {
                    await Discord.Connect(_discordToken, TokenType.Bot);
                }
                catch (Exception ex)
                {
                    Log("Could not connect to Discord API.\n:Error Message: " + ex.Message);
                    Console.WriteLine("Could not connect to Discord API.\n" + ex.Message);
                }
            });



        }

        public async void AddDefaultRoleToUser(UserEventArgs e)
        {
            foreach (var role in e.Server.Roles)
            {
                if (role.Name == "Low Trash")
                {
                    if (!e.User.HasRole(role))
                    {
                        var rolesToAdd = new Role[1];
                        rolesToAdd[0] = role;
                        LogMessage = e.User.Name + " has been granted the role " + role;
                        Console.WriteLine(LogMessage);
                        Log(LogMessage);
                        await e.User.AddRoles(rolesToAdd);
                    }
                }
            }
        }

        private void RegisterCommands()
        {
            RegisterMemeCommand();
            RegisterPurgeCommand();
            RegisterTrashCommand();
            RegisterPeaceCommandsCommand();
            RegisterShutDownCommand();
            RegisterKickCommand();
        }

        private void RegisterPeaceCommandsCommand()
        {
            Commands.CreateCommand("peacecommands")
    .Do(async (e) =>
                {
                    LogMessage = e.User.Name + " on " + e.Server.Name + " requested commands.";
                    Log(LogMessage);
                    Console.WriteLine(LogMessage);
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
                        LogMessage = e.User.Name + " turned me off.";
                        Console.WriteLine(LogMessage);
                        Log(LogMessage);
                        await e.Channel.SendMessage("Bye!");
                        Thread.Sleep(1000);
                        await Discord.Disconnect();
                    }
                    else
                    {
                        LogMessage = (e.User.Name + " tried to turn me off.");
                        Console.WriteLine(LogMessage);
                        Log(LogMessage);
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
                            LogMessage = e.User.Name + " on " + e.Server.Name + " purged " + amnt + " messages.";
                            Log(LogMessage);
                            Console.WriteLine(LogMessage);
                        }
                        else if (amnt > 100)
                        {
                            LogMessage = e.User.Name + " on " + e.Server.Name + " tried to purged " + amnt +
                                         " messages but the limit is 100.";
                            Log(LogMessage);
                            Console.WriteLine(LogMessage);
                            await e.Channel.SendMessage(e.User.Mention + ",The maximum purge amount is 100.");
                        }
                        else
                        {
                            LogMessage = e.User.Name + " on " + e.Server.Name + " tried to purged " + amnt +
                                         " messages but it was an invalid parameter.";
                            Log(LogMessage);
                            Console.WriteLine(LogMessage);
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
                    string currentMeme = GetFreshMemes();
                    await e.Channel.SendMessage(currentMeme);
                    LogMessage = e.User.Name + " on " + e.Server.Name + " requested a meme and got " + currentMeme;
                    Log(LogMessage);
                    Console.WriteLine(LogMessage);
                });
        }

        private void RegisterTrashCommand()
        {
            Commands.CreateCommand("trash")
                .Do(async (e) =>
                {
                    LogMessage = e.User.Name + " gave himself OG Role.";
                    var userRole = e.Server.FindRoles("OG").FirstOrDefault();
                    Console.WriteLine(LogMessage);
                    Log(LogMessage);
                    await e.User.AddRoles(userRole);
                });
        }

        private void RegisterKickCommand()
        {
            Commands.CreateCommand("kick")
                .Parameter("userToKick", ParameterType.Optional)
                .Do(async (e) =>
                {
                    string userToKick = e.GetArg("userToKick");
                    var user = e.Server.FindUsers(userToKick).FirstOrDefault();
                    LogMessage = e.User.Name + " tried to kick the user " + user.Name;
                    Console.WriteLine(LogMessage);
                    Log(LogMessage);
                    if (user != null)
                    {
                        if (e.User.HasRole(e.Server.FindRoles("Mod").FirstOrDefault()))
                        {
                            LogMessage = user.Name + " was kicked.";
                            Console.WriteLine(LogMessage);
                            Log(LogMessage);
                            await e.Channel.SendMessage(LogMessage);
                            await user.Kick();
                        }
                        else
                        {
                            await e.Channel.SendMessage(e.User.Mention + " just tried to kick " + user.Mention +
                                                        " but did not have the correct role. What a noob!");
                        }

                    }
                    else
                    {
                        await e.Channel.SendMessage(userToKick + " is not a valid target.");
                    }

                });
        }

        private static void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
            using (var w =
           new StreamWriter("Memory/log.txt", true))
            {

                if (OnceBool == false)
                {
                    w.WriteLine("\n-------------------|Start logging|-----------------------");
                    OnceBool = true;
                }
                w.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), e.Message);

            }
        }

        private static void FixIfForceClosed()
        {
            var lastLine = File.ReadLines("Log/log.txt").Last();
            if (lastLine.Contains("peacedude turned me off.")) return;
            if (!lastLine.Contains("Disconnected"))
            {

                Log("Bot was forced to close last session.");
            }
        }

        public static void Log(string logMessage)
        {
            using (var w =
            new StreamWriter("Log/log.txt", true))
            {

                if (OnceBool == false)
                {
                    w.WriteLine("\n-------------------|Start logging|-----------------------");
                    OnceBool = true;
                }
                w.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
            }
        }

        public string GetFreshMemes()
        {
            var rnd = new Random();
            var freshestMemes = new[]
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
            var randomMemeIndex = rnd.Next(freshestMemes.Length);

            while (randomMemeIndex == _lastNumber)
            {
                randomMemeIndex = _rand.Next(freshestMemes.Length);
            }
            _lastNumber = randomMemeIndex;
            var memeToPost = freshestMemes[randomMemeIndex];
            return memeToPost;
        }


    }

}
