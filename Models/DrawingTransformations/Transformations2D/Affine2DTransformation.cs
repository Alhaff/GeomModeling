using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class Affine2DTransformation : TransformationBase
    {
        public Affine2DTransformation() 
        {
            ApplyPriority = 3;
        }
        #region Variables
        private Vector3 _r0 = new Vector3(0, 0, 1);
        private Vector3 _rx = new Vector3(1, 0, 1);
        private Vector3 _ry = new Vector3(0, 1, 1);
        #endregion

        public Vector3 R0
        {
            get => _r0;
            set => SetProperty(ref _r0, value, nameof(R0));
        }
        public Vector3 RX
        {
            get => _rx;
            set => SetProperty(ref _rx, value, nameof(RX));
        }

        public Vector3 RY
        {
            get => _ry;
            set => SetProperty(ref _ry, value, nameof(RY));
        }

        public override Transformation Transform => vector =>
        {
            var res = R0 + (RX * vector.X) + (RY * vector.Y);
            return new Vector3(res.X,res.Y,res.Z);
        };
    }
}
