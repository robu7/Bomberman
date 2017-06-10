using System;
using System.Drawing;

namespace BombermanGame {

    /// <summary>
    /// 
    /// </summary>
    abstract class GameObject
    {
        public Tile mapTile { get; set; }
        protected MovementManager objectMovement;

        public GameObject(Tile mapTile) {
            this.mapTile = mapTile;
        }

        public void AttachToTile(Tile mapTile) {
            this.mapTile = mapTile;
        }

        public bool IsDescructible { get; }
        public void Destroy(double currentTime) {
            if (!IsDescructible) {
                return;
            }
            OnDestroy(currentTime);
        }
        protected virtual void OnDestroy(double currentTime) {
            if (this.mapTile == null) {
                return;
            }
            this.mapTile.Object = null;
            this.mapTile = null;
        }

        public abstract void Update(double totalTime);
        public abstract void Draw(SharpDX.Direct2D1.RenderTarget target);
    }

    /// <summary>
    /// 
    /// </summary>
    abstract class MovementManager
    {
        protected GameObject owner { get; }
        protected RectangleF bounds;
        public RectangleF Bounds { get { return bounds; } }
        protected PointF centerPosition;
        public PointF Velocity { get; set; }
        public Game.Direction Direction { get; set; }

        public MovementManager(GameObject owner) {
            this.owner = owner;
            this.bounds = new RectangleF(owner.mapTile.Bounds.Location, Game.boxSize);
            this.centerPosition = new PointF(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
        }

        public abstract void UpdateMovement(double currentTime, double delta);
    }


    /// <summary>
    /// 
    /// </summary>
    class FixedMovement : MovementManager
    {
        public FixedMovement(GameObject owner) : base(owner) {

        }

        public override void UpdateMovement(double currentTime, double delta) {
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class FloatingMovement : MovementManager
    {
        //public PointF Velocity { get; set; }
        //public Game.Direction Direction { get; set; }

        public FloatingMovement(GameObject owner) : base(owner) {
            //this.currentTile = startLocationTile;
            Direction = Game.Direction.None;
        }


        public override void UpdateMovement(double currentTime, double delta) {
            var xDelta = Velocity.X * (float)delta;
            var yDelta = Velocity.Y * (float)delta;

            var nextTile = owner.mapTile.GetNextTileInDirection(Direction);

            var newBounds = new RectangleF(new PointF(this.bounds.Left + xDelta, this.bounds.Top + yDelta), this.bounds.Size);

            if (!nextTile.Bounds.IntersectsWith(this.bounds) && nextTile.Bounds.IntersectsWith(newBounds)) {
                // Entering a new tile
                if (!CanEnterTile(nextTile)) {
                    // Not allowed to enter tile, constrain the movement
                    if (xDelta != 0) {
                        // Adjust x movement
                        if (xDelta < 0) {
                            xDelta = Math.Max(owner.mapTile.Bounds.Left - this.bounds.Left, xDelta);
                        }
                        else {
                            xDelta = Math.Min(owner.mapTile.Bounds.Right - this.bounds.Right, xDelta);
                        }
                    }
                    if (yDelta != 0) {
                        // Adjust y movement
                        if (yDelta < 0) {
                            yDelta = Math.Max(owner.mapTile.Bounds.Top - this.bounds.Top, yDelta);
                        }
                        else {
                            yDelta = Math.Min(owner.mapTile.Bounds.Bottom - this.bounds.Bottom, yDelta);
                        }
                    }
                    Velocity = new PointF(0, 0);
                }
            }

            MoveBy(xDelta, yDelta);

            // Mark tiles around the player as dirty
            owner.mapTile.MarkAsDirty();
            owner.mapTile.West.MarkAsDirty();
            owner.mapTile.North.MarkAsDirty();
            owner.mapTile.East.MarkAsDirty();
            owner.mapTile.South.MarkAsDirty();

            if (!owner.mapTile.Bounds.Contains(this.centerPosition) && nextTile.Bounds.Contains(this.centerPosition)) {
                // Switched current tile
                owner.mapTile = nextTile;
            }

            if (!Velocity.IsEmpty) {
                AlignWithTileBounds();
            }
        }

        protected void MoveBy(float xDelta, float yDelta) {
            this.centerPosition.X += xDelta;
            this.centerPosition.Y += yDelta;
            this.bounds.Offset(xDelta, yDelta);
        }

        private bool CanEnterTile(Tile tileToEnter) {
            if (tileToEnter == Tile.OutOfBounds) {
                return false;
            }

            if (tileToEnter.Object is Block || tileToEnter.Object is ConstBlock || tileToEnter.Object is Bomb) {
                return false;
            }

            return true;
        }

        private void AlignWithTileBounds() {
            switch (Direction) {
                case Game.Direction.East:
                case Game.Direction.West:
                    // Adjust vertical position
                    if (this.bounds.Top < owner.mapTile.Bounds.Top) {
                        MoveBy(0, owner.mapTile.Bounds.Top - this.bounds.Top);
                    }
                    else if (this.bounds.Bottom > owner.mapTile.Bounds.Bottom) {
                        MoveBy(0, owner.mapTile.Bounds.Bottom - this.bounds.Bottom);
                    }
                    break;
                case Game.Direction.North:
                case Game.Direction.South:
                    // Adjust horizontal position
                    if (this.bounds.Left < owner.mapTile.Bounds.Left) {
                        MoveBy(owner.mapTile.Bounds.Left - this.bounds.Left, 0);
                    }
                    else if (this.bounds.Right > owner.mapTile.Bounds.Right) {
                        MoveBy(owner.mapTile.Bounds.Right - this.bounds.Right, 0);
                    }
                    break;
            }
        }

    }








    /// <summary>
    /// An object that is bound to a tile on the map
    /// </summary>
    abstract class FixedMapObject {
        protected Tile mapTile;

        public FixedMapObject(bool destructible = true) {
            IsDescructible = destructible;
        }

        public void AttachToTile(Tile mapTile) {
            this.mapTile = mapTile;
        }

        public bool IsDescructible { get; }
        public void Destroy(double currentTime) {
            if (!IsDescructible) {
                return;
            }
            OnDestroy(currentTime);
        }
        protected virtual void OnDestroy(double currentTime) {
            if (this.mapTile == null) {
                return;
            }
            this.mapTile.Object = null;
            this.mapTile = null;
        }

        public abstract void Update(double totalTime);
        public abstract void Draw(SharpDX.Direct2D1.RenderTarget target);
    }

    /// <summary>
    /// An object that is not positioned in a specific tile, but may move around on the map
    /// </summary>
    abstract class FloatingObject {

        protected Tile currentTile;
        protected RectangleF bounds;
        protected PointF centerPosition;

        protected FloatingObject(Tile startLocationTile, Size size) {
            this.currentTile = startLocationTile;
            this.bounds = new RectangleF(this.currentTile.Bounds.Location, size);
            this.centerPosition = new PointF(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
            Direction = Game.Direction.None;
        }

        protected void MoveBy(float xDelta, float yDelta) {
            this.centerPosition.X += xDelta;
            this.centerPosition.Y += yDelta;
            this.bounds.Offset(xDelta, yDelta);
        }

        public abstract void Update(double totalTime);
        public abstract void Draw(SharpDX.Direct2D1.RenderTarget target);

        public PointF Velocity { get; set; }
        public Game.Direction Direction { get; set; }
    }
}
