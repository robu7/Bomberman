using SharpDX.Direct2D1;

namespace BombermanGame
{
    class Ground : IGraphicsResourceLoader {
        private static BitmapBrush groundBrush;

        public void LoadGraphics(RenderTarget target) {
            var sprite = Properties.Resources.Ground.CreateDirectX2D1Bitmap(target);
            groundBrush = new BitmapBrush(target, sprite, new BitmapBrushProperties { ExtendModeX = ExtendMode.Wrap, ExtendModeY = ExtendMode.Wrap });
        }

        public static void Draw(SharpDX.Direct2D1.RenderTarget target, System.Drawing.RectangleF bounds) {
            target.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom), groundBrush);
        }
    }
}

