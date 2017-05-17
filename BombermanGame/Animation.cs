using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{

    abstract class Animation : IUpdateable
    {
        protected IReadOnlyList<Bitmap> spriteSequence = new List<Bitmap>();
        protected double interval;
        protected int currentFrame;
        protected double totalTicks;
        protected double animationStartTime;

        public abstract void start(Game.Direction direction, double animationStartTime);
        public abstract void stop();


        public Bitmap getFrame() { return spriteSequence[currentFrame]; }

        public abstract void update(double tick, double totalTime);
    }


    class PlayerAnimation : Animation
    {

        //private List<Bitmap> spriteSequence;

        static private List<Bitmap> up;
        static private List<Bitmap> down;
        static private List<Bitmap> left;
        static private List<Bitmap> right;
        static private List<Bitmap> death;
        private const double timeBetweenFrames = 0.2;
        private bool active;
        //private bool deathInitilized;
        //private Timer interval;
        //private int currentFrame;

        public PlayerAnimation(Bitmap original) {
            up = new List<Bitmap>();
            down = new List<Bitmap>();
            left = new List<Bitmap>();
            right = new List<Bitmap>();
            death = new List<Bitmap>();
            currentFrame = 0;
            active = false;
            //deathInitilized = false;
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

            spriteSequence = down;
        }

        public void startDeathAnimation() {
            spriteSequence = death;
            //interval.Close();
            interval = 0.15;
            currentFrame = 0;
            active = true;
        }

        public override void start(Game.Direction direction, double startTime) {

            animationStartTime = startTime;

            switch (direction) {
                case Game.Direction.Down:
                    spriteSequence = down;
                    break;
                case Game.Direction.Up:
                    spriteSequence = up;
                    break;
                case Game.Direction.Right:
                    spriteSequence = right;
                    break;
                case Game.Direction.Left:
                    spriteSequence = left;
                    break;
            }
            active = true;
        }
        public override void stop() {
            currentFrame = 0;
            active = false;
        }

        public override void update(double tick, double totalTime) {
            if (active) {
                interval -= tick;
                if(interval <= 0) {
                    interval = timeBetweenFrames;
                    nextFrame();
                }
            }
        }

        private void nextFrame() {
            if (currentFrame == spriteSequence.Count-1)
                currentFrame = 0;
            else
                ++currentFrame; 
        }

        private void deathAnimation() {
            if (currentFrame == 7)
                stop();
            else
                ++currentFrame;
        }
    }
}
