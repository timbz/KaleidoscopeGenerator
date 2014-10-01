using NSpec;
using KaleidoscopeGenerator.Data;
using KaleidoscopeGenerator.Data.Test.Mocks;
using System;

namespace KaleidoscopeGenerator.Data.Test.Specs
{
    class given_a_triangle_kaleidoscope : nspec
    {
        IKaleidoscope<NodeMock, GeometryMock, TransformationMock> _kaleidoscope;
        NodeMock _node;
        GeometryMock _geometry;
        TransformationMock _transformation;

        void before_each()
        {
            var factory = new KaleidoscopeFactory<NodeMock, GeometryMock, TransformationMock>();
            _kaleidoscope = factory.Get(KaleidoscopeTypes.Triangle);
        }

        void it_should_not_be_null()
        {
            _kaleidoscope.should_not_be_null();
        }

        void given_a_generated_node() 
        {
            before = () => _node = _kaleidoscope.Generate(10, null, 100, 100);

            it["should not be null"] = () => _node.should_not_be_null();
            it["should have 4 children"] = () => _node.Children.Count.should_be(4);

            context["given its geometry"] = () =>
            {
                before = () => _geometry = _node.Geometry;
                it["should not be null"] = () => _geometry.should_not_be_null();
                it["should contain 3 points"] = () => _geometry.Points.Count.should_be(3);
            };

            context["given its transformation"] = () =>
            {
                before = () => _transformation = _node.Transformation;
                it["should not be null"] = () => _transformation.should_not_be_null();
                it["should be a translation"] = () =>
                    _transformation.Type.should_be(TransformationMock.TransformationType.Translation);
            };

            context["given its first child"] = () =>
            {
                before = () => _node = _node.Children[0];
                it["should not be null"] = () => _node.should_not_be_null();
                it["should have a geometry"] = () => _node.Geometry.should_not_be_null();
                it["should have a geometry with 3 points"] = () => _node.Geometry.Points.Count.should_be(3);
                it["should have a translation and flip transformation"] = () =>
                {
                    _node.Transformation.should_not_be_null();
                    _node.Transformation.Type.should_be(TransformationMock.TransformationType.Group);
                    _node.Transformation.Parameters.should_not_be_null();
                    _node.Transformation.Parameters.Length.should_be(2);
                    ((TransformationMock)_node.Transformation.Parameters[0]).Type.should_be(TransformationMock.TransformationType.Flip);
                    ((TransformationMock)_node.Transformation.Parameters[1]).Type.should_be(TransformationMock.TransformationType.Translation);
                };
            };

            // TODO: roughly check positioning
        }
    }
}
