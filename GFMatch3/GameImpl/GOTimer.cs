using System;
using System.Runtime.InteropServices;
using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class GOTimer : GameObject {
        private double _timeLeft = BoardGameConfig.GameTime;

        public double TimeLeft {
            get { return _timeLeft; }
        }

        public GOTimer() {
            Renderer = new GRText(_timeLeft.ToString(), TextAlignment.Center, VerticalAlignment.Top);
            AddAction(new GameActionDelegated(gameObject => {
                _timeLeft = Math.Max(0, _timeLeft - GameDirector.Instance.DeltaTime);

                ((GRText) gameObject.Renderer).Text = Math.Ceiling(_timeLeft).ToString();

                gameObject.Transform.X = GameDirector.Instance.ScreenWidth / 2;
                gameObject.Transform.Y = 10;
                gameObject.Transform.ScaleX = 1.5;
                gameObject.Transform.ScaleY = 1.5;
            }, null, null));
        }
    }
}