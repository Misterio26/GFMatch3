using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class GRText : GameRenderer, IColorable, IRected {

        private static readonly Typeface GameFont = new Typeface(
            new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Quicksand"), FontStyles.Normal,
            FontWeights.Regular, FontStretches.Normal
        );

        private string _text;
        private FormattedText _textFormatted;

        private TextAlignment _textAlignment;
        private VerticalAlignment _verticalAlignment;

        private SolidColorBrush _brush = new SolidColorBrush(Colors.White);

        public Color Color {
            get { return _brush.Color; }
            set { _brush.Color = value; }
        }

        public Rect Rect {
            get {
                double w = _textFormatted.Width;
                double h = _textFormatted.Height;
                double x = 0;
                double y = 0;
                switch (_verticalAlignment) {
                    case VerticalAlignment.Bottom:
                        y = -h;
                        break;
                    case VerticalAlignment.Center:
                        y = -h / 2;
                        break;
                }
                switch (_textAlignment) {
                    case TextAlignment.Right:
                        x = -w;
                        break;
                    case TextAlignment.Center:
                        x = -w / 2;
                        break;
                }
                return new Rect(x, y, w, h);
            }
        }

        public string Text {
            get { return _text; }
            set {
                _text = value;
                _textFormatted = new FormattedText(_text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                    GameFont, 32, _brush);
                _textFormatted.TextAlignment = _textAlignment;
            }
        }

        public GRText(string text, TextAlignment textAlignment, VerticalAlignment verticalAlignment) {
            _text = text;
            _textAlignment = textAlignment;
            _verticalAlignment = verticalAlignment;

            Text = text;
        }

        public override void OnDraw(DrawingContext dc) {
            double y = 0;
            switch (_verticalAlignment) {
                case VerticalAlignment.Bottom:
                    y = -_textFormatted.Height;
                    break;
                case VerticalAlignment.Center:
                    y = -_textFormatted.Height / 2;
                    break;
            }
            dc.DrawText(_textFormatted, new Point(0, y));
        }
    }
}