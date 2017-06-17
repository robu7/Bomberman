using SharpDX.Direct2D1;
using BombermanGame.MapObjects;

namespace BombermanGame {
    class ConstBlock : GameObject {

        private static Bitmap sprite;

        public static void LoadGraphics(SharpDX.Direct2D1.RenderTarget target) {
            sprite = Properties.Resources.constblock.CreateDirectX2D1Bitmap(target);
        }

        public ConstBlock() {
            IsDescructible = false;
        }

        public override void Draw(RenderTarget target) {
            var b = Tile.Bounds;
            target.DrawBitmap(sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override void Update(double totalTime) {
            // Nothing to do here
        }
    }
}
