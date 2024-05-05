using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations
{
    public class VoidTransformation : TransformationBase
    {
        
        public override Transformation Transform =>  vector => vector;
    }
}
