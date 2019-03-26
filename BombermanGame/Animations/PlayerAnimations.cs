using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame
{
    enum SpriteSet { Blue, Red, Green }
}

namespace BombermanGame.Animations {

    class PlayerAnimations : IGraphicsResourceLoader {
        static private List<SharpDX.Direct2D1.Bitmap> up;
        static private List<SharpDX.Direct2D1.Bitmap> down;
        static private List<SharpDX.Direct2D1.Bitmap> left;
        static private List<SharpDX.Direct2D1.Bitmap> right;
        static private List<SharpDX.Direct2D1.Bitmap> death;

        public void LoadGraphics(SharpDX.Direct2D1.RenderTarget target) {
            up = new List<SharpDX.Direct2D1.Bitmap>();
            down = new List<SharpDX.Direct2D1.Bitmap>();
            left = new List<SharpDX.Direct2D1.Bitmap>();
            right = new List<SharpDX.Direct2D1.Bitmap>();
            death = new List<SharpDX.Direct2D1.Bitmap>();

            Bitmap original = Properties.Resources.blue;
            Bitmap sprite;
            Rectangle srcRect;

            // Init down
            int i = 0;
            for (; i < 3; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = original.Clone(srcRect, original.PixelFormat);
                down.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }

            // Init right
            for (; i < 6; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = original.Clone(srcRect, original.PixelFormat);
                right.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }

            // Init left
            for (; i < 9; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = original.Clone(srcRect, original.PixelFormat);
                left.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }

            // Init up
            for (; i < 12; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = original.Clone(srcRect, original.PixelFormat);
                up.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }

            // Init death
            for (; i < 20; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = original.Clone(srcRect, original.PixelFormat);
                death.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }
        }

        /// <summary>
        /// Returns the appropriate animation for the specified direction of a moving player
        /// </summary>
        public static Animation GetWalkAnimation(Game.Direction walkDirection, double animationDuration) {
            // Get pre-generated sprites
            IReadOnlyList<SharpDX.Direct2D1.Bitmap> spriteSequence = null;
            switch (walkDirection) {
                case Game.Direction.North:
                    spriteSequence = up;
                    break;
                case Game.Direction.South:
                    spriteSequence = down;
                    break;
                case Game.Direction.West:
                    spriteSequence = left;
                    break;
                case Game.Direction.East:
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
