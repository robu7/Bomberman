using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{
    class Bomb : MapObject, IDestroyable 
    {

        private Player owner;
        private Bitmap sprite;
        public Timer TTL;
        private int range;

        public Bomb(PointF position, Player _owner)
            : base(position, Properties.Resources.bomb) {
                owner = _owner;
                sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);
                range = _owner.getBombRange();
                TTL = new Timer(2500);
                TTL.AutoReset = false;
                TTL.Enabled = true;
        }

        public Bomb(Point position, Player _owner)
            : base(position, Properties.Resources.bomb) {
            owner = _owner;
            range = _owner.getBombRange();
            sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);
            TTL = new Timer(2500);
            TTL.AutoReset = false;
            TTL.Enabled = true;
        }

        public Player getOwner() { return owner; }
        public int getRange() { return range; }

        public override Bitmap getSprite() { return sprite; }

        public void destroy() {
            TTL.Interval = 0.000001;
        }

    }
}
