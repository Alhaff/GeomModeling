using GeometricModeling.Extensions;
using GeometricModeling.Models.DrawingTransformation;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class SierpinskiNapkin : ModelBase
    {
        private double _angle = Transformation2DService.AngleFromDegreeToRadian(60);

        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value, nameof(Angle));
        }

        private int _lastIter = 0;

        public int LastIter
        {
            get => _lastIter;
            set
            {
                if (value < 0) return;
                if(value != _lastIter) FractalPoints.Clear();
                SetProperty(ref _lastIter, value, nameof(LastIter));
            }
        }

        private void F(List<Vector3> fractal, ref Vector3 direction, int currIter, int lastIter)
        {
            if (currIter >= lastIter)
            {
                fractal.Add(fractal.Last() + direction);
                return;
            }
            F(fractal, ref direction, currIter +1, lastIter);
            F(fractal, ref direction, currIter + 1,lastIter);
        }
        
        private void Plus(ref Vector3 direction) => direction = direction.Rotate(Angle);

        private void Minus(ref Vector3 direction) => direction = direction.Rotate(-Angle);
        private void X(List<Vector3> fractal, ref Vector3 direction, int currIter, int lastIter)
        {
            if(currIter == lastIter)
            {
                return; 
            }
            Minus(ref direction);
            Minus(ref direction);
            F(fractal,ref direction, currIter + 1, lastIter);
            X(fractal,ref direction,currIter + 1, lastIter);
            F(fractal, ref direction, currIter + 1, lastIter);
            Plus(ref direction);
            Plus(ref direction);
            F(fractal, ref direction, currIter + 1, lastIter);
            X(fractal, ref direction, currIter + 1, lastIter);
            F(fractal, ref direction, currIter + 1, lastIter);
            Plus(ref direction);
            Plus(ref direction);
            F(fractal, ref direction, currIter + 1, lastIter);
            X(fractal, ref direction, currIter + 1, lastIter);
            F(fractal, ref direction, currIter + 1, lastIter);
            Minus(ref direction);
            Minus(ref direction);
        }

        private List<Vector3> FractalPoints { get; set; } = new List<Vector3>();
        private List<Vector3> Axiom(int lastIter)
        {
            if(FractalPoints.Count == 0) FractalPoints.Add(new Vector3(0,0,1));
            Vector3 direction = new Vector3(1, 0, 0);
            F(FractalPoints, ref direction, 0, lastIter);
            X(FractalPoints, ref direction, 0, lastIter);
            F(FractalPoints, ref direction, 0, lastIter);
            Minus(ref direction);
            Minus(ref direction);
            F(FractalPoints, ref direction, 0, lastIter);
            F(FractalPoints, ref direction, 0, lastIter);
            Minus(ref direction);
            Minus(ref direction);
            F(FractalPoints, ref direction, 0, lastIter);
            F(FractalPoints, ref direction, 0, lastIter);
            return FractalPoints;
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return FractalPoints.Count != 0? FractalPoints : Axiom(LastIter);
        }
    }
}
