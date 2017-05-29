using SharpDX.Direct2D1;

namespace BombermanGame.Powerups {
    class ExtraBombPowerup : Powerup {

        private static Bitmap sprite;

        protected override Bitmap Sprite => sprite;

        public static void LoadGraphics(RenderTarget target) {
            sprite = Properties.Resources.bomb.CreateDirectX2D1Bitmap(target);
        }

        protected override void ApplyToPlayer(Player player) {
            player.BombCap += 1;
        }

        public ExtraBombPowerup(double creationTime) : base(creationTime) {}
    }
}
