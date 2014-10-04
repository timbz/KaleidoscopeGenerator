using System;
using System.IO;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk
{
	public class SettingsModel
	{
		public SettingsModel ()
		{
			ImageUri = new Uri (Path.GetFullPath (@"Assets/default.png"));
			Type = KaleidoscopeTypes.Triangle;
			GeometyWidth = 200;
		}

		public Uri ImageUri { get; set; }

		public KaleidoscopeTypes Type { get; set; }

		public void SetType(string s)
		{
			if (s == "4") {
				Type = KaleidoscopeTypes.Square;
			} else {
				Type = KaleidoscopeTypes.Triangle;
			}
		}

		public double GeometyWidth { get; set; }
	}
}

