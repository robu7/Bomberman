using System;
using System.Drawing;

namespace BombermanGame.MapObjects
{
    class LocationResolvStatus
    {
        public GameObject CollisionObject { get; set; } = null;
        public Tile PrevTile { get; set; } = null;
        public bool ChangedTile { get; set; } = false;
        public bool HaveCollided { get; set; } = false;

        public LocationResolvStatus()
        {

        }
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

        public abstract LocationResolvStatus UpdateLocation(double currentTime, double delta);
    }


    /// <summary>
    /// 
    /// </summary>
    class FixedLocationResolver : LocationResolver
    {
        public FixedLocationResolver(Tile tile)
        {
            CurrentTile = tile;
            this.bounds = tile.Bounds;
        }

        public override LocationResolvStatus UpdateLocation(double currentTime, double delta)
        {
            return null;
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
        protected LocationResolvStatus status;

        public FloatingLocationResolver(Tile initialTile)
        {
            Console.WriteLine("New resolver");
            CurrentTile = initialTile;
            Direction = Game.Direction.None;
            this.bounds = new RectangleF(initialTile.Bounds.Location, Game.boxSize);
            this.centerPosition = new PointF(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
        }


        public override LocationResolvStatus UpdateLocation(double currentTime, double delta)
        {
            this.status = new LocationResolvStatus();

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
                        } else {
                            xDelta = Math.Min(CurrentTile.Bounds.Right - Bounds.Right, xDelta);
                        }
                    }
                    if (yDelta != 0) {
                        // Adjust y movement
                        if (yDelta < 0) {
                            yDelta = Math.Max(CurrentTile.Bounds.Top - Bounds.Top, yDelta);
                        } else {
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
                status.ChangedTile = true;
                status.PrevTile = CurrentTile;
                CurrentTile = nextTile;

            }

            if (!Velocity.IsEmpty) {
                AlignWithTileBounds();
            }

            return status;
        }

        protected void MoveBy(float xDelta, float yDelta)
        {
            this.centerPosition.X += xDelta;
            this.centerPosition.Y += yDelta;
            this.bounds.Offset(xDelta, yDelta);
        }

        private bool CanEnterTile(Tile tileToEnter)
        {
            if (tileToEnter == Tile.OutOfBounds) {
                return false;
            }

            if (tileToEnter.Object is Block || tileToEnter.Object is ConstBlock || tileToEnter.Object is Bomb) {
                status.CollisionObject = tileToEnter.Object;
                status.HaveCollided = true;
                return false;
            }

            return true;
        }

        private void AlignWithTileBounds()
        {
            switch (Direction) {
                case Game.Direction.East:
                case Game.Direction.West:
                    // Adjust vertical position
                    if (Bounds.Top < CurrentTile.Bounds.Top) {
                        MoveBy(0, CurrentTile.Bounds.Top - Bounds.Top);
                    } else if (Bounds.Bottom > CurrentTile.Bounds.Bottom) {
                        MoveBy(0, CurrentTile.Bounds.Bottom - Bounds.Bottom);
                    }
                    break;
                case Game.Direction.North:
                case Game.Direction.South:
                    // Adjust horizontal position
                    if (Bounds.Left < CurrentTile.Bounds.Left) {
                        MoveBy(CurrentTile.Bounds.Left - Bounds.Left, 0);
                    } else if (Bounds.Right > CurrentTile.Bounds.Right) {
                        MoveBy(CurrentTile.Bounds.Right - Bounds.Right, 0);
                    }
                    break;
            }
        }
    }
}
