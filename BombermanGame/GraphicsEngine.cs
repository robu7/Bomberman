using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    class GraphicsEngine
    {
        Graphics panelGraphics;
        Player player1;
        Map map;
        Bitmap frame;
        Graphics frameGraphics;
        Bitmap background;

        public GraphicsEngine(Graphics _panelGraphics, Player _player1, Map _map) {
            panelGraphics = _panelGraphics;
            player1 = _player1;
            map = _map;
            frame = new Bitmap(1100, 1100);
            frameGraphics = Graphics.FromImage(frame);

            background = new Bitmap(1100, 1100);
            /*
            Graphics backgroundGraphics = Graphics.FromImage(background);

            var initGround = new Ground(new PointF(0,0));
            var initGround2 = initGround.getSprite();
            for (int x = 0; x < 11; x++) {
                for (int y = 0; y < 11; y++) {
                    backgroundGraphics.DrawImage(initGround2, new PointF(x * Game.tileSize, y * Game.tileSize));
                }
            }*/

        }

        public void draw() {

            //panelGraphics.DrawRectangle(new Pen(Color.AliceBlue, 3), Rectangle.Round(player1.getHitbox()));
            //panelGraphics.DrawRectangle(
            //frameGraphics.DrawImage(background, 0, 0);

            try {
                foreach (MapObject mapObject in map.getMap()) {
                    frameGraphics.DrawImage(mapObject.getSprite(), mapObject.getPosition());
                }
            }
            catch {
                Console.WriteLine("Exception in graphic enginge");
            }
            if (player1.shouldDraw())
                frameGraphics.DrawImage(player1.getSprite(), player1.getPosition());

            panelGraphics.DrawImage(frame, 0, 0);
            //panelGraphics.DrawImageUnscaledAndClipped();
        }
    }
}
