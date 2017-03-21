using System.Windows;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOScore : GameObject {
        public int Score;

        public GOScore() {
            Renderer = new GRText("Score: 0", TextAlignment.Right, VerticalAlignment.Top);
            AddAction(new GameActionDelegated(gameObject => {
                ((GRText) gameObject.Renderer).Text = "Score: " + Score.ToString();

                gameObject.Transform.X = GameDirector.Instance.ScreenWidth - 20;
                gameObject.Transform.Y = 10;
                gameObject.Transform.ScaleX = 1.5;
                gameObject.Transform.ScaleY = 1.5;
            }, null, null));
        }
    }
}