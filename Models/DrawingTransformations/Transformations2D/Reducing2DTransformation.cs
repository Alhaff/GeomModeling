using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class Reducing2DTransformation : TransformationBase
    {
        private float _reducingFactor = 10;
        public float ReducingFactor
        {
            get => _reducingFactor;
            set
            {
                if (value <= 0) return;
                SetProperty(ref _reducingFactor, value, nameof(ReducingFactor));
            }
        }
        public Reducing2DTransformation() { }
        public Reducing2DTransformation(float reducingFactor) : this()
        {
            ReducingFactor = reducingFactor;
        }
        public override Transformation Transform => v => new System.Numerics.Vector3(v.X / ReducingFactor, v.Y / ReducingFactor, v.Z);


    }
}
