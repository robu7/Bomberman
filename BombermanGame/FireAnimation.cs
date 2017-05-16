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
     
        public static FireAnimation Left { get; } = new FireAnimation(Properties.Resources.ExplosionLeft);
        public static FireAnimation Right { get; } = new FireAnimation(Properties.Resources.ExplosionRight);
        public static FireAnimation Up { get; } = new FireAnimation(Properties.Resources.ExplosionUp);
        public static FireAnimation Down { get; } = new FireAnimation(Properties.Resources.ExplosionDown);
        public static FireAnimation Horizontal { get; } = new FireAnimation(Properties.Resources.ExplosionHorizontal);
        public static FireAnimation Vertical { get; } = new FireAnimation(Properties.Resources.ExplosionVertical);
        public static FireAnimation Center { get; } = new FireAnimation(Properties.Resources.ExplosionCentre);

        private FireAnimation(Bitmap original){
            currentFrame = 0;
            interval = timeBetweenFrames;
            Bitmap sprite;
            Rectangle srcRect;

            for (int i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                spriteSequence.Add(new Bitmap(sprite, Game.boxSize));
                sprite.Dispose();
            }
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
