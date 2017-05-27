using System.Drawing;

namespace BombermanGame.Powerups
{
    enum PowerUpType { ExtraBomb, RangeBoost, KickAbility }

    class Powerup : FixedMapObject
    {
        private Bitmap sprite;
        public PowerUpType Type { get; set; }
        public double CreationTime { get; }
        public double TimeToLive { get; private set; }

        public Powerup(PowerUpType type, double creationTime) {
            Type = type;
            CreationTime = creationTime;
            TimeToLive = 5;

            switch (type) {
                case PowerUpType.RangeBoost:
                    sprite = new Bitmap(Properties.Resources.fire, Game.boxSize);
                    break;
                case PowerUpType.ExtraBomb:
                    sprite = new Bitmap(Properties.Resources.bomb, Game.boxSize);
                    break;
                case PowerUpType.KickAbility:
                    sprite = new Bitmap(Properties.Resources.PlayerSprite, Game.boxSize);
                    break;
            }
        }

        public override void Update(double totalTime) {
            if (totalTime - CreationTime >= TimeToLive) {
                // Remove from the map
                this.mapTile.Object = null;
                this.mapTile = null;
            }
        }

        public override void Draw(Graphics g) {
            g.DrawImage(this.sprite, this.mapTile.Bounds.Location);
        }

        public void ApplyToPlayer(Player player) {
            switch (Type) {
                case PowerUpType.RangeBoost:
                    player.BombRange += 1;
                    break;
            }
            this.mapTile.Object = null;
            this.mapTile = null;
        }
    }
}
