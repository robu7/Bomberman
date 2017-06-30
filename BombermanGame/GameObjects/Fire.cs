namespace BombermanGame.GameObjects {
    public enum FireType { Center, Horizontal, Vertical, Up, Down, Left, Right };

    class Fire : GameObject {

        private Animation spriteAnimation;

        public Fire(FireType direction, double startTime) {
            spriteAnimation = FireAnimations.GetFireAnimation(direction, 1);
            spriteAnimation.Start(startTime);
            IsDescructible = false;
        }

        public override void Update(double totalTime) {
            spriteAnimation.Update(totalTime);
            Tile?.MarkAsDirty();
            if (spriteAnimation.State == AnimationState.Stopped) {
                Tile.Object = null;
            }
        }

        public override void Draw(SharpDX.Direct2D1.RenderTarget target) {
            if (this.spriteAnimation.CurrentFrame == null) {
                return;
            }
            var b = Tile.Bounds;
            target.DrawBitmap(this.spriteAnimation.CurrentFrame, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public double GetStartTime() { return spriteAnimation.AnimationStartTime; }

        public override string ToString() {
            return "Fire";
        }
    }
}
