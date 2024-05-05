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
        private Reducing2DTransformation _reducingTransformation;
        private Rotate2DTransformation _rotate;
        #endregion
        private DrawingEngineService DrawingEngine { get; set; }
        public Transformation2DService(DrawingEngineService drawingEngineService) 
        { 
            DrawingEngine = drawingEngineService;
            Offset = new Offset2DTransformation();
            Offset.ApplyPriority = 2;
            Affine = new Affine2DTransformation();
            Affine.ApplyPriority = 4;
            Rotate = new Rotate2DTransformation(new DragAblePoint(DrawingEngine, 0, 0, 0, Color.Red));
            Rotate.ApplyPriority = 3;
            DrawingEngine.Models.AddModel(Rotate.RotatePoint);
            ReducingTransformation = new Reducing2DTransformation(5);
            ReducingTransformation.ApplyPriority = 1;
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
       
        public Reducing2DTransformation ReducingTransformation
        {
            get => _reducingTransformation;
            set => SetProperty(ref _reducingTransformation, value, nameof(ReducingTransformation));
        }
    }
}
