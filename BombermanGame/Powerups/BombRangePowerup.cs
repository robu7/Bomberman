using System;
using SharpDX.Direct2D1;

namespace BombermanGame.Powerups {
    class BombRangePowerup : Powerup {

        private BitmapLoader loader = new BombRangePowerupGraphicsLoader();

        protected override Bitmap Sprite => loader.Bitmap;

        protected override void ApplyToPlayer(Player player) {
            player.BombRange += 1;
        }

        public BombRangePowerup(double creationTime) : base(creationTime) {}
    }

    class BombRangePowerupGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public BombRangePowerupGraphicsLoader() : base(Properties.Resources.fire, s => sprite = s, () => sprite) { }
    }
}
