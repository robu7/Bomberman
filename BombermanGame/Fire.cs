using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{
    public enum FireType { Center, Horizontal, Vertical, Up, Down, Left, Right };

    class Fire : MapObject, ITimedMapObject
    {

        private Animation spriteAnimation;
        private double timeToLive;
        private bool finished = false;
        public bool Finished { get { return finished; } }
        public Point MapPos { get { return mapPosition; } }

        public Fire(PointF position, FireType direction, double startTime)
            : base(position, Properties.Resources.fire) {

            spriteAnimation = getFireAnimation(direction);
            spriteAnimation.start(0, startTime);
            timeToLive = 1;
        }

        public Fire(Point position, FireType direction, double startTime)
         : base(position, Properties.Resources.fire) {
            spriteAnimation = getFireAnimation(direction);
            spriteAnimation.start(0, startTime);
            timeToLive = 1;
        }

        private static FireAnimation getFireAnimation(FireType direction) {

            switch (direction) {
                case FireType.Center: return FireAnimation.Center;
                case FireType.Up: return FireAnimation.Up;
                case FireType.Down: return FireAnimation.Down;
                case FireType.Right: return FireAnimation.Right;
                case FireType.Left: return FireAnimation.Left;
                case FireType.Horizontal: return FireAnimation.Horizontal;
                case FireType.Vertical: return FireAnimation.Vertical;
            }
            throw new Exception("Unknown Fire Type: " +direction);
        }

     


        //public override Bitmap getSprite() { return spriteAnimation.getFrame(); }
        public override Bitmap getSprite() { return spriteAnimation.getFrame(); }

        public void update(double tick, double totalTime) {
            timeToLive -= tick;
            spriteAnimation.update(tick, totalTime);
            if (timeToLive <= 0) {
                finished = true;
                spriteAnimation.stop();
            }
        }

        public override string ToString() {
            return "Fire";
        }
    }
}
