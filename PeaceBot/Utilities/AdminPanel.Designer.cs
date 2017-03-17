namespace PeaceBot.Utilities
{
    partial class AdminPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kickButton = new System.Windows.Forms.Button();
            this.banButton = new System.Windows.Forms.Button();
            this.userList = new System.Windows.Forms.ListBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.usersOnServer = new System.Windows.Forms.Label();
            this.botsOnServer = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kickButton
            // 
            this.kickButton.Location = new System.Drawing.Point(7, 404);
            this.kickButton.Name = "kickButton";
            this.kickButton.Size = new System.Drawing.Size(75, 23);
            this.kickButton.TabIndex = 0;
            this.kickButton.Text = "Kick User";
            this.kickButton.UseVisualStyleBackColor = true;
            this.kickButton.Click += new System.EventHandler(this.kickButton_Click);
            // 
            // banButton
            // 
            this.banButton.Location = new System.Drawing.Point(88, 404);
            this.banButton.Name = "banButton";
            this.banButton.Size = new System.Drawing.Size(75, 23);
            this.banButton.TabIndex = 3;
            this.banButton.Text = "Ban User";
            this.banButton.UseVisualStyleBackColor = true;
            this.banButton.Click += new System.EventHandler(this.banButton_Click);
            // 
            // userList
            // 
            this.userList.FormattingEnabled = true;
            this.userList.Items.AddRange(new object[] {
            "Refresh to see users"});
            this.userList.Location = new System.Drawing.Point(18, 33);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(234, 381);
            this.userList.Sorted = true;
            this.userList.TabIndex = 4;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(169, 404);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // usersOnServer
            // 
            this.usersOnServer.AutoSize = true;
            this.usersOnServer.Location = new System.Drawing.Point(12, 465);
            this.usersOnServer.Name = "usersOnServer";
            this.usersOnServer.Size = new System.Drawing.Size(46, 13);
            this.usersOnServer.TabIndex = 6;
            this.usersOnServer.Text = "Users: 0";
            // 
            // botsOnServer
            // 
            this.botsOnServer.AutoSize = true;
            this.botsOnServer.Location = new System.Drawing.Point(90, 465);
            this.botsOnServer.Name = "botsOnServer";
            this.botsOnServer.Size = new System.Drawing.Size(40, 13);
            this.botsOnServer.TabIndex = 7;
            this.botsOnServer.Text = "Bots: 0";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.errorLabel.Location = new System.Drawing.Point(142, 461);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 17);
            this.errorLabel.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.refreshButton);
            this.groupBox1.Controls.Add(this.kickButton);
            this.groupBox1.Controls.Add(this.banButton);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 436);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Control";
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 487);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.botsOnServer);
            this.Controls.Add(this.usersOnServer);
            this.Controls.Add(this.groupBox1);
            this.Name = "AdminPanel";
            this.Text = "AdminPanel";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button kickButton;
        private System.Windows.Forms.Button banButton;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label usersOnServer;
        private System.Windows.Forms.Label botsOnServer;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}