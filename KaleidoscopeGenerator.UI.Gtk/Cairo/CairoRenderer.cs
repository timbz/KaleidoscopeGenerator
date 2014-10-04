using System;
using Gtk;
using Cairo;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk.Cairo
{
	public class CairoRenderer : IRenderer
	{
		KaleidoscopeFactory<CairoNode, CairoGeometry, CairoTransformation> _factory;

		public CairoRenderer ()
		{
			_factory = new KaleidoscopeFactory<CairoNode, CairoGeometry, CairoTransformation> ();
		}

		public void Render (DrawingArea area, SettingsModel settings)
		{
			var width = area.Allocation.Width;
			var height = area.Allocation.Height;

			var kaleidoscope = _factory.Get (settings.Type);
			var rootNode = kaleidoscope.Generate (
				settings.GeometyWidth, 
				settings.ImageUri,
				width, height);

			ImageSurface surface = new ImageSurface(Format.Argb32, width, height); 

			using (var context = new Context (surface)) {
				context.Translate(width / 2, height / 2);
				rootNode.Render (context);
			}
			rootNode.Geometry.Dispose ();

			using (Context context = Gdk.CairoHelper.Create (area.GdkWindow)) {
				context.Rectangle(0, 0, width, height); 
				context.SetSource(surface); 
				context.Fill(); 
				context.GetTarget ().Dispose ();
			}
			surface.Dispose ();
		}
	}
}

