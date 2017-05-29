using System;
using System.Drawing;
using System.Timers;
using BombermanGame.Animations;
using BombermanGame.Powerups;

namespace BombermanGame
{
    class Player : FloatingObject {
        private bool alive;
        private bool draw;
        private double lastUpdateTime;
        private Animation spriteAnimation;

        public Player(Tile startTile) : base(startTile, Game.boxSize) {
            draw = true;
            alive = true;
            BombCap = 4;
            BombRange = 1;
        }

        public void Init() {
            spriteAnimation = PlayerAnimations.GetWalkAnimation(Game.Direction.South, 0.6);
        }

        public int BombCap { get; set; }
        public int BombRange { get; set; }

        public bool isAlive() { return alive; }
        public bool ShouldDraw() { return draw; }
        private void stopDrawing() { draw = false; }

        public void startAnimating(Game.Direction direction, double currentTime) {
            this.spriteAnimation = PlayerAnimations.GetWalkAnimation(direction, 0.6);
            this.spriteAnimation.Start(currentTime);
        }
        public void stopAnimating() { spriteAnimation.Stop(); }

        private Tile debugNextTile;
        public override void Update(double currentTime) {
            var timeDelta = currentTime - this.lastUpdateTime;
            var previousBounds = this.bounds;

            var xDelta = Velocity.X * (float)timeDelta;
            var yDelta = Velocity.Y * (float)timeDelta;

            var nextTile = this.currentTile.GetNextTileInDirection(Direction);
            debugNextTile = nextTile;
            var newBounds = new RectangleF(new PointF(this.bounds.Left + xDelta, this.bounds.Top + yDelta), this.bounds.Size);
            if (!nextTile.Bounds.IntersectsWith(this.bounds) && nextTile.Bounds.IntersectsWith(newBounds)) {
                // Entering a new tile
                if (!CanEnterTile(nextTile)) {
                    // Not allowed to enter tile, constrain the movement
                    if (xDelta != 0) {
                        // Adjust x movement
                        if (xDelta < 0) {
                            xDelta = Math.Max(this.currentTile.Bounds.Left - this.bounds.Left, xDelta);
                        } else {
                            xDelta = Math.Min(this.currentTile.Bounds.Right - this.bounds.Right, xDelta);
                        }
                    }
                    if (yDelta != 0) {
                        // Adjust y movement
                        if (yDelta < 0) {
                            yDelta = Math.Max(this.currentTile.Bounds.Top - this.bounds.Top, yDelta);
                        } else {
                            yDelta = Math.Min(this.currentTile.Bounds.Bottom - this.bounds.Bottom, yDelta);
                        }
                    }
                    Velocity = new PointF(0, 0);
                }
            }

            MoveBy(xDelta, yDelta);

            // Mark tiles around the player as dirty
            this.currentTile.MarkAsDirty();
            this.currentTile.West.MarkAsDirty();
            this.currentTile.North.MarkAsDirty();
            this.currentTile.East.MarkAsDirty();
            this.currentTile.South.MarkAsDirty();

            if (!this.currentTile.Bounds.Contains(this.centerPosition) && nextTile.Bounds.Contains(this.centerPosition)) {
                // Switched current tile
                this.currentTile = nextTile;
            }

            if (!Velocity.IsEmpty) {
                AlignWithTileBounds();
            }

            InteractWithTileContent(this.currentTile);

            spriteAnimation?.Update(currentTime);
            this.lastUpdateTime = currentTime;
        }

        private bool CanEnterTile(Tile tileToEnter) {
            if (tileToEnter == Tile.OutOfBounds) {
                return false;
            }

            if (tileToEnter.Object is Block || tileToEnter.Object is ConstBlock) {
                return false;
            }

            return true;
        }

        private void AlignWithTileBounds() {
            switch (Direction) {
                case Game.Direction.East:
                case Game.Direction.West:
                    // Adjust vertical position
                    if (this.bounds.Top < this.currentTile.Bounds.Top) {
                        MoveBy(0, this.currentTile.Bounds.Top - this.bounds.Top);
                    } else if(this.bounds.Bottom > this.currentTile.Bounds.Bottom) {
                        MoveBy(0, this.currentTile.Bounds.Bottom - this.bounds.Bottom);
                    }
                    break;
                case Game.Direction.North:
                case Game.Direction.South:
                    // Adjust horizontal position
                    if (this.bounds.Left < this.currentTile.Bounds.Left) {
                        MoveBy(this.currentTile.Bounds.Left - this.bounds.Left, 0);
                    } else if (this.bounds.Right > this.currentTile.Bounds.Right) {
                        MoveBy(this.currentTile.Bounds.Right - this.bounds.Right, 0);
                    }
                    break;
            }
        }

        private void InteractWithTileContent(Tile enteredTile) {
            var tileObject = enteredTile.Object;
            if (tileObject is Fire) {
                destroy();
            } else if (tileObject is Powerup) {
                (tileObject as Powerup).ApplyAndConsume(this);
            }
        }

        public override void Draw(SharpDX.Direct2D1.RenderTarget target) {
            var sprite = this.spriteAnimation?.CurrentFrame;
            if (sprite == null) {
                return;
            }
            var b = this.bounds;
            target.DrawBitmap(sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public void updateMovement(Game.Direction updatedDirection, double currentTime) {
            if (updatedDirection == Direction) {
                // No need to update when direction is the same
                return;
            }

            // Update direction
            Direction = updatedDirection;

            // Update velocity
            switch (updatedDirection) {
                case Game.Direction.North:
                    Velocity = new PointF(0, -225);
                    break;
                case Game.Direction.South:
                    Velocity = new PointF(0, 225);
                    break;
                case Game.Direction.West:
                    Velocity = new PointF(-225, 0);
                    break;
                case Game.Direction.East:
                    Velocity = new PointF(225, 0);
                    break;
                case Game.Direction.None:
                    Velocity = new PointF(0, 0);
                    break;
            }

            // Update animation
            if (updatedDirection == Game.Direction.None) {
                stopAnimating();
            } else {
                startAnimating(updatedDirection, currentTime);
            }
        }

        public void DeployBomb() {
            //Console.WriteLine("Bomb deployed at: {0} by player: {1}", player.getMapPosition(), players.First(x => x.Value == player).Key);
            if (BombCap == 0) {
                return;
            }
            if (this.currentTile?.Object as Bomb != null) {
                // There is already a bomb at this location
                return;
            }

            // Preconditions ok, deploy the bomb
            BombCap--;
            Bomb newBomb = new Bomb(this, this.lastUpdateTime);
            this.currentTile.Object = newBomb;
        }

        public void destroy() {
            alive = false;
            Console.WriteLine("Destroy player ");
            this.spriteAnimation = PlayerAnimations.GetDeathAnimation(1);
            Timer deathTimer = new Timer(1200);
            deathTimer.AutoReset = false;
            deathTimer.Elapsed += delegate { stopDrawing(); };
            deathTimer.Start();
        }
    }
}
