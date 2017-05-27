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

        public GraphicsEngine(Graphics panelGraphics, List<Player> players, Map map) {
            this.panelGraphics = panelGraphics;
            this.players = players;
            this.map = map;
            frame = new Bitmap(map.Size.Width, map.Size.Height);
            frameGraphics = Graphics.FromImage(frame);
        }

        public void Draw() {
            
            try {
                this.map.Draw(frameGraphics);
            }
            catch {
                Console.WriteLine("Exception in graphic enginge");
            }
            foreach (var player in players) {
                if (player.ShouldDraw()) {
                    player.Draw(frameGraphics);
                }
            }

            panelGraphics.DrawImage(frame, 0, 0);
        }
    }
}
