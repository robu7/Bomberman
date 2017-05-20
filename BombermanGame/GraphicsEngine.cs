using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BombermanGame
{
    class GraphicsEngine
    {
        Graphics panelGraphics;
        List<Player> players;
        Map map;
        Bitmap frame;
        Graphics frameGraphics;
        Bitmap background;
        Timer renderTiming;

        public GraphicsEngine(Graphics _panelGraphics, List<Player> _players, Map _map) {
            panelGraphics = _panelGraphics;
            players = _players;
            map = _map;
            frame = new Bitmap(1100, 1100);
            frameGraphics = Graphics.FromImage(frame);

            //renderTiming = new Timer(16);
            //renderTiming.Elapsed += delegate { draw(); };
            //renderTiming.AutoReset = true;

            /*
            Graphics backgroundGraphics = Graphics.FromImage(background);

            var initGround = new Ground(new PointF(0,0));
            var initGround2 = initGround.getSprite();
            for (int x = 0; x < 11; x++) {
                for (int y = 0; y < 11; y++) {
                    backgroundGraphics.DrawImage(initGround2, new PointF(x * Game.tileSize, y * Game.tileSize));
                }
            }*/

            //panelGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }

        public void startRendering() { renderTiming.Start(); }

        public void draw() {

            //panelGraphics.DrawRectangle(new Pen(Color.AliceBlue, 3), Rectangle.Round(player1.getHitbox()));
            //panelGraphics.DrawRectangle(
            //frameGraphics.DrawImage(background, 0, 0);
            
            try {
                foreach (MapObject mapObject in map.getMap()) {
                    var sprite = mapObject.getSprite();
                    if (sprite != null) {
                        frameGraphics.DrawImage(sprite, mapObject.getPosition());
                    }
                }
            }
            catch {
                Console.WriteLine("Exception in graphic enginge");
            }
            foreach (var player in players) {
                if (player.shouldDraw()) {
                    var sprite = player.getSprite();
                    if (sprite != null) {
                        frameGraphics.DrawImage(player.getSprite(), player.getPosition());
                    }
                }
            }


            panelGraphics.DrawImage(frame, 0, 0);
            //panelGraphics.Dispose();
            //panelGraphics.DrawImageUnscaledAndClipped();
        }
    }
}
