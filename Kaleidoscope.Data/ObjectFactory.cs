using System;
using System.Collections.Generic;
namespace KaleidoscopeGenerator.Data
{
    public interface IGeometry<TNode, TGeometry, TTransformation> 
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        List<Tuple<double, double>> Points { set; }
        Uri ImageUri { set; }
    }

    public interface ITransformation<TNode, TGeometry, TTransformation> 
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        void initAsFlip(double angle);
        void initAsTranslation(double x, double y);
        void initAsGroup(TTransformation[] transformatins);
        TTransformation Clone();
    }

    public interface INode<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        void AddChild(TNode child);
        TGeometry Geometry { get;  set; }
        TTransformation Transformation { get; set; }
        TNode Clone();
    }

    public class ObjectFactory<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        public TNode Node()
        {
            return new TNode();
        }

        public TGeometry Geometry(Uri imageUri, List<Tuple<double, double>> points)
        {
            var geometry = new TGeometry();
            geometry.Points = points;
            geometry.ImageUri = imageUri;
            return geometry;
        }

        public TTransformation FlipTransformation(double angle)
        {
            var transformation = new TTransformation();
            transformation.initAsFlip(angle);
            return transformation;
        }

        public TTransformation TranslationTransformation(double x, double y)
        {
            var transformation = new TTransformation();
            transformation.initAsTranslation(x, y);
            return transformation;
        }

        public TTransformation TransformationGroup(params TTransformation[] transformatins)
        {
            var transformation = new TTransformation();
            transformation.initAsGroup(transformatins);
            return transformation;
        }
    }
}
