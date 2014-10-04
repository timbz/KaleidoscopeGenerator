using System;
using Cairo;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk.Cairo
{
	public class CairoTransformation : ITransformation<CairoNode, CairoGeometry, CairoTransformation>
	{
		private Matrix _matrix;

		public void initAsFlip (double angle)
		{
			var cos = Math.Cos(angle * 2);
			var sin = Math.Sin(angle * 2);
			_matrix = new Matrix(cos, sin, sin, -cos, 0, 0);
		}

		public void initAsTranslation (double x, double y)
		{
			_matrix = new Matrix (1, 0, 0, 1, x, y);
		}

		public void initAsGroup (CairoTransformation[] transformatins)
		{
			_matrix = new Matrix ();
			foreach (var trans in transformatins) {
				_matrix.Multiply(trans._matrix);
			}
		}

		public void Apply(Context context)
		{
			context.Transform (_matrix);
		}

		public CairoTransformation Clone ()
		{
			var clone = new CairoTransformation ();
			clone._matrix = (Matrix)(_matrix.Clone ());
			return clone;
		}
	}
}

