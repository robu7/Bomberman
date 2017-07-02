using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpDX.Direct2D1;

namespace BombermanGame.Animations
{
    class BlockAnimation : IGraphicsResourceLoader
    {
        static private List<SharpDX.Direct2D1.Bitmap> destruction;

        public void LoadGraphics(RenderTarget target)
        {
            destruction = new List<SharpDX.Direct2D1.Bitmap>();

            System.Drawing.Bitmap original = Properties.Resources.BlockDestruction;
            System.Drawing.Bitmap sprite;
            Rectangle srcRect;

            // Init down

            for (int i = 0; i < 4; ++i) {
                srcRect = new Rectangle(i * 87, 0, 87, 87);
                sprite = original.Clone(srcRect, original.PixelFormat);
                destruction.Add(sprite.CreateDirectX2D1Bitmap(target));
                sprite.Dispose();
            }
        }

        public static Animation GetDestroyAnimation()
        {
            return new Animation(destruction, 1.2, false);
        }

    }
}
