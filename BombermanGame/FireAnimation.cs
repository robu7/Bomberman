using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame {
    class FireAnimation : Animation {

        private const double timeBetweenFrames = 0.2;

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
        public static FireAnimation getFireAnimation(FireType direction) {
            // Get pre-generated sprites
            IReadOnlyList<Bitmap> spriteSequence;
            if (!sprites.TryGetValue(direction, out spriteSequence)) {
                throw new Exception("Unknown Fire Type: " + direction);
            }
            return new FireAnimation(spriteSequence);
        }

        private FireAnimation(IReadOnlyList<Bitmap> spriteSequence) {
            currentFrame = 0;
            interval = timeBetweenFrames;
            this.spriteSequence = spriteSequence;
        }

        public override void start(Game.Direction direction, double animationStartTime) { this.animationStartTime = animationStartTime; }

        public override void stop() {
            currentFrame = 0;
        }

        public override void update(double tick, double totalTime) {
            double elapsed = totalTime - this.animationStartTime;
            this.currentFrame = Math.Min((int)Math.Floor(elapsed / interval), 6);
        }
    }
}
