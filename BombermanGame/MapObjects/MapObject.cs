using System;
using System.Drawing;

namespace BombermanGame {

    /// <summary>
    /// 
    /// </summary>
    abstract class GameObject
    {
        private LocationResolver location;
        protected LocationResolver LocationResolver {
            get {
                return location;
            }
            set {
                if (this.location != null) {
                    // Clean up
                }
                this.location = value;
            }
        }
        public Tile Tile => LocationResolver?.CurrentTile;

        public void AttachToTile(Tile mapTile) {
            LocationResolver = new FixedLocationResolver(mapTile);
        }

        public bool IsDescructible { get; protected set; } = true;
        public void Destroy(double currentTime) {
            if (!IsDescructible) {
                return;
            }
            OnDestroy(currentTime);
        }
        protected virtual void OnDestroy(double currentTime) {
            if (LocationResolver?.CurrentTile?.Object != this) {
                return;
            }
            LocationResolver.CurrentTile.Object = null;
            LocationResolver = null;
        }

        public abstract void Update(double totalTime);
        public abstract void Draw(SharpDX.Direct2D1.RenderTarget target);
    }

    /// <summary>
    /// 
    /// </summary>
    abstract class LocationResolver
    {
        public Tile CurrentTile { get; protected set; }
        // Need to use a field to be able to update in place
        protected RectangleF bounds;
        public RectangleF Bounds => bounds;

        public abstract void UpdateLocation(double currentTime, double delta);
    }


    /// <summary>
    /// 
    /// </summary>
    class FixedLocationResolver : LocationResolver
    {
        public FixedLocationResolver(Tile tile) {
            CurrentTile = tile;
            this.bounds = tile.Bounds;
        }

        public override void UpdateLocation(double currentTime, double delta) {
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class FloatingLocationResolver : LocationResolver
    {
        public PointF Velocity { get; set; }
        public Game.Direction Direction { get; set; }
        protected PointF centerPosition;

        public FloatingLocationResolver(Tile initialTile) {
            CurrentTile = initialTile;
            Direction = Game.Direction.None;
            this.bounds = new RectangleF(initialTile.Bounds.Location, Game.boxSize);
            this.centerPosition = new PointF(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
        }


        public override void UpdateLocation(double currentTime, double delta) {
            var xDelta = Velocity.X * (float)delta;
            var yDelta = Velocity.Y * (float)delta;

            var nextTile = CurrentTile.GetNextTileInDirection(Direction);

            var newBounds = new RectangleF(new PointF(Bounds.Left + xDelta, Bounds.Top + yDelta), Bounds.Size);

            if (!nextTile.Bounds.IntersectsWith(Bounds) && nextTile.Bounds.IntersectsWith(newBounds)) {
                // Entering a new tile
                if (!CanEnterTile(nextTile)) {
                    // Not allowed to enter tile, constrain the movement
                    if (xDelta != 0) {
                        // Adjust x movement
                        if (xDelta < 0) {
                            xDelta = Math.Max(CurrentTile.Bounds.Left - Bounds.Left, xDelta);
                        }
                        else {
                            xDelta = Math.Min(CurrentTile.Bounds.Right - Bounds.Right, xDelta);
                        }
                    }
                    if (yDelta != 0) {
                        // Adjust y movement
                        if (yDelta < 0) {
                            yDelta = Math.Max(CurrentTile.Bounds.Top - Bounds.Top, yDelta);
                        }
                        else {
                            yDelta = Math.Min(CurrentTile.Bounds.Bottom - Bounds.Bottom, yDelta);
                        }
                    }
                    Velocity = new PointF(0, 0);
                }
            }

            MoveBy(xDelta, yDelta);

            // Mark tiles around the player as dirty
            CurrentTile.MarkAsDirty();
            CurrentTile.West.MarkAsDirty();
            CurrentTile.North.MarkAsDirty();
            CurrentTile.East.MarkAsDirty();
            CurrentTile.South.MarkAsDirty();

            if (!CurrentTile.Bounds.Contains(this.centerPosition) && nextTile.Bounds.Contains(this.centerPosition)) {
                // Switched current tile
                CurrentTile = nextTile;
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
                    if (Bounds.Top < CurrentTile.Bounds.Top) {
                        MoveBy(0, CurrentTile.Bounds.Top - Bounds.Top);
                    }
                    else if (Bounds.Bottom > CurrentTile.Bounds.Bottom) {
                        MoveBy(0, CurrentTile.Bounds.Bottom - Bounds.Bottom);
                    }
                    break;
                case Game.Direction.North:
                case Game.Direction.South:
                    // Adjust horizontal position
                    if (Bounds.Left < CurrentTile.Bounds.Left) {
                        MoveBy(CurrentTile.Bounds.Left - Bounds.Left, 0);
                    }
                    else if (Bounds.Right > CurrentTile.Bounds.Right) {
                        MoveBy(CurrentTile.Bounds.Right - Bounds.Right, 0);
                    }
                    break;
            }
        }
    }
}
