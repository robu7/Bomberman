using SharpDX.Direct2D1;

namespace BombermanGame.Powerups {
    class ExtraBombPowerup : Powerup {

        private BitmapLoader loader = new ExtraBombPowerupGraphicsLoader();

        protected override Bitmap Sprite => loader.Bitmap;

        protected override void ApplyToPlayer(Player player) {
            player.BombCap += 1;
        }

        public ExtraBombPowerup(double creationTime) : base(creationTime) {}
    }

    class ExtraBombPowerupGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public ExtraBombPowerupGraphicsLoader() : base(Properties.Resources.bomb, s => sprite = s, () => sprite) { }
    }
}
