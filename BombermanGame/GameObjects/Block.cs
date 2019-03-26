using SharpDX.Direct2D1;
using BombermanGame.GameObjects;
using System;
using BombermanGame.GameObjects.Powerups;
using BombermanGame.Animations;

namespace BombermanGame {
    class Block : GameObject {
        private BitmapLoader loader = new BlockGraphicsLoader();
        public Powerup HiddenItem = null;
        private Animation spriteAnimation;
        private int? TimeToLive = null;

        public override void Draw(RenderTarget target) {
            var bounds = Tile.Bounds;

            if (spriteAnimation?.State == AnimationState.InProgress) {
                target.DrawBitmap(spriteAnimation.CurrentFrame, new SharpDX.Mathematics.Interop.RawRectangleF(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
            } else {
                target.DrawBitmap(loader.Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
            }
        }

        public override void Update(double totalTime) {
            // No need to do anything here
            if (TimeToLive.HasValue) {
                if (spriteAnimation.State == AnimationState.Stopped) {
                    base.OnDestroy(totalTime);
                    Tile.Object = HiddenItem;
                    HiddenItem?.SpawnedFromBlock(totalTime);
                } else
                    spriteAnimation.Update(totalTime);

                Tile.MarkAsDirty();
            }
        }

        protected override void OnDestroy(double currentTime)
        {
            //base.OnDestroy(currentTime);
            //Tile.Object = HiddenItem;
            //HiddenItem?.SpawnedFromBlock(currentTime);
            spriteAnimation = BlockAnimation.GetDestroyAnimation();
            //spriteAnimation = BlockAnimation.GetExplosionAnimation();
            spriteAnimation.Start(currentTime);
            TimeToLive = 4;

        }
    }

    class BlockGraphicsLoader : BitmapLoader {
        private static Bitmap sprite;
        public BlockGraphicsLoader() : base(Properties.Resources.block, s => sprite = s, () => sprite) { }
    }
}
