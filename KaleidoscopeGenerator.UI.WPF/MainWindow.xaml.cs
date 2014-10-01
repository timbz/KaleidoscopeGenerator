using KaleidoscopeGenerator.Data;
using KaleidoscopeGenerator.UI.WPF.FileSystem;
using KaleidoscopeGenerator.UI.WPF.ViewModel;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace KaleidoscopeGenerator.UI.WPF
{
    public partial class MainWindow : Window
    {
        private IInteraction _fileSystem;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppModel()
            {
                Settings = new SettingsModel(),
                Renderer = new RendererModel()
            };
            _fileSystem = new XmlInteraction();
        }

        void OnClickSwitchRenderer(object sender, RoutedEventArgs a)
        {
            ((AppModel)DataContext).Renderer.SwitchRendererType();
        }

        void OnClickSelectImage(object sender, RoutedEventArgs a)
        {
            var dlg = new OpenFileDialog();
            // dlg.Filter = "Text documents (.jpg)|*.png"; // do we need a filter?

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                var settings = ((AppModel)DataContext).Settings;
                var oldPaht = settings.ImagePath;
                try
                {
                    settings.ImagePath = dlg.FileName;
                }
                catch (Exception e)
                {
                    settings.ImagePath = oldPaht;
                    MessageBox.Show(e.Message);
                }
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
                    var settings = ((AppModel)DataContext).Settings;
                    _fileSystem.save(settings, dlg.FileName);
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
                    ((AppModel)DataContext).Settings.Merge(loadedSettings);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        
    }
}
