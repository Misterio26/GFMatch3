using System;
using System.Windows;
using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GRBackground : GameRenderer {
        public Size Size;
        private Size _backgroundSize;

        public GRBackground() {
            _backgroundSize.Width = ResourcesManager.Instance.BackgroundBitmap.PixelWidth;
            _backgroundSize.Height = ResourcesManager.Instance.BackgroundBitmap.PixelHeight;
        }

        public override void OnDraw(DrawingContext dc) {
            int countX = (int) Math.Ceiling(Size.Width / (_backgroundSize.Width - 4));
            int countY = (int) Math.Ceiling(Size.Height / (_backgroundSize.Height - 4));
            for (int x = 0; x < countX; x++) {
                for (int y = 0; y < countY; y++) {
                    dc.DrawImage(ResourcesManager.Instance.BackgroundBitmap,
                        new Rect(new Point(-2 + x * (_backgroundSize.Width - 4), -2 + y * (_backgroundSize.Height - 4)),
                            _backgroundSize));
                }
            }
        }
    }
}