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
        public double TimeToLive { get; private set; }
        public bool Finished { get; private set; }
        public Point MapPos => this.mapPosition;

        public Fire(PointF position, FireType direction, double startTime)
            : base(position) {
            TimeToLive = 1;
            spriteAnimation = FireAnimations.getFireAnimation(direction, TimeToLive);
            spriteAnimation.start(startTime);
        }

        public Fire(Point position, FireType direction, double startTime)
         : base(position) {
            TimeToLive = 1;
            spriteAnimation = FireAnimations.getFireAnimation(direction, TimeToLive);
            spriteAnimation.start(startTime);
            
        }


        //public override Bitmap getSprite() { return spriteAnimation.getFrame(); }
        public override Bitmap getSprite() { return spriteAnimation.CurrentFrame; }

        public void update(double tick, double totalTime) {
            TimeToLive -= tick;
            spriteAnimation.update(tick, totalTime);
            if (TimeToLive <= 0) {
                Finished = true;
                spriteAnimation.stop();
            }
        }

        public override string ToString() {
            return "Fire";
        }
    }
}
