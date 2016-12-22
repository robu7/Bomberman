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

        private List<Bitmap> up;
        private List<Bitmap> down;
        private List<Bitmap> left;
        private List<Bitmap> right;
        private List<Bitmap> center;
        private List<Bitmap> vertical;
        private List<Bitmap> horizontel;


        public FireAnimation(Bitmap original) {
            center = new List<Bitmap>();
            up = new List<Bitmap>();
            down = new List<Bitmap>();
            left = new List<Bitmap>();
            right = new List<Bitmap>();
            vertical = new List<Bitmap>();
            horizontel = new List<Bitmap>();

            currentFrame = 0;
            Bitmap sprite;
            Rectangle srcRect;

            int i = 0;
            for (; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                center.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionUp;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                up.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionDown;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                down.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionLeft;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                left.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionRight;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                right.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionVertical;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                vertical.Add(new Bitmap(sprite, Game.boxSize));
            }

            original = Properties.Resources.ExplosionHorizontal;
            for (i = 0; i < 7; ++i) {
                srcRect = new Rectangle(i * 32, 0, 32, 32);
                sprite = (Bitmap)original.Clone(srcRect, original.PixelFormat);
                horizontel.Add(new Bitmap(sprite, Game.boxSize));
            }


            spriteSequence = up;
            interval = new Timer(200);
            interval.AutoReset = true;
            interval.Elapsed += delegate { nextFrame(); };
        }

        // TODO, change this
        public override void start(Game.Direction direction) { }

        public void startAnimation(FireType direction) {

            switch (direction) {
                case FireType.Center:
                    spriteSequence = center;
                    break;
                case FireType.Horizontal:
                    spriteSequence = horizontel;
                    break;
                case FireType.Vertical:
                    spriteSequence = vertical;
                    break;
                case FireType.Up:
                    spriteSequence = up;
                    break;
                case FireType.Down:
                    spriteSequence = down;
                    break;
                case FireType.Left:
                    spriteSequence = left;
                    break;
                case FireType.Right:
                    spriteSequence = right;
                    break;
            }
            interval.Start();
        }
        public override void stop() {
            currentFrame = 0;
            interval.Stop();
        }

        private void nextFrame() {
            if (currentFrame == 6)
                currentFrame = 0;
            else
                ++currentFrame;
        }

    }
}
