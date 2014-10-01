namespace KaleidoscopeGenerator.Data.Test.Mocks
{
    class TransformationMock : ITransformation<NodeMock, GeometryMock, TransformationMock>
    {
        public enum TransformationType
        {
            Flip, Translation, Group
        }

        public TransformationType Type { get; set; }

        public object[] Parameters { get; set; }

        public void initAsFlip(double angle)
        {
            Parameters = new object[] { angle };
            Type = TransformationType.Flip;
        }

        public void initAsTranslation(double x, double y)
        {
            Parameters = new object[] { x, y };
            Type = TransformationType.Translation;
        }

        public void initAsGroup(TransformationMock[] transformatins)
        {
            Parameters = transformatins;
            Type = TransformationType.Group;
        }

        public TransformationMock Clone()
        {
            var clone = new TransformationMock();
            clone.Type = Type;
            if (Parameters != null)
            {
                clone.Parameters = (object[])Parameters.Clone();
            }
            return clone;
        }
    }
}
