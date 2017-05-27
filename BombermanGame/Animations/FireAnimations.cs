using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame {

    static class FireAnimations {

        // Used to only extract sprites from the original resources once
        private static readonly IReadOnlyDictionary<FireType, IReadOnlyList<Bitmap>> sprites = new Dictionary<FireType, IReadOnlyList<Bitmap>> {
            { FireType.Left, BuildSpriteSequence(Properties.Resources.ExplosionLeft) },
            { FireType.Right, BuildSpriteSequence(Properties.Resources.ExplosionRight) },
            { FireType.Up, BuildSpriteSequence(Properties.Resources.ExplosionUp) },
            { FireType.Down, BuildSpriteSequence(Properties.Resources.ExplosionDown) },
            { FireType.Horizontal, BuildSpriteSequence(Properties.Resources.ExplosionHorizontal) },
            { FireType.Vertical, BuildSpriteSequence(Properties.Resources.ExplosionVertical) },
            { FireType.Center, BuildSpriteSequence(Properties.Resources.ExplosionCentre) },
        };

        /// <summary>
        /// Crops out sprites from an image containing the whole animation sequence
        /// </summary>
        private static List<Bitmap> BuildSpriteSequence(Bitmap original) {
            Rectangle srcRect;
            List<Bitmap> sequence = new List<Bitmap>();
            for (int i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sequence.Add((Bitmap)original.Clone(srcRect, original.PixelFormat));
            }
            return sequence;
        }

        /// <summary>
        /// Returns the appropriate animation for the specified part of an explosion fire
        /// </summary>
        public static Animation GetFireAnimation(FireType direction, double animationDuration) {
            // Get pre-generated sprites
            IReadOnlyList<Bitmap> spriteSequence;
            if (!sprites.TryGetValue(direction, out spriteSequence)) {
                throw new Exception("Unknown Fire Type: " + direction);
            }
            return new Animation(spriteSequence, animationDuration, false);
        }
    }
}
