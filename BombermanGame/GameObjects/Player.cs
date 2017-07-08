using System;
using System.Drawing;
using System.Timers;
using BombermanGame.Animations;
using BombermanGame.GameObjects.Powerups;

namespace BombermanGame.GameObjects
{
    class Player : GameObject {
        private bool alive;
        private bool draw;
        private double lastUpdateTime;
        private Animation spriteAnimation;
        private FloatingLocationResolver movement;

        public Player(Tile startTile) {
            draw = true;
            alive = true;
            BombCap = 1;
            BombRange = 1;
            CanKick = false;
            this.movement = new FloatingLocationResolver(startTile);
            LocationResolver = this.movement;
        }

        public void Init() {
            spriteAnimation = PlayerAnimations.GetWalkAnimation(Game.Direction.South, 0.6);
        }

        public int BombCap { get; set; }
        public int BombRange { get; set; }
        public bool CanKick { get; set; }

        public bool isAlive() { return alive; }
        public bool ShouldDraw() { return draw; }
        private void StopDrawing() { draw = false; }

        public void startAnimating(Game.Direction direction, double currentTime) {
            this.spriteAnimation = PlayerAnimations.GetWalkAnimation(direction, 0.6);
            this.spriteAnimation.Start(currentTime);
        }
        public void StopAnimating() { spriteAnimation.Stop(); }
        
        public override void Update(double currentTime){
            var timeDelta = currentTime - this.lastUpdateTime;

            if (!alive) {
                if(spriteAnimation?.State == AnimationState.Stopped) {
                    StopDrawing();
                    StopAnimating();
                    //this.alive = false;
                    this.PendingDestroy = true;
                    // Remove player
                }

                Tile.MarkAsDirty();
                Tile.South.MarkAsDirty();
                Tile.North.MarkAsDirty();
                Tile.East.MarkAsDirty();
                Tile.West.MarkAsDirty();
                spriteAnimation?.Update(currentTime);
                return;
            }

            var status = this.movement.UpdateLocation(currentTime, timeDelta);
            
            if(status.HaveCollided && CanKick && status.CollisionObject is Bomb) {
                (status.CollisionObject as Bomb).Kicked(this.movement.Direction);
                Console.WriteLine("Player Kicked a Bomb");
            }

            InteractWithTileContent(this.movement.CurrentTile, currentTime);

            spriteAnimation?.Update(currentTime);
            this.lastUpdateTime = currentTime;
        }

        private void InteractWithTileContent(Tile enteredTile, double currentTime) {
            var tileObject = enteredTile.Object;
            if (tileObject is Fire) {
                Destroy(currentTime);
            } else if (tileObject is Powerup) {
                (tileObject as Powerup).ApplyAndConsume(this);
            }
        }

        public override void Draw(SharpDX.Direct2D1.RenderTarget target) {
            var sprite = this.spriteAnimation?.CurrentFrame;
            if (sprite == null) {
                return;
            }
            var b = this.movement.Bounds;
            target.DrawBitmap(sprite, new SharpDX.Mathematics.Interop.RawRectangleF(b.Left, b.Top, b.Right, b.Bottom), 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public void NewMovementDirection(Game.Direction newDirection, double currentTime) {
            if (newDirection == this.movement.Direction) {
                // No need to update when direction is the same
                return;
            }

            // Update direction
            this.movement.Direction = newDirection;

            // Update velocity
            switch (newDirection) {
                case Game.Direction.North:
                    this.movement.Velocity = new PointF(0, -225);
                    break;
                case Game.Direction.South:
                    this.movement.Velocity = new PointF(0, 225);
                    break;
                case Game.Direction.West:
                    this.movement.Velocity = new PointF(-225, 0);
                    break;
                case Game.Direction.East:
                    this.movement.Velocity = new PointF(225, 0);
                    break;
                case Game.Direction.None:
                    this.movement.Velocity = new PointF(0, 0);
                    break;
            }

            // Update animation
            if (newDirection == Game.Direction.None) {
                StopAnimating();
            } else {
                startAnimating(newDirection, currentTime);
            }
        }

        public void DeployBomb() {
            //Console.WriteLine("Bomb deployed at: {0} by player: {1}", player.getMapPosition(), players.First(x => x.Value == player).Key);
            if (BombCap == 0) {
                return;
            }
            if (this.movement.CurrentTile.Object as Bomb != null) {
                // There is already a bomb at this location
                return;
            }

            // Preconditions ok, deploy the bomb
            BombCap--;
            Bomb newBomb = new Bomb(this, this.lastUpdateTime);
            this.movement.CurrentTile.Object = newBomb;
        }

        protected override void OnDestroy(double currentTime) {
            this.alive = false;
            //this.PendingDestroy = true;
            Console.WriteLine("Destroy player ");
            this.spriteAnimation = PlayerAnimations.GetDeathAnimation(1);
            this.spriteAnimation.Start(currentTime);
        }
    }
}
