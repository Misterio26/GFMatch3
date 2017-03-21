using System;
using System.Windows.Media.Imaging;

namespace GFMatch3.GameImpl {
    public class ResourcesManager {
        public static readonly ResourcesManager Instance = new ResourcesManager();

        private volatile CachedBitmap _backgroundBitmap;

        public CachedBitmap BackgroundBitmap {
            get {
                if (_backgroundBitmap == null) {
                    lock (this) {
                        if (_backgroundBitmap == null) {
                            _backgroundBitmap =
                                new CachedBitmap(
                                    new BitmapImage(new Uri("pack://application:,,,/Assets/background.png")),
                                    BitmapCreateOptions.None, BitmapCacheOption.Default);
                        }
                    }
                }
                return _backgroundBitmap;
            }
        }

        private ResourcesManager() {
        }

    }
}