﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
//using System.Windows.Input;
using System.Windows.Forms;

namespace BombermanGame
{
    class InputHandler
    {
        private Keys currentKey;

        private Game.Direction pressedDirection;
        public Game.Direction PressedDirection { get { return pressedDirection; } set { pressedDirection = value; } }

        private bool deployBomb;
        public bool DeployBomb { get{ return deployBomb; } set { deployBomb = value; } }

        private bool updatedInput;
        public bool UpdatedInput { get { return updatedInput; } set { updatedInput = value; } }

        public InputHandler() {
            //Keyboard.AddKeyDownHandler(gameGraphics, OnKeyDownHandler);
            //Keyboard.KeyDownEvent += buttonPressed;
        }


        /*
         * This function will check for new inpout and return true is this is the case,
         * otherwise return false;
         */
        public bool handleInput() {
            return false;
        }


        static private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Return) {
                Console.WriteLine("fdsfsdfdsfdsfsd");
            }
        }

        public void buttonPressed(Keys pressedButton) {

            if (pressedButton == Keys.B) {
                deployBomb = true;
                return;
            }

            if (currentKey != pressedButton ) {
                currentKey = pressedButton;
                switch (pressedButton) {
                    case Keys.W:
                        pressedDirection = Game.Direction.Up;
                        updatedInput = true;
                        break;
                    case Keys.S:
                        pressedDirection = Game.Direction.Down;
                        updatedInput = true;
                        break;
                    case Keys.A:
                        pressedDirection = Game.Direction.Left;
                        updatedInput = true;
                        break;
                    case Keys.D:
                        pressedDirection = Game.Direction.Right;
                        updatedInput = true;
                        break;
                    case Keys.B:
                        deployBomb = true;
                        break;
                }
            }
        }

        public void buttonReleased(Keys releasedButton) {
            if(releasedButton == Keys.B) {
                deployBomb = false;
                return;
            }

            if (currentKey == releasedButton) {
                switch (releasedButton) {
                    case Keys.W:
                    case Keys.S:
                    case Keys.A:
                    case Keys.D:
                        pressedDirection = Game.Direction.None;
                        updatedInput = true;
                        currentKey = Keys.End;
                        break;
                    case Keys.B:
                        deployBomb = false;
                        currentKey = Keys.End;
                        break;
                }
            }
        }
    }
}
