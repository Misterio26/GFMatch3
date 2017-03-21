using System.Windows;
using System.Windows.Media;

namespace GFMatch3.GameCore {
    public class GameRenderer {

        private bool _hasClip;
        private Rect _clipArea;

        public virtual void OnDraw(DrawingContext dc) {

        }

        public bool HasClip => _hasClip;
        public Rect ClipArea => _clipArea;

        public void SetClip(Rect clipArea) {
            _clipArea = clipArea;
            _hasClip = true;
        }

        public void RemoveClip() {
            _hasClip = false;
        }

    }
}