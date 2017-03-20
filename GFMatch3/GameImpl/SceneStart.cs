using System.Windows;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class SceneStart : GameScene {

        private readonly GameObject _playButton;

        public SceneStart() {
            _playButton = new GameObject();
            _playButton.Renderer = new GRText("PLAY", TextAlignment.Center, VerticalAlignment.Center);
            _playButton.AddAction(new GameActionDelegated(gameObject => {
                gameObject.Transform.X = GameDirector.Instance.ScreenWidth / 2;
                gameObject.Transform.Y = GameDirector.Instance.ScreenHeight / 2;
                gameObject.Transform.ScaleX = 4;
                gameObject.Transform.ScaleY = 4;
       
                if (GameDirector.Instance.IsMouseClick) {
                    Point point = gameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                    Rect textRect = ((IRected) gameObject.Renderer).Rect;
                    if (textRect.Contains(point)) {
                        GameDirector.Instance.SetScene(new SceneBoard());
                    }
                }
            }, null, null));
            _playButton.AddAction(new GAColorableOnClick());
            AddChild(_playButton);
        }
    }
}