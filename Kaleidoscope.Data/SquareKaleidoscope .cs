using System;
using System.Collections.Generic;

namespace KaleidoscopeGenerator.Data
{
    class SquareKaleidoscope<TNode, TGeometry, TTransformation> : IKaleidoscope<TNode, TGeometry, TTransformation>
        where TNode : INode<TNode, TGeometry, TTransformation>, new()
        where TGeometry : IGeometry<TNode, TGeometry, TTransformation>, new()
        where TTransformation : ITransformation<TNode, TGeometry, TTransformation>, new()
    {

        private ObjectFactory<TNode, TGeometry, TTransformation> _factory;
        private double _offset; // stores the 2/3s of the triangle height

        public SquareKaleidoscope()
        {
            _factory = new ObjectFactory<TNode, TGeometry, TTransformation>();
        }

        public TNode Generate(double squareWidth, Uri imageUri, double canvasWidth, double canvasHeight)
        {
            _offset = squareWidth;
            _offset *= 0.99; // reduce offset because of rounding errors

            var root = _factory.Node();

            root.Geometry = CreateSquare(squareWidth, imageUri);

            // we generate one row
            var horizontalElementPerSide = (int)(canvasWidth / squareWidth / 2) + 1;
            GenerateMiddleRow(root, horizontalElementPerSide);
            // we clone, flip and translate the middle row
            var verticalElementsPerSide = (int)(canvasHeight / squareWidth / 2) + 1;
            GenerateColumnsFromMiddleRow(root, verticalElementsPerSide);
            var numberOfNodes = (horizontalElementPerSide * 2 + 1) * (verticalElementsPerSide * 2 + 1);
            System.Console.WriteLine("Nodes " + numberOfNodes);
            return root;
        }

        private TGeometry CreateSquare(double width, Uri uri)
        {
            var half = width / 2;
            var points = new List<Tuple<double, double>>();
            // TODO: support indexed display list

            // first triangle
            points.Add(Tuple.Create(-half, -half));
            points.Add(Tuple.Create(half, -half));
            points.Add(Tuple.Create(half, half));

            // second triangle
            points.Add(Tuple.Create(half, half));
            points.Add(Tuple.Create(-half, half));
            points.Add(Tuple.Create(-half, -half));

            return _factory.Geometry(uri, points);
        }

        private void GenerateMiddleRow(TNode root, int elementsPerDirection)
        {
            for (var j = 0; j < 2; j++ )
            {
                var parent = root;
                for (var i = 0; i < elementsPerDirection; i++)
                {
                    var child = _factory.Node();
                    parent.AddChild(child);
                    child.Geometry = parent.Geometry;
                    var offset = _offset;
                    if ((i + j) % 2 == 1)
                    {
                        offset *= -1;
                    }
                    child.Transformation = _factory.TransformationGroup(
                        _factory.FlipTransformation(Math.PI / 2),
                        _factory.TranslationTransformation(offset, 0)
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
                    var offset = _offset;
                    if ((i + j) % 2 == 1)
                    {
                        offset *= -1;
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
