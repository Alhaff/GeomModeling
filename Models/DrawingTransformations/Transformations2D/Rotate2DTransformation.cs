using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class Rotate2DTransformation : TransformationBase
    {

        public Rotate2DTransformation(DragAblePoint rotatePoint)
        {
            ApplyPriority = 2;
            RotatePoint = rotatePoint;
        }
        #region Variables
        private double _angle = 0;
        private DragAblePoint _rotatePoint;
        #endregion

        #region Propreties

        public DragAblePoint RotatePoint
        {
            get => _rotatePoint;
            set => SetProperty(ref _rotatePoint, value, nameof(RotatePoint)); 
        }
        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value, nameof(Angle));
        }
        private float CosAngle { get => (float)Math.Cos(Angle); }
        private float SinAngle { get => (float)Math.Sin(Angle); }

        private Matrix3X3 RotationMatrix
        {
            get =>
                new Matrix3X3
                (
                     CosAngle, SinAngle, 0,
                    -1 * SinAngle, CosAngle, 0,
                    (float)(-1 * RotatePoint.Center.X * (CosAngle - 1) + RotatePoint.Center.Y * SinAngle),
                    (float)(-1 * RotatePoint.Center.X * SinAngle - RotatePoint.Center.Y * (CosAngle - 1)), 1
                );
        }
        #endregion

        public override Transformation Transform => v => v * RotationMatrix;
    }
}
