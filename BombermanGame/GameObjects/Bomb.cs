﻿using System.Drawing;

namespace BombermanGame.GameObjects {
    class Bomb : GameObject {
        
        private BitmapLoader loader = new BombGraphicsLoader();

        private Player owner;
        private double creationTime;
        private double lastTime;
        private readonly double timeToLive = 2.5;
        private readonly int range;
        public bool HasExploded { get; private set; } = false;

        public Bomb(Player owner, double creationTime) {
            this.owner = owner;
            this.creationTime = creationTime;
            this.range = owner.BombRange;
            this.LocationResolver = new FixedLocationResolver(owner.Tile);
        }

        protected override void OnDestroy(double currentTime) {
            Explode(currentTime);
            base.OnDestroy(currentTime);
        }

        public override void Update(double currentTime) {
            if (HasExploded) {
                return;
            }
            if (currentTime - creationTime >= timeToLive) {
                Explode(currentTime);
            }
            var status = LocationResolver.UpdateLocation(currentTime, currentTime - lastTime);
            
            if(status != null)
                if(status.HaveCollided) {
                    Tile.Object = this;
                    LocationResolver = new FixedLocationResolver(Tile);
                    FloatingObjectRegistry.UnRegister(this);
                    if(status.CollisionObject is Bomb) {
                        (status.CollisionObject as Bomb).Kicked(status.CollisionDirection);
                    }
                }

            lastTime = currentTime;
        }

        public void Kicked(Game.Direction kickedFromSide)
        {
            System.Console.WriteLine("Kicked from direction: " + kickedFromSide);

            if (!(this.LocationResolver is FloatingLocationResolver))
                this.LocationResolver = new FloatingLocationResolver(this.Tile);

            var resolver = this.LocationResolver as FloatingLocationResolver;

            switch (kickedFromSide) {
                case Game.Direction.East:
                    resolver.Direction = Game.Direction.East;
                    resolver.Velocity = new PointF(300, 0);
                    break;
                case Game.Direction.North:
                    resolver.Direction = Game.Direction.North;
                    resolver.Velocity = new PointF(0, -300);
                    break;
                case Game.Direction.South:
                    resolver.Direction = Game.Direction.South;
                    resolver.Velocity = new PointF(0, 300);
                    break;
                case Game.Direction.West:
                    resolver.Direction = Game.Direction.West;
                    resolver.Velocity = new PointF(-300, 0);
                    break;
            }

            Tile.Object = null;
            FloatingObjectRegistry.Register(this);
        }

        private void Explode(double currentTime) {
            //Console.WriteLine("Bomb exploded at: {0}", bomb.getMapPosition());
            if (HasExploded)
                return;

            HasExploded = true;
            PendingDestroy = true;

            Tile.Object = new Fire(FireType.Center, currentTime);
            
            ExplodeInDirection(Game.Direction.North, FireType.Up, currentTime);
            ExplodeInDirection(Game.Direction.South, FireType.Down, currentTime);
            ExplodeInDirection(Game.Direction.West, FireType.Left, currentTime);
            ExplodeInDirection(Game.Direction.East, FireType.Right, currentTime);

            this.owner.BombCap++;
        }
        
        private void ExplodeInDirection(Game.Direction direction, FireType fireType, double currentTime) {
            FireType connectionType = (fireType == FireType.Up || fireType == FireType.Down) ? FireType.Vertical : FireType.Horizontal;

            var remainingRange = this.range;
            var tile = this.Tile;
            while(remainingRange > 0) {
                tile = tile.GetNextTileInDirection(direction);
                remainingRange--;
                if (tile == Tile.OutOfBounds) {
                    break;
                }

                foreach (var item in FloatingObjectRegistry.GetMovingObjects()) {
                    if(item.IsDescructible)
                        if (item.Tile == tile) {
                            item.Destroy(currentTime);
                        }
                }

                var elem = tile.Object;
                if (elem == null) {
                    if (remainingRange == 0) {
                        tile.Object = new Fire(fireType, currentTime);
                    } else {
                        tile.Object = new Fire(connectionType, currentTime);
                    }
                }  else if (elem.IsDescructible) {
                    elem.Destroy(currentTime);

                    if (tile.Object == null) {
                        tile.Object = new Fire(fireType, currentTime);
                    }

                    //if (tile.Object?.IsDescructible ?? true) {
                    //    tile.Object = new Fire(fireType, currentTime);
                    //}
                    break;
                } else if (elem is Fire) {
                    var elemStart = (elem as Fire).GetStartTime();
                    if (elemStart < currentTime) {
                        elem.Destroy(currentTime);
                        tile.Object = new Fire(remainingRange == 0 ? fireType : connectionType, currentTime);
                    } else
                        break;

                } else
                    break;
            }
        }

        public override void Draw(SharpDX.Direct2D1.RenderTarget target) {
            var b = LocationResolver.Bounds;

            target.DrawBitmap(loader.Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public override string ToString() {
            return "Bomb";
        }
    }

    class BombGraphicsLoader : BitmapLoader {
        private static SharpDX.Direct2D1.Bitmap sprite;
        public BombGraphicsLoader() : base(Properties.Resources.bomb, s => sprite = s, () => sprite) { }
    }
}
