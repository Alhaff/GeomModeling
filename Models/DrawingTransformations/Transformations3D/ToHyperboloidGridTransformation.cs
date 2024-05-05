using GeometricModeling.Models.DrawingModels.Models3D;
using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class ToHyperboloidGridTransformation : TransformationBase
    {
        private Hyperboloid _hyperboloid = null;
        public Hyperboloid Hyperboloid
        {
            get => _hyperboloid;
            set => SetProperty(ref _hyperboloid, value, nameof(Hyperboloid));
        }
        
        public ToHyperboloidGridTransformation(Hyperboloid hyperboloid)
        {
            Hyperboloid = hyperboloid;
        }
        public ToHyperboloidGridTransformation()
        {

        }
        public float FromXToUPoint(float x) => x * (float)(2 * Math.PI / Hyperboloid.UStepCount);
        public float FromYToVPoint(float y) => (float)Math.Asinh(y * Hyperboloid.HeightStep / Hyperboloid.C);
        public override Transformation Transform => vect =>
        {
            var u = FromXToUPoint(vect.X);
            var v = FromYToVPoint(vect.Y);
            return Hyperboloid.GetHyperboloidPoint(u, v);
        };
    }
}
