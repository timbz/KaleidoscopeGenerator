using KaleidoscopeGenerator.Data;
using System;
using System.Windows.Media;

namespace KaleidoscopeGenerator.UI.WPF.Imaging
{
    class Transformation2D : ITransformation<Node2D, Geometry2D, Transformation2D>
    {
        public Transform Transform
        {
            get;
            private set;
        }

        public void initAsFlip(double angle)
        {
            var cos = Math.Cos(angle * 2);
            var sin = Math.Sin(angle * 2);
            Transform = new MatrixTransform(cos, sin, sin, -cos, 0, 0);
        }

        public void initAsTranslation(double x, double y)
        {
            Transform = new TranslateTransform(x, y);
        }


        public void initAsGroup(Transformation2D[] transformatins)
        {
            var group = new TransformGroup();
            foreach (var t in transformatins)
            {
                group.Children.Add(t.Transform);
            }
            Transform = group;
        }


        public Transformation2D Clone()
        {
            var clone = new Transformation2D();
            if (Transform != null)
                clone.Transform = Transform.Clone();
            return clone;
        }
    }

}
