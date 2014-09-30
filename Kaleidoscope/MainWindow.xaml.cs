using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
using System;
using System.Xml.Serialization;
using System.IO;
using KaleidoscopeGenerator.UI.WPF.FileSystem;
using System.Reflection;

namespace KaleidoscopeGenerator.UI.WPF
{
    public partial class MainWindow : Window
    {
        private IInteraction _fileSystem;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new SettingsModel();
            _fileSystem = new XmlInteraction();
        }

        void OnClickSelectImage(object sender, RoutedEventArgs a)
		{
			var dlg = new OpenFileDialog();
            // dlg.Filter = "Text documents (.jpg)|*.png"; // do we need a filter?

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                var settings = DataContext as SettingsModel;
                settings.ImagePath = dlg.FileName;
            }
		}

        void OnClickSaveSettings(object sender, RoutedEventArgs a)
        {   
            var dlg = new SaveFileDialog();
            dlg.FileName = "settings";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    _fileSystem.save(DataContext, dlg.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }


        void OnClickLoadSettings(object sender, RoutedEventArgs a)
        {   
            var dlg = new OpenFileDialog();
            dlg.FileName = "settings";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    var loadedSettings = _fileSystem.load<SettingsModel>(dlg.FileName);
                    ((SettingsModel)DataContext).Merge(loadedSettings);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        
    }
}
