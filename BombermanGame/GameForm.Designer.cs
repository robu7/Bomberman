namespace BombermanGame
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.JoinGamePanel = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ServerPanel = new System.Windows.Forms.Panel();
            this.StartGame = new System.Windows.Forms.Button();
            this.Player1 = new System.Windows.Forms.TextBox();
            this.Player2 = new System.Windows.Forms.TextBox();
            this.Player3 = new System.Windows.Forms.TextBox();
            this.Player4 = new System.Windows.Forms.TextBox();
            this.GamePanel = new System.Windows.Forms.Panel();
            this.CreateGameButton = new System.Windows.Forms.Button();
            this.JoinButton = new System.Windows.Forms.Button();
            this.MainPanel.SuspendLayout();
            this.JoinGamePanel.SuspendLayout();
            this.ServerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ServerPanel);
            this.MainPanel.Controls.Add(this.JoinGamePanel);
            this.MainPanel.Controls.Add(this.CreateGameButton);
            this.MainPanel.Controls.Add(this.JoinButton);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1100, 1054);
            this.MainPanel.TabIndex = 0;
            // 
            // JoinGamePanel
            // 
            this.JoinGamePanel.Controls.Add(this.textBox1);
            this.JoinGamePanel.Controls.Add(this.textBox4);
            this.JoinGamePanel.Controls.Add(this.textBox3);
            this.JoinGamePanel.Controls.Add(this.textBox2);
            this.JoinGamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JoinGamePanel.Location = new System.Drawing.Point(0, 0);
            this.JoinGamePanel.Name = "JoinGamePanel";
            this.JoinGamePanel.Size = new System.Drawing.Size(1100, 1054);
            this.JoinGamePanel.TabIndex = 2;
            this.JoinGamePanel.Visible = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(12, 153);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 109);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 64);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // ServerPanel
            // 
            this.ServerPanel.Controls.Add(this.Player1);
            this.ServerPanel.Controls.Add(this.Player2);
            this.ServerPanel.Controls.Add(this.Player3);
            this.ServerPanel.Controls.Add(this.Player4);
            this.ServerPanel.Controls.Add(this.StartGame);
            this.ServerPanel.Controls.Add(this.GamePanel);
            this.ServerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerPanel.Location = new System.Drawing.Point(0, 0);
            this.ServerPanel.Name = "ServerPanel";
            this.ServerPanel.Size = new System.Drawing.Size(1100, 1054);
            this.ServerPanel.TabIndex = 1;
            this.ServerPanel.Visible = false;
            // 
            // StartGame
            // 
            this.StartGame.Location = new System.Drawing.Point(12, 179);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(75, 23);
            this.StartGame.TabIndex = 4;
            this.StartGame.Text = "Start Game";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            // 
            // Player1
            // 
            this.Player1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Player1.Location = new System.Drawing.Point(12, 38);
            this.Player1.Name = "Player1";
            this.Player1.ReadOnly = true;
            this.Player1.Size = new System.Drawing.Size(100, 20);
            this.Player1.TabIndex = 3;
            // 
            // Player2
            // 
            this.Player2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Player2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Player2.Location = new System.Drawing.Point(12, 64);
            this.Player2.Name = "Player2";
            this.Player2.ReadOnly = true;
            this.Player2.Size = new System.Drawing.Size(100, 20);
            this.Player2.TabIndex = 2;
            // 
            // Player3
            // 
            this.Player3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Player3.Location = new System.Drawing.Point(12, 90);
            this.Player3.Name = "Player3";
            this.Player3.ReadOnly = true;
            this.Player3.Size = new System.Drawing.Size(100, 20);
            this.Player3.TabIndex = 1;
            // 
            // Player4
            // 
            this.Player4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Player4.Location = new System.Drawing.Point(12, 116);
            this.Player4.Name = "Player4";
            this.Player4.ReadOnly = true;
            this.Player4.Size = new System.Drawing.Size(100, 20);
            this.Player4.TabIndex = 0;
            // 
            // GamePanel
            // 
            this.GamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GamePanel.Location = new System.Drawing.Point(0, 0);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(1100, 1054);
            this.GamePanel.TabIndex = 3;
            // 
            // CreateGameButton
            // 
            this.CreateGameButton.Location = new System.Drawing.Point(90, 109);
            this.CreateGameButton.Name = "CreateGameButton";
            this.CreateGameButton.Size = new System.Drawing.Size(75, 23);
            this.CreateGameButton.TabIndex = 1;
            this.CreateGameButton.Text = "Create game";
            this.CreateGameButton.UseVisualStyleBackColor = true;
            this.CreateGameButton.Click += new System.EventHandler(this.CreateGameButton_Click);
            // 
            // JoinButton
            // 
            this.JoinButton.Location = new System.Drawing.Point(90, 80);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(75, 23);
            this.JoinButton.TabIndex = 0;
            this.JoinButton.Text = "Join game";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 1054);
            this.Controls.Add(this.MainPanel);
            this.Name = "GameForm";
            this.Text = "Bomberman";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            this.MainPanel.ResumeLayout(false);
            this.JoinGamePanel.ResumeLayout(false);
            this.JoinGamePanel.PerformLayout();
            this.ServerPanel.ResumeLayout(false);
            this.ServerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Button CreateGameButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Panel JoinGamePanel;
        private System.Windows.Forms.Panel ServerPanel;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.TextBox Player1;
        private System.Windows.Forms.TextBox Player2;
        private System.Windows.Forms.TextBox Player3;
        private System.Windows.Forms.TextBox Player4;
        private System.Windows.Forms.Panel GamePanel;
    }
}

