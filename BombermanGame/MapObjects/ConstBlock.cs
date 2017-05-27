using System.Drawing;

namespace BombermanGame {
    class ConstBlock : FixedMapObject {

        private static Bitmap sprite = new Bitmap(Properties.Resources.constblock, Game.boxSize);

        public ConstBlock() : base(destructible: false) { }

        public override void Draw(Graphics g) {
            g.DrawImage(sprite, this.mapTile.Bounds.Location);
        }

        public override void Update(double totalTime) {
            // Nothing to do here
        }
    }
}
