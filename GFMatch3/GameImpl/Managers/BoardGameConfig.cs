using System.Windows.Media;

namespace GFMatch3.GameImpl {
    public class BoardGameConfig {
        public const int BoardSize = 8;
        public const int BoardCellSize = 80;
        public const int GameTime = 60;
        public const int MinimalMatchesInRow = 3;

        public const double AnimationMoveTime = 0.15;
        public const double AnimationHideTime = 0.25;
        public const double AnimationSelectionTime = 0.1;

        public const double AnimationBombExplosionTime = 0.25;

        public const double BombActivationTime = 0.15;

        public const double DestoryerTimeForCell = 0.15;

        public static readonly Color[] ColoredTypes =
            {Colors.Red, Colors.Purple, Colors.Gold, Colors.ForestGreen, Colors.DeepSkyBlue};
    }
}