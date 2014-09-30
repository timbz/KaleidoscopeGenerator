using KaleidoscopeGenerator.Data;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace KaleidoscopeGenerator.UI.WPF.Media3D
{
    class Node3D : INode<Node3D, Geometry3D, Transformation3D>
    {
        private Transformation3D _transformation;
        private Geometry3D _geometry;

        public Node3D()
        {
            Model3D = new Model3DGroup();
        }

        public Model3DGroup Model3D
        {
            get;
            private set;
        }

        public void AddChild(Node3D child)
        {
            Model3D.Children.Add(child.Model3D);
        }

        public Geometry3D Geometry
        {
            get
            {
                return _geometry;
            }
            set
            {
                _geometry = value;
                Model3D.Children.Add(_geometry.Model3D);
            }
        }

        public Transformation3D Transformation
        {
            get
            {
                return _transformation;
            }
            set
            {
                _transformation = value;
                Model3D.Transform = _transformation.Transform;
            }
        }


        public Node3D Clone()
        {
            var clone = new Node3D();
            clone.Geometry = Geometry; // no need to deep copy the geometry, all nodes have a reference to the same instance
            if (Transformation != null)
                clone.Transformation = Transformation.Clone();
            if (Model3D != null)
                clone.Model3D = Model3D.Clone();
            return clone;
        }
    }
}
