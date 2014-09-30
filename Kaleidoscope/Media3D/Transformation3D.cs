using KaleidoscopeGenerator.Data;
using System;
using System.Windows.Media.Media3D;

namespace KaleidoscopeGenerator.UI.WPF.Media3D
{
    class Transformation3D : ITransformation<Node3D, Geometry3D, Transformation3D>
    {

        public Transform3D Transform
        {
            get;
            private set;
        }

        public void initAsFlip(double angle)
        {
            var cos = Math.Cos(angle * 2);
            var sin = Math.Sin(angle * 2);
            var matrix = new Matrix3D(cos, sin, 0, 0, sin, -cos, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            Transform = new MatrixTransform3D(matrix);
        }

        public void initAsTranslation(double x, double y)
        {
            Transform = new TranslateTransform3D(x, y, 0);
        }


        public void initAsGroup(Transformation3D[] transformatins)
        {   
            var group = new Transform3DGroup();
            foreach (var t in transformatins)
            {
                group.Children.Add(t.Transform);
            }
            Transform = group;
        }


        public Transformation3D Clone()
        {
            var clone = new Transformation3D();
            if (Transform != null)
                clone.Transform = Transform.Clone();
            return clone;
        }
    }

}
