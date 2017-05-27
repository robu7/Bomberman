using System.Drawing;

namespace BombermanGame
{
    class Ground {
        private static Bitmap sprite = new Bitmap(Properties.Resources.Ground, Game.boxSize);

        public static void Draw(Graphics g, PointF location) {
            g.DrawImage(sprite, location);
        }
    }
}

