using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame {

    static class FireAnimations {

        // Used to only extract sprites from the original resources once
        private static IReadOnlyDictionary<FireType, IReadOnlyList<SharpDX.Direct2D1.Bitmap>> sprites;

        public static void LoadGraphics(SharpDX.Direct2D1.RenderTarget target) {
            sprites = new Dictionary<FireType, IReadOnlyList<SharpDX.Direct2D1.Bitmap>> {
                { FireType.Left, BuildSpriteSequence(Properties.Resources.ExplosionLeft, target) },
                { FireType.Right, BuildSpriteSequence(Properties.Resources.ExplosionRight, target) },
                { FireType.Up, BuildSpriteSequence(Properties.Resources.ExplosionUp, target) },
                { FireType.Down, BuildSpriteSequence(Properties.Resources.ExplosionDown, target) },
                { FireType.Horizontal, BuildSpriteSequence(Properties.Resources.ExplosionHorizontal, target) },
                { FireType.Vertical, BuildSpriteSequence(Properties.Resources.ExplosionVertical, target) },
                { FireType.Center, BuildSpriteSequence(Properties.Resources.ExplosionCentre, target) },
            };
        }

        /// <summary>
        /// Crops out sprites from an image containing the whole animation sequence
        /// </summary>
        private static List<SharpDX.Direct2D1.Bitmap> BuildSpriteSequence(Bitmap original, SharpDX.Direct2D1.RenderTarget target) {
            Rectangle srcRect;
            var sequence = new List<SharpDX.Direct2D1.Bitmap>();
            for (int i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sequence.Add(original.Clone(srcRect, original.PixelFormat).CreateDirectX2D1Bitmap(target));
            }
            return sequence;
        }

        /// <summary>
        /// Returns the appropriate animation for the specified part of an explosion fire
        /// </summary>
        public static Animation GetFireAnimation(FireType direction, double animationDuration) {
            // Get pre-generated sprites
            IReadOnlyList<SharpDX.Direct2D1.Bitmap> spriteSequence;
            if (!sprites.TryGetValue(direction, out spriteSequence)) {
                throw new Exception("Unknown Fire Type: " + direction);
            }
            return new Animation(spriteSequence, animationDuration, false);
        }
    }
}
