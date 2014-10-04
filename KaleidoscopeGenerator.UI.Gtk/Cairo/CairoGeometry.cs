using System;
using System.Collections.Generic;
using Cairo;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk.Cairo
{
	public class CairoGeometry : IGeometry<CairoNode, CairoGeometry, CairoTransformation>, IDisposable
	{			
		private List<Tuple<double, double>> _points;
		private Gdk.Pixbuf _pixBuf;
		private Gdk.Pixbuf _scaledPixBuf;

		public List<Tuple<double, double>> Points 
		{ 
			get
			{
				return _points;
			} 
			set
			{
				_points = value;
				if (_pixBuf != null) {
					GenerateScaledBuffer ();
				}
			}
		}

		public Uri ImageUri 
		{ 
			set {
				if (_pixBuf != null) {
					_pixBuf.Dispose ();
				}
				_pixBuf = new Gdk.Pixbuf (value.OriginalString);
				if (_points != null) {
					GenerateScaledBuffer ();
				}
			} 
		}

		private void GenerateScaledBuffer()
		{
			var xMin = 0.0;
			var xMax = 0.0;
			var yMin = 0.0;
			var yMax = 0.0;
			foreach (var point in Points)
			{
				if (point.Item1 < xMin)
					xMin = point.Item1;
				if (point.Item1 > xMax)
					xMax = point.Item1;
				if (point.Item1 < yMin)
					yMin = point.Item2;
				if (point.Item1 > yMax)
					yMax = point.Item2;
			}
			var width = xMax - xMin;
			var height = yMax - yMin;

			var xDiff = _pixBuf.Width - width;
			var yDiff = _pixBuf.Height - height; 

			var scale = 1.0;
			if (xDiff > yDiff) {
				scale = height / _pixBuf.Height;
			} else {
				scale = width / _pixBuf.Width;
			}

			var scaledWidth = (int)(_pixBuf.Width * scale);
			var scaledHeight = (int)(_pixBuf.Height * scale);

			if (_scaledPixBuf != null) {
				_scaledPixBuf.Dispose ();
			}
			_scaledPixBuf = _pixBuf.ScaleSimple (scaledWidth, scaledHeight, Gdk.InterpType.Bilinear);
		}

		public void Dispose()
		{
			if (_pixBuf != null)
				_pixBuf.Dispose();
			if (_scaledPixBuf != null)
				_scaledPixBuf.Dispose ();
		}

		public void SetSourcePixbuf (Context context)
		{
			var xMin = 0.0;
			var yMin = 0.0;
			foreach (var point in Points)
			{
				if (point.Item1 < xMin)
					xMin = point.Item1;
				if (point.Item1 < yMin)
					yMin = point.Item2;
			}
			Gdk.CairoHelper.SetSourcePixbuf(context, _scaledPixBuf, xMin, yMin);
		}

		public void Render (Context context)
		{
			SetSourcePixbuf (context);

			if (Points.Count > 0) {
				context.NewPath ();
				var first = true;
				foreach (var p in Points) {
					if (first) {
						first = false;
						context.MoveTo (p.Item1, p.Item2);
					} else {
						context.LineTo (p.Item1, p.Item2);
					}
				}
				context.ClosePath ();
				context.Fill ();
			}
		}
	}
}

