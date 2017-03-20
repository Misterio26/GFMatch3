using System.Diagnostics;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBoardElement : GameObject {

        public GOBoardElement() {
            Renderer = new GRBoardElementCircle();
            AddAction(new GABoardElement());
            AddAction(new GAAnimationPuls());
        }

        private class GABoardElement : GameAction {
            public override void OnStart() {
                base.OnStart();
                Debug.WriteLine("GABoardElement OnStart");
            }

            public override void OnUpdate() {
                base.OnUpdate();
//                Debug.WriteLine("GABoardElement OnStart");
            }

            public override void OnStop() {
                base.OnStop();
                Debug.WriteLine("GABoardElement OnStop");
            }
        }

    }
}