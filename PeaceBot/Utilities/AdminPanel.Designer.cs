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
            this.SuspendLayout();
            // 
            // kickButton
            // 
            this.kickButton.Location = new System.Drawing.Point(12, 430);
            this.kickButton.Name = "kickButton";
            this.kickButton.Size = new System.Drawing.Size(75, 23);
            this.kickButton.TabIndex = 0;
            this.kickButton.Text = "Kick User";
            this.kickButton.UseVisualStyleBackColor = true;
            this.kickButton.Click += new System.EventHandler(this.kickButton_Click);
            // 
            // banButton
            // 
            this.banButton.Location = new System.Drawing.Point(93, 430);
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
            this.userList.Location = new System.Drawing.Point(12, 28);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(234, 381);
            this.userList.Sorted = true;
            this.userList.TabIndex = 4;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(174, 430);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 487);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.banButton);
            this.Controls.Add(this.kickButton);
            this.Name = "AdminPanel";
            this.Text = "AdminPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button kickButton;
        private System.Windows.Forms.Button banButton;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.Button refreshButton;
    }
}