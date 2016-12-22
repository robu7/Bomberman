using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;


namespace BombermanGame
{
    partial class Game
    {
        /*
        * ---Time variables to ensure even game flow---
        */
        private double gameTime;
        private double elapsedTime;
        private double _lastTime = 0;

        /*
         * ---Size values to game--- 
         */
        public const float mapHeight = 1100;
        public const float mapWidth = 1100;
        public const float tileSize = 1100 / 11;

        static public Size boxSize = new Size((int)tileSize, (int)tileSize);


        /*
         * Function to start game
         */
        public void startGame() {
            //player1 = new Player(new PointF(1 * Game.tileSize, 1 * Game.tileSize));
            //gEngine = new GraphicsEngine();
            //gEngine.startGraphics();
            GameThread = new Thread(new ThreadStart(GameLoop));
            GameThread.Start();
        }

        /*
         * Function to stop game
         */
        public void stopGame() {
            //gEngine.stopGraphics();
            this.GameThread.Abort();
        }

        private bool isCollidingWith(MapObject obj, MapObject elem ) {

            if (!(elem is BombermanGame.Ground)) {
                if (obj.getHitbox().IntersectsWith(elem.getHitbox())) {
                    return true;
                }
            }
            return false;
        }

        private Point getNextMapPosition(MoveableObject obj) {

            Point nextMapPos = obj.getMapPosition();
            switch (obj.getDirection()) {
                case Direction.None:
                    break;
                case Direction.Up:
                    nextMapPos.Y -= 1;
                    break;
                case Direction.Down:
                    nextMapPos.Y += 1;
                    break;
                case Direction.Left:
                    nextMapPos.X -= 1;
                    break;
                case Direction.Right:
                    nextMapPos.X += 1;
                    break;
            }
            return nextMapPos;
        }


    }
}




/*

        private void checkForCollisions() {
            Point nextMapPos = new Point(0, 0);
            //PointF position;
            //RectangleF hitbox;
            foreach (MoveableObject movingObject in objectsInMotion) {
                nextMapPos = movingObject.getMapPosition();
                switch (movingObject.getDirection()) {  
                    case Direction.None:
                        break;
                    case Direction.Up:
                        nextMapPos.Y -= 1;
                        break;
                    case Direction.Down:
                        nextMapPos.Y += 1;
                        break;
                    case Direction.Left:
                        nextMapPos.X -= 1;
                        break;
                    case Direction.Right:
                        nextMapPos.X += 1;
                        break;
                }

                MapObject elem = map.getElem(nextMapPos);
                if (!(map.getElem(nextMapPos) is BombermanGame.Ground)) {
                    if (movingObject.getHitbox().IntersectsWith(elem.getHitbox())) {
                        if (elem is BombermanGame.Fire) {
                            (movingObject as Player).destroy();
                            Console.WriteLine("destroyObject ");//destroyObject(movingObject);
                        }
                           
                        //else if (elem is BombermanGame.)
                        //else if (elem is MoveableObject)
                        else
                            movingObject.collide();
                    }
                }
            }
        }


*/