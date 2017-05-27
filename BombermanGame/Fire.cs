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

        public override void Draw(Graphics g) {
            if (this.spriteAnimation.CurrentFrame == null) {
                return;
            }
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            var oversizedBounds = this.mapTile.Bounds;
            oversizedBounds.Inflate(1, 1);
            g.DrawImage(this.spriteAnimation.CurrentFrame, oversizedBounds);
        }

        public override string ToString() {
            return "Fire";
        }
    }
}
