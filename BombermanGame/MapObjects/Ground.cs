using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    class Ground : MapObject
    {

        static private Bitmap sprite = new Bitmap(Properties.Resources.Ground, Game.boxSize);

        public Ground(PointF position)
            : base(position) {}


        public override Bitmap getSprite() { return sprite; }
    }
}

