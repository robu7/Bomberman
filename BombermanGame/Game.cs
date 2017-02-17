using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Forms;
using System.Timers;
using System.Media;

namespace BombermanGame
{
    partial class Game
    {
        private GraphicsEngine gEngine;     // ---Graphics engine---

        private Map map;                    // ---Current map ---

        private Thread GameThread;          // ---Game loop thread---

        private List<MoveableObject> objectsInMotion;   // ---List of objects currently moving

        private InputHandler inputHandler;

        private Communicator communicationHandler;

        /*
         * Player related 
         */
        private Player player1;
        public enum Direction {None, Up, Right, Down, Left};
        public List<Bomb> bombList;

        /*
         * Default contructor
         */
        public Game(Graphics panelGraphics, InputHandler _inputHandler, Communicator _communicationHandler) {
            communicationHandler = _communicationHandler;
            objectsInMotion = new List<MoveableObject>();
            inputHandler = _inputHandler;
            player1 = new Player(new PointF(100, 100));
            map = new Map();
            gEngine = new GraphicsEngine(panelGraphics, player1, map);

            bombList = new List<Bomb>();
            objectsInMotion.Add(player1);
            GameThread = new Thread(new ThreadStart(GameLoop));


            //SoundPlayer themeMusic = new SoundPlayer(@"C:\Users\Robin\Pictures\theme.wav");
            // Ska flyttas
            SoundPlayer themeMusic = new SoundPlayer(Properties.Resources.theme1);
            themeMusic.Play();
            /*
            Assembly assembly;
            Stream soundStream;
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream("BombermanGame.theme.wav"));
            sp.Play();*/
            
            GameThread.Start();
        }

        /*
         * Main gameloop, updates all objects necassary 
         */
        private void GameLoop() {
           // try {
                while (true) {

                    gameTime = Environment.TickCount / 1000.0;
                    elapsedTime = gameTime - _lastTime;
                    _lastTime = gameTime;

                    if (player1.isAlive())
                        player1.update(elapsedTime);

                    //if(objectInMotion.Count == 0)
                    if (player1.isAlive())
                        checkForPlayerCollision();
                    //checkForCollisions();

                    gEngine.draw();
                    // Distribute actions to other users
                }
            //}
            //catch (System.AccessViolationException ex) {
             //   Console.WriteLine("Exception in graphic enginge: {0}", ex.Source);
            //}
        }

        private void checkForPlayerCollision() {
            /*
             * 1. Check the element we are standing on 
             */
            MapObject elem = map.getElem(player1.getMapPosition());
            if (isCollidingWith(player1, elem)) {
                if (elem is Fire) {
                    player1.destroy();
                    return;
                }
            }

            /*
             * 2. Check the element in front of player
             */
            Point nextMapPos = getNextMapPosition(player1);
            elem = map.getElem(nextMapPos);
            if (isCollidingWith(player1, elem)) {
                Console.WriteLine("Colliding with {1} at {0}", nextMapPos, elem);
                if (!(elem is Fire)) {
                    player1.collide();
                }
            }

            /* TODO
             * 3. Smooth out movement around corners
             */
            //Direction playerDirection = player1.getDirection();
            PointF position = player1.getPosition();
            PointF nextTilePos = new PointF(nextMapPos.X * tileSize, nextMapPos.Y * tileSize);
            switch (player1.getDirection()) {
                case Direction.Down:
                case Direction.Up:
                    if (position.X < nextTilePos.X) {
                        player1.smoothAdjustPosition(Direction.Right);
                    }
                    else if (position.X > nextTilePos.X) {
                        player1.smoothAdjustPosition(Direction.Left);
                    }

                    break;
                case Direction.Left:
                case Direction.Right:
                    if (position.Y < nextTilePos.Y || position.Y > nextTilePos.Y) {
                        player1.smoothAdjustPosition(Direction.Up);
                    }
                    break;
            }
        }

        public void destroyObject(MapObject objectToDestroy) {
            lock (map) {
                map.destroyObject(objectToDestroy.getMapPosition());
            }
            //objectInMotion.Remove(player);
        }

        /*
        * Functions to handle input from user
        */
        public void handleKeyDownInput(KeyEventArgs keyBoardInput) {

            if (player1.isAlive()) {
                switch (keyBoardInput.KeyCode) {

                    case Keys.W:
                        player1.setVelocity(new PointF(0, -225));
                        player1.startAnimating(Direction.Up);
                        player1.setDirection(Direction.Up);
                        break;
                    case Keys.S:
                        player1.setVelocity(new PointF(0, 225));
                        player1.startAnimating(Direction.Down);
                        player1.setDirection(Direction.Down);
                        break;
                    case Keys.A:
                        player1.setVelocity(new PointF(-225, 0));
                        player1.startAnimating(Direction.Left);
                        player1.setDirection(Direction.Left);
                        break;
                    case Keys.D:
                        player1.setVelocity(new PointF(225, 0));
                        player1.startAnimating(Direction.Right);
                        player1.setDirection(Direction.Right);
                        break;
                    case Keys.B:
                        if (player1.getBombCap() > 0) {
                            player1.decBombCap();
                            Bomb newBomb = new Bomb(player1.getMapPosition(), player1);
                            bombList.Add(newBomb);
                            map.addObject(newBomb, player1.getMapPosition());
                            newBomb.TTL.Elapsed += delegate { bombExplosion(newBomb); };
                        }
                        //Console.WriteLine("new bomb at: {0}", player1.getMapPosition());
                        break;
                }
            }
        }

