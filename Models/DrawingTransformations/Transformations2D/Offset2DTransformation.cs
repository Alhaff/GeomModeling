using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class Offset2DTransformation : TransformationBase
    {
        public Offset2DTransformation()
        {
            ApplyPriority = 1;
        }
        #region Variables
        private float _dX = 0;
        private float _dY = 0;
        #endregion
        public float DX
        {
            get => _dX;
            set => SetProperty(ref _dX, value,nameof(DX));
        }

        public float DY
        {
            get => _dY;
            set => SetProperty(ref _dY, value,nameof(DY));
        }

        public override Transformation Transform => vector => vector + new System.Numerics.Vector3(DX,DY,0);
    }
}
