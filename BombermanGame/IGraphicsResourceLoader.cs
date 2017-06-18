namespace BombermanGame {
    /// <summary>
    /// Interface used to load graphics before starting a game
    /// </summary>
    public interface IGraphicsResourceLoader {
        void LoadGraphics(SharpDX.Direct2D1.RenderTarget target);
    }
}
