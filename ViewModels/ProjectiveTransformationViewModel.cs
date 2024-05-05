using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class ProjectiveTransformationViewModel : Global2DTransformationBaseViewModel
    {
        public DrawingEngineService DrawingEngine { get; set; }
        public ProjectiveTransformationViewModel(Transformation2DService transformation2DService, DrawingEngineService drawingEngine) : base(transformation2DService)
        {
            DrawingEngine = drawingEngine;
            R0X = TransformationService.Projective.R0Point.Center.X;
            R0Y = TransformationService.Projective.R0Point.Center.Y;
            R0W = TransformationService.Projective.R0Point.Center.Z;
            RxX = TransformationService.Projective.RXPoint.Center.X;
            RxY = TransformationService.Projective.RXPoint.Center.Y;
            RxW = TransformationService.Projective.RXPoint.Center.Z;
            RyX = TransformationService.Projective.RYPoint.Center.X;
            RyY = TransformationService.Projective.RYPoint.Center.Y;
            RyW = TransformationService.Projective.RYPoint.Center.Z;
            TransformationService.Projective.R0Point.PropertyChanged += (v, e) =>
            {
                if(e.PropertyName == "Center")
                {
                    R0X = TransformationService.Projective.R0Point.Center.X;
                    R0Y = TransformationService.Projective.R0Point.Center.Y;
                    R0W = TransformationService.Projective.R0Point.Center.Z;
                }
            };
            TransformationService.Projective.RXPoint.PropertyChanged += (v, e) =>
            {
                if (e.PropertyName == "Center")
                {
                    RxX = TransformationService.Projective.RXPoint.Center.X;
                    RxY = TransformationService.Projective.RXPoint.Center.Y;
                    RxW = TransformationService.Projective.RXPoint.Center.Z;
                }
            };
            TransformationService.Projective.RYPoint.PropertyChanged += (v, e) =>
            {
                if (e.PropertyName == "Center")
                {
                    RyX = TransformationService.Projective.RYPoint.Center.X;
                    RyY = TransformationService.Projective.RYPoint.Center.Y;
                    RyW = TransformationService.Projective.RYPoint.Center.Z;
                }
            };
        }

        public override void SetR0()
        {
            TransformationService.Projective.R0Point.Center
                = new System.Numerics.Vector3(R0X, R0Y, R0W);
        }
        public override void SetRX()
        {
            TransformationService.Projective.RXPoint.Center
                 = new System.Numerics.Vector3(RxX, RxY, RxW);
        }
        public override void SetRY()
        {
            TransformationService.Projective.RYPoint.Center
                = new System.Numerics.Vector3(RyX, RyY, RyW);
        }
        private bool IsApplied = false;

        public void CeaseProjective()
        {
            if (IsApplied)
            {
                TransformationService.Projective.R0Point.RemovePointFromCanvas(DrawingEngine.Canvas);
                TransformationService.Projective.RXPoint.RemovePointFromCanvas(DrawingEngine.Canvas);
                TransformationService.Projective.RYPoint.RemovePointFromCanvas(DrawingEngine.Canvas);
                DrawingEngine.Transformations.RemoveTransformation(TransformationService.Projective);
                DrawingEngine.Transformations.AddTransformation(TransformationService.Affine);
                IsApplied = false;
            }
        }

        public void ApplyProjective()
        {
            if (!IsApplied)
            {
                TransformationService.Projective.R0Point.AddPointOnCanvas(DrawingEngine.Canvas);
                TransformationService.Projective.RXPoint.AddPointOnCanvas(DrawingEngine.Canvas);
                TransformationService.Projective.RYPoint.AddPointOnCanvas(DrawingEngine.Canvas);
                DrawingEngine.Transformations.AddTransformation(TransformationService.Projective);
                DrawingEngine.Transformations.RemoveTransformation(TransformationService.Affine);
                IsApplied = true;
            }
        }

        private RelayCommand _ceaseProjectiveCommand;
        public RelayCommand CeaseProjectiveCommand
        {
            get => _ceaseProjectiveCommand ?? (_ceaseProjectiveCommand = new RelayCommand(CeaseProjective));
        }
        private RelayCommand _applyProjectiveCommand;
        public RelayCommand ApplyProjectiveCommand
        {
            get => _applyProjectiveCommand ?? (_applyProjectiveCommand = new RelayCommand(ApplyProjective));
        }
    }
}
