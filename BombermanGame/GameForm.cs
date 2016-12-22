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

        public GameForm() {
            InitializeComponent();
            Console.WriteLine("Constructor Gameform");
            inputHandler = new InputHandler();
            communicator = new Communicator(this);
            //communicator = new Communicator();
        }

        private void GameForm_Load(object sender, EventArgs e) {

        }


        private void JoinButton_Click(object sender, EventArgs e) {
            foreach (Control control in MainPanel.Controls) {
                control.Hide();
            }
            communicator.connectToHost();
            JoinGamePanel.Show();
        }

        private void CreateGameButton_Click(object sender, EventArgs e) {

            foreach (Control control in MainPanel.Controls) {
                control.Hide();
            }
            ServerPanel.Show();
            //updatePlayerList(1, "zirrobin");
            //communicator.StartListening();
        }
         
        private void textBox2_TextChanged(object sender, EventArgs e) {

        }

        private void StartGame_Click(object sender, EventArgs e) {

            foreach (Control control in ServerPanel.Controls) {
                control.Hide();
            }
            //communicator.StopListening();
            GamePanel.Show();
            game = new Game(GamePanel.CreateGraphics(), inputHandler, communicator);
            KeyPreview = true;
            
        }


        private void GameForm_KeyDown(object sender, KeyEventArgs e) {
            //inputHandler.buttonPressed(e.KeyCode);
            game.handleKeyDownInput(e);
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e) {
            //inputHandler.buttonReleased(e.KeyCode);
            game.handleKeyUpInput(e);
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e) {
            game.stopGame();
        }

        public void updatePlayerList(int i, string username) {
            switch (i) {
                case 1:
                    Player1.AppendText(username);
                    break;
                case 2:
                    Player2.AppendText(username);
                    break;
                case 3:
                    Player3.AppendText(username);
                    break;
                case 4:
                    Player4.AppendText(username);
                    break;

            }
        }

      

    
    }
}
