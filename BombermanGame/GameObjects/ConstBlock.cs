using SharpDX.Direct2D1;
using BombermanGame.GameObjects;

namespace BombermanGame {
    class ConstBlock : GameObject {

        private static BitmapLoader loader = new ConstBlockGraphicsLoader();

        public ConstBlock() {
            IsDescructible = false;
        }

        public override void Draw(RenderTarget target) {
            var b = Tile.Bounds;
            target.DrawBitmap(loader.Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override void Update(double totalTime) {
            // Nothing to do here
        }
    }

    class ConstBlockGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public ConstBlockGraphicsLoader() : base(Properties.Resources.constblock, s => sprite = s, () => sprite) { }
    }
}