        public void startPlayerMovement(Player player, Direction dir) {

        }


        public void handleKeyUpInput(KeyEventArgs keyBoardInput) {
            Direction dir = player1.getDirection();
            switch (keyBoardInput.KeyCode) {
                case Keys.W:
                    if (dir == Direction.Up) {
                        player1.setVelocity(new PointF(0, 0));
                        player1.setDirection(Direction.None);
                        player1.stopAnimating();
                    }
                    break;
                case Keys.S:
                     if (dir == Direction.Down) {
                        player1.setVelocity(new PointF(0, 0));
                        player1.setDirection(Direction.None);
                        player1.stopAnimating();
                    }
                    break;
                case Keys.A:
                    if (dir == Direction.Left) {
                        player1.setVelocity(new PointF(0, 0));
                        player1.setDirection(Direction.None);
                        player1.stopAnimating();
                    }
                    break;
                case Keys.D:
                    if (dir == Direction.Right) {
                        player1.setVelocity(new PointF(0, 0));
                        player1.setDirection(Direction.None);
                        player1.stopAnimating();
                    }
                    break;
            }
            //player1.setDirection(Direction.None);
        }


        private void bombExplosion(Bomb bomb) {
            Console.WriteLine("Bomb exploded at: {0}", bomb.getMapPosition());

            Point currentMapPos = bomb.getMapPosition();
            map.destroyObject(currentMapPos);
            bomb.getOwner().incBombCap();
            IDestroyable objectToDestroy;

            map.addFire(new Fire(currentMapPos, FireType.Center), currentMapPos);

            for (int i = 1; i <= bomb.getRange(); ++i) {
                Point up = new Point(currentMapPos.X, currentMapPos.Y - i);
                if (map.getElem(up) is IDestroyable) {
                    objectToDestroy = map.getElem(up) as IDestroyable;
                    objectToDestroy.destroy();
                    destroyObject(objectToDestroy as MapObject);
                    map.addFire(new Fire(up, FireType.Up), up);
                    break;
                }
                else if (map.getElem(up) is BombermanGame.Ground || map.getElem(up) is BombermanGame.Fire) {
                    if (i == bomb.getRange())
                        map.addFire(new Fire(up, FireType.Up), up);
                    else {
                        map.addFire(new Fire(up, FireType.Vertical), up);
                    }
                }
                else
                    break;
            }

            for (int i = 1; i <= bomb.getRange(); ++i) {
                Point down = new Point(currentMapPos.X, currentMapPos.Y + i);
                if (map.getElem(down) is IDestroyable) {
                    objectToDestroy = map.getElem(down) as IDestroyable;
                    objectToDestroy.destroy();
                    destroyObject(objectToDestroy as MapObject);
                    map.addFire(new Fire(down, FireType.Down), down);
                    break;
                }
                else if (map.getElem(down) is BombermanGame.Ground || map.getElem(down) is BombermanGame.Fire) {
                    if (i == bomb.getRange())
                        map.addFire(new Fire(down, FireType.Down), down);
                    else {
                        map.addFire(new Fire(down, FireType.Vertical), down);
                    }
                }
                else
                    break;
            }

            for (int i = 1; i <= bomb.getRange(); ++i) {
                Point left = new Point(currentMapPos.X - i, currentMapPos.Y);
                if (map.getElem(left) is IDestroyable) {
                    objectToDestroy = map.getElem(left) as IDestroyable;
                    objectToDestroy.destroy();
                    destroyObject(objectToDestroy as MapObject);
                    map.addFire(new Fire(left, FireType.Left), left);
                    break;
                }
                else if (map.getElem(left) is BombermanGame.Ground || map.getElem(left) is BombermanGame.Fire) {
                    if (i == bomb.getRange())
                        map.addFire(new Fire(left, FireType.Left), left);
                    else {
                        map.addFire(new Fire(left, FireType.Horizontal), left);
                    }
                }
                else
                    break;
            }

            for (int i = 1; i <= bomb.getRange(); ++i) {
                Point right = new Point(currentMapPos.X + i, currentMapPos.Y);
                if (map.getElem(right) is IDestroyable) {
                    objectToDestroy = map.getElem(right) as IDestroyable;
                    objectToDestroy.destroy();
                    destroyObject(objectToDestroy as MapObject);
                    map.addFire(new Fire(right, FireType.Right), right);
                    break;
                }
                else if (map.getElem(right) is BombermanGame.Ground || map.getElem(right) is BombermanGame.Fire) {
                    if (i == bomb.getRange())
                        map.addFire(new Fire(right, FireType.Right), right);
                    else {
                        map.addFire(new Fire(right, FireType.Horizontal), right);
                    }
                }
                else
                    break;
            }
        }
    }
}
