using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class Rotate3DTransformation : TransformationBase
    {
        #region Variables
        private Vector3 _center = Vector3.Zero;
        private double _angleX = 0;
        private double _angleY = 0;
        private double _angleZ = 0;
        #endregion

        #region Propreties

        public Vector3 Center
        {
            get => _center;
            set => SetProperty(ref _center, value, nameof(Center));
        }

        public double AngleX 
        {
            get => _angleX;
            set => SetProperty(ref _angleX, value, nameof(AngleX));
        }
        public double AngleY
        {
            get => _angleY;
            set => SetProperty(ref _angleY,value, nameof(AngleY));
        }
        public double AngleZ
        {
            get => _angleZ;
            set => SetProperty(ref _angleZ,value, nameof(AngleZ));
        }
        private float Cos(double angle) => (float)Math.Cos(angle);
        private float Sin(double angle) => (float)Math.Sin(angle);

        private Offset3DTransformation GetOffsetTransformation(Vector3 offset) => new Offset3DTransformation(offset);
        private Matrix4X4 RotateX
        {
            get => new Matrix4X4
                (
                  1, 0, 0, 0,
                  0, Cos(_angleX), Sin(_angleX), 0,
                  0, -Sin(_angleX), Cos(_angleX), 0,
                  0, 0, 0, 1
                );
        }
        private Matrix4X4 RotateY
        {
            get => new Matrix4X4
                (
                  Cos(_angleY), 0, -Sin(_angleY), 0,
                        0, 1, 0, 0,
                  Sin(_angleY), 0, Cos(_angleY), 0,
                        0, 0, 0, 1
                );
        }
        private Matrix4X4 RotateZ
        {
            get => new Matrix4X4
                (
                   Cos(_angleZ), Sin(_angleZ), 0, 0,
                  -Sin(_angleZ), Cos(_angleZ), 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1
                );
        }
        #endregion

        public Rotate3DTransformation()
        {
            ApplyPriority = 11;
        }
        public override Transformation Transform => v =>
        {
            var offsetToCenter = GetOffsetTransformation(-1 * Center);
            var offsetBack = GetOffsetTransformation(Center);
            var tmpV = offsetToCenter.Transform(v);
            tmpV *= RotateX;
            tmpV *= RotateY;
            tmpV *= RotateZ;
            tmpV = offsetBack.Transform(tmpV);
            return tmpV;
        };
       
    }
}
