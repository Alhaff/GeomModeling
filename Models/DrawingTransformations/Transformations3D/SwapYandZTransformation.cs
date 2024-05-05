using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class SwapYandZTransformation : TransformationBase
    {
        public override Transformation Transform => v => new System.Numerics.Vector3(v.X,v.Z,v.Y);
    }
}
