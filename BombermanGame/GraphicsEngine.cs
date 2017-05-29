using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BombermanGame.Animations;
using BombermanGame.Powerups;

namespace BombermanGame
{
    class GraphicsEngine
    {
        private Control gamePanel;
        private List<Player> players;
        private Map map;
        private RenderTarget target;

        public GraphicsEngine(Control gamePanel, List<Player> players, Map map) {
            this.gamePanel = gamePanel;
            this.players = players;
            this.map = map;

            // Create the DirectX render target
            this.target = new WindowRenderTarget(
                new SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded, DebugLevel.None),
                new RenderTargetProperties(RenderTargetType.Default,
                    new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                    0,
                    0,
                    RenderTargetUsage.GdiCompatible, FeatureLevel.Level_DEFAULT),
                new HwndRenderTargetProperties {
                    Hwnd = gamePanel.Handle,
                    PixelSize = new SharpDX.Size2(map.Size.Width, map.Size.Height)
                });

            // Prepare all the graphics resources used in the game
            // TODO: Do this using Reflection instead
            Ground.LoadGraphics(target);
            Block.LoadGraphics(target);
            ConstBlock.LoadGraphics(target);
            PlayerAnimations.LoadGraphics(target);
            Bomb.LoadGraphics(target);
            FireAnimations.LoadGraphics(target);
            BombRangePowerup.LoadGraphics(target);
            ExtraBombPowerup.LoadGraphics(target);
            KickAbilityPowerup.LoadGraphics(target);
        }

        public void Draw() {
            target.BeginDraw();

            try {
                this.map.Draw(this.target);
            }
            catch {
                Console.WriteLine("Exception in graphic engine");
            }
            foreach (var player in players) {
                if (player.ShouldDraw()) {
                    player.Draw(this.target);
                }
            }

            target.EndDraw();
        }
    }
}
