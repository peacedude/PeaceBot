using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace PeaceBot
{
    class MyBot
    {
        private DiscordClient discord;
        CommandService commands;

        public MyBot()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '#';
                x.AllowMentionPrefix = true;
            });

            
            commands = discord.GetService<CommandService>();


            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjkwODAzOTI0NTM2MDAwNTEy.C6gRvQ.eyMVt6U507LHn_T4hsgCxEkcXZc", TokenType.Bot);
            });
        }



        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
