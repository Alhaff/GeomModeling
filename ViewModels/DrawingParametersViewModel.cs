using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class DrawingParametersViewModel : ViewModelBase
    {
        private Lab6ViewModel _modelViewModel;
        private LinearTransformationViewModel _linear2DViewModel;
        private LinearTransformation3DViewModel _linear3DViewModel;
        private ProjectionSetingsViewModel _projectionSetingsViewModel;

        public DrawingParametersViewModel(DrawingEngineService drawingEngineService, Lab6ViewModel model, LinearTransformationViewModel linear2D, LinearTransformation3DViewModel linear3D,
           ProjectionSetingsViewModel projectionSetingsViewModel)
        {
            DrawingEngine = drawingEngineService;
            ModelViewModel = model;
            Linear2DViewModel = linear2D;
            Linear3DViewModel = linear3D;
            ProjectionViewModel = projectionSetingsViewModel;
        }

        public DrawingEngineService DrawingEngine { get; set; }
        public Lab6ViewModel ModelViewModel
        {
            get => _modelViewModel;
            set => SetProperty(ref _modelViewModel, value, nameof(ModelViewModel));
        }
        public LinearTransformationViewModel Linear2DViewModel
        {
            get => _linear2DViewModel;
            set => SetProperty(ref _linear2DViewModel, value, nameof(Linear2DViewModel));
        }
        public LinearTransformation3DViewModel Linear3DViewModel
        {
            get => _linear3DViewModel;
            set => SetProperty(ref _linear3DViewModel, value, nameof(Linear3DViewModel));
        }
        public ProjectionSetingsViewModel ProjectionViewModel
        {
            get => _projectionSetingsViewModel;
            set => SetProperty(ref _projectionSetingsViewModel, value, nameof(ProjectionViewModel));
        }

       
        public int PixelsInCell
        {
            get => DrawingEngine.PixelsInCell;
        }

        public void SetPixelsInCell(string text)
        {
            int NewValue = 0;
            bool isSuccesses = int.TryParse(text,out NewValue);
            if (isSuccesses)
            {
                DrawingEngine.PixelsInCell = NewValue;
            }
            OnPropertyChanged(nameof(PixelsInCell));
        }
        
        private RelayCommand<String> _setPixelsInCellCommand;
        public RelayCommand<String> SetPixelsInCellCommand
        {
            get => _setPixelsInCellCommand ?? (_setPixelsInCellCommand = new RelayCommand<String>(SetPixelsInCell));
        }
    }
}
