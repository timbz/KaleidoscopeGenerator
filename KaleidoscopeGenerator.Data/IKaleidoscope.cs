using System;
namespace KaleidoscopeGenerator.Data
{
    public interface IKaleidoscope<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        TNode Generate(double triagleWidth, Uri imageUri, double canvasWidth, double canvasHeight);
    }
}
