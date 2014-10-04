using System;
using System.Collections.Generic;
using Cairo;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk.Cairo
{
	public class CairoNode : INode<CairoNode, CairoGeometry, CairoTransformation>
	{	
		public List<CairoNode> Children { set; get; }

		public CairoNode ()
		{
			Children = new List<CairoNode> ();
		}

		public void AddChild (CairoNode child)
		{
			Children.Add (child);
		}

		public CairoNode Clone ()
		{
			var clone = new CairoNode ();
			clone.Geometry = Geometry;
			if (Transformation != null)
				clone.Transformation = Transformation.Clone();
			foreach (var child in Children)
			{
				clone.AddChild(child.Clone());
			}
			return clone;

		}

		public CairoGeometry Geometry { get; set; }

		public CairoTransformation Transformation { get; set; }

		public void Render (Context context)
		{
			context.Save ();
			if (Transformation != null) {
				Transformation.Apply (context);
			}
			Geometry.Render (context);
			foreach (var child in Children) {
				child.Render (context);
			}
			context.Restore ();
		}
	}
}

