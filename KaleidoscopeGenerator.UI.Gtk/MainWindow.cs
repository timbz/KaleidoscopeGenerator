using Gtk;
using System;
using System.IO;
using KaleidoscopeGenerator.Data;

namespace KaleidoscopeGenerator.UI.Gtk
{

	public partial class MainWindow: Window
	{
		private SettingsModel _model;
		private IRenderer _renderer;

		public MainWindow () : base (WindowType.Toplevel)
		{
			Build ();
			_model = new SettingsModel ();
			_renderer = new Cairo.CairoRenderer ();

			_drawingArea.AppPaintable = true;
			_drawingArea.DoubleBuffered = false;
			_drawingArea.ExposeEvent += OnExpose;

			_geometryWidthSpinButton.Value = _model.GeometyWidth;
			_geometryWidthSpinButton.Changed += OnGeometryWidthTextBoxChanged;

			_typeComboBox.AppendText("3");
			_typeComboBox.AppendText("4");
			_typeComboBox.Active = 0;
			_typeComboBox.Changed += OnTypeComboBoxChanged;

			_imageSelectButton.Clicked += OnImageSelectButtonClicked;

			RenderBaseImagePreview ();
		}

		private void OnImageSelectButtonClicked (object sender, EventArgs args)
		{
			FileChooserDialog fileChooser = new FileChooserDialog (
				"Choose an image file to open",
				this,
				FileChooserAction.Open,
				"Cancel", ResponseType.Cancel,
				"Open", ResponseType.Accept);
			var filter = new FileFilter ();
			filter.Name = "Images";
			filter.AddMimeType ("image/*");
			fileChooser.AddFilter (filter);
			if (fileChooser.Run() == (int)ResponseType.Accept) 
			{
				_model.ImageUri = new Uri (fileChooser.Filename);
				RenderBaseImagePreview ();
				RenderKaleidoscope ();
			}
			fileChooser.Destroy();
			filter.Destroy ();
		}

		private void OnTypeComboBoxChanged (object sender, EventArgs args)
		{
			_model.SetType(((ComboBox)sender).ActiveText);
			RenderKaleidoscope ();
		}

		private void OnGeometryWidthTextBoxChanged (object sender, EventArgs args)
		{
			_model.GeometyWidth = ((SpinButton)sender).ValueAsInt;
			RenderKaleidoscope ();
		}

		private void OnDeleteEvent (object sender, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}

		private void OnExpose (object sender, ExposeEventArgs args)
		{
			RenderKaleidoscope ();
		}

		private void RenderBaseImagePreview()
		{
			var pixBuf = new Gdk.Pixbuf (_model.ImageUri.OriginalString);
			var scale = (float)_previewImage.Parent.Allocation.Width / (float)pixBuf.Width;
			var scaledWidht = (int)(pixBuf.Width * scale);
			var scaledHeight = (int)(pixBuf.Height * scale);
			var scaledPixBuf = pixBuf.ScaleSimple (scaledWidht, scaledHeight, Gdk.InterpType.Bilinear);
			_previewImage.Pixbuf = scaledPixBuf;
			pixBuf.Dispose ();
			scaledPixBuf.Dispose ();
		}

		private void RenderKaleidoscope()
		{
			_renderer.Render (_drawingArea, _model);
		}
	}
}