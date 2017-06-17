using SharpDX.Direct2D1;

namespace BombermanGame {
    class Block : GameObject {
        static private Bitmap sprite;

        public static void LoadGraphics(SharpDX.Direct2D1.RenderTarget target) {
            sprite = Properties.Resources.block.CreateDirectX2D1Bitmap(target);
        }

        public override void Draw(RenderTarget target) {
            var b = Tile.Bounds;
            target.DrawBitmap(sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override void Update(double totalTime) {
            // No need to do anything here
        }
    }
}
