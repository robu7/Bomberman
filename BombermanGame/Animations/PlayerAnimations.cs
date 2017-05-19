using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame.Animations {
    static class PlayerAnimations {
        static private List<Bitmap> up;
        static private List<Bitmap> down;
        static private List<Bitmap> left;
        static private List<Bitmap> right;
        static private List<Bitmap> death;

        static PlayerAnimations() {
            up = new List<Bitmap>();
            down = new List<Bitmap>();
            left = new List<Bitmap>();
            right = new List<Bitmap>();
            death = new List<Bitmap>();

            Bitmap original = Properties.Resources.blue;
            Bitmap sprite;
            Rectangle srcRect;

            // Init down
            int i = 0;
            for (; i < 3; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                down.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }

            // Init right
            for (; i < 6; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                right.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }

            // Init left
            for (; i < 9; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                left.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }

            // Init up
            for (; i < 12; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                up.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }

            // Init death
            for (; i < 20; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                death.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }
        }

        /// <summary>
        /// Returns the appropriate animation for the specified direction of a moving player
        /// </summary>
        public static Animation GetWalkAnimation(Game.Direction walkDirection, double animationDuration) {
            // Get pre-generated sprites
            IReadOnlyList<Bitmap> spriteSequence = null;
            switch (walkDirection) {
                case Game.Direction.Up:
                    spriteSequence = up;
                    break;
                case Game.Direction.Down:
                    spriteSequence = down;
                    break;
                case Game.Direction.Left:
                    spriteSequence = left;
                    break;
                case Game.Direction.Right:
                    spriteSequence = right;
                    break;
                default:
                    throw new Exception("Unknown walking direction: " + walkDirection);
            }
            return new Animation(spriteSequence, animationDuration, true);
        }

        /// <summary>
        /// Returns the appropriate animation for the specified direction of a moving player
        /// </summary>
        public static Animation GetDeathAnimation(double animationDuration) {
            return new Animation(death, animationDuration, false);
        }
    }
}
