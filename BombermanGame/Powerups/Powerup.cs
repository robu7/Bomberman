using System;
using System.Drawing;

namespace BombermanGame.Powerups
{
    enum PowerUpType { ExtraBomb, RangeBoost, KickAbility }

    class Powerup : MapObject, IDestroyable, ITimedMapObject
    {
        static private Bitmap sprite;
        public PowerUpType Type { get; set; }
        public double TimeToLive { get; private set; }
        public bool Finished { get; private set; }
        public Point MapPos => this.mapPosition;

        public Powerup(Point mapPosition, PowerUpType type) :base(mapPosition){
            Type = type;
            Finished = false;
            TimeToLive = 5;

            switch (type) {
                case PowerUpType.RangeBoost:
                    sprite = new Bitmap(Properties.Resources.fire, Game.boxSize);
                    break;
                case PowerUpType.ExtraBomb:
                    sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);
                    break;
                case PowerUpType.KickAbility:
                    sprite = new Bitmap(Properties.Resources.PlayerSprite, Game.boxSize);
                    break;
            }
        }

        public override Bitmap getSprite() { return sprite; }

        public void destroy() {
            Finished = true;
        }

        public void update(double tick, double totalTime) {
            TimeToLive -= tick;
            if (TimeToLive <= 0) {
                Finished = true;
            }
        }
    }
}
