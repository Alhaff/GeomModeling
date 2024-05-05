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
        private Lab3ViewModel _modelViewModel;
        private LinearTransformationViewModel _linearViewModel;
        private AffineTransformationViewModel _affineViewModel;
        private ProjectiveTransformationViewModel _projectiveViewModel;

        public DrawingParametersViewModel(DrawingEngineService drawingEngineService, Lab3ViewModel model, LinearTransformationViewModel linear,
            AffineTransformationViewModel affine, ProjectiveTransformationViewModel projective)
        {
            DrawingEngine = drawingEngineService;
            ModelViewModel = model;
            LinearViewModel = linear;
            AffineViewModel = affine;
            ProjectiveViewModel = projective;
        }

        public DrawingEngineService DrawingEngine { get; set; }
        public Lab3ViewModel ModelViewModel
        {
            get => _modelViewModel;
            set => SetProperty(ref _modelViewModel, value, nameof(ModelViewModel));
        }
        public LinearTransformationViewModel LinearViewModel
        {
            get => _linearViewModel;
            set => SetProperty(ref _linearViewModel, value, nameof(LinearViewModel));
        }
        public AffineTransformationViewModel AffineViewModel
        {
            get => _affineViewModel;
            set => SetProperty(ref _affineViewModel, value, nameof(AffineViewModel));
        }

        public ProjectiveTransformationViewModel ProjectiveViewModel
        {
            get => _projectiveViewModel;
            set => SetProperty(ref _projectiveViewModel, value, nameof(AffineViewModel));
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
