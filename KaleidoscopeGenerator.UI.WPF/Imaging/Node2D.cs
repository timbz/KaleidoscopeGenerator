using KaleidoscopeGenerator.Data;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KaleidoscopeGenerator.UI.WPF.Imaging
{
    class Node2D : INode<Node2D, Geometry2D, Transformation2D>
    {
        private Transformation2D _transformation;
        private Geometry2D _geometry;

        public Node2D()
        {
            Drawing = new DrawingGroup();
        }

        public DrawingGroup Drawing
        {
            get;
            private set;
        }

        public void AddChild(Node2D child)
        {
            Drawing.Children.Add(child.Drawing);
        }

        public Geometry2D Geometry
        {
            get
            {
                return _geometry;
            }
            set
            {
                _geometry = value;
                Drawing.Children.Add(_geometry.Drawing);
            }
        }

        public Transformation2D Transformation
        {
            get
            {
                return _transformation;
            }
            set
            {
                _transformation = value;
                Drawing.Transform = _transformation.Transform;
            }
        }


        public Node2D Clone()
        {
            var clone = new Node2D();
            clone.Geometry = Geometry; // no need to deep copy the geometry, all nodes have a reference to the same instance
            if (Transformation != null)
                clone.Transformation = Transformation.Clone();
            if (Drawing != null)
                clone.Drawing = Drawing.Clone();
            return clone;
        }
    }
}
