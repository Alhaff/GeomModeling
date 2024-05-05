using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformations.Transformations2D;
using GeometricModeling.Models.DrawingTransformations.Transformations3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Services
{
    public class Transformation3DService : ObservableObject
    {
        #region
        private Offset3DTransformation _offset3D;
        private Rotate3DTransformation _rotate3D;
        private Rotate3DTransformation _projectionRotation;
        private IsometricProjectionOnYEqualPTransformation _isometricProjectionOnYEqualP;
        #endregion

        #region Propreties
        private DrawingEngineService DrawingEngine { get; set; }
        public static double AngleFromRadianToDegree(double angleInRadian)
        {
            return angleInRadian * 180 / Math.PI;
        }
        public static double AngleFromDegreeToRadian(double angleInDegree)
        {
            return angleInDegree * Math.PI / 180;
        }
        public Offset3DTransformation Offset3D
        {
            get => _offset3D;
            set => SetProperty(ref _offset3D, value, nameof(Offset3D));
        }

        public Rotate3DTransformation Rotate3D
        {
            get => _rotate3D;
            set => SetProperty(ref _rotate3D, value, nameof(Rotate3D));
        }

        public SwapYandZTransformation Swap { get; set; } = new SwapYandZTransformation();

        public IsometricProjectionOnYEqualPTransformation IsometricProjection
        {
            get => _isometricProjectionOnYEqualP;
            set => SetProperty(ref _isometricProjectionOnYEqualP, value, nameof(IsometricProjection));
        }
        #endregion

        public Transformation3DService()
        {
            Offset3D = new Offset3DTransformation(0, 0, 0);
            Rotate3D = new Rotate3DTransformation();
            IsometricProjection = new IsometricProjectionOnYEqualPTransformation();
            IsometricProjection.ApplyPriority = 19;
            Swap.ApplyPriority = 20;
        }

    }
}
