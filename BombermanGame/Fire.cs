using System.Drawing;

namespace BombermanGame
{
    public enum FireType { Center, Horizontal, Vertical, Up, Down, Left, Right };

    class Fire : FixedMapObject {

        private Animation spriteAnimation;

        public Fire(FireType direction, double startTime) : base(destructible: false) {
            spriteAnimation = FireAnimations.GetFireAnimation(direction, 1);
            spriteAnimation.Start(startTime);
        }

        public override void Update(double totalTime) {
            spriteAnimation.Update(totalTime);
            this.mapTile?.MarkAsDirty();
            if (spriteAnimation.State == AnimationState.Stopped) {
                this.mapTile.Object = null;
                this.mapTile = null;
            }
        }

        public override void Draw(SharpDX.Direct2D1.RenderTarget target) {
            if (this.spriteAnimation.CurrentFrame == null) {
                return;
            }
            var b = this.mapTile.Bounds;
            target.DrawBitmap(this.spriteAnimation.CurrentFrame, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override string ToString() {
            return "Fire";
        }
    }
}
