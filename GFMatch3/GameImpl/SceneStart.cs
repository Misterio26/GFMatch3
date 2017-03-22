using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Начальная сцена и точка входа в игру (entry point).
    /// Жестко забита как первая в GameDirector'е.
    /// </summary>
    public class SceneStart : GameScene {
        public SceneStart() {
            var playButton = new GameObject();
            playButton.Renderer = new GRText("PLAY", TextAlignment.Center, VerticalAlignment.Center);
            playButton.AddAction(new GameActionDelegated(gameObject => {
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
            playButton.AddAction(new GAColorableOnClick());
            AddChild(playButton);
            AddChild(new GOBackground());
        }
    }
}