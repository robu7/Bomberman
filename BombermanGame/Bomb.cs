using System.Drawing;

namespace BombermanGame
{
    class Bomb : FixedMapObject {
        
        private static Bitmap sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);

        private Player owner;
        private double creationTime;
        private readonly double timeToLive = 2.5;
        private readonly int range;
        public bool HasExploded { get; private set; } = false;

        public Bomb(Player owner, double creationTime) {
            this.owner = owner;
            this.creationTime = creationTime;
            this.range = owner.BombRange;
        }

        protected override void OnDestroy(double currentTime) {
            this.mapTile.Object = null;
            Explode(currentTime);
            this.mapTile = null;
        }

        public override void Update(double currentTime) {
            if (HasExploded) {
                return;
            }
            if (currentTime - creationTime >= timeToLive) {
                Explode(currentTime);
            }      
        }

        private void Explode(double currentTime) {
            //Console.WriteLine("Bomb exploded at: {0}", bomb.getMapPosition());

            this.mapTile.Object = new Fire(FireType.Center, currentTime);
            
            ExplodeInDirection(Game.Direction.North, FireType.Up, currentTime);
            ExplodeInDirection(Game.Direction.South, FireType.Down, currentTime);
            ExplodeInDirection(Game.Direction.West, FireType.Left, currentTime);
            ExplodeInDirection(Game.Direction.East, FireType.Right, currentTime);

            HasExploded = true;
            this.owner.BombCap++;
        }
        
        private void ExplodeInDirection(Game.Direction direction, FireType fireType, double currentTime) {
            FireType connectionType = (fireType == FireType.Up || fireType == FireType.Down) ? FireType.Vertical : FireType.Horizontal;

            var remainingRange = this.range;
            var tile = this.mapTile;
            while(remainingRange > 0) {
                tile = tile.GetNextTileInDirection(direction);
                remainingRange--;
                if (tile == Tile.OutOfBounds) {
                    break;
                }

                var elem = tile.Object;
                if (elem == null) {
                    if (remainingRange == 0) {
                        tile.Object = new Fire(fireType, currentTime);
                    } else {
                        tile.Object = new Fire(connectionType, currentTime);
                    }
                } else if (elem.IsDescructible) {
                    elem.Destroy(currentTime);
                    if (tile.Object?.IsDescructible ?? true) {
                        tile.Object = new Fire(fireType, currentTime);
                    }
                    break;
                } else
                    break;
            }
        }

        public override void Draw(Graphics g) {
            g.DrawImage(sprite, this.mapTile.Bounds.Location);
        }

        public override string ToString() {
            return "Bomb";
        }
    }
}
