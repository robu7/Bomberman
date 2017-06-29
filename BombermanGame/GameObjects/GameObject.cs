using System;
using System.Drawing;

namespace BombermanGame.GameObjects
{

    /// <summary>
    /// 
    /// </summary>
    abstract class GameObject
    {
        public bool PendingDestroy { get; set; }
        private LocationResolver location;
        public LocationResolver LocationResolver {
            get {
                return location;
            }
            protected set {
                if (this.location != null) {
                    // Clean up
                }
                this.location = value;
            }
        }
        public Tile Tile => LocationResolver?.CurrentTile;

        public void AttachToTile(Tile mapTile)
        {
            LocationResolver = new FixedLocationResolver(mapTile);
        }

        public bool IsDescructible { get; protected set; } = true;
        public void Destroy(double currentTime)
        {
            if (!IsDescructible) {
                return;
            }
            OnDestroy(currentTime);
        }
        protected virtual void OnDestroy(double currentTime)
        {
            if (LocationResolver?.CurrentTile?.Object != this) {
                return;
            }
            LocationResolver.CurrentTile.Object = null;
            LocationResolver = null;
        }

        public abstract void Update(double totalTime);
        public abstract void Draw(SharpDX.Direct2D1.RenderTarget target);
    }
}
