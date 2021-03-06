﻿using System;
using System.Windows.Input;
using System.Windows.Forms;

using Direction = BombermanGame.Game.Direction;

namespace BombermanGame
{
    struct Input
    {
        public Input(Direction movement, bool deployBomb)
        {
            MovementInput = movement;
            DeployBomb = deployBomb;
        }
        public Direction MovementInput { get; }
        public bool DeployBomb { get; }
    }


    class InputHandler
    {
        private Keys currentKey;

        private Direction pressedDirection;
        public Direction PressedDirection { get { return pressedDirection; } set { pressedDirection = value; } }

        private bool deployBomb;
        public bool DeployBomb { get { return deployBomb; } set { deployBomb = value; } }

        private bool updatedInput;
        public bool UpdatedInput { get { return updatedInput; } set { updatedInput = value; } }

        public InputHandler() {
            //currentInput = new Input{ movementInput = Game.Direction.None, deployBomb = false };
            //Keyboard.AddKeyDownHandler(gameGraphics, OnKeyDownHandler);
            //Keyboard.KeyDownEvent += buttonPressed;
        }

        /// <summary>
        /// This function will check for new inpout and return true is this is the case,
        /// otherwise return false.
        /// </summary>
        public Input CheckInput() {
            deployBomb = false;

            // Check movement input
            if (Keyboard.IsKeyDown(Key.W)) {
                pressedDirection = Direction.North;
            }
            else if (Keyboard.IsKeyDown(Key.S)) {
                pressedDirection = Direction.South;
            }
            else if (Keyboard.IsKeyDown(Key.A)) {
                pressedDirection = Direction.West;
            }
            else if (Keyboard.IsKeyDown(Key.D)) {
                pressedDirection = Direction.East;
            }
            else {
                pressedDirection = Direction.None;
            }

            // Check action input
            if (Keyboard.IsKeyDown(Key.B)) {
                deployBomb = true;
            }

            return new Input(pressedDirection, deployBomb);
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
                        pressedDirection = Game.Direction.North;
                        updatedInput = true;
                        break;
                    case Keys.S:
                        pressedDirection = Game.Direction.South;
                        updatedInput = true;
                        break;
                    case Keys.A:
                        pressedDirection = Game.Direction.West;
                        updatedInput = true;
                        break;
                    case Keys.D:
                        pressedDirection = Game.Direction.East;
                        updatedInput = true;
                        break;
                    case Keys.B:
                        deployBomb = true;
                        break;
                    default:
                        pressedDirection = Game.Direction.None;
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
