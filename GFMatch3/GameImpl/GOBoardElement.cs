using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBoardElement : GameObject {
        private GATransitionTransform _selectionTransition;

        private GameObject _animatablePart = new GameObject();

        private bool _selected;

        public bool Selected {
            get { return _selected; }
            set {
                _selected = value;
                _selectionTransition.SetState(_selected);
            }
        }

        public GOBoardElement() {
            AddAction(new GABoardElement());

            _selectionTransition = new GATransitionTransform(GameTransform.Default, GameTransform.Default * 1.35, 0.100);

            _animatablePart.Renderer = new GRBoardElementCircle();
            _animatablePart.AddAction(_selectionTransition);
            AddChild(_animatablePart);
        }
    }
}