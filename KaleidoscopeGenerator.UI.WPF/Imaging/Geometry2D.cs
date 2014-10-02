using KaleidoscopeGenerator.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KaleidoscopeGenerator.UI.WPF.Imaging
{
    class Geometry2D : IGeometry<Node2D, Geometry2D, Transformation2D>
    {
        private StreamGeometry _geometry;
        private ImageBrush _brush;

        public Geometry2D()
        {
            _geometry = new StreamGeometry();
            _brush = new ImageBrush();
            Drawing = new GeometryDrawing(_brush, null, _geometry);
        }

        public GeometryDrawing Drawing
        {
            get;
            private set;
        }

        public Uri ImageUri
        {
            set
            {
                var source = new BitmapImage();

                // TODO: currently this resized the images to 400px but we should resize depending on the geometry size
                source.BeginInit();
                source.UriSource = value;
                source.DecodePixelHeight = 400;
                source.DecodePixelWidth = 400;
                source.EndInit();

                _brush.ImageSource = source;
            }
        }

        private void LoadBitmap()
        {
        }

        public List<Tuple<double, double>> Points
        {
            set
            {
                using (var context = _geometry.Open())
                {
                    var first = true;
                    foreach (var point in value)
                    {
                        if (first)
                        {
                            context.BeginFigure(new Point(point.Item1, point.Item2), true, true);
                            first = false;
                        }
                        else
                        {
                            context.LineTo(new Point(point.Item1, point.Item2), false, false);
                        }
                    }
                }
            }
        }
    }
}
