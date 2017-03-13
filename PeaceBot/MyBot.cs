using System;
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

            RegisterMemeCommand();
            RegisterPurgeCommand();

            _discord.ExecuteAndWait(async () =>
            {
                await _discord.Connect("MjkwODAzOTI0NTM2MDAwNTEy.C6gRvQ.eyMVt6U507LHn_T4hsgCxEkcXZc", TokenType.Bot);
            });
        }

        private void RegisterPurgeCommand()
        {
            _commands.CreateCommand("purge")
                .Parameter("purgeAmount")
                .Do(async (e) =>
                {
                    var amnt = 0;
                    try
                    {
                        amnt = Convert.ToInt32(e.GetArg("purgeAmount"));
                    }
                    catch (FormatException)
                    {
                        await e.Channel.SendMessage("Invalid amount.");
                    }
                    if (amnt <= 100)
                    {
                        var messagesToDelete = await e.Channel.DownloadMessages(amnt);
                        await e.Channel.DeleteMessages(messagesToDelete);
                        await e.Channel.SendMessage(e.GetArg("purgeAmount") + " messages deleted.");
                    }
                    else if(amnt > 100)
                    {
                        await e.Channel.SendMessage("The maximum amount is 100.");
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

        private static void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
