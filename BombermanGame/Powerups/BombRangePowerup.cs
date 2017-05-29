using System;
using SharpDX.Direct2D1;

namespace BombermanGame.Powerups {
    class BombRangePowerup : Powerup {

        private static Bitmap sprite;

        protected override Bitmap Sprite => sprite;

        public static void LoadGraphics(RenderTarget target) {
            sprite = Properties.Resources.fire.CreateDirectX2D1Bitmap(target);
        }

        protected override void ApplyToPlayer(Player player) {
            player.BombRange += 1;
        }

        public BombRangePowerup(double creationTime) : base(creationTime) {}
    }
}
