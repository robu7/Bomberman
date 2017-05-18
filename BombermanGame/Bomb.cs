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
        static private Bitmap sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);
        private double timeToLive;
        private int range;
        private bool hasExploded;
        public bool HasExploded { get { return hasExploded; } }

        public Bomb(PointF position, Player _owner)
            : base(position) {
            owner = _owner;
            initBomb();
        }

        public Bomb(Point mapPosition, Player _owner)
             : base(mapPosition) {
            owner = _owner;
            initBomb();
        }

        private void initBomb() {
            range = owner.getBombRange();
            timeToLive = 2.5;// set time to 2.5 seconds
            hasExploded = false;
        }

        public Player getOwner() { return owner; }
        public int getRange() { return range; }

        public override Bitmap getSprite() { return sprite; }

        public bool update(double tick) {
            if (!hasExploded) {
                timeToLive -= tick;
                if (timeToLive <= 0) {
                    hasExploded = true;
                    return true;
                }
            }
            return false;       
        }

        public void destroy() {
            timeToLive = 0;
        }

        public override string ToString() {
            return "Bomb";
        }
    }
}
