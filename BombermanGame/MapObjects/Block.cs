using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    class Block : MapObject, IDestroyable
    {
        static private Bitmap sprite = new Bitmap(Properties.Resources.block, Game.boxSize);

        public Block(PointF position) : base(position) {
        }


        public void destroy() { }


        public override Bitmap getSprite() { return sprite; }
    }
}
