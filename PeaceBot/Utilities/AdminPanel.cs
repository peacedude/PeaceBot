using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public AdminPanel(DiscordClient client, CommandEventArgs e)
        {
            this.client = client;
            this.e = e;

            InitializeComponent();
        }

        private void kickButton_Click(object sender, EventArgs e)
        {
            var usernameToKick = userList.SelectedItem.ToString().ToUpper();

            var userToKick =
                this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToKick);

            try
            {
                userToKick.Kick();
                MyBot.Log(this.e.User.Name + " tried to kick " + userToKick.Name);
            }

            catch (NullReferenceException ex)
            {
                Console.WriteLine("Did you select someone? Error: " + ex.Message);
            }
            
        }

        private void banButton_Click(object sender, EventArgs e)
        {
            var usernameToBan = userList.SelectedItem.ToString().ToUpper();

            var userToBan =
                this.e.Channel.Users.FirstOrDefault(input => input.Name.ToUpper() == usernameToBan);

            try
            {
                this.e.Server.Ban(userToBan);
                MyBot.Log(this.e.User.Name + " tried to ban " + userToBan.Name);
            }

            catch (NullReferenceException ex)
            {
                Console.WriteLine("Did you select someone? Error: " + ex.Message);
            }
            
        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            userList.Items.Clear();

            var serverUsers = this.e.Server.Users;

            foreach (var user in serverUsers)
            {
                if (!user.IsBot)
                {
                    userList.Items.Add(user.Name);
                }
            }
        }
    }
}
