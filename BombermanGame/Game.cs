using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
//using System.Windows.Input;
using System.Windows.Forms;
using System.Timers;
using System.Media;
using System.Diagnostics;

namespace BombermanGame
{
    partial class Game : NetworkReciever
    {
        private GraphicsEngine gEngine;     // ---Graphics engine---

        private Map map;                    // ---Current map ---

        private Thread GameThread;          // ---Game loop thread---

        private List<MoveableObject> objectsInMotion;   // ---List of objects currently moving

        private InputHandler inputHandler;

        private Communicator communicationHandler;

        private List<Task> networkCommands;

        private ManualResetEvent readySignal;

        private Stopwatch timer;

        /*
         * Player related 
         */
        private Dictionary<int, Player> players;
        private Player myPlayerInstance;
        private int myID;
        public enum Direction {None, Up, Right, Down, Left};
        public List<Bomb> bombList;


        public void HandleCommand<T>(T command) {
            Console.WriteLine("Recieved message: {0}", typeof(T).Name);
            switch (typeof(T).Name) {
                case "PlayerMovement":
                    startPlayerMovement(command as PlayerMovement);
                    //networkCommands.Add(new Task(new Action (startPlayerMovement(command as PlayerMovement)));
                    break;
                case "DeployBomb":
                    DeployBomb cmd = command as DeployBomb;
                    deployBomb(players[cmd.PeerID]);
                    break;
            }
            //networkCommands.Add(new Task());
            //networkCommands.Add(Task.Run(() => Thread.Sleep(2000)));

        }
        
        public void ReadySignal(int peerID) {
            readySignal.Set();
        }

        /*
         * Default contructor
         */
        public Game(Graphics panelGraphics, InputHandler _inputHandler, Communicator _communicationHandler) {
            communicationHandler = _communicationHandler;
            inputHandler = _inputHandler;

            communicationHandler.ActiveControl = this;
            objectsInMotion = new List<MoveableObject>();

            players = new Dictionary<int, Player>();
            networkCommands = new List<Task>();
            myID = _communicationHandler.PeerID;
            players.Add(myID, new Player(new PointF(100, 100)));
            myPlayerInstance = players[myID];

            foreach (var peer in _communicationHandler.PeerList) {
                players.Add(peer.Key, new Player(new PointF(100, 100)));
                Console.WriteLine("Added player: {0}", peer.Key);
            }
            Console.WriteLine("Game Init, my ID: {0}", myID);
            //player1 = new Player(new PointF(100, 100));

            readySignal = new ManualResetEvent(false);
            map = new Map();
            gEngine = new GraphicsEngine(panelGraphics, players.Values.ToList(), map);

            bombList = new List<Bomb>();
            //objectsInMotion.Add(player1);
            timer = new Stopwatch();

            GameThread = new Thread(new ThreadStart(GameLoop));


            //SoundPlayer themeMusic = new SoundPlayer(@"C:\Users\Robin\Pictures\theme.wav");
            // Ska flyttas
            //SoundPlayer themeMusic = new SoundPlayer(Properties.Resources.theme1);
            //themeMusic.Play();

            GameThread.Start();
            //gEngine.startRendering();
        }

        /*
         * Main gameloop, updates all objects necassary 
         */
        private void GameLoop() {

            double t = 0.0;
            double dt = 1.0/60.0;
            //Console.WriteLine(dt);
            double delta;

            timer.Start();
            currentTime = ((double)timer.ElapsedMilliseconds) / 1000;

            //currentTime = Environment.TickCount / 1000.0;
            

            while (true) {

                /*
                 * Step 1: determine timestep values
                 */
                //newTime = Environment.TickCount / 1000.0;
                newTime = ((double)timer.ElapsedMilliseconds) / 1000;
                frameTime = newTime - currentTime;
                currentTime = newTime;

                //Console.WriteLine("frametime: "+ frameTime);
                while (frameTime > 0.0) {

                    //Console.WriteLine(dt);
                    delta = Math.Min(frameTime, dt);
                    //delta = dt;
                    //Console.WriteLine(delta);
                    /*
                     * Step 2: Check keyboard input and replicate to other players
                     */
                    if (myPlayerInstance.isAlive()) {
                        if (inputHandler.UpdatedInput) {
                            myPlayerInstance.updateMovement(inputHandler.PressedDirection, currentTime);
                            communicationHandler.Broadcast(PacketBuilder.Build_PlayerMovement(myID, inputHandler.PressedDirection));
                            inputHandler.UpdatedInput = false;
                        }
                        if (inputHandler.DeployBomb) {
                            if (myPlayerInstance.getBombCap() > 0)
                                deployBomb(myPlayerInstance);
                            //inputHandler.DeployBomb = false;
                            communicationHandler.Broadcast(PacketBuilder.Build_DeployBomb(myID));
                        }
                    }


                    /*
                     * Step 3: Increase bomb timers and explosions
                     */
                    updateBombs(delta, currentTime);
                    map.updateAll(delta, currentTime);


                    /*
                     * Step 4: Check for collisions
                     */
                    foreach (var player in players) {
                        if (player.Value.isAlive()) {
                            player.Value.update(delta, currentTime);
                            checkPlayerCollision(player.Value);
                        }
                    }


                    /*
                     * Step 5: Execute netwok syncronizations messages,
                     *         and wait for them to finish
                     */
                    foreach (var command in networkCommands) {
                        command.Start();
                    }
                    Task.WaitAll(networkCommands.ToArray());

                    frameTime -= delta;
                    t += delta;
                }
                /*
                * Step 6: Draw current game state
                */
                //double test = Environment.TickCount / 1000.0;
                gEngine.draw();
                //communicationHandler.Broadcast(PacketBuilder.Build_Ready(myID));
                //readySignal.WaitOne();
                //readySignal.
                //double test2 = Environment.TickCount / 1000.0;
                //Console.WriteLine(test2-test);
            }
        }

