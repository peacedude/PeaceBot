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
        private string errorMessage;

        public AdminPanel(DiscordClient client, CommandEventArgs e)
        {
            this.client = client;
            this.e = e;

            InitializeComponent();
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
            var userToKick = this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToKick);
            if (!userToKick.HasRole((this.e.Server.FindRoles("Mod").FirstOrDefault())))
            {
                userToKick.Kick();
                MyBot.Log(this.e.User.Name + " tried to kick " + userToKick.Name);
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
            var userToBan =
                    this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToBan);
            if (!userToBan.HasRole((this.e.Server.FindRoles("Mod").FirstOrDefault())))
            {
                this.e.Server.Ban(userToBan);
                MyBot.Log(this.e.User.Name + " tried to ban " + userToBan.Name);
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
            errorLabel.Text = "";
            var serverUsers = this.e.Server.Users;
            var enumerable = serverUsers as User[] ?? serverUsers.ToArray();
            int users = 0;
            int bots = 0;


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
    }
}
