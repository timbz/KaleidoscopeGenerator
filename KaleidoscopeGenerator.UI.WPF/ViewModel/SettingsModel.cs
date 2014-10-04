using KaleidoscopeGenerator.Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace KaleidoscopeGenerator.UI.WPF.ViewModel
{
    public class SettingsModel : INotifyPropertyChanged
    {
        public class KaleidoscopeTypeComboxBoxItem
        {
            public string Name { get; set; }
            public KaleidoscopeTypes Type { get; set; }

            public override int GetHashCode()
            {
                return Type.GetHashCode();
            }

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

        private int _geometryWidth;
        private int _maxGeometryWidth;
        private int _minGeometryWidth;

        private string _imagePath;

        private KaleidoscopeTypeComboxBoxItem[] _availableKaleidoscopeTypes;
        private KaleidoscopeTypeComboxBoxItem _selectedKaleidoscopeType;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            _minGeometryWidth = 100;
            _maxGeometryWidth = 400;
            _geometryWidth = 200;
            _imagePath = Path.GetFullPath(@"Assets/default.png");

            _availableKaleidoscopeTypes = new KaleidoscopeTypeComboxBoxItem[] 
            {
                new KaleidoscopeTypeComboxBoxItem()
                {
                    Name = "3",
                    Type = KaleidoscopeTypes.Triangle
                },
                new KaleidoscopeTypeComboxBoxItem()
                {
                    Name = "4",
                    Type = KaleidoscopeTypes.Square
                }
            };
            _selectedKaleidoscopeType = _availableKaleidoscopeTypes[0];
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
        
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
