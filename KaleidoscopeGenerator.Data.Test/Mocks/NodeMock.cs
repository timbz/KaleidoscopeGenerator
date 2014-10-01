using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data.Test.Mocks
{
    class NodeMock : INode<NodeMock, GeometryMock, TransformationMock>
    {
        public NodeMock()
        {
            Children = new List<NodeMock>();
        }

        public List<NodeMock> Children
        {
            get;
            private set;
        }

        public void AddChild(NodeMock child)
        {
            Children.Add(child);
        }

        public GeometryMock Geometry
        {
            get;
            set;
        }

        public TransformationMock Transformation
        {
            get;
            set;
        }

        public NodeMock Clone()
        {
            var clone = new NodeMock();
            clone.Geometry = Geometry;
            clone.Transformation = Transformation.Clone();
            foreach (var child in Children)
            {
                clone.AddChild(child.Clone());
            }
            return clone;
        }
    }
}
