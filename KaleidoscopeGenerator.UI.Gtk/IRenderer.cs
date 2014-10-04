using System;
using Gtk;

namespace KaleidoscopeGenerator.UI.Gtk
{
	public interface IRenderer
	{
		void Render (DrawingArea area, SettingsModel settings);
	}
}

