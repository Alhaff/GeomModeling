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
        private Lab5ViewModel _modelViewModel;
        private LinearTransformation3DViewModel _linear3DViewModel;
        private ProjectionSetingsViewModel _projectionSetingsViewModel;

        public DrawingParametersViewModel(DrawingEngineService drawingEngineService, Lab5ViewModel model, LinearTransformation3DViewModel linear,
           ProjectionSetingsViewModel projectionSetingsViewModel)
        {
            DrawingEngine = drawingEngineService;
            ModelViewModel = model;
            LinearViewModel = linear;
            ProjectionViewModel = projectionSetingsViewModel;
        }

        public DrawingEngineService DrawingEngine { get; set; }
        public Lab5ViewModel ModelViewModel
        {
            get => _modelViewModel;
            set => SetProperty(ref _modelViewModel, value, nameof(ModelViewModel));
        }
        public LinearTransformation3DViewModel LinearViewModel
        {
            get => _linear3DViewModel;
            set => SetProperty(ref _linear3DViewModel, value, nameof(LinearViewModel));
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
