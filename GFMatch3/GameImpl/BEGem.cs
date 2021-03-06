﻿using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Элемент на поле - БАЗОВЫЙ.
    /// </summary>
    public class BEGem : GOBoardElement {
        public BEGem(int coloredType) : base(coloredType, new GRBoardElementGem(coloredType)) {
        }

        public override void OnActivate(CellCoord cellCoord, bool fast) {
            if (fast) {
                RemoveFromParent();
            } else {
                AnimationTransformToDestroy animationTransformToDestroy = new AnimationTransformToDestroy(
                    GameTransform.Zero, BoardGameConfig.AnimationHideTime, true);
                animationTransformToDestroy.OnDestroy = RemoveFromParent;
                AnimatablePart.AddAction(animationTransformToDestroy);
            }
        }
    }
}