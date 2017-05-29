using SharpDX.Direct2D1;

namespace BombermanGame.Powerups {
    class KickAbilityPowerup : Powerup {

        private static Bitmap sprite;

        protected override Bitmap Sprite => sprite;

        public static void LoadGraphics(RenderTarget target) {
            sprite = Properties.Resources.PlayerSprite.CreateDirectX2D1Bitmap(target);
        }

        protected override void ApplyToPlayer(Player player) {
            // TODO
        }

        public KickAbilityPowerup(double creationTime) : base(creationTime) {}
    }
}
