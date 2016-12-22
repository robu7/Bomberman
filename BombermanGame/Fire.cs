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

    class Fire : MapObject
    {

        private Animation spriteAnimation;
        public Timer TTL;

        public Fire(PointF position, FireType direction)
            : base(position, Properties.Resources.fire) {
                spriteAnimation = new FireAnimation(Properties.Resources.ExplosionCentre);
                ((FireAnimation)spriteAnimation).startAnimation(direction);
            TTL = new Timer(1400);
            TTL.AutoReset = false;
            TTL.Enabled = true;
        }

        public Fire(Point position, FireType direction)
            : base(position, Properties.Resources.fire) {
                spriteAnimation = new FireAnimation(Properties.Resources.ExplosionCentre);
                ((FireAnimation)spriteAnimation).startAnimation(direction);
            TTL = new Timer(1400);
            TTL.AutoReset = false;
            TTL.Enabled = true;
        }


        public override Bitmap getSprite() { return spriteAnimation.getFrame(); }

    }
}
