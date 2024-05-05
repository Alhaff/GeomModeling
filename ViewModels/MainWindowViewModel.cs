using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GeometricModeling.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _drawingEngineViewModel;
        private ViewModelBase _drawingParametersViewModel;
        SolidColorBrush _color;
        public SolidColorBrush BackgroundColor 
        { 
            get => _color; 
            set => SetProperty(ref _color, value, nameof(BackgroundColor));
        }
        public MainWindowViewModel(DrawingEngineViewModel drawingEngineViewModel, DrawingParametersViewModel drawingParametersViewModel)
        {
            DrawingEngineView = drawingEngineViewModel;
            DrawingParametrsView = drawingParametersViewModel;
            BackgroundColor = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        }
        public ViewModelBase DrawingEngineView 
        {
            get => _drawingEngineViewModel;
            set => SetProperty(ref _drawingEngineViewModel, value, nameof(DrawingEngineView));
        }

        public ViewModelBase DrawingParametrsView 
        {
            get => _drawingParametersViewModel;
            set => SetProperty(ref _drawingParametersViewModel, value, nameof(DrawingParametrsView));
        }
    }
}
