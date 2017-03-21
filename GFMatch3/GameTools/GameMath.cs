using System.Windows;

namespace GFMatch3.GameTools {
    public class GameMath {
        public const int DirectionUp = 0;
        public const int DirectionRight = 1;
        public const int DirectionDown = 2;
        public const int DirectionLeft = 3;

        public static readonly Point[] Directions = new Point[] {
            new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0),
        };

    }
}