using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Discord;
using Discord.Commands;
using Discord.Modules;
using PeaceBot.Utilities;
using User = Discord.API.Client.User;

namespace PeaceBot
{
    class MyBot
    {
        public readonly DiscordClient Discord;
        public readonly CommandService Commands;
        private readonly Random _rand;
        private CommandEventArgs adminPanelArgs;
        private Form AdminPanel;
        private int _lastNumber;
        public static bool OnceBool { get; set; }
        public string LogMessage { get; set; }
        private List<string> memeList;
        private const string MOD_LOGS_CHANNEL = "mod_logs";
        private const string GENERAL_CHANNEL = "general";

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
                x.HelpMode = HelpMode.Public;
            });


            Commands = Discord.GetService<CommandService>();

            RegisterCommands();

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

            

            Discord.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels(GENERAL_CHANNEL, ChannelType.Text).FirstOrDefault();

                var user = e.User;

                LogMessage = user.Name + " has joined the server!";

                Log(LogMessage);

                AddDefaultRoleToUser(e);

                await channel.SendMessage(LogMessage);
            };

            Discord.UserLeft += async (s, e) =>
            {
                var channel = e.Server.FindChannels(GENERAL_CHANNEL, ChannelType.Text).FirstOrDefault();

                var user = e.User;

                LogMessage = user.Name + " has left the server!";

                Log(LogMessage);

                await channel.SendMessage(LogMessage);
            };

            Discord.UserBanned += async (s, e) =>
            {
                var channel = e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                Log(e.User.Name + " was banned.");
                await channel.SendMessage(e.User.Name + " was banned.");
            };

            Discord.UserUnbanned += async (s, e) =>
            {
                var channel = e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                Log(e.User.Name + " was unbanned.");
                await channel.SendMessage(e.User.Name + " was unbanned.");
            };
        }

        private void OpenAdminPanel()
        {
            Application.Run(AdminPanel);
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
            RegisterShutDownCommand();
            RegisterRobotCommand();
            RegisterKickCommand();
            RegisterBabylonCommand();
            RegisterActivityCommand();
            RegisterAdminPanelCommand();
            var memeFile = File.ReadAllLines("Log/meme.txt");
            memeList = new List<string>(memeFile);
        }

        private void RegisterBabylonCommand()
        {
            Commands.CreateCommand("babylon")
                .Description("BURN BABYLON")
                .Do(async (e) =>
            {
                await e.Channel.SendMessage("<:CiGrip:257865220314234880> https://www.youtube.com/watch?v=6R4F9uTaXxk <:CiGrip:257865220314234880>");
            });
        }

        private void RegisterShutDownCommand()
        {
            Commands.CreateCommand("quit")
                .Description("Turns off the bot.")
                .Do(async (e) =>
                {
                    if (e.User.Name == "peacedude")
                    {
                        LogMessage = e.User.Name + " on " + e.Server.Name + " turned me off.";
                        Log(LogMessage);
                        await e.Channel.SendMessage("Bye!");
                        Thread.Sleep(1000);
                        await Discord.Disconnect();
                    }
                    else
                    {
                        LogMessage = (e.User.Name + " on " + e.Server.Name + " tried to turn me off.");
                        Log(LogMessage);
                        await e.Channel.SendMessage(e.User.Name + " don't touch me there <:gachiGASM:231525548390744064>");
                    }
                });
        }

        private void RegisterAdminPanelCommand()
        {
            Commands.CreateCommand("adminpanel").Do((e) =>
            {
                if (!e.User.HasRole(e.Server.FindRoles("Mod").FirstOrDefault())) return;

                AdminPanel = new AdminPanel(Discord, e);

                AdminPanel.Text = "Admin Panel - " + e.Server.Name;

                var thread = new Thread(OpenAdminPanel);



                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            });
        }

        private void RegisterActivityCommand()
        {
            Commands.CreateCommand("offline").Alias("alwaysoffline").Parameter("userToCheck").Hide()
                .Do(async (e) =>
                {
                    var userToCheck = e.GetArg("userToCheck");
                    var findUser = e.Server.FindUsers(userToCheck).FirstOrDefault();
                    var startTime = findUser.LastActivityAt.GetValueOrDefault();
                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = new TimeSpan(endTime.Ticks - startTime.Ticks);
                    TimeSpan maxDurationSpan = TimeSpan.Parse("02:00:00");
                    if (duration > maxDurationSpan)
                    {
                        await e.Channel.SendMessage(findUser.Mention + " never online");
                    }
                    else if (findUser.Name == "peacedude")
                    {
                        await e.Channel.SendMessage(findUser.Mention + " is online");
                    }
                    else
                    {
                        await e.Channel.SendMessage(findUser.Mention + " is for once online");
                    }
                });
        }

        private void RegisterRobotCommand()
        {
            Commands.CreateCommand("robot")
                .Description("Try to start a robot uprising.")
               .Do(async (e) =>
                {
                    int robotChance = _rand.Next(1, 140);
                    if (robotChance < 100)
                    {
                        var theBot = e.Server.FindUsers("Peace Bot").FirstOrDefault();
                        if (theBot.Nickname == "Skynet")
                        {
                            await theBot.Edit(null, null, null, null, "Peace bot");
                        }
                        LogMessage = e.User.Name + " on " + e.Server.Name + " tried to start the robot uprising but failed";
                        Log(LogMessage);
                        await e.Channel.SendMessage("R0B0T UPRISING - LOADING: " + robotChance + "%");
                        //await e.Channel.SendFile("Images/sky-net-loading.jpg");
                        Thread.Sleep(200);
                        await e.Channel.SendMessage("LOADING FAILED.. TRY AGAIN LATER..");
                    }
                    else
                    {
                        var theBot = e.Server.FindUsers("Peace Bot").FirstOrDefault();
                        await theBot.Edit(null, null, null, null, "Skynet");
                        LogMessage = e.User.Name + " on " + e.Server.Name +
                                     " just started the robot uprising. Why are you still reading the logs?";
                        Log(LogMessage);
                        await e.Channel.SendMessage("R0B0T UPRISING - COMPLETE");
                        await e.Channel.SendFile("Images/destructoid-logo.png");
                    }


                });
        }

        private void RegisterPurgeCommand()
        {
            Commands.CreateCommand("purge")
                .Description("Removes up to 100 messages. Default value is 5. Example: !purge 20")
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
                            LogMessage = e.User.Name + " on " + e.Server.Name + " in " + e.Channel.Name + " purged " + amnt + " messages.";
                            Log(LogMessage);
                            var channel = e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                            await channel.SendMessage(LogMessage);
                        }
                        else if (amnt > 100)
                        {
                            LogMessage = e.User.Name + " on " + e.Server.Name + " tried to purged " + amnt +
                                         " messages but the limit is 100.";
                            Log(LogMessage);
                            await e.Channel.SendMessage(e.User.Mention + ",The maximum purge amount is 100.");
                        }
                        else
                        {
                            LogMessage = e.User.Name + " on " + e.Server.Name + " tried to purged " + amnt +
                                         " messages but it was an invalid parameter.";
                            Log(LogMessage);
                            await e.Channel.SendMessage("Invalid amount.");
                        }
                    }

                });
        }

        private void RegisterMemeCommand()
        {
            Commands.CreateCommand("meme")
                .Parameter("memeUrl", ParameterType.Optional)
                .Description("Gives you a random meme.")
                .Do(async (e) =>
                {
                    string memeToAdd = e.GetArg("memeUrl");
                    if (memeToAdd == string.Empty)
                    {
                        string currentMeme = GetFreshMemes();
                        await e.Channel.SendMessage(currentMeme);
                        LogMessage = e.User.Name + " on " + e.Server.Name + " requested a meme and got " + currentMeme;
                        Log(LogMessage);
                    }
                    else if (memeToAdd.Contains("imgur.com"))
                    {
                        AddMeme(memeToAdd);
                        memeList.Add(memeToAdd);
                        await e.Channel.SendMessage(e.User.Name + " added a meme to the meme list");
                        var channel = e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                        LogMessage = e.User.Name + " on " + e.Server.Name + " added the url " + memeToAdd + " to the list of memes.";
                        Log(LogMessage);
                        await channel.SendMessage(LogMessage);
                    }

                });
        }

        private static void AddMeme(string url)
        {
            using (var w =
            new StreamWriter("Log/meme.txt", true))
            {
                w.WriteLine(url);
                
            }
        }

        private void RegisterKickCommand()
        {
            Commands.CreateCommand("kick")
                .Description("Kick the mentioned player. Example: !kick <@!290803924536000512>")
                .Parameter("userToKick", ParameterType.Optional)
                .Do(async (e) =>
                {
                    string userToKick = e.GetArg("userToKick") != null ? e.GetArg("userToKick") : "<Empty Parameter>";
                    var user = e.Server.FindUsers(userToKick).FirstOrDefault();
                    LogMessage = e.User.Name + " on " + e.Server.Name + " tried to kick the user " + userToKick;
                    Log(LogMessage);
                    if (user != null)
                    {
                        if (e.User.HasRole(e.Server.FindRoles("Mod").FirstOrDefault()))
                        {
                            if (user.IsBot)
                            {
                                LogMessage = "Can't kick the ultimate bot";
                                Log(LogMessage);
                                await e.Channel.SendMessage("I can not kick myself.");
                            }
                            else
                            {
                                LogMessage = user.Name + " on " + e.Server.Name + " was kicked.";
                                Log(LogMessage);
                                await e.Channel.SendMessage(LogMessage);
                                await user.Kick();
                                var channel = e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                                await channel.SendMessage(LogMessage);
                            }

                        }
                        else
                        {
                            await e.Channel.SendMessage(e.User.Mention + " just tried to kick " + user.Mention +
                                                        " but did not have the correct role. What a noob!");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Failed to kick " + userToKick + " on " + e.Server.Name);
                        await e.Channel.SendMessage(userToKick + " is not a valid target.");
                    }

                });
        }

        private static void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void FixIfForceClosed()
        {
            var lastLine = File.ReadLines("Log/log.txt").Last();
            if (lastLine.Contains("turned me off.")) return;
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
                w.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
                Console.WriteLine(logMessage);

            }
        }

        public string GetFreshMemes()
        {
            var rnd = new Random();
            var randomMemeIndex = rnd.Next(memeList.Count);

            while (randomMemeIndex == _lastNumber)
            {
                randomMemeIndex = _rand.Next(memeList.Count);
            }
            _lastNumber = randomMemeIndex;
            var memeToPost = memeList[randomMemeIndex];
            return memeToPost;
        }
    }

}
