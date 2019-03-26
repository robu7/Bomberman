using System;
using System.Drawing;
using BombermanGame.GameObjects.Powerups;
using BombermanGame.GameObjects;
using System.Collections.Generic;

namespace BombermanGame {

    class FloatingObjectRegistry
    {
        private static List<GameObject> objectsInMotion = new List<GameObject>();

        public static void Register(GameObject movingObject)
        {
            objectsInMotion.Add(movingObject);
        }

        public static void UnRegister(GameObject movingObject)
        {
            if(objectsInMotion.Contains(movingObject))
                objectsInMotion.Remove(movingObject);
        }
        public static List<GameObject> GetMovingObjects() { return new List<GameObject>(objectsInMotion); }

        public static void RemoveDestroyedObjects()
        {
            objectsInMotion.RemoveAll(x => x.PendingDestroy);
        }
    }


    class Map {
        private Tile[,] tiles;

        public Map(GameObject[,] mapObjects) {
            var columns = mapObjects.GetLength(0);
            var rows = mapObjects.GetLength(1);
            Size = new Size(columns * Game.boxSize.Width, rows * Game.boxSize.Height);
            this.tiles = new Tile[columns, rows];
            // Create tiles that wrap the given objects...
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < columns; x++) {
                    var tile = new Tile(new Point(y,x)) {
                        Bounds = new RectangleF(x * Game.tileSize, y * Game.tileSize, Game.tileSize, Game.tileSize),
                        Object = mapObjects[x, y]
                    };
                    tiles[x, y] = tile;
                }
            }
            // ...and create a graph where each tile is a node with connections to its neighbors
            for (int y = 0; y < rows - 1; y++) {
                for (int x = 0; x < columns - 1; x++) {
                    tiles[x, y].ConnectNeighbors(tiles[x + 1, y], tiles[x, y + 1]);
                }
            }
        }

        public Size Size { get; }

        public Tile GetTileAt(PointF pos) {
            var x = (int)Math.Floor(pos.X / Game.tileSize);
            var y = (int)Math.Floor(pos.Y / Game.tileSize);
            return tiles[x, y];
        }

        public void Update(double totalTime, double delta) {
            foreach (var tile in this.tiles) {
                if (tile.IsChanged) {
                    // The tile has already been updated (e.g. as a side-effect of another tile update)
                    // No need to apply further updates
                    continue;
                }
                if (tile.Object == null) {
                    // No need to update when there is no contained game object
                    continue;
                }
                // Update the object inside the tile
                tile.Object.Update(totalTime);
            }
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget target) {
            List<GameObject> objectsToDraw = new List<GameObject>();
            foreach (var tile in this.tiles) {
                if (!tile.IsChanged) {
                    // The content within the tile has not changed
                    continue;
                }
                if(tile.Object != null)
                    objectsToDraw.Add(tile.Object);
                // Tile has been updated, redraw
                tile.Draw(target);
            }

            foreach (var item in objectsToDraw) {
                item.Draw(target);
            }
        }

        public static Map CreateDefault(int noPlayers) {
            var objects = new GameObject[11, 11];
            for (int x = 0; x < 11; x++) {
                for (int y = 0; y < 11; y++) {
                    if (x == 0 || x == 10 || y == 0 || y == 10) {
                        objects[x, y] = new ConstBlock();
                    } else if (x % 2 == 0 && y % 2 == 0) {
                        objects[x, y] = new ConstBlock();
                    } else {
                        objects[x, y] = new Block();
                    }
                }
            }

            objects[1, 1] = null;
            objects[1, 2] = null;
            objects[1, 3] = null;
            objects[2, 1] = null;
            objects[3, 1] = null;
            //objects[3, 1] = new BombRangePowerup(0);

            //GenerateMapPowerups(objects);

            return new Map(objects);
        }

        public static IEnumerable<KeyValuePair<Point, PowerupType>> GenerateMapPowerups(Map map)
        {
            foreach (var item in map.tiles) {
                if (item.Object is Block) {
                    Random rand = new Random(item.GetHashCode());
                    int randValue = rand.Next(0, 100);
                    if (randValue < 50) {
                        PowerupType newPowerUp = 0;

                        if (randValue < 20) {
                            (item.Object as Block).HiddenItem = new BombRangePowerup(0);
                            newPowerUp = PowerupType.ExtraPower;
                        } else if (randValue < 40) {
                            (item.Object as Block).HiddenItem = new ExtraBombPowerup(0);
                            newPowerUp = PowerupType.ExtraBomb;
                        } else {
                            (item.Object as Block).HiddenItem = new KickAbilityPowerup(0);
                            newPowerUp = PowerupType.KickAbility;
                        }

                        yield return new KeyValuePair<Point, PowerupType>(item.MapCoordinate, newPowerUp);
                    }
                }
            }
            yield break;
        }
    }
}
