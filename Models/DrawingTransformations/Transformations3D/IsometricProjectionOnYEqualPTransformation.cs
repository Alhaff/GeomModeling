using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class IsometricProjectionOnYEqualPTransformation : TransformationBase
    {
		private float _p;
		private double Sqrt2 = Math.Sqrt(2); 
		private float _cosA;
		private float _sinA;
		private float _cosG;
		private float _sinG;
		public float P
		{
			get => _p;
			set => SetProperty(ref _p, value, nameof(P));
		}
		
		public float CosA 
		{ 
			get => _cosA;
			set => SetProperty(ref _cosA,value,nameof(CosA));
		}
		public float CosASeterWithoutPropertyChanged
		{
			set => _cosA = value;
		}
        public float SinASeterWithoutPropertyChanged
        {
            set => _sinA = value;
        }
        public float CosGSeterWithoutPropertyChanged
        {
            set => _cosG = value;
        }
        public float SinGSeterWithoutPropertyChanged
        {
            set => _sinG = value;
        }
        public float SinA 
		{ 
			get => _sinA;
			set => SetProperty(ref _sinA, value, nameof(SinA));
        }
        public float CosG 
		{ 
			get => _cosG;
			set => SetProperty(ref _cosG, value, nameof(CosG));
        }
        public float SinG 
		{ 
			get => _sinG; 
			set => SetProperty(ref _sinG, value, nameof(SinG));
        }

		private Matrix4X4 IsometricProjection
		{
			get => new Matrix4X4(
						 CosG,        0,    0, 0,
						-CosA * SinG, 0, SinA, 0,
						SinA * SinG,  0, CosA, 0,
						       0,     P,    0, 1

			);
		}
        public IsometricProjectionOnYEqualPTransformation()
        {
			ApplyPriority = 13;
			CosA = (float)Math.Sqrt(Sqrt2 / 2);
			SinA = (float)Math.Sqrt(1 - Sqrt2 / 2);
			CosG = (float)Math.Sqrt(2 - Sqrt2);
			SinG = (float)Math.Sqrt(Sqrt2 - 1);
        }
        public override Transformation Transform => v => v * IsometricProjection;
    }
}
