using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    class ConstBlock : MapObject
    {
        private static Bitmap sprite = new Bitmap(Properties.Resources.constblock, Game.boxSize);

         public ConstBlock(PointF position) : base(position) {}


         public override Bitmap getSprite() { return sprite; }
    }
}
