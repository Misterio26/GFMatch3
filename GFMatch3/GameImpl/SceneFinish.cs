using System.Windows;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class SceneFinish : GameScene {

        public SceneFinish() {
            GameObject goText = new GameObject();
            goText.Renderer = new GRText("GameOver", TextAlignment.Center, VerticalAlignment.Bottom);
            goText.AddAction(new GameActionDelegated(gameObject => {
                gameObject.Transform.X = GameDirector.Instance.ScreenWidth / 2;
                gameObject.Transform.Y = GameDirector.Instance.ScreenHeight / 2;
                gameObject.Transform.ScaleX = 8;
                gameObject.Transform.ScaleY = 8;
            }, null, null));
            AddChild(goText);

            GameObject okButton = new GameObject();
            okButton.Renderer = new GRText("OK", TextAlignment.Center, VerticalAlignment.Bottom);
            okButton.AddAction(new GameActionDelegated(gameObject => {
                gameObject.Transform.X = GameDirector.Instance.ScreenWidth / 2;
                gameObject.Transform.Y = GameDirector.Instance.ScreenHeight - 10;
                gameObject.Transform.ScaleX = 4;
                gameObject.Transform.ScaleY = 4;

                if (GameDirector.Instance.IsMouseClick) {
                    Point point = gameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                    Rect textRect = ((IRected) gameObject.Renderer).Rect;
                    if (textRect.Contains(point)) {
                        GameDirector.Instance.SetScene(new SceneStart());
                    }
                }
            }, null, null));
            okButton.AddAction(new GAColorableOnClick());
            AddChild(okButton);
        }
    }
}