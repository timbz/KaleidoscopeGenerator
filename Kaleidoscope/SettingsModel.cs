using KaleidoscopeGenerator.Data;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace KaleidoscopeGenerator.UI.WPF
{
    public class KaleidoscopeTypeComboxBoxItem
    {
        public string Name { get; set; }
        public KaleidoscopeType Type { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var item = obj as KaleidoscopeTypeComboxBoxItem;
            if (item == null)
            {
                return false;
            }
            return item.Type == Type;
        }
    }

    public class SettingsModel : INotifyPropertyChanged
    {
        private int _geometryWidth;
        private int _maxGeometryWidth;
        private int _minGeometryWidth;

        private string _imagePath;

        private KaleidoscopeTypeComboxBoxItem[] _availableKaleidoscopeTypes;
        private KaleidoscopeTypeComboxBoxItem _selectedKaleidoscopeType;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            _minGeometryWidth = 50;
            _maxGeometryWidth = 400;
            _geometryWidth = 200;
            _imagePath = @"Assets\default.png";

            _availableKaleidoscopeTypes = new KaleidoscopeTypeComboxBoxItem[] 
            {
                new KaleidoscopeTypeComboxBoxItem()
                {
                    Name = "3",
                    Type = KaleidoscopeType.TRIANGLE
                },
                new KaleidoscopeTypeComboxBoxItem()
                {
                    Name = "4",
                    Type = KaleidoscopeType.SQUARE
                }
            };
            _selectedKaleidoscopeType = _availableKaleidoscopeTypes[1];
        }

        public KaleidoscopeTypeComboxBoxItem[] AvailableKaleidoscopeTypes
        {
            get
            {
                return _availableKaleidoscopeTypes;
            }
        }

        public KaleidoscopeTypeComboxBoxItem SelectedKaleidoscopeType { 
            get 
            {
                return _selectedKaleidoscopeType;
            }
            set
            {
                if (SelectedKaleidoscopeType != value)
                {
                    _selectedKaleidoscopeType = value;
                    OnPropertyChanged("SelectedKaleidoscopeType");
                }
            }
        }

        public int MaxGeometryWidth
        {
            get
            {
                return _maxGeometryWidth;
            }
        }

        public int MinGeometryWidth
        {
            get
            {
                return _minGeometryWidth;
            }
        }

        public int GeometryWidth
        {
            get
            {
                return _geometryWidth;
            }
            set
            {
                if (value < MinGeometryWidth || value > MaxGeometryWidth)
                    throw new ArgumentException();
                if (GeometryWidth != value)
                {
                    _geometryWidth = value;
                    OnPropertyChanged("GeometryWidth");
                }
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (ImagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged("ImagePath");
                }
            }
        }

        public void Merge(SettingsModel toMerge)
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(toMerge));
                }
            }
        }
        private void OnPropertyChanged(String info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
