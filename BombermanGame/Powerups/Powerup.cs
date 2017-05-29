using SharpDX.Direct2D1;

namespace BombermanGame.Powerups
{
    abstract class Powerup : FixedMapObject
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
                this.mapTile.Object = null;
                this.mapTile = null;
            }
        }

        public override void Draw(RenderTarget target) {
            if (Sprite == null) { return; }
            var b = this.mapTile.Bounds;
            target.DrawBitmap(Sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public void ApplyAndConsume(Player player) {
            ApplyToPlayer(player);
            this.mapTile.Object = null;
            this.mapTile = null;
        }

        protected abstract void ApplyToPlayer(Player player);
    }
}
