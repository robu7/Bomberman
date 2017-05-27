using System.Drawing;

namespace BombermanGame {

    /// <summary>
    /// Represents a tile on the map. It provides bounds and accessors to it's neighbors.
    /// It may or may not contain an object.
    /// </summary>
    class Tile {

        public static readonly Tile OutOfBounds = new Tile();

        private RectangleF bounds;
        private FixedMapObject containedObject;

        public RectangleF Bounds {
            get {
                return this.bounds;
            }
            set {
                if (value == this.bounds) {
                    // Nothing changed
                    return;
                }
                this.bounds = value;
                MarkAsDirty();
            }
        }

        public FixedMapObject Object {
            get {
                return this.containedObject;
            }
            set {
                if (value == containedObject) {
                    // Nothing changed
                    return;
                }
                this.containedObject = value;
                if (this.containedObject != null) {
                    this.containedObject.AttachToTile(this);
                }
                MarkAsDirty();
            }
        }

        public Tile West { get; private set; } = OutOfBounds;
        public Tile North { get; private set; } = OutOfBounds;
        public Tile East { get; private set; } = OutOfBounds;
        public Tile South { get; private set; } = OutOfBounds;

        public bool IsChanged { get; private set; } = true;

        public Tile GetNextTileInDirection(Game.Direction direction) {
            switch (direction) {
                case Game.Direction.None: return this;
                case Game.Direction.North: return North;
                case Game.Direction.South: return South;
                case Game.Direction.West: return West;
                case Game.Direction.East: return East;
            }
            return null;
        }

        /// <summary>
        /// Signal that the tile should be redrawn
        /// </summary>
        public void MarkAsDirty() {
            IsChanged = true;
        }

        /// <summary>
        /// Connects neighbors towards the south and east.
        /// The connection is made both ways.
        /// </summary>
        public void ConnectNeighbors(Tile east, Tile south) {
            East = east;
            if (East != null) {
                East.West = this;
            }
            South = south;
            if (South != null) {
                South.North = this;
            }
        }

        public void Draw(Graphics g) {
            g.SetClip(this.bounds);
            Ground.Draw(g, this.bounds.Location);
            this.containedObject?.Draw(g);
            g.ResetClip();
            IsChanged = false;
        }
    }
}
