using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class Global2DTransformationBaseViewModel : ViewModelBase
    {
        public Transformation2DService TransformationService { get; set; }
        public Global2DTransformationBaseViewModel(Transformation2DService transformation2DService)
        {
            TransformationService = transformation2DService;
        }
        private float _r0X;
        public float R0X
        {
            get => _r0X;
            set => SetProperty(ref _r0X, value, nameof(R0X));
        }
        private float _r0Y;
        public float R0Y
        {
            get => _r0Y;
            set => SetProperty(ref _r0Y, value, nameof(R0Y));
        }
        private float _r0W;
        public float R0W
        {
            get => _r0W;
            set => SetProperty(ref _r0W, value, nameof(R0W));
        }

        private float _rxX;
        public float RxX
        {
            get => _rxX;
            set => SetProperty(ref _rxX, value, nameof(RxX));
        }
        private float _rxY;
        public float RxY
        {
            get => _rxY;
            set => SetProperty(ref _rxY, value, nameof(RxY));
        }
        private float _rxW;
        public float RxW
        {
            get => _rxW;
            set => SetProperty(ref _rxW, value, nameof(RxW));
        }
        private float _ryX;
        public float RyX
        {
            get => _ryX;
            set => SetProperty(ref _ryX, value, nameof(RyX));
        }
        private float _ryY;
        public float RyY
        {
            get => _ryY;
            set => SetProperty(ref _ryY, value, nameof(RyY));
        }
        private float _ryW;
        public float RyW
        {
            get => _ryW;
            set => SetProperty(ref _ryW, value, nameof(RyW));
        }

        public virtual void SetR0()
        {
            TransformationService.Affine.R0
                = new System.Numerics.Vector3(R0X, R0Y, R0W);
        }
        public virtual void SetRX()
        {
            TransformationService.Affine.R0
                = new System.Numerics.Vector3(R0X, R0Y, R0W);
        }
        public virtual void SetRY()
        {
            TransformationService.Affine.R0
                = new System.Numerics.Vector3(R0X, R0Y, R0W);
        }

        private RelayCommand _setR0Command;
        public RelayCommand SetR0Command
        {
            get => _setR0Command ?? (_setR0Command = new RelayCommand(SetR0));
        }
        private RelayCommand _setRXCommand;
        public RelayCommand SetRXCommand
        {
            get => _setRXCommand ?? (_setRXCommand = new RelayCommand(SetRX));
        }
        private RelayCommand _setRYCommand;
        public RelayCommand SetRYCommand
        {
            get => _setRYCommand ?? (_setRYCommand = new RelayCommand(SetRY));
        }
    }
}
