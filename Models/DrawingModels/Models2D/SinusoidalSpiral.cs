using GeometricModeling.Extensions;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class SinusoidalSpiral : ModelBase
    {
        #region Variables
        private Vector3 _center =  new Vector3(0,0,1);
        private float _n = 2;
        private float _a = 1;
        private double _tangentTeta = 0;
        private bool _isTangentVisible = false;
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get => _center;
            set => SetProperty(ref _center, value,nameof(Center));
        }

        public float N
        {
            get => _n;
            set
            {
                if (value == 0) return;
                SetProperty(ref _n, value, nameof(N));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(Curvature));
            }
        }

        public float A
        {
            get => _a;
            set
            {
                if(value == 0) return;
                SetProperty(ref _a, value, nameof(A));
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(Curvature));
            }
        }
        //N > 0 -П/2N <= Teta<= П/2N
        public double Length
        {
            get => CalculateLen();

        }
        private Vector3 TangentPoint { get => GetPoint(TangentTeta); }
     
        //N > 0 -П/2N <= Teta<= П/2N
        public double Area
        {
            get => CalculateArea();
        }
     
        public double TangentTeta
        {
            get => _tangentTeta;
            set    
            {
                SetProperty(ref _tangentTeta, value, nameof(TangentTeta));
                OnPropertyChanged(nameof(Curvature));
            }
        }

        public double Curvature
        {
            get
            {
                return (N + 1) / (A * Math.Pow(Math.Cos(N * TangentTeta), 1 / (N - 1)));
            }
        }

        public bool IsTangentVisible
        {
            get => _isTangentVisible;
            set => SetProperty(ref _isTangentVisible, value, nameof(IsTangentVisible));
        }

        private double EndAngle { get; set; } = 2 * Math.PI;

        #endregion
        public SinusoidalSpiral()
        {
            N = -1f;
            A = 10;
        }

        //Calculate Integral with Numerics methods
        private double CentralRectangle(Func<double, double> f, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = (f(a) + f(b)) / 2;
            for (var i = 1; i < n; i++)
            {
                var x = a + h * i;
                sum += f(x);
            }

            var result = h * sum;
            return result;
        }

        private double CalculateLen()
        {
            Func<double, double> F = a => Math.Pow(Math.Cos(a), -1 + 1 / N);
            return A *Math.Pow(2,1/N)* CentralRectangle(F, 0, Math.PI/2, 1000);
        }

        private double CalculateArea()
        {
            Func<double, double> F = a => Math.Pow(Math.Cos(a), 2 / N);
            return A *A *CentralRectangle(F, 0, Math.PI/2, 1000);
        }

        private double GetR(double angleTeta)
        {
            var cos = Math.Cos(N * angleTeta);
            EndAngle = 2 * Math.PI;
            if(N >1)
            {
                return A * Math.Pow(cos, N + 1/ N);
            }
            else if(N<-1)
            {
                return A * Math.Pow(cos, N -  1 / N);
            }
            else
            {
                EndAngle = Math.Round(Math.Abs(1/N)) * Math.PI;
                return A * Math.Pow(cos, Math.Round(1d/N));
            }
        }
        public Vector3 GetPoint(double angleTeta)
        {
            var x = Center.X + GetR(angleTeta) * Math.Cos(angleTeta);
            var y = Center.Y + GetR(angleTeta) * Math.Sin(angleTeta);
            return new Vector3((float)x, (float)y, 1);
        }
       
        private IEnumerable<Vector3> GetCurvePoints()
        {
            for (double angle = 0; angle <= EndAngle; angle += Math.PI / 180)
            {

                yield return GetPoint(angle);

            }


        }

        public double Tangent(double angleTeta)
        {
           var p1 = GetPoint(angleTeta);
           var p2 = GetPoint(angleTeta + 0.00001);
            var res = p2 - p1;
            return res.Angle();
        }
     
        private IEnumerable<Vector3> GetTangentPointPart(double angle)
        {
            yield return Line.GetLineEndPoint(TangentPoint, 0.05f, Math.PI + angle);
            yield return Line.GetLineEndPoint(TangentPoint, 0.05f, angle);
        }
        private IEnumerable<Vector3> GetNormal()
        {
            yield return TangentPoint;
            yield return Line.GetLineEndPoint(TangentPoint, 1.5f, -Math.PI / 2 + Tangent(TangentTeta));
        }

        private IEnumerable<Vector3> GetTangent()
        {
            yield return Line.GetLineEndPoint(TangentPoint, 1.5f, Tangent(TangentTeta));
            yield return Line.GetLineEndPoint(TangentPoint, 1.5f, Math.PI + Tangent(TangentTeta));
        }
      
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return GetCurvePoints();
            if (IsTangentVisible)
            {
                var tmpColor = LineColor;
                LineColor = Color.FromArgb(255,0,83,235);
                yield return GetTangent();
                LineColor = Color.FromArgb(255, 0, 232, 234);
                yield return GetNormal();
                LineColor = Color.Red;
                for (double angle = 0; angle <= Math.PI; angle += Math.PI / 180)
                {
                    yield return GetTangentPointPart(angle);
                }
                LineColor = tmpColor;
            }
        }
    }
}
