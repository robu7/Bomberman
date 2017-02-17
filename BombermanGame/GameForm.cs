using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace BombermanGame
{
    public partial class GameForm : Form
    {
        Game game;
        InputHandler inputHandler;
        Communicator communicator;
        HostGameMenuControl hostGameMenu;


        public GameForm() {
            InitializeComponent();
            Console.WriteLine("Constructor Gameform");
            inputHandler = new InputHandler();
            communicator = new Communicator(this);
        }

        public static void ShowErrorDialog(string message) {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void StartGame(Graphics gameGraphics) {
            game = new Game(gameGraphics, inputHandler, communicator);
            KeyPreview = true;
        }

        private void JoinButton_Click(object sender, EventArgs e) {

            //communicator.connectToHost();
            communicator.Connect();
            hostGameMenu = new HostGameMenuControl();
            MainPanel.SuspendLayout();
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(hostGameMenu);
            MainPanel.ResumeLayout(false);
            //updatePlayerList(1, "zirrobin");
            communicator.StartListening();
            hostGameMenu.AddLobbyPlayer(2, "Client");

            /*
            JoinGameMenuControl joinGameMenu = new JoinGameMenuControl();
            MainPanel.SuspendLayout();
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(joinGameMenu);
            MainPanel.ResumeLayout(false);*/

        }

        private void CreateGameButton_Click(object sender, EventArgs e) {

            hostGameMenu = new HostGameMenuControl();
            MainPanel.SuspendLayout();
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(hostGameMenu);
            MainPanel.ResumeLayout(false);
            //updatePlayerList(1, "zirrobin");
            communicator.StartListening();
            hostGameMenu.AddLobbyPlayer(1, "Host");
            hostGameMenu.EnterHostSettings();
        }
        
        public void SendChatMessage(string message) {
            communicator.SendChatMessage(message);
        }

        public void PostChatMessage(string message) {
            hostGameMenu.PostChatMessage(message);
        }

        public void AddPlayerToLobby() {

        }



        /* TODO: implement InputHandler instead
         * Controller input is passed on to the game class
         */
        private void GameForm_KeyDown(object sender, KeyEventArgs e) {
            //inputHandler.buttonPressed(e.KeyCode);
            game.handleKeyDownInput(e);
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e) {
            //inputHandler.buttonReleased(e.KeyCode);
            game.handleKeyUpInput(e);
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e) {
            if(game != null)
                game.stopGame();
        }

    }
}
