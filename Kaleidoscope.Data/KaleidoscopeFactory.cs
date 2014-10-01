using System;
using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data
{
    public class KaleidoscopeFactory<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        private Dictionary<KaleidoscopeTypes, IKaleidoscope<TNode, TGeometry, TTransformation>> _instanceCache;

        public KaleidoscopeFactory()
        {
            _instanceCache = new Dictionary<KaleidoscopeTypes, IKaleidoscope<TNode, TGeometry, TTransformation>>();
        }

        public IKaleidoscope<TNode, TGeometry, TTransformation> Get(KaleidoscopeTypes type)
         {
             if (_instanceCache.ContainsKey(type))
             {
                 return _instanceCache[type];
             }
             switch (type)
             {
                 case KaleidoscopeTypes.Triangle:
                     var triangleInstance = new TriangleKaleidoscope<TNode, TGeometry, TTransformation>();
                     _instanceCache[type] = triangleInstance;
                     return triangleInstance;
                 case KaleidoscopeTypes.Square:
                     var squareInstance = new SquareKaleidoscope<TNode, TGeometry, TTransformation>();
                     _instanceCache[type] = squareInstance;
                     return squareInstance;
             }
             throw new NotImplementedException();
         }
    }
}
