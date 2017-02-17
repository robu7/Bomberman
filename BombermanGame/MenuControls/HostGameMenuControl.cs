﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace BombermanGame
{
    public partial class HostGameMenuControl : UserControl
    {
        public HostGameMenuControl()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in ServerPanel.Controls)
                ctrl.Visible = false;

            //communicator.StopListening();
            GamePanel.Show();
            ((GameForm)this.ParentForm).StartGame(GamePanel.CreateGraphics());

        }

        private void ChatText_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && ChatText.Text != "") {
                ChatWindow.Items.Add("Me: " + ChatText.Text);
                // Post message to other lobby members
                ((GameForm)this.ParentForm).SendChatMessage(ChatText.Text);
                ChatText.Clear();
            }

        }

        public void PostChatMessage(string message) {
            Invoke((Action)delegate {
                ChatWindow.Items.Add("Other: " + message);
            });
        }

        public void EnterNewPlayer() {

        }

        private void Lobby_Enter(object sender, EventArgs e) {
            
        }

        public void AddLobbyPlayer(int playerNum, string name) {
            Invoke((Action)delegate {
                PlayerList.Items.Add(playerNum.ToString() + ":  " + name);
            });
        }

        public void EnterHostSettings() {
            //var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToStr‌​ing();
            //Console.WriteLine("My IP: " + ip);
        }
    }
}
