using System.Drawing;

namespace BombermanGame {
    class Block : FixedMapObject {
        static private Bitmap sprite = new Bitmap(Properties.Resources.block, Game.boxSize);
        
        public void destroy() { }

        public override void Draw(Graphics g) {
            g.DrawImage(sprite, this.mapTile.Bounds.Location);
        }

        public override void Update(double totalTime) {
            // No need to do anything here
        }
    }
}
