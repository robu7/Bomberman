using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame
{
    class FireAnimation : Animation
    {
 
        private const double timeBetweenFrames = 0.2;

        private static readonly IReadOnlyList<Bitmap> leftEdgeSprites = buildSpriteSequence(Properties.Resources.ExplosionLeft);
        private static readonly IReadOnlyList<Bitmap> rightEdgeSprites = buildSpriteSequence(Properties.Resources.ExplosionRight);
        private static readonly IReadOnlyList<Bitmap> topEdgeSprites = buildSpriteSequence(Properties.Resources.ExplosionUp);
        private static readonly IReadOnlyList<Bitmap> bottomEdgeSprites = buildSpriteSequence(Properties.Resources.ExplosionDown);
        private static readonly IReadOnlyList<Bitmap> horizontalSectionSprites = buildSpriteSequence(Properties.Resources.ExplosionHorizontal);
        private static readonly IReadOnlyList<Bitmap> verticalSectionSprites = buildSpriteSequence(Properties.Resources.ExplosionVertical);
        private static readonly IReadOnlyList<Bitmap> centerSprites = buildSpriteSequence(Properties.Resources.ExplosionCentre);

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

        private FireAnimation(IReadOnlyList<Bitmap> spriteSequence) {
            currentFrame = 0;
            interval = timeBetweenFrames;
            this.spriteSequence = spriteSequence;
        }

        public static FireAnimation getFireAnimation(FireType direction) {
            switch (direction) {
                case FireType.Center: return new FireAnimation(centerSprites);
                case FireType.Up: return new FireAnimation(topEdgeSprites);
                case FireType.Down: return new FireAnimation(bottomEdgeSprites);
                case FireType.Right: return new FireAnimation(rightEdgeSprites);
                case FireType.Left: return new FireAnimation(leftEdgeSprites);
                case FireType.Horizontal: return new FireAnimation(horizontalSectionSprites);
                case FireType.Vertical: return new FireAnimation(verticalSectionSprites);
            }
            throw new Exception("Unknown Fire Type: " + direction);
        }

        public override void start(Game.Direction direction, double animationStartTime) { this.animationStartTime = animationStartTime; }

        public override void stop() {
            currentFrame = 0;
        }

        public override void update(double tick, double totalTime) {
            double elapsed = totalTime - this.animationStartTime;
            this.currentFrame = Math.Min((int)Math.Floor(elapsed / interval),6);
        }
    }
}
