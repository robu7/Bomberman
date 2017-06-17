using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace BombermanGame {
    class Game : NetworkReciever {
        private GraphicsEngine gEngine;     // ---Graphics engine---
        
        private Map map;                    // ---Current map ---

        private Thread GameThread;          // ---Game loop thread---

        private List<GameObject> objectsInMotion = new List<GameObject>();   // ---List of objects currently moving

        private InputHandler inputHandler;

        private Communicator communicationHandler;

        private List<Task> networkCommands;

        private ManualResetEvent readySignal;

        private Stopwatch timer;

        //
        // Player related 
        //
        private Dictionary<int, Player> players;
        private Player myPlayerInstance;
        private int myID;
        public enum Direction {None, North, East, South, West};
        public List<Bomb> bombList;

        //
        // Time variables to ensure even game flow
        //
        private double newTime;
        private double timeUntilNextFrame;
        private double currentTime = 0;

        //
        // Size values to game
        //
        public const float mapHeight = 1100;
        public const float mapWidth = 1100;
        public const float tileSize = 1100 / 11;

        static public Size boxSize = new Size((int)tileSize, (int)tileSize);

        public void HandleCommand<T>(T command) {
            Console.WriteLine("Recieved message: {0}", typeof(T).Name);
            switch (typeof(T).Name) {
                case "PlayerMovement":
                    startPlayerMovement(command as PlayerMovement);
                    //networkCommands.Add(new Task(new Action (startPlayerMovement(command as PlayerMovement)));
                    break;
                case "DeployBomb":
                    DeployBomb cmd = command as DeployBomb;
                    players[cmd.PeerID].DeployBomb();
                    break;
            }
            //networkCommands.Add(new Task());
            //networkCommands.Add(Task.Run(() => Thread.Sleep(2000)));

        }
        
        public void ReadySignal(int peerID) {
            readySignal.Set();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Game(Control gamePanel/*Graphics panelGraphics*/, InputHandler inputHandler, Communicator communicationHandler) {
            this.communicationHandler = communicationHandler;
            this.inputHandler = inputHandler;

            this.communicationHandler.ActiveControl = this;

            map = Map.CreateDefault();
            players = new Dictionary<int, Player>();
            networkCommands = new List<Task>();
            myID = communicationHandler.PeerID;
            players.Add(myID, new Player(map.GetTileAt(new PointF(100, 100))));
            myPlayerInstance = players[myID];

            foreach (var peer in communicationHandler.PeerList) {
                players.Add(peer.Key, new Player(map.GetTileAt(new PointF(100, 100))));
                Console.WriteLine("Added player: {0}", peer.Key);
            }
            Console.WriteLine("Game Init, my ID: {0}", myID);
            //player1 = new Player(new PointF(100, 100));

            readySignal = new ManualResetEvent(false);
            gEngine = new GraphicsEngine(gamePanel, players.Values.ToList(), map);

            foreach (var p in players.Values) {
                p.Init();
            }
            bombList = new List<Bomb>();
            //objectsInMotion.Add(player1);
            timer = new Stopwatch();

            GameThread = new Thread(new ThreadStart(GameLoop));

            //SoundPlayer themeMusic = new SoundPlayer(@"C:\Users\Robin\Pictures\theme.wav");
            // Ska flyttas
            //SoundPlayer themeMusic = new SoundPlayer(Properties.Resources.theme1);
            //themeMusic.Play();

            GameThread.Start();
        }

        /// <summary>
        /// Main gameloop, updates all objects necassary 
        /// </summary>
        private void GameLoop() {

            double t = 0.0;
            double dt = 1.0/60.0;
            double delta;

            timer.Start();
            currentTime = ((double)timer.ElapsedMilliseconds) / 1000;
            
            while (true) {

                //
                // Step 1: determine timestep values
                //
                newTime = ((double)timer.ElapsedMilliseconds) / 1000;
                timeUntilNextFrame = newTime - currentTime;
                currentTime = newTime;
                
                while (timeUntilNextFrame > 0.0) {
                    delta = Math.Min(timeUntilNextFrame, dt);

                    //
                    // Step 2: Check keyboard input and replicate to other players
                    //
                    if (myPlayerInstance.isAlive()) {
                        if (inputHandler.UpdatedInput) {
                            myPlayerInstance.NewMovementDirection(inputHandler.PressedDirection, currentTime);
                            communicationHandler.Broadcast(PacketBuilder.Build_PlayerMovement(myID, inputHandler.PressedDirection));
                            inputHandler.UpdatedInput = false;
                        }
                        if (inputHandler.DeployBomb) {
                            if (myPlayerInstance.BombCap > 0)
                                myPlayerInstance.DeployBomb();
                            //inputHandler.DeployBomb = false;
                            communicationHandler.Broadcast(PacketBuilder.Build_DeployBomb(myID));
                        }
                    }
                    
                    //
                    // Step 3: Update fixed objects on the game map
                    //
                    map.Update(currentTime);

                    //
                    // Step 4: Update floating objects
                    //
                    foreach (var player in players.Where(p => p.Value.isAlive())) {
                        player.Value.Update(currentTime);
                        //checkPlayerCollision(player.Value);
                    }
                    
                    //
                    // Step 5: Execute netwok syncronizations messages,
                    //         and wait for them to finish
                    //
                    foreach (var command in networkCommands) {
                        command.Start();
                    }
                    Task.WaitAll(networkCommands.ToArray());

                    timeUntilNextFrame -= delta;
                    t += delta;
                }

                //
                // Step 6: Draw current game state
                //
                gEngine.Draw();
                //communicationHandler.Broadcast(PacketBuilder.Build_Ready(myID));
                //readySignal.WaitOne();
            }
        }

        /// <summary>
        /// Functions to handle input from user
        /// </summary>
        public void handleKeyDownInput(KeyEventArgs keyBoardInput) {
            inputHandler.buttonPressed(keyBoardInput.KeyCode);          
        }
        public void handleKeyUpInput(KeyEventArgs keyBoardInput) {
            inputHandler.buttonReleased(keyBoardInput.KeyCode);
        }

        private void updatePlayerMovement(Player player, Direction moveDirection) {
            Console.WriteLine("Player: {0} started moving: {1}", players.First(x => x.Value == player).Key, moveDirection.ToString());
            player.NewMovementDirection(moveDirection, this.currentTime);
        }

        private void startPlayerMovement(PlayerMovement newMove) {
            updatePlayerMovement(players[newMove.PeerID], newMove.MoveDirection);
        }

        public void stopGame() {
            this.GameThread.Abort();
        }
    }
}
