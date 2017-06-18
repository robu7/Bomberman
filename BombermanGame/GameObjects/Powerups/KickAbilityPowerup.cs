using SharpDX.Direct2D1;

namespace BombermanGame.GameObjects.Powerups {
    class KickAbilityPowerup : Powerup {

        private static BitmapLoader loader = new KickAbilityPowerupGraphicsLoader();

        protected override Bitmap Sprite => loader.Bitmap;

        protected override void ApplyToPlayer(Player player) {
            // TODO
        }

        public KickAbilityPowerup(double creationTime) : base(creationTime) {}
    }

    class KickAbilityPowerupGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public KickAbilityPowerupGraphicsLoader() : base(Properties.Resources.PlayerSprite, s => sprite = s, () => sprite) { }
    }
}
