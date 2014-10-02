using KaleidoscopeGenerator.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KaleidoscopeGenerator.UI.WPF.Media3D
{
    class Geometry3D : IGeometry<Node3D, Geometry3D, Transformation3D>
    {
        private MeshGeometry3D _geometry;
        private ImageBrush _brush;

        public Geometry3D()
        {
            _geometry = new MeshGeometry3D();
            _brush = new ImageBrush();

            Material material = new DiffuseMaterial(_brush);
            Model3D = new GeometryModel3D(_geometry, material);
        }

        public GeometryModel3D Model3D
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

        public List<Tuple<double, double>> Points
        {
            set
            {
                // some calculations to do texture mapping
                var xMin = 0.0;
                var xMax = 0.0;
                var yMin = 0.0;
                var yMax = 0.0;
                foreach (var point in value)
                {
                    if (point.Item1 < xMin)
                        xMin = point.Item1;
                    if (point.Item1 > xMax)
                        xMax = point.Item1;
                    if (point.Item1 < yMin)
                        yMin = point.Item2;
                    if (point.Item1 > yMax)
                        yMax = point.Item2;
                }
                var width = xMax - xMin;
                var height = yMax - yMin;
                
                var i = 0;
                foreach (var point in value)
                {
                    var p = new Point3D(point.Item1, point.Item2, 0);
                    _geometry.Positions.Add(p);

                    var texturePoint = new Point(
                        1 - (point.Item1 - xMin) / width, 
                        (point.Item2 - yMin) / height // TODO: maybe we should use max(widht,height) here to avoid distorting the image
                    );
                    _geometry.TextureCoordinates.Add(texturePoint);

                    _geometry.TriangleIndices.Add(i++);
                }
            }
        }
    }
}
