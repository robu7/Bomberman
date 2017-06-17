using SharpDX.Direct2D1;
using BombermanGame.MapObjects;

namespace BombermanGame.Powerups
{
    abstract class Powerup : GameObject
    {
        protected abstract Bitmap Sprite { get; }
        public double CreationTime { get; }
        public double TimeToLive { get; private set; }

        public Powerup(double creationTime) {
            CreationTime = creationTime;
            TimeToLive = 5;
        }

        public override void Update(double totalTime) {
            if (totalTime - CreationTime >= TimeToLive) {
                // Remove from the map
                Tile.Object = null;
            }
        }

        public override void Draw(RenderTarget target) {
            if (Sprite == null) { return; }
            var b = Tile.Bounds;
            target.DrawBitmap(Sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public void ApplyAndConsume(Player player) {
            ApplyToPlayer(player);
            Tile.Object = null;
        }

        protected abstract void ApplyToPlayer(Player player);
    }
}
