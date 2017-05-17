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
            : base(position) {

            spriteAnimation = FireAnimation.getFireAnimation(direction);
            spriteAnimation.start(0, startTime);
            timeToLive = 1;
        }

        public Fire(Point position, FireType direction, double startTime)
         : base(position) {
            spriteAnimation = FireAnimation.getFireAnimation(direction);
            spriteAnimation.start(0, startTime);
            timeToLive = 1;
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
