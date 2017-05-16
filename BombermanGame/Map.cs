﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BombermanGame
{
    interface ITimedMapObject : IUpdateable
    {
        bool Finished { get; }
        Point MapPos { get; }
    }

    class Map
    {
        public Stack<MapObject>[,] mapObjects = new Stack<MapObject>[11, 11];
        private MapObject[,] map = new MapObject[11, 11];
        private List<ITimedMapObject> itemsToUpdate;
        //private Ground ground;

        public Map(int numPlayers = 1) {
            initMap();
            itemsToUpdate = new List<ITimedMapObject>();
            //ground = new Ground(new PointF(0,0));
        }

        public MapObject[,] getMap() { return map; }

        public List<MapObject> getTopLayer() {
            List<MapObject> topLayer = new List<MapObject>();
            MapObject topElem;
            lock (mapObjects) {
                for (int x = 0; x < 11; ++x) {
                    for (int y = 0; y < 11; ++y) {
                        topElem = mapObjects[x, y].Peek();
                        if (topElem != null)
                            topLayer.Add(mapObjects[x, y].Peek());
                    }
                }
            }
            return topLayer;
        }

        private void initMap() {

            for (int x = 0; x < 11; x++) {
                for (int y = 0; y < 11; y++) {
                    map[x, y] = new Ground(new PointF(x * Game.tileSize, y * Game.tileSize));
                    if (x == 0 || x == 10 || y == 0 || y == 10)
                        map[x, y] = new ConstBlock(new PointF(x * Game.tileSize, y * Game.tileSize));

                    else if (x % 2 == 0 && y % 2 == 0)
                        map[x, y] = new ConstBlock(new PointF(x * Game.tileSize, y * Game.tileSize));
                    //mapObjects[x, y] = MapObject.ConstBlock;
                    else
                        map[x, y] = new Block(new PointF(x * Game.tileSize, y * Game.tileSize));
                    //mapObjects[x, y] = MapObject.Block;   
                }
            }

            map[1, 1] = new Ground(new PointF(1 * Game.tileSize, 1 * Game.tileSize));
            map[1, 2] = new Ground(new PointF(1 * Game.tileSize, 2 * Game.tileSize));
            map[1, 3] = new Ground(new PointF(1 * Game.tileSize, 3 * Game.tileSize));
            map[2, 1] = new Ground(new PointF(2 * Game.tileSize, 1 * Game.tileSize));
            map[3, 1] = new Ground(new PointF(3 * Game.tileSize, 1 * Game.tileSize));
            //mapObjects[3, 1].Push(new Fire(new PointF(3 * Game.tileSize, 1 * Game.tileSize)));
        }

        public MapObject getElem(Point objectPos) {
            return map[objectPos.X, objectPos.Y];
        }

        public void addObject(MapObject newObj, Point pos) {
            Console.WriteLine("{0} Object added at: {1}", newObj, pos);
            map[pos.X, pos.Y] = (newObj);
        }

        public void addFire(Fire fire, Point pos) {
            //Console.WriteLine("Fire added at: {0}", pos);
            if (map[pos.X, pos.Y] is Fire) {
                itemsToUpdate.RemoveAll(item => item.MapPos == pos);
            }
            map[pos.X, pos.Y] = fire;
            itemsToUpdate.Add(fire);
        }

        public void destroyObject(Point pos) {
            map[pos.X, pos.Y] = new Ground(new PointF(pos.X * Game.tileSize, pos.Y * Game.tileSize));
        }

        public void updateAll(double tick, double totalTime) {
            foreach (var item in itemsToUpdate) {
                item.update(tick, totalTime);
                if(item is ITimedMapObject) {
                    if(item.Finished) {
                        destroyObject(item.MapPos);
                    }
                }
            }
            itemsToUpdate.RemoveAll(item => item.Finished);
        }

    }
}
