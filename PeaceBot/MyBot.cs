using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using Discord.Commands;

namespace PeaceBot
{
    class MyBot
    {
        private readonly DiscordClient _discord;
        private readonly CommandService _commands;

        private Random rand;

        private const string sessionIDPath = "Memory/SessionID.txt";
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
                "http://i.imgur.com/kAgqBPQ.jpg"

            };

            _discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Debug;
                x.LogHandler = Log;
            });

            _discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });


            _commands = _discord.GetService<CommandService>();

            RegisterCommands();

            _discord.ExecuteAndWait(async () =>
            {
                
                try
                {
                    await _discord.Connect("MjkwODAzOTI0NTM2MDAwNTEy.C6hk0Q.wcG1X7t8ZzbF0pKzC4tCYZSYcRE", TokenType.Bot);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not connect to Discord API.\n" + ex.Message);
                }
            });
            
            var sesID = _discord.SessionId;
            var cancelToken =_discord.CancelToken;
            lineChanger(sesID, 1);
        }


        private void RegisterCommands()
        {
            RegisterMemeCommand();
            RegisterPurgeCommand();
            RegisterTestCommand();
            RegisterPeaceCommandsCommand();
        }

        private void RegisterPeaceCommandsCommand()
        {
            _commands.CreateCommand("peacecommands")
    .Do(async (e) =>
    {
        await e.Channel.SendMessage("!peacecommands - Get commands.\n!purge param - Delete 'param' amount of messages. Default = 5\n!meme - Posts a meme.");

    });
        }

        private void RegisterPurgeCommand()
        {
            _commands.CreateCommand("purge")
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
            _commands.CreateCommand("meme")
                .Do(async (e) =>
                {
                    var randomMemeIndex = rand.Next(_freshestMemes.Length);
                    var memeToPost = _freshestMemes[randomMemeIndex];
                    await e.Channel.SendMessage(memeToPost);
                    // Old Meme Command. Reads from file.
                    //var randomMemeIndex = rand.Next(_freshestMemes.Length);
                    //var memeToPost = _freshestMemes[randomMemeIndex];
                    //await e.Channel.SendFile(memeToPost);
                });
        }

        private void RegisterTestCommand()
        {
            _commands.CreateCommand("test")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("https://i.imgur.com/Obg1wqc.jpg");
                });
        }


        private static void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void lineChanger(string newText, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(sessionIDPath);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(sessionIDPath, arrLine);
        }
    }

}
