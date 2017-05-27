using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanGame
{
    class GraphicsEngine
    {
        Graphics panelGraphics;
        List<Player> players;
        Map map;
        Bitmap frame;
        Graphics frameGraphics;

        public GraphicsEngine(Graphics _panelGraphics, List<Player> _players, Map _map) {
            panelGraphics = _panelGraphics;
            players = _players;
            map = _map;
            frame = new Bitmap(_map.Size.Width, _map.Size.Height);
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

        //public void startRendering() { renderTiming.Start(); }

        public void draw() {
            
            try {
                this.map.Draw(frameGraphics);
            }
            catch {
                Console.WriteLine("Exception in graphic enginge");
            }
            foreach (var player in players) {
                if (player.shouldDraw()) {
                    player.Draw(frameGraphics);
                }
            }

            panelGraphics.DrawImage(frame, 0, 0);
        }
    }
}
