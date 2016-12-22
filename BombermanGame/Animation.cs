using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{

    abstract class Animation
    {
        protected List<Bitmap> spriteSequence;
        protected Timer interval;
        protected int currentFrame;

        public Animation() { }

        public abstract void start(Game.Direction direction);
        public abstract void stop();

        public Bitmap getFrame() { return spriteSequence[currentFrame]; }
    }


    class PlayerAnimation : Animation
    {

        //private List<Bitmap> spriteSequence;

        private List<Bitmap> up;
        private List<Bitmap> down;
        private List<Bitmap> left;
        private List<Bitmap> right;
        private List<Bitmap> death;
        //private Timer interval;
        //private int currentFrame;

        public PlayerAnimation(Bitmap original) {
            up = new List<Bitmap>();
            down = new List<Bitmap>();
            left = new List<Bitmap>();
            right = new List<Bitmap>();
            death = new List<Bitmap>();
            currentFrame = 0;
            Bitmap sprite;
            Rectangle srcRect;

            // Init down
            int i = 0;
            for (; i < 3; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                down.Add(new Bitmap(sprite, Game.boxSize));
            }

            // Init right
            for (; i < 6; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                right.Add(new Bitmap(sprite, Game.boxSize));
            }

            // Init left
            for (; i < 9; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                left.Add(new Bitmap(sprite, Game.boxSize));
            }

            // Init up
            for (; i < 12; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                up.Add(new Bitmap(sprite, Game.boxSize));
            }

            // Init death
            for (; i < 20; ++i) {
                srcRect = new Rectangle(i * 48, 0, 48, 48);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                death.Add(new Bitmap(sprite, Game.boxSize));
            }

            spriteSequence = down;
            interval = new Timer(150);
            interval.AutoReset = true;
            interval.Elapsed += delegate { nextFrame(); };

        }

        public void startDeathAnimation() {
            spriteSequence = death;
            //interval.Close();
            interval = new Timer(150);
            interval.AutoReset = true;
            interval.Elapsed += delegate { deathAnimation(); };
            currentFrame = 0;
            interval.Start();
        }

        public override void start(Game.Direction direction) {

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

            interval.Start();
        }
        public override void stop() {
            currentFrame = 0;
            interval.Stop();
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
