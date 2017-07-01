using SharpDX.Direct2D1;
using BombermanGame.GameObjects;
using System;
using BombermanGame.GameObjects.Powerups;

namespace BombermanGame {
    class Block : GameObject {
        private BitmapLoader loader = new BlockGraphicsLoader();
        public Powerup HiddenItem = null;

        public override void Draw(RenderTarget target) {
            var b = Tile.Bounds;
            target.DrawBitmap(loader.Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override void Update(double totalTime) {
            // No need to do anything here
        }

        protected override void OnDestroy(double currentTime)
        {
            base.OnDestroy(currentTime);
            Tile.Object = HiddenItem;
            HiddenItem?.SpawnedFromBlock(currentTime);          
        }
    }

    class BlockGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public BlockGraphicsLoader() : base(Properties.Resources.block, s => sprite = s, () => sprite) { }
    }
}