        private void checkPlayerCollision(Player player) {
            /*
             * 1. Check the element we are standing on 
             */
            MapObject elem = map.getElem(player.getMapPosition());
            if (isCollidingWith(player, elem)) {
                if (elem is Fire) {
                    player.destroy();
                    return;
                }
            }

            /*
             * 2. Check the element in front of player
             */
            Point nextMapPos = getNextMapPosition(player);
            elem = map.getElem(nextMapPos);
            if (isCollidingWith(player, elem)) {
                Console.WriteLine("Colliding with {1} at {0}", nextMapPos, elem);
                if (!(elem is Fire)) {
                    player.collide();
                }
            }

            /* TODO
             * 3. Smooth out movement around corners
             */
            //Direction playerDirection = player.getDirection();
            PointF position = player.getPosition();
            PointF nextTilePos = new PointF(nextMapPos.X * tileSize, nextMapPos.Y * tileSize);
            switch (player.getDirection()) {
                case Direction.Down:
                case Direction.Up:
                    if (position.X < nextTilePos.X) {
                        player.smoothAdjustPosition(Direction.Right);
                    }
                    else if (position.X > nextTilePos.X) {
                        player.smoothAdjustPosition(Direction.Left);
                    }

                    break;
                case Direction.Left:
                case Direction.Right:
                    if (position.Y < nextTilePos.Y || position.Y > nextTilePos.Y) {
                        player.smoothAdjustPosition(Direction.Up);
                    }
                    break;
            }
        }


        /*
         * Function to handle destruction of object
         */
        public void destroyObject(MapObject objectToDestroy) {
                map.destroyObject(objectToDestroy.getMapPosition());
            //objectInMotion.Remove(player);
        }

        /*
        * Functions to handle input from user
        */
        public void handleKeyDownInput(KeyEventArgs keyBoardInput) {
            inputHandler.buttonPressed(keyBoardInput.KeyCode);          
        }
        public void handleKeyUpInput(KeyEventArgs keyBoardInput) {
            inputHandler.buttonReleased(keyBoardInput.KeyCode);
        }

        private void updatePlayerMovement(Player player, Direction moveDirection) {
            Console.WriteLine("Player: {0} started moving: {1}", players.First(x => x.Value == player).Key, moveDirection.ToString());
            player.updateMovement(moveDirection, this.currentTime);
        }

        private void startPlayerMovement(PlayerMovement newMove) {
            updatePlayerMovement(players[newMove.PeerID], newMove.MoveDirection);
        }
        private void stopPlayerMovement(Player player, Direction dir) {

        }

        private void deployBomb(Player player) {
            Console.WriteLine("Bomb deployed at: {0} by player: {1}", player.getMapPosition(), players.First(x => x.Value == player).Key);
            if (player.getBombCap() > 0) {
                if (map.getElem(player.getMapPosition()).ToString() != "Bomb") {
                    player.decBombCap();
                    Bomb newBomb = new Bomb(player.getMapPosition(), player);
                    bombList.Add(newBomb);
                    map.addObject(newBomb, player.getMapPosition());
                }              
            }
        }

        private void updateBombs(double delta, double totalTime) {

            foreach (var bomb in bombList) {
                if (bomb.update(delta)) {
                    bombExplosion(bomb, totalTime);
                     //Task.Run(() => bombExplosion(bomb));  
                }           
            }
            bombList.RemoveAll(item => item.HasExploded);
        }


        private void bombExplosion(Bomb bomb, double totalTime) {
            //Console.WriteLine("Bomb exploded at: {0}", bomb.getMapPosition());

            Point currentMapPos = bomb.getMapPosition();
            map.destroyObject(currentMapPos);
            bomb.getOwner().incBombCap();

            map.addFire(new Fire(currentMapPos, FireType.Center, totalTime), currentMapPos);
            
            int upModifier = -1, downModifier = 1, leftModifier = -1, rightModifier = 1;
            explodeInDirection(bomb, 0, upModifier, FireType.Up, totalTime);
            explodeInDirection(bomb, 0, downModifier, FireType.Down, totalTime);
            explodeInDirection(bomb, leftModifier, 0, FireType.Left, totalTime);
            explodeInDirection(bomb, rightModifier, 0, FireType.Right, totalTime);
            
            bomb.destroy();
        }

        /*
         * Helper function to bombExplosion(...)
         */
        private void explodeInDirection(Bomb bomb,int xModifier, int yModifier, FireType fireType, double totalTime) {
            Point currentMapPos = bomb.getMapPosition();
            IDestroyable objectToDestroy;

            FireType connectionType = (fireType == FireType.Up || fireType == FireType.Down) ? FireType.Vertical : FireType.Horizontal;
            
            for (int i = 1; i <= bomb.getRange(); ++i) {
                Point elemPos = new Point(currentMapPos.X + i * xModifier, currentMapPos.Y + i * yModifier);
                MapObject elem = map.getElem(elemPos);
                if (elem is IDestroyable) {
                    objectToDestroy = elem as IDestroyable;
                    objectToDestroy.destroy();
                    destroyObject(objectToDestroy as MapObject);
                    if(!(elem is Bomb))
                        map.addFire(new Fire(elemPos, fireType, totalTime), elemPos);
                    break;
                }
                else if (elem is BombermanGame.Ground || elem is BombermanGame.Fire) {
                    if (i == bomb.getRange())
                        map.addFire(new Fire(elemPos, fireType, totalTime), elemPos);
                    else {
                        map.addFire(new Fire(elemPos, connectionType, totalTime), elemPos);
                    }
                }
                else
                    break;
            }
        }
    }
}
