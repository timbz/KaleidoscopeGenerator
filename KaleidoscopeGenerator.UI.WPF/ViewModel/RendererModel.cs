using System.ComponentModel;

namespace KaleidoscopeGenerator.UI.WPF.ViewModel
{
    public class RendererModel : INotifyPropertyChanged
    {
        private string _rendererType;
        private string[] _availableRendererTypes;

        public RendererModel()
        {
            _availableRendererTypes = new string[] { "2D", "3D" };
            RendererType = _availableRendererTypes[0];
        }

        public string[] AvailableRendererTypes
        {
            get
            {
                return _availableRendererTypes;
            }
        }

        public string RendererTypeOther
        {
            get
            {
                foreach (var type in AvailableRendererTypes)
                {
                    if (RendererType != type)
                        return type;
                }
                return null;
            }
        }

        public string RendererType
        {
            get
            {
                return _rendererType;
            }
            set
            {
                if (RendererType != value)
                {
                    _rendererType = value;
                    OnPropertyChanged("RendererType");
                    OnPropertyChanged("RendererTypeOther");
                }
            }
        }

        public void SwitchRendererType()
        {
            RendererType = RendererTypeOther;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
