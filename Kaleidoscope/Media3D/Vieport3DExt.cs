﻿using KaleidoscopeGenerator.Data;
using KaleidoscopeGenerator.UI.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KaleidoscopeGenerator.UI.WPF.Media3D
{

    public class Vieport3DExt : Viewport3D
    {
        private KaleidoscopeFactory<Node3D, Geometry3D, Transformation3D> _kaleidoscopes;
        private Light _light;
        private PerspectiveCamera _camera; // store a referece to the PerspectiveCamera to access position
        
        public Vieport3DExt() : base()
        {
            _kaleidoscopes = new KaleidoscopeFactory<Node3D, Geometry3D, Transformation3D>();
            _light = new AmbientLight(Colors.White);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
            _camera = new PerspectiveCamera()
            {
                Position= new Point3D(0, 0, 1067), 
                LookDirection = new Vector3D(0, 0, -40),
                UpDirection = new Vector3D(0, -1, 0)
            };
            Camera = _camera;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var settings = ((AppModel)DataContext).Settings;
            settings.PropertyChanged += OnPropertyChange;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // TODO: looks like context is always null, maybe we dont need this
            var context = DataContext as AppModel;
            if (context != null)
            {
                context.Settings.PropertyChanged -= OnPropertyChange;
            }
        }

        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            var settings = sender as SettingsModel;
            if (settings != null)
            {
                RenderKaleidoscope(settings);
            }
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            var context = DataContext as AppModel;
            if (context != null)
            {
                SetCameraPosition(); 
                RenderKaleidoscope(context.Settings);
            }
        }

        private void SetCameraPosition()
        {
            // this position calculation makes the resulting image look like the 2d version
            var z = ActualWidth * 1067 / 884;
            _camera.Position = new Point3D(0, 0, z);
        }

        private void RenderKaleidoscope(SettingsModel settings)
        {
            var kaleidoscope = _kaleidoscopes.Get(settings.SelectedKaleidoscopeType.Type);
            var imageUri = new Uri(settings.ImagePath, UriKind.Absolute);
            var rootNode = kaleidoscope.Generate(settings.GeometryWidth, imageUri, ActualWidth, ActualHeight);

            var model = new ModelVisual3D();
            model.Content = rootNode.Model3D;

            Children.Clear();
            Children.Add(model);

            var light = new ModelVisual3D();
            light.Content = _light;
            Children.Add(light);
        }
    }
}
