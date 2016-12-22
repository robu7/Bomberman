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

        private Bitmap sprite;

        public Ground(PointF position)
            : base(position, Properties.Resources.Ground) {

                sprite = new Bitmap(Properties.Resources.Ground, Game.boxSize);
        }


        public override Bitmap getSprite() { return sprite; }
    }
}

