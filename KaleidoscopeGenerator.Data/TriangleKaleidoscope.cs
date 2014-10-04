using System;
using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data
{
    class TriangleKaleidoscope<TNode, TGeometry, TTransformation> : IKaleidoscope<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {
        enum TransformationDirection
        {
            TOP, BOTTOM, LEFT, RIGHT
        }

        private ObjectFactory<TNode, TGeometry, TTransformation> _factory;
        private double _offset; // stores the 2/3s of the triangle height
        private Dictionary<TransformationDirection, Dictionary<int, double>> _flipAngles;
        private Dictionary<TransformationDirection, Dictionary<int, double>> _translateAngles;

        public TriangleKaleidoscope()
        {
            _factory = new ObjectFactory<TNode, TGeometry, TTransformation>();
            CalculateHorizontalFlipAndTranslationAngles();
        }

        private void CalculateHorizontalFlipAndTranslationAngles()
        {
            _flipAngles = new Dictionary<TransformationDirection, Dictionary<int, double>>();
            _flipAngles[TransformationDirection.RIGHT] = new Dictionary<int, double>();
            _flipAngles[TransformationDirection.RIGHT][0] = Math.PI * 2 / 3;
            _flipAngles[TransformationDirection.RIGHT][1] = Math.PI;
            _flipAngles[TransformationDirection.RIGHT][2] = -Math.PI * 2 / 3;
            _flipAngles[TransformationDirection.LEFT] = new Dictionary<int, double>();
            _flipAngles[TransformationDirection.LEFT][0] = Math.PI / 3;
            _flipAngles[TransformationDirection.LEFT][1] = Math.PI;
            _flipAngles[TransformationDirection.LEFT][2] = -Math.PI / 3;

            _translateAngles = new Dictionary<TransformationDirection, Dictionary<int, double>>();
            _translateAngles[TransformationDirection.RIGHT] = new Dictionary<int, double>();
            _translateAngles[TransformationDirection.RIGHT][0] = Math.PI / 6;
            _translateAngles[TransformationDirection.RIGHT][1] = -Math.PI / 2;
            _translateAngles[TransformationDirection.RIGHT][2] = Math.PI * 5 / 6;
            _translateAngles[TransformationDirection.LEFT] = new Dictionary<int, double>();
            _translateAngles[TransformationDirection.LEFT][0] = Math.PI * 5 / 6;
            _translateAngles[TransformationDirection.LEFT][1] = -Math.PI / 2;
            _translateAngles[TransformationDirection.LEFT][2] = Math.PI / 6;
        }

        public TNode Generate(double triagleWidth, Uri imageUri, double canvasWidth, double canvasHeight)
        {
            var triagleHeight = Math.Tan(Math.PI / 3) * triagleWidth / 2;
            _offset = triagleHeight * 2 / 3;
            _offset *= 0.99; // reduce offset because of rounding errors

            var root = _factory.Node();

            root.Geometry = CreateTriangle(triagleWidth, triagleHeight, imageUri);
            root.Transformation = _factory.TranslationTransformation(0, -_offset / 4);

            // we generate one row
            var horizontalElementPerSide = (int)(canvasWidth / triagleWidth) + 1;
			GenerateMiddleRow(root, horizontalElementPerSide);
            // we clone, flip and translate the middle row
            var verticalElementsPerSide = (int)(canvasHeight / triagleHeight / 2) + 1;
			GenerateColumnsFromMiddleRow(root, verticalElementsPerSide);
            var numberOfNodes = (horizontalElementPerSide * 2 + 1) * (verticalElementsPerSide * 2 + 1);
            return root;
        }

        private TGeometry CreateTriangle(double width, double height, Uri uri)
        {
            var x1 = -width / 2;
            var y1 = -height / 3;
            var points = new List<Tuple<double, double>>();
            points.Add(Tuple.Create(x1, y1));
            points.Add(Tuple.Create(-x1, y1));
            points.Add(Tuple.Create(0.0, height + y1));
            return _factory.Geometry(uri, points);
        }

        private void GenerateMiddleRow(TNode root, int elementsPerDirection)
        {
            TransformationDirection[] directions = {TransformationDirection.RIGHT, TransformationDirection.LEFT};
            foreach (var direction in directions)
            {
                var parent = root;
                for (var i = 0; i < elementsPerDirection; i++)
                {
                    var child = _factory.Node();
                    parent.AddChild(child);
                    child.Geometry = parent.Geometry;
                    var flipAngle = _flipAngles[direction][i % 3];
                    var translationAngle = _translateAngles[direction][i % 3];
                    child.Transformation = _factory.TransformationGroup(
                        _factory.FlipTransformation(flipAngle),
                        _factory.TranslationTransformation(Math.Cos(translationAngle) * _offset, Math.Sin(translationAngle) * _offset)
                    );
                    parent = child;
                }
            }
        }

        private void GenerateColumnsFromMiddleRow(TNode root, int elementsPerDirection)
        {
            if (elementsPerDirection <= 0)
            {
                return; // the middle row is enough
            }

            var topChild = root.Clone();
            var buttomChild = root.Clone();

            var rootChildren = new TNode[] { topChild, buttomChild };
            for (var j = 0; j < rootChildren.Length; j++)
            {
                var current = rootChildren[j];
                for (var i = 0; i < elementsPerDirection; i++)
                {
                    var offset = -_offset;
                    if (i % 2 == j)
                    {
                        offset = _offset * 2;
                    }
                    current.Transformation = _factory.TransformationGroup(
                        _factory.FlipTransformation(0),
                        _factory.TranslationTransformation(0, offset)
                    );
                    var child = current.Clone();
                    current.AddChild(child);
                    current = child;
                }
            }

            root.AddChild(topChild);
            root.AddChild(buttomChild);
        }
    }
}
