using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class AffineTransformationViewModel : Global2DTransformationBaseViewModel
    {
      
        public AffineTransformationViewModel(Transformation2DService transformation2DService)
            : base(transformation2DService)
        {
            R0X = TransformationService.Affine.R0.X;
            R0Y = TransformationService.Affine.R0.Y;
            R0W = TransformationService.Affine.R0.Z;
            RxX = TransformationService.Affine.RX.X;
            RxY = TransformationService.Affine.RX.Y;
            RxW = TransformationService.Affine.RX.Z;
            RyX = TransformationService.Affine.RY.X;
            RyY = TransformationService.Affine.RY.Y;
            RyW = TransformationService.Affine.RY.Z;
        }
        public override void SetR0()
        {
            TransformationService.Affine.R0
                = new System.Numerics.Vector3(R0X, R0Y, R0W);
        }
        public override void SetRX()
        {
            TransformationService.Affine.RX
                = new System.Numerics.Vector3(RxX, RxY, RxW);
        }
        public override void SetRY()
        {
            TransformationService.Affine.RY
                = new System.Numerics.Vector3(RyX, RyY, RyW);
        }
    }
}
