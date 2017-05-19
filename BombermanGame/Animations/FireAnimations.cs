using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame {

    static class FireAnimations {

        // Used to only extract sprites from the original resources once
        private static readonly IReadOnlyDictionary<FireType, IReadOnlyList<Bitmap>> sprites = new Dictionary<FireType, IReadOnlyList<Bitmap>> {
            { FireType.Left, buildSpriteSequence(Properties.Resources.ExplosionLeft) },
            { FireType.Right, buildSpriteSequence(Properties.Resources.ExplosionRight) },
            { FireType.Up, buildSpriteSequence(Properties.Resources.ExplosionUp) },
            { FireType.Down, buildSpriteSequence(Properties.Resources.ExplosionDown) },
            { FireType.Horizontal, buildSpriteSequence(Properties.Resources.ExplosionHorizontal) },
            { FireType.Vertical, buildSpriteSequence(Properties.Resources.ExplosionVertical) },
            { FireType.Center, buildSpriteSequence(Properties.Resources.ExplosionCentre) },
        };

        /// <summary>
        /// Crops out sprites from an image containing the whole animation sequence
        /// </summary>
        private static List<Bitmap> buildSpriteSequence(Bitmap original) {
            Bitmap sprite;
            Rectangle srcRect;
            List<Bitmap> sequence = new List<Bitmap>();
            for (int i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                sequence.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }
            return sequence;
        }

        /// <summary>
        /// Returns the appropriate animation for the specified part of an explosion fire
        /// </summary>
        public static Animation getFireAnimation(FireType direction, double animationDuration) {
            // Get pre-generated sprites
            IReadOnlyList<Bitmap> spriteSequence;
            if (!sprites.TryGetValue(direction, out spriteSequence)) {
                throw new Exception("Unknown Fire Type: " + direction);
            }
            return new Animation(spriteSequence, animationDuration, false);
        }
    }
}
