using System;
using System.Collections.Generic;
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


        private readonly string[] _freshestMemes;

        public MyBot()
        {
            rand = new Random();

            _freshestMemes = new[]
            {
                "meme/meme1.jpg",
                "meme/meme2.jpg",
                "meme/meme3.jpg"

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
                await _discord.Connect("MjkwODAzOTI0NTM2MDAwNTEy.C6gRvQ.eyMVt6U507LHn_T4hsgCxEkcXZc", TokenType.Bot);
            });
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
                .Do(async (u) =>
                {
                    var randomMemeIndex = rand.Next(_freshestMemes.Length);
                    var memeToPost = _freshestMemes[randomMemeIndex];
                    await u.Channel.SendFile(memeToPost);
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
    }
}
