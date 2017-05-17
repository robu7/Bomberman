using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;


namespace BombermanGame
{
    class Player : MoveableObject, IDestroyable
    {
        private bool alive;
        private bool draw;
        private int bombCap;
        private int range;
        private Animation spriteAnimation;

        public Player(PointF position) : base(position) { 
            direction = Game.Direction.None;
            spriteAnimation = new PlayerAnimation(Properties.Resources.blue);
            draw = true;
            alive = true;
            bombCap = 4;
            range = 2;
        }


        public int getBombCap() { return bombCap; }
        public int getBombRange() { return range; }
        public void incBombCap() { ++bombCap; }
        public void decBombCap() { --bombCap; }

        public bool isAlive() { return alive; }
        public bool shouldDraw() { return draw; }
        private void stopDrawing() { draw = false; }

        public void startAnimating(Game.Direction direction) { spriteAnimation.start(direction, 0); }
        public void stopAnimating() { spriteAnimation.stop(); }

        public override Bitmap getSprite() { return spriteAnimation.getFrame(); }

        public override void update(double tick) {
            mapPosition.X = (int)Math.Round(position.X / Game.tileSize);
            mapPosition.Y = (int)Math.Round(position.Y / Game.tileSize);

            position.X += velocity.X * (float)tick;
            position.Y += velocity.Y * (float)tick;
            hitbox.Location = position;

            spriteAnimation.update(tick, 0);
            //hitbox = new RectangleF(position, new SizeF(Game.tileSize, Game.tileSize));
            //direction = Game.Direction.None;
        }

        public override void collide() {
            hitbox.Location = new PointF(mapPosition.X * Game.tileSize, mapPosition.Y * Game.tileSize);
            switch (direction) {
                case Game.Direction.Up:
                case Game.Direction.Down:
                    position.Y = mapPosition.Y * Game.tileSize;
                    break;
                case Game.Direction.Left:
                case Game.Direction.Right:
                    position.X = mapPosition.X * Game.tileSize;
                    break;
            }
            velocity.X = 0;
            velocity.Y = 0;
            direction = Game.Direction.None;
            stopAnimating();

        }

        public void smoothAdjustPosition(Game.Direction adjustDirection) {
            switch (adjustDirection) {
                case Game.Direction.Right:
                case Game.Direction.Left:
                    position.X = mapPosition.X * Game.tileSize;
                    break;
                case Game.Direction.Up:
                case Game.Direction.Down:
                    position.Y = mapPosition.Y * Game.tileSize;
                    break;
            }
        }
      
        public void updateMovement(Game.Direction updatedDirection) {
            switch (updatedDirection) {
                case Game.Direction.Up:
                    setVelocity(new PointF(0, -225));
                    startAnimating(Game.Direction.Up);
                    setDirection(Game.Direction.Up);
                    break;
                case Game.Direction.Down:
                    setVelocity(new PointF(0, 225));
                    startAnimating(Game.Direction.Down);
                    setDirection(Game.Direction.Down);
                    break;
                case Game.Direction.Left:
                    setVelocity(new PointF(-225, 0));
                    startAnimating(Game.Direction.Left);
                    setDirection(Game.Direction.Left);
                    break;
                case Game.Direction.Right:
                    setVelocity(new PointF(225, 0));
                    startAnimating(Game.Direction.Right);
                    setDirection(Game.Direction.Right);
                    break;
                case Game.Direction.None:
                    setVelocity(new PointF(0, 0));
                    stopAnimating();
                    setDirection(Game.Direction.None);
                    break;
            }
            
        }


        public void destroy() {
            alive = false;
            Console.WriteLine("Destroy player ");
            (spriteAnimation as PlayerAnimation).startDeathAnimation();
            Timer deathTimer = new Timer(1200);
            deathTimer.AutoReset = false;
            deathTimer.Elapsed += delegate { stopDrawing(); };
            deathTimer.Start();
        }

    }
}
