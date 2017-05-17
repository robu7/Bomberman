using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{
    class FireAnimation : Animation
    {
 
        private const double timeBetweenFrames = 0.2;
     
        /*
        public static FireAnimation Left { get; } = new FireAnimation(Properties.Resources.ExplosionLeft);
        public static FireAnimation Right { get; } = new FireAnimation(Properties.Resources.ExplosionRight);
        public static FireAnimation Up { get; } = new FireAnimation(Properties.Resources.ExplosionUp);
        public static FireAnimation Down { get; } = new FireAnimation(Properties.Resources.ExplosionDown);
        public static FireAnimation Horizontal { get; } = new FireAnimation(Properties.Resources.ExplosionHorizontal);
        public static FireAnimation Vertical { get; } = new FireAnimation(Properties.Resources.ExplosionVertical);
        public static FireAnimation Center { get; } = new FireAnimation(Properties.Resources.ExplosionCentre);
        */

        private static List<Bitmap> Left { get; } = buildSpriteSequence(Properties.Resources.ExplosionLeft);
        private static List<Bitmap> Right { get; } = buildSpriteSequence(Properties.Resources.ExplosionRight);
        private static List<Bitmap> Up { get; } = buildSpriteSequence(Properties.Resources.ExplosionUp);
        private static List<Bitmap> Down { get; } = buildSpriteSequence(Properties.Resources.ExplosionDown);
        private static List<Bitmap> Horizontal { get; } = buildSpriteSequence(Properties.Resources.ExplosionHorizontal);
        private static List<Bitmap> Vertical { get; } = buildSpriteSequence(Properties.Resources.ExplosionVertical);
        private static List<Bitmap> Center { get; } = buildSpriteSequence(Properties.Resources.ExplosionCentre);

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

        private FireAnimation(List<Bitmap> spriteSequence) {
            currentFrame = 0;
            interval = timeBetweenFrames;
            this.spriteSequence = spriteSequence;
        }

        public static FireAnimation getFireAnimation(FireType direction) {
            switch (direction) {
                case FireType.Center: return new FireAnimation(FireAnimation.Center);
                case FireType.Up: return new FireAnimation(FireAnimation.Up);
                case FireType.Down: return new FireAnimation(FireAnimation.Down);
                case FireType.Right: return new FireAnimation(FireAnimation.Right);
                case FireType.Left: return new FireAnimation(FireAnimation.Left);
                case FireType.Horizontal: return new FireAnimation(FireAnimation.Horizontal);
                case FireType.Vertical: return new FireAnimation(FireAnimation.Vertical);
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
