using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class Offset3DTransformation : TransformationBase
    {
        #region Variables
        private float _dX = 0;
        private float _dY = 0;
        private float _dZ = 0;
        #endregion

        #region Propreties
        public float DX
        {
            get => _dX; 
            set => SetProperty(ref _dX, value,nameof(DX));
        }

        public float DY
        {
            get => _dY;
            set => SetProperty(ref _dY, value, nameof(DY));
        }
        public float DZ
        {
            get => _dZ;
            set => SetProperty(ref _dZ, value, nameof(DZ));
        }
        #endregion
        public Offset3DTransformation()
        {
            ApplyPriority = 10; 
        }
        public Offset3DTransformation(Vector3 offset) :
            this(offset.X, offset.Y, offset.Z) { }

        public Offset3DTransformation(float dX, float dY, float dZ) 
            : this()
        {
            DX = dX;
            DY = dY;
            DZ = dZ;
        }
        public override Transformation Transform => v => v + new Vector3(DX, DY, DZ);
    }
}
