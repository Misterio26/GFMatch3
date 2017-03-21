﻿using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public abstract class GOBoardElement : GameObject {
        private AnimationTransformTransition _selectionTransformTransition;

        protected readonly GameObject AnimatablePart = new GameObject();

        private int _coloredType;

        public int ColoredType => _coloredType;

        public bool Selected {
            set {
                _selectionTransformTransition.SetToTo(value);
                AnimatablePart.AddAction(_selectionTransformTransition);
            }
        }

        public GOBoardElement(int coloredType, GRBoardElement grBoardElement) {
            _coloredType = coloredType;
            AddAction(new GABoardElement());

            _selectionTransformTransition = new AnimationTransformTransition(GameTransform.Default,
                GameTransform.Default * 1.35, BoardGameConfig.AnimationsPlayerSpeed);
            AnimatablePart.Renderer = grBoardElement;
            AddChild(AnimatablePart);
        }

        public void AnimateTransitionFromCellToCell(CellCoord fromCell, CellCoord toCell) {
            CellCoord reverseOffset = fromCell - toCell;

            AnimationTransformTransition animationTransformTransition = new AnimationTransformTransition(new GameTransform(
                reverseOffset.X * BoardGameConfig.BoardCellSize,
                reverseOffset.Y * BoardGameConfig.BoardCellSize,
                0, 0, 1, 1
            ), GameTransform.Default, BoardGameConfig.AnimationsCommonSpeed, true);
            AnimatablePart.AddAction(
                animationTransformTransition
            );
        }

        public void Activate() {
            OnAtivate();
            GameDirector.Instance.GetScene<SceneBoard>()?.AddScore();
        }

        public abstract void OnAtivate();
    }
}