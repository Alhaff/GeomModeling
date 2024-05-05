using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class LinearTransformation3DViewModel : ViewModelBase
    {
        #region Variables
        private float _centerX;
        private float _centerY;
        private float _centerZ;
        private float _dx;
        private float _dy;
        private float _dz;
        private double _angleX;
        private double _angleY;
        private double _angleZ;
        #endregion

        #region Propreties
        public float CenterX
        {
            get => _centerX;
            set => SetProperty(ref _centerX, value, nameof(CenterX));
        }
        public float CenterY
        {
            get => _centerY;
            set => SetProperty(ref _centerY, value, nameof(CenterY));
        }
        public float CenterZ
        {
            get => _centerZ;
            set => SetProperty(ref _centerZ, value, nameof(CenterZ));
        }

        public float OffsetDX
        {
            get => _dx;
            set => SetProperty(ref _dx, value, nameof(OffsetDX));
        }
        public float OffsetDY
        {
            get => _dy;
            set => SetProperty(ref _dy, value, nameof(OffsetDY));
        }
        public float OffsetDZ
        {
            get => _dz;
            set => SetProperty(ref _dz, value, nameof(OffsetDZ));
        }

        public double AngleX
        {
            get => _angleX;
            set => SetProperty(ref _angleX, value, nameof(AngleX));
        }
        public double AngleY
        {
            get => _angleY;
            set => SetProperty(ref _angleY, value, nameof(AngleY));
        }
        public double AngleZ
        {
            get => _angleZ;
            set => SetProperty(ref _angleZ, value, nameof(AngleZ));
        }
        #endregion
        public DrawingEngineService DrawingEngine { get; set; }    
        public Transformation3DService Transformation3D { get; set; }

        public LinearTransformation3DViewModel(DrawingEngineService drawingEngine, Transformation3DService transformation3D)
        {
            DrawingEngine = drawingEngine;
            Transformation3D = transformation3D;  
        }
      

        #region Commands

        #region OffsetCommands
        private RelayCommand _setOffsetDXCommand;
        public RelayCommand SetOffsetDXCommand
        {
            get => _setOffsetDXCommand ?? (_setOffsetDXCommand = new RelayCommand(() => Transformation3D.Offset3D.DX = OffsetDX));
        }

        private RelayCommand _addOffsetDXCommand;
        public RelayCommand AddOffsetDXCommand
        {
            get => _addOffsetDXCommand ?? (_addOffsetDXCommand = new RelayCommand(() => Transformation3D.Offset3D.DX += OffsetDX));
        }

        private RelayCommand _setOffsetDYCommand;
        public RelayCommand SetOffsetDYCommand
        {
            get => _setOffsetDYCommand ?? (_setOffsetDYCommand = new RelayCommand(() => Transformation3D.Offset3D.DY = OffsetDY));
        }

        private RelayCommand _addOffsetDYCommand;
        public RelayCommand AddOffsetDYCommand
        {
            get => _addOffsetDYCommand ?? (_addOffsetDYCommand = new RelayCommand(() => Transformation3D.Offset3D.DY += OffsetDY));
        }

        private RelayCommand _setOffsetDZCommand;
        public RelayCommand SetOffsetDZCommand
        {
            get => _setOffsetDZCommand ?? (_setOffsetDZCommand = new RelayCommand(() => Transformation3D.Offset3D.DZ = OffsetDZ));
        }

        private RelayCommand _addOffsetDZCommand;
        public RelayCommand AddOffsetDZCommand
        {
            get => _addOffsetDZCommand ?? (_addOffsetDZCommand = new RelayCommand(() => Transformation3D.Offset3D.DZ += OffsetDZ));
        }
        #endregion

        #region AngleCommands
        private RelayCommand _setAngleXCommand;
        public RelayCommand SetAngleXCommand
        {
            get => _setAngleXCommand ?? (_setAngleXCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleX = Transformation3DService.AngleFromDegreeToRadian(AngleX)));
        }

        private RelayCommand _addAngleXCommand;
        public RelayCommand AddAngleXCommand
        {
            get => _addAngleXCommand ?? (_addAngleXCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleX += Transformation3DService.AngleFromDegreeToRadian(AngleX)));
        }

        private RelayCommand _setAngleYCommand;
        public RelayCommand SetAngleYCommand
        {
            get => _setAngleYCommand ?? (_setAngleYCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleY = Transformation3DService.AngleFromDegreeToRadian(AngleY)));
        }

        private RelayCommand _addAngleYCommand;
        public RelayCommand AddAngleYCommand
        {
            get => _addAngleYCommand ?? (_addAngleYCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleY += Transformation3DService.AngleFromDegreeToRadian(AngleY)));
        }

        private RelayCommand _setAngleZCommand;
        public RelayCommand SetAngleZCommand
        {
            get => _setAngleZCommand ?? (_setAngleZCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleZ = Transformation3DService.AngleFromDegreeToRadian(AngleZ)));
        }

        private RelayCommand _addAngleZCommand;
        public RelayCommand AddAngleZCommand
        {
            get => _addAngleZCommand ?? (_addAngleZCommand = new RelayCommand(
                () => Transformation3D.Rotate3D.AngleZ += Transformation3DService.AngleFromDegreeToRadian(AngleZ)));
        }

        private async Task RotatingAnimationAroundXAxis()
        {
            var start = AngleX;
            for(int i = 0; i < 359; i++)
            {
                Transformation3D.Rotate3D.AngleX += Transformation3DService.AngleFromDegreeToRadian(1);
                AngleX += 1;
                await Task.Delay(10);
            }
            Transformation3D.Rotate3D.AngleX = Transformation3DService.AngleFromDegreeToRadian(start);
            AngleX = start;
        }

        private AsyncRelayCommand _rotatingAnimationAroundXAxisCommand;

        public AsyncRelayCommand RotatingAnimationAroundXAxisCommand
        {
            get => _rotatingAnimationAroundXAxisCommand?? (_rotatingAnimationAroundXAxisCommand = new AsyncRelayCommand(RotatingAnimationAroundXAxis));
        }
        private async Task RotatingAnimationAroundYAxis()
        {
            var start = AngleY;
            for (int i = 0; i < 359; i++)
            {
                Transformation3D.Rotate3D.AngleY += Transformation3DService.AngleFromDegreeToRadian(1);
                AngleY += 1;
                await Task.Delay(10);
            }
            Transformation3D.Rotate3D.AngleY = Transformation3DService.AngleFromDegreeToRadian(start);
            AngleY = start;
        }

        private AsyncRelayCommand _rotatingAnimationAroundYAxisCommand;

        public AsyncRelayCommand RotatingAnimationAroundYAxisCommand
        {
            get => _rotatingAnimationAroundYAxisCommand ?? (_rotatingAnimationAroundYAxisCommand = new AsyncRelayCommand(RotatingAnimationAroundYAxis));
        }
        private async Task RotatingAnimationAroundZAxis()
        {
            var start = AngleZ;
            for (int i = 0; i < 359; i++)
            {
                Transformation3D.Rotate3D.AngleZ += Transformation3DService.AngleFromDegreeToRadian(1);
                AngleZ += 1;
                await Task.Delay(10);
            }
            Transformation3D.Rotate3D.AngleZ = Transformation3DService.AngleFromDegreeToRadian(start);
            AngleZ = start;
        }

        private AsyncRelayCommand _rotatingAnimationAroundZAxisCommand;

        public AsyncRelayCommand RotatingAnimationAroundZAxisCommand
        {
            get => _rotatingAnimationAroundZAxisCommand ?? (_rotatingAnimationAroundZAxisCommand = new AsyncRelayCommand(RotatingAnimationAroundZAxis));
        }
        #endregion

        private RelayCommand _setCenterCommand;
        public RelayCommand SetCenterCommand
        {
            get => _setCenterCommand ?? (_setCenterCommand = 
                                           new RelayCommand(() =>Transformation3D.Rotate3D.Center 
                                               = new System.Numerics.Vector3(CenterX, CenterY, CenterZ)));
        }
        #endregion


    }
}
