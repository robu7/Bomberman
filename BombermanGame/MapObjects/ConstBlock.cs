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
        private Bitmap sprite;

         public ConstBlock(PointF position) : base(position, Properties.Resources.constblock) {

             sprite = new Bitmap(Properties.Resources.constblock, Game.boxSize);
        }


         public override Bitmap getSprite() { return sprite; }
    }
}
