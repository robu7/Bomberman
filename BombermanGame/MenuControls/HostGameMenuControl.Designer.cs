namespace BombermanGame
{
    partial class HostGameMenuControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerPanel = new System.Windows.Forms.Panel();
            this.StartGame = new System.Windows.Forms.Button();
            this.ChatWindow = new System.Windows.Forms.ListBox();
            this.ChatText = new System.Windows.Forms.TextBox();
            this.PlayerLabel = new System.Windows.Forms.Label();
            this.GamePanel = new System.Windows.Forms.Panel();
            this.PlayerList = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.IpLabel = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.ServerPanel.SuspendLayout();
            this.GamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerPanel
            // 
            this.ServerPanel.Controls.Add(this.ChatWindow);
            this.ServerPanel.Controls.Add(this.ChatText);
            this.ServerPanel.Controls.Add(this.StartGame);
            this.ServerPanel.Controls.Add(this.PlayerList);
            this.ServerPanel.Controls.Add(this.PlayerLabel);
            this.ServerPanel.Controls.Add(this.GamePanel);
            this.ServerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerPanel.Location = new System.Drawing.Point(0, 0);
            this.ServerPanel.Name = "ServerPanel";
            this.ServerPanel.Size = new System.Drawing.Size(1100, 1054);
            this.ServerPanel.TabIndex = 2;
            // 
            // StartGame
            // 
            this.StartGame.Location = new System.Drawing.Point(621, 261);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(75, 23);
            this.StartGame.TabIndex = 4;
            this.StartGame.Text = "Start Game";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            // 
            // ChatWindow
            // 
            this.ChatWindow.BackColor = System.Drawing.Color.Black;
            this.ChatWindow.Font = new System.Drawing.Font("MS PGothic", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ChatWindow.ForeColor = System.Drawing.Color.Chartreuse;
            this.ChatWindow.FormattingEnabled = true;
            this.ChatWindow.HorizontalScrollbar = true;
            this.ChatWindow.Location = new System.Drawing.Point(347, 327);
            this.ChatWindow.Name = "ChatWindow";
            this.ChatWindow.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.ChatWindow.Size = new System.Drawing.Size(349, 186);
            this.ChatWindow.TabIndex = 0;
            // 
            // ChatText
            // 
            this.ChatText.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ChatText.ForeColor = System.Drawing.Color.Chartreuse;
            this.ChatText.Location = new System.Drawing.Point(347, 519);
            this.ChatText.Name = "ChatText";
            this.ChatText.Size = new System.Drawing.Size(349, 20);
            this.ChatText.TabIndex = 0;
            this.ChatText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChatText_KeyUp);
            // 
            // PlayerLabel
            // 
            this.PlayerLabel.AutoSize = true;
            this.PlayerLabel.BackColor = System.Drawing.Color.Black;
            this.PlayerLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PlayerLabel.Location = new System.Drawing.Point(344, 164);
            this.PlayerLabel.Name = "PlayerLabel";
            this.PlayerLabel.Size = new System.Drawing.Size(41, 13);
            this.PlayerLabel.TabIndex = 0;
            this.PlayerLabel.Text = "Players";
            // 
            // GamePanel
            // 
            this.GamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GamePanel.Controls.Add(this.PortLabel);
            this.GamePanel.Controls.Add(this.textBox2);
            this.GamePanel.Controls.Add(this.IpLabel);
            this.GamePanel.Controls.Add(this.textBox1);
            this.GamePanel.Location = new System.Drawing.Point(0, 0);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(1100, 1054);
            this.GamePanel.TabIndex = 3;
            // 
            // PlayerList
            // 
            this.PlayerList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PlayerList.FormattingEnabled = true;
            this.PlayerList.ItemHeight = 20;
            this.PlayerList.Location = new System.Drawing.Point(347, 180);
            this.PlayerList.Name = "PlayerList";
            this.PlayerList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.PlayerList.Size = new System.Drawing.Size(120, 104);
            this.PlayerList.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(347, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // IpLabel
            // 
            this.IpLabel.AutoSize = true;
            this.IpLabel.Location = new System.Drawing.Point(347, 23);
            this.IpLabel.Name = "IpLabel";
            this.IpLabel.Size = new System.Drawing.Size(51, 13);
            this.IpLabel.TabIndex = 1;
            this.IpLabel.Text = "Server IP";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(347, 94);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(347, 78);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(59, 13);
            this.PortLabel.TabIndex = 3;
            this.PortLabel.Text = "Server port";
            // 
            // HostGameMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ServerPanel);
            this.Name = "HostGameMenuControl";
            this.Size = new System.Drawing.Size(1100, 1054);
            this.ServerPanel.ResumeLayout(false);
            this.ServerPanel.PerformLayout();
            this.GamePanel.ResumeLayout(false);
            this.GamePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ServerPanel;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Panel GamePanel;
        private System.Windows.Forms.ListBox ChatWindow;
        private System.Windows.Forms.TextBox ChatText;
        private System.Windows.Forms.Label PlayerLabel;
        private System.Windows.Forms.ListBox PlayerList;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.TextBox textBox1;
    }
}
