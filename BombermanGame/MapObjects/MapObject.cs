using System.Drawing;

namespace BombermanGame {
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
