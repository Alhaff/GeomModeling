using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformations.Transformations2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GeometricModeling.Models.DrawingModels;

namespace GeometricModeling.Services
{
    public class Transformation2DService : ObservableObject
    {
        #region
        private Offset2DTransformation _offset;
        private Affine2DTransformation _affine;
        private ProjectiveTransformation _projective;
        private Rotate2DTransformation _rotate;
        #endregion
        private DrawingEngineService DrawingEngine { get; set; }
        public Transformation2DService(DrawingEngineService drawingEngineService) 
        { 
            DrawingEngine = drawingEngineService;
            Offset = new Offset2DTransformation();
            Affine = new Affine2DTransformation();
            Rotate = new Rotate2DTransformation(new DragAblePoint(DrawingEngine, 10, 10, 1, Color.Red));
            DrawingEngine.Models.AddModel(Rotate.RotatePoint);
            Projective = new ProjectiveTransformation(
                                    new DragAblePoint(DrawingEngine, 0, 0, 1500, Color.Blue),
                                    new DragAblePoint(DrawingEngine, 10, 0, 100, Color.Blue),
                                    new DragAblePoint(DrawingEngine, 0, 10, 100, Color.Blue));
            Projective.R0Point.IsAffectedByExternalTransformation = false;
            Projective.RXPoint.IsAffectedByExternalTransformation = false;
            Projective.RYPoint.IsAffectedByExternalTransformation = false;
            DrawingEngine.Models.AddModel(Projective.RXPoint);
            DrawingEngine.Models.AddModel(Projective.RYPoint);
            DrawingEngine.Models.AddModel(Projective.R0Point);
            DrawingEngine.PropertyChanged += (e, a) =>
            {
                if (a.PropertyName == "Canvas")
                {
                    Rotate.RotatePoint.AddPointOnCanvas(DrawingEngine.Canvas);
                }
            };
        }

        public static double AngleFromRadianToDegree(double angleInRadian)
        {
            return angleInRadian * 180 / Math.PI;
        }
        public static double AngleFromDegreeToRadian(double angleInDegree)
        {
            return angleInDegree * Math.PI / 180;
        }
        public Offset2DTransformation Offset
        {
            get => _offset;
            set => SetProperty(ref _offset, value, nameof(Offset));
        }

        public Rotate2DTransformation Rotate
        {
            get => _rotate;
            set => SetProperty(ref _rotate, value, nameof(Rotate));
        }

        public Affine2DTransformation Affine 
        {
            get => _affine;
            set => SetProperty(ref _affine, value, nameof(Affine));
        }

        public ProjectiveTransformation Projective
        {
            get => _projective;
            set => SetProperty(ref _projective, value, nameof(Projective));
        }
    }
}
