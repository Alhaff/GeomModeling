using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class LinearTransformationViewModel : ViewModelBase
    {

        public DrawingEngineService DrawingEngine { get; set; }
        public Transformation2DService TransformationService { get; set; }
        public LinearTransformationViewModel(DrawingEngineService drawingEngineService, Transformation2DService transformation2DService)
        {
            DrawingEngine = drawingEngineService;
            TransformationService = transformation2DService;
            //TransformationService.Rotate.RotatePoint.AddPointOnCanvas(DrawingEngine.Canvas);
            _dx = TransformationService.Offset.DX;
            _dy = TransformationService.Offset.DY;
            _centerX = TransformationService.Rotate.RotatePoint.Center.X;
            _centerY = TransformationService.Rotate.RotatePoint.Center.Y;
            _angle = Transformation2DService.AngleFromRadianToDegree(TransformationService.Rotate.Angle);
            TransformationService.Rotate.RotatePoint.PropertyChanged += RotatePoint_PropertyChanged;
        }

        private void RotatePoint_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Center")
            {
                CenterX = TransformationService.Rotate.RotatePoint.Center.X;
                CenterY = TransformationService.Rotate.RotatePoint.Center.Y;
            }
        }

        private float _dx;
        public float OffsetDX
        {
            get => _dx;
            set => SetProperty(ref _dx, value, nameof(OffsetDX));
        }
        private float _dy;
        public float OffsetDY
        {
            get => _dy;
            set => SetProperty(ref _dy, value, nameof(OffsetDY));
        }
        private double _angle;

        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value, nameof(Angle));
        }
        private float _centerX;
        public float CenterX
        {
            get => _centerX;
            set => SetProperty(ref _centerX, value,nameof(CenterX));
        }
        private float _centerY;
        public float CenterY
        {
            get => _centerY;
            set => SetProperty(ref _centerY, value, nameof(CenterY));
        }
        public void SetOffsetDX()
        { 
            TransformationService.Offset.DX = OffsetDX; 
        }
        public void SetOffsetDY()
        {  
            TransformationService.Offset.DY = OffsetDY;
        }
        public void SetAngle()
        {
            TransformationService.Rotate.Angle = Transformation2DService.AngleFromDegreeToRadian(_angle);
        }
        public void SetCenter() 
        {
            TransformationService.Rotate.RotatePoint.Center
                = new System.Numerics.Vector3(CenterX, CenterY, TransformationService.Rotate.RotatePoint.Center.Z);
        }
        public void AddOffsetDX()
        {
         
                TransformationService.Offset.DX += OffsetDX; 
        }
        public void AddOffsetDY()
        {
                TransformationService.Offset.DY += OffsetDY; 
        }

        public void AddAngle()
        {
            TransformationService.Rotate.Angle += Transformation2DService.AngleFromDegreeToRadian(_angle);
        }

        private RelayCommand _setOffsetDXCommand;
        public RelayCommand SetOffsetDXCommand
        {
            get => _setOffsetDXCommand ?? (_setOffsetDXCommand = new RelayCommand(SetOffsetDX));
        }
        private RelayCommand _setOffsetDYCommand;
        public RelayCommand SetOffsetDYCommand
        {
            get => _setOffsetDYCommand ?? (_setOffsetDYCommand = new RelayCommand(SetOffsetDY));
        }

        private RelayCommand _addOffsetDXCommand;
        public RelayCommand AddOffsetDXCommand
        {
            get => _addOffsetDXCommand ?? (_addOffsetDXCommand = new RelayCommand(AddOffsetDX));
        }
        private RelayCommand _addOffsetDYCommand;
        public RelayCommand AddOffsetDYCommand
        {
            get => _addOffsetDYCommand ?? (_addOffsetDYCommand = new RelayCommand(AddOffsetDY));
        }

        private RelayCommand _setAngleCommand;
        public RelayCommand SetAngleCommand
        {
            get => _setAngleCommand ?? (_setAngleCommand = new RelayCommand(SetAngle));
        }

        private RelayCommand _addAngleCommand;
        public RelayCommand AddAngleCommand
        {
            get => _addAngleCommand ?? (_addAngleCommand = new RelayCommand(AddAngle));
        }

        private RelayCommand _setCenterCommand;
        public RelayCommand SetCenterCommand
        {
            get => _setCenterCommand ?? (_setCenterCommand = new RelayCommand(SetCenter));
        }
    }
}

