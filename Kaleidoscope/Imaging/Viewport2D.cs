using KaleidoscopeGenerator.Data;
using KaleidoscopeGenerator.UI.WPF.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace KaleidoscopeGenerator.UI.WPF.Imaging
{
    class Viewport2D : FrameworkElement
    {
        private DrawingVisual _drawingVisual;
        private KaleidoscopeFactory<Node2D, Geometry2D, Transformation2D> _kaleidoscopes;
        
        public Viewport2D()
        {
            _kaleidoscopes = new KaleidoscopeFactory<Node2D, Geometry2D, Transformation2D>();
            SizeChanged += OnSizeChanged;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var settings = ((AppModel)DataContext).Settings;
            settings.PropertyChanged += OnPropertyChange;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // settings is null, maybe we dont need this
            //var settings = ((AppModel)DataContext).Settings;
            //settings.PropertyChanged -= OnPropertyChange;
        }

        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            System.Console.WriteLine("Property changed: " + e.PropertyName);
            RenderKaleidoscope();
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            RenderKaleidoscope();
        }

        private void RenderKaleidoscope()
        {
            var context = DataContext as AppModel;
            if (context == null)
                return;
            var settings = context.Settings;

            var kaleidoscope = _kaleidoscopes.Get(settings.SelectedKaleidoscopeType.Type);
            var imageUri = new Uri(settings.ImagePath, UriKind.Absolute);
            var rootNode = kaleidoscope.Generate(settings.GeometryWidth, imageUri, ActualWidth, ActualHeight);

            var drawingFromCenter = new DrawingGroup();
            drawingFromCenter.Transform = new TranslateTransform(ActualWidth / 2, ActualHeight / 2);
            drawingFromCenter.Children.Add(rootNode.Drawing);

            RemoveVisualChild(_drawingVisual);
            _drawingVisual = new DrawingVisual();
            using(var drawingContext = _drawingVisual.RenderOpen())
            {
                // clear brackgroud
                var background = new RectangleGeometry(new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
                drawingContext.DrawGeometry(new SolidColorBrush(Colors.White), null, background);
                // render kaleidoscope
                drawingContext.DrawDrawing(drawingFromCenter);
            }
            AddVisualChild(_drawingVisual);
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return _drawingVisual;
        }

    }
}
