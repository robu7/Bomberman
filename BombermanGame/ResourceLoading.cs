using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;

namespace BombermanGame {
    static class ResourceLoading {
        /// <summary>
        /// Creates a Direct2D Bitmap aimed for the specified target
        /// </summary>
        public static Bitmap CreateDirectX2D1Bitmap(this System.Drawing.Bitmap source, RenderTarget renderTarget) {
            using (var bitmap = new System.Drawing.Bitmap(source)) {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true)) {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++) {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++) {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }
    }

    /// <summary>
    /// Convenience class to reduce boilerplate code when rendering a single bitmap
    /// </summary>
    public abstract class BitmapLoader : IGraphicsResourceLoader {
        private System.Drawing.Bitmap sourceBitmap;
        private Action<Bitmap> setter;
        private Func<Bitmap> getter;

        public BitmapLoader(System.Drawing.Bitmap sourceBitmap, Action<Bitmap> setter, Func<Bitmap> getter) {
            this.sourceBitmap = sourceBitmap;
            this.getter = getter;
            this.setter = setter;
        }

        public void LoadGraphics(RenderTarget target) {
            Bitmap = this.sourceBitmap.CreateDirectX2D1Bitmap(target);
        }

        public Bitmap Bitmap {
            get {
                return this.getter();
            }
            private set {
                this.setter(value);
            }
        }
    }
}
