using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    abstract class MapObject 
    {
        protected PointF position;
        protected Point mapPosition; 
    //    protected Bitmap sprite;
        protected RectangleF hitbox;

        protected MapObject(PointF _position, Bitmap _sprite) {
            //sprite = new Bitmap(_sprite, Game.boxSize);
            position = _position;
            mapPosition.X = (int)Math.Round(position.X / Game.tileSize);
            mapPosition.Y = (int)Math.Round(position.Y / Game.tileSize);
            hitbox = new RectangleF(position, new SizeF(Game.tileSize, Game.tileSize));
        }
        protected MapObject(Point _mapPosition, Bitmap _sprite) {
            //sprite = new Bitmap(_sprite, Game.boxSize);
            mapPosition = _mapPosition;
            position.X = _mapPosition.X * Game.tileSize;
            position.Y = _mapPosition.Y * Game.tileSize;
            hitbox = new RectangleF(position, new SizeF(Game.tileSize, Game.tileSize));
        }


        /*
         * Getters
         */
        public RectangleF getHitbox() { return hitbox; }
        public abstract Bitmap getSprite();// { return sprite; }
        public PointF getPosition() { return position; }
        public Point getMapPosition() { return mapPosition; }
    }

    abstract class MoveableObject : MapObject, ICollideable
    {

        protected PointF velocity;    
        //protected Point mapPosition; 
        protected Game.Direction direction;

        protected MoveableObject(PointF _position, Bitmap _sprite) : base(_position, _sprite){
            direction = Game.Direction.None;
         
        }

        public abstract void update(double elapsedTime);
        public abstract void collide();


        public PointF getVelocity() { return velocity; }
        //public Point getMapPosition() { return mapPosition; }
        //public Point getNextMapPosition() { }

        public Game.Direction getDirection() {  return direction; }
        public void setVelocity(PointF newVelocity) { velocity = newVelocity; }
        public void setDirection(Game.Direction newDirection) { direction = newDirection; }

    }



    interface IDestroyable
    {
        void destroy();
    }

    interface ICollideable
    {
        void collide();
    }

}
