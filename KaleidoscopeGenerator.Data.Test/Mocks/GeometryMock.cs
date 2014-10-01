using System;
using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data.Test.Mocks
{
    class GeometryMock : IGeometry<NodeMock, GeometryMock, TransformationMock>
    {
        public List<Tuple<double, double>> Points
        {
            set;
            get;
        }

        public Uri ImageUri
        {
            set;
            get;
        }
    }
}
