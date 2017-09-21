using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFExtension;

namespace RestInMusic.Controls
{
    class OverlayedText : FrameworkElement
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata("Overlay Content", FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ScaleProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FillProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(Color.FromArgb(200, 26, 26, 26), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(new FontFamily(null, "sans"), FrameworkPropertyMetadataOptions.AffectsRender));
                
        public static readonly DependencyProperty RadiusXProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RadiusYProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DebugModeProperty =
            DependencyHelper.Register(
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));


        public object Content
        {
            get => this.GetValue<object>(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        public double Scale
        {
            get => this.GetValue<double>(ScaleProperty);
            set => this.SetValue(ScaleProperty, value);
        }

        public double FontSize
        {
            get => this.GetValue<double>(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }

        public Color Fill
        {
            get => this.GetValue<Color>(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        public FontFamily FontFamily
        {
            get => this.GetValue<FontFamily>(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, value);
        }

        public double RadiusX
        {
            get => this.GetValue<double>(RadiusXProperty);
            set => this.SetValue(RadiusXProperty, value);
        }

        public double RadiusY
        {
            get => this.GetValue<double>(RadiusYProperty);
            set => this.SetValue(RadiusYProperty, value);
        }

        public double CenterX
        {
            get => this.ActualWidth / 2;
        }

        public double CenterY
        {
            get => this.ActualHeight / 2;
        }

        public bool DebugMode
        {
            get => this.GetValue<bool>(DebugModeProperty);
            set => this.SetValue(DebugModeProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Geometry contentGeometry = null;

            switch (Content.GetType().Name)
            {
                case nameof(String):
                    FormattedText formattedText = new FormattedText(
                        (string)this.Content,
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        this.FontFamily.GetTypefaces().First(),
                        this.FontSize * Scale,
                        Brushes.Black
                        );
                    
                    contentGeometry = formattedText.BuildGeometry(new Point(ActualWidth / 2 - formattedText.Width / 2, ActualHeight / 2 - formattedText.Height / 2));
                    break;

                case nameof(Path):
                    var data = (Path)Content;
                    contentGeometry = data.Data.GetOutlinedPathGeometry(0, ToleranceType.Absolute);

                    var tg = new TransformGroup();
                    contentGeometry.Transform = tg;

                    tg.Children.Add(new ScaleTransform(Scale, Scale));

                    double contentHalfWidth = contentGeometry.Bounds.Width / 2;
                    double contentHalfHeight = contentGeometry.Bounds.Height / 2;

                    double spaceWidth = contentGeometry.Bounds.Left;
                    double spaceHeight = contentGeometry.Bounds.Top;

                    double centerZeroX = CenterX - spaceWidth;
                    double centerZeroY = CenterY - spaceHeight;

                    double transPositionX = centerZeroX - contentHalfWidth;
                    double transPositionY = centerZeroY - contentHalfHeight;

                    tg.Children.Add(new TranslateTransform(transPositionX, transPositionY));
                    break;
            }

            if (DebugMode)
            {
                // Draw two crossing lines for check where control's center.
                // 컨트롤의 중심을 확인하기 위해 두 개의 교차되는 선을 그립니다.
                // horizontal line
                // 가로선
                drawingContext.DrawLine(new Pen(Brushes.Red, 1),
                    new Point(0, ActualHeight / 2), new Point(ActualWidth, ActualHeight / 2));
                // vertical line
                // 세로선
                drawingContext.DrawLine(new Pen(Brushes.Red, 1),
                    new Point(ActualWidth / 2, 0), new Point(ActualWidth / 2, ActualHeight));

                // Draw Rectangle arround content geometry for view there size  of occupy.
                // 지오메트리가 차지하는 공간을 확인하기 위해 사각형을 그립니다.
                drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 1), new Rect(contentGeometry.Bounds.TopLeft, contentGeometry.Bounds.BottomRight));
                
                // Draw two crossing lines for check where geometry's center.
                // 지오메트리의 중심을 확인하기 위해 두 개의 교차되는 선을 그립니다.
                // horizontal line
                // 가로선
                drawingContext.DrawLine(new Pen(Brushes.Blue, 1),
                    new Point(contentGeometry.Bounds.TopLeft.X, contentGeometry.Bounds.TopLeft.Y + contentGeometry.Bounds.Height / 2),
                    new Point(contentGeometry.Bounds.TopRight.X, contentGeometry.Bounds.TopRight.Y + contentGeometry.Bounds.Height / 2));
                // vertical line
                // 세로선
                drawingContext.DrawLine(new Pen(Brushes.Blue, 1),
                    new Point(contentGeometry.Bounds.TopLeft.X + contentGeometry.Bounds.Width / 2, contentGeometry.Bounds.TopLeft.Y),
                    new Point(contentGeometry.Bounds.BottomLeft.X + contentGeometry.Bounds.Width / 2, contentGeometry.Bounds.BottomLeft.Y));
            }

            var rectangleGeometry = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight), RadiusX, RadiusY);
            var combined = new CombinedGeometry(GeometryCombineMode.Xor, rectangleGeometry, contentGeometry);

            var visualColor = new SolidColorBrush(Fill);

            drawingContext.DrawGeometry(visualColor, null, combined);
        }
    }
}
