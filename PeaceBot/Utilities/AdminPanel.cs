using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using Discord.Commands;

namespace PeaceBot.Utilities
{
    public partial class AdminPanel : Form
    {
        private CommandEventArgs e;
        private DiscordClient client;
        private DateTime startTime;
        private DateTime currentTime;
        private string errorMessage;
        private const string MOD_LOGS_CHANNEL = "mod_logs";

        public AdminPanel(DiscordClient client, CommandEventArgs e, DateTime startTime)
        {
            this.client = client;
            this.e = e;
            this.startTime = startTime;

            InitializeComponent();

            foreach (var channel in this.e.Server.TextChannels)
            {
                channelBox.Items.Add(channel.Name);
            }
        }


        private void kickButton_Click(object sender, EventArgs e)
        {
            string usernameToKick = string.Empty;
            try
            {
                usernameToKick = userList.SelectedItem.ToString().ToUpper();
            }

            catch (NullReferenceException)
            {
                errorMessage = "Error: Did you select someone?";
                Console.WriteLine(errorMessage);
                errorLabel.Text = errorMessage;
            }
            var channel = this.e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
            var userToKick = this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToKick);
            if (!userToKick.HasRole((this.e.Server.FindRoles("Mod").FirstOrDefault())))
            {
                userToKick.Kick();
                MyBot.Log(this.e.User.Name + " tried to kick " + userToKick.Name);
                channel.SendMessage(this.e.User.Name + " kicked " + userToKick.Name);
            }
            else
            {
                errorLabel.Text = "Error: Can't kick a Mod or Owner";
            }

        }

        private void banButton_Click(object sender, EventArgs e)
        {
            string usernameToBan = string.Empty;
            try
            {
                usernameToBan = userList.SelectedItem.ToString().ToUpper(); 
            }

            catch (NullReferenceException)
            {
                errorMessage = "Error: Did you select someone?";
                Console.WriteLine(errorMessage);
                errorLabel.Text = errorMessage;
            }
            var channel = this.e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
            var userToBan =
                    this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToBan);
            if (!userToBan.HasRole((this.e.Server.FindRoles("Mod").FirstOrDefault())))
            {
                this.e.Server.Ban(userToBan);
                MyBot.Log(this.e.User.Name + " tried to ban " + userToBan.Name);
                channel.SendMessage(this.e.User.Name + " banned " + userToBan.Name);
            }
            else
            {
                errorLabel.Text = "Error: Can't ban a Mod or Owner";
            }

        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            userList.Items.Clear();
            channelBox.Items.Clear();
            errorLabel.Text = "";
            currentTime = DateTime.Now;
            var minutes = Math.Round((currentTime - startTime).TotalMinutes, 2);
            uptimeLabel.Text = $"Uptime: {minutes} minutes";
            var serverUsers = this.e.Server.Users;
            
                
            var enumerable = serverUsers as User[] ?? serverUsers.ToArray();
            int users = 0;
            int bots = 0;


            foreach (var channel in this.e.Server.TextChannels)
            {
                channelBox.Items.Add(channel.Name);
            }
           


            foreach (var user in enumerable)
            {
                if (!user.IsBot)
                {
                    userList.Items.Add(user.Name);
                    users++;
                }
                else if (user.IsBot)
                {
                    bots++;
                }
            }


            usersOnServer.Text = $"Users: {users}";
            botsOnServer.Text = $"Bots: {bots}";
        }

        private async void purgeButton_Click(object sender, EventArgs e)
        {
            int amnt = int.TryParse(purgeTextBox.Text, out amnt) ? int.Parse(purgeTextBox.Text) : 5;
            var channel = this.e.Server.FindChannels(channelBox.Text).FirstOrDefault();
            var LogMessage = string.Empty;

            if (amnt <= 100)
            {
                var messagesToDelete = await this.e.Channel.DownloadMessages(amnt);
                await this.e.Channel.DeleteMessages(messagesToDelete);
                await this.e.Channel.SendMessage(amnt + " messages deleted.");
                LogMessage = this.e.User.Name + " on " + this.e.Server.Name + " in " + this.e.Channel.Name + " purged " + amnt + " messages.";
                MyBot.Log(LogMessage);
                channel = this.e.Server.FindChannels(MOD_LOGS_CHANNEL, ChannelType.Text).FirstOrDefault();
                await channel.SendMessage(LogMessage);
            }
            else if (amnt > 100)
            {
                LogMessage = this.e.User.Name + " on " + this.e.Server.Name + " tried to purged " + amnt +
                             " messages but the limit is 100.";
                MyBot.Log(LogMessage);
                await this.e.Channel.SendMessage(this.e.User.Mention + ",The maximum purge amount is 100.");
            }
            else
            {
                LogMessage = this.e.User.Name + " on " + this.e.Server.Name + " tried to purged " + amnt +
                             " messages but it was an invalid parameter.";
                MyBot.Log(LogMessage);
                await this.e.Channel.SendMessage("Invalid amount.");
            }
        }
    }
}
