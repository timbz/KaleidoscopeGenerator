using System;
using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data
{
    public class KaleidoscopeFactory<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        private Dictionary<KaleidoscopeType, IKaleidoscope<TNode, TGeometry, TTransformation>> _instanceCache;

        public KaleidoscopeFactory()
        {
            _instanceCache = new Dictionary<KaleidoscopeType, IKaleidoscope<TNode, TGeometry, TTransformation>>();
        }

        public IKaleidoscope<TNode, TGeometry, TTransformation> Get(KaleidoscopeType type)
         {
             if (_instanceCache.ContainsKey(type))
             {
                 return _instanceCache[type];
             }
             switch (type)
             {
                 case KaleidoscopeType.TRIANGLE:
                     var triangleInstance = new TriangleKaleidoscope<TNode, TGeometry, TTransformation>();
                     _instanceCache[type] = triangleInstance;
                     return triangleInstance;
                 case KaleidoscopeType.SQUARE:
                     var squareInstance = new SquareKaleidoscope<TNode, TGeometry, TTransformation>();
                     _instanceCache[type] = squareInstance;
                     return squareInstance;
             }
             throw new NotImplementedException();
         }
    }
}
