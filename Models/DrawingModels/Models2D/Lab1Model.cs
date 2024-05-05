using GeometricModeling.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class Lab1Model : ModelBase
    {
        public Lab1Model()
        {
            LineColor = Color.Purple;
            var center = new Vector3(10, 10, 1);
            _inR = 2;
            _outR = 10;
            _midR = 7;
            InnerCircle = new Circle(center, InR, LineColor);
            OuterCircle = new Circle(center, OutR, LineColor);
            int angleInDegree = 15;
            Arc1 = new Circle(center, MidR, Transformation2DService.AngleFromDegreeToRadian(0 + angleInDegree),
                                            Transformation2DService.AngleFromDegreeToRadian(90 - angleInDegree),
                                            LineColor);

            Arc2 = new Circle(center, MidR, Transformation2DService.AngleFromDegreeToRadian(90 + angleInDegree),
                                            Transformation2DService.AngleFromDegreeToRadian(180 - angleInDegree),
                                            LineColor);

            Arc3 = new Circle(center, MidR, Transformation2DService.AngleFromDegreeToRadian(180 + angleInDegree),
                                            Transformation2DService.AngleFromDegreeToRadian(270 - angleInDegree),
                                            LineColor);

            Arc4 = new Circle(center, MidR, Transformation2DService.AngleFromDegreeToRadian(270 + angleInDegree),
                                            Transformation2DService.AngleFromDegreeToRadian(360 - angleInDegree),
                                            LineColor);
            var (c1, c2, c3, c4) = GetSemiCirclesCenters();
            var r = GetSemiCircleR();
            SemiCircle1 = new Circle(c1, r, Transformation2DService.AngleFromDegreeToRadian(270),
                                            Transformation2DService.AngleFromDegreeToRadian(90),
                                            LineColor);

            SemiCircle2 = new Circle(c2, r, Transformation2DService.AngleFromDegreeToRadian(0),
                                            Transformation2DService.AngleFromDegreeToRadian(180),
                                            LineColor);

            SemiCircle3 = new Circle(c3, r, Transformation2DService.AngleFromDegreeToRadian(90),
                                            Transformation2DService.AngleFromDegreeToRadian(270),
                                            LineColor);
            
            SemiCircle4 = new Circle(c4, r, Transformation2DService.AngleFromDegreeToRadian(180),
                                            Transformation2DService.AngleFromDegreeToRadian(360),
                                            LineColor);

            Len1 = 3;
            this.PropertyChanged += (e, a) =>
            {
                if (a.PropertyName == "InR")
                {
                    if (InnerCircle != null)
                    {
                        InnerCircle.R = InR;
                    }
                }
                else if(a.PropertyName == "MidR")
                {
                    if(Arc1 != null && Arc2 != null && Arc3 != null && Arc4 != null)
                    {
                        Arc1.R = MidR;
                        Arc2.R = MidR;
                        Arc3.R = MidR;
                        Arc4.R = MidR;
                        var (c1, c2, c3, c4) = GetSemiCirclesCenters();
                        var r = GetSemiCircleR();
                        SemiCircle1.Center = c1; 
                        SemiCircle2.Center = c2;
                        SemiCircle3.Center = c3;
                        SemiCircle4.Center = c4;
                        SemiCircle1.R = SemiCircle2.R = SemiCircle3.R = SemiCircle4.R = r;
                    }
                }
                else if(a.PropertyName == "OutR")
                {
                    if(OuterCircle != null)
                    {
                        OuterCircle.R = OutR;
                    }
                }
            };
        }

        #region Variables
        private Circle _outerCircle;
        private Circle _innerCircle;
        private Circle _arc1;
        private Circle _arc2;
        private Circle _arc3;
        private Circle _arc4;
        private Circle _semiCircle1;
        private Circle _semiCircle2;
        private Circle _semiCircle3;
        private Circle _semiCircle4;
        private float _len1;
        private float _inR;
        private float _outR;
        private float _midR;
        #endregion

        #region Propreties
        public Circle OuterCircle
        {
            get => _outerCircle;
            set => SetProperty(ref _outerCircle, value, nameof(OuterCircle));
        }

        public Circle InnerCircle
        {
            get => _innerCircle;
            set => SetProperty(ref _innerCircle, value, nameof(InnerCircle));
        }

        public Circle Arc1
        {
            get => _arc1;
            set  => SetProperty(ref _arc1, value, nameof(Arc1));
        }

        public Circle Arc2
        {
            get => _arc2;
            set  => SetProperty(ref _arc2, value, nameof(Arc2));
        }

        public Circle Arc3
        {
            get => _arc3;
            set => SetProperty(ref _arc3, value, nameof(Arc3));
        }

        public Circle Arc4
        {
            get=> _arc4;
            set => SetProperty(ref _arc4, value, nameof(Arc4));
        }

        public Circle SemiCircle1
        {
            get => _semiCircle1; 
            set => SetProperty(ref _semiCircle1,value, nameof(SemiCircle1));
        }

        public Circle SemiCircle2
        {
            get => _semiCircle2;
            set => SetProperty(ref _semiCircle2,value, nameof(SemiCircle2));
        }

        public Circle SemiCircle3
        {
            get => _semiCircle3;
            set => SetProperty(ref _semiCircle3,value, nameof(SemiCircle3));
        }

        public Circle SemiCircle4
        {
            get => _semiCircle4;
            set => SetProperty(ref _semiCircle4, value , nameof(SemiCircle4));
        }

        private (Vector3,Vector3,Vector3,Vector3) GetSemiCirclesCenters()
        {
            var c1 = (Arc1.EndBreakPoint + Arc2.StartBreakPoint) / 2;
            var c2 = (Arc2.EndBreakPoint + Arc3.StartBreakPoint) / 2;
            var c3 = (Arc3.EndBreakPoint + Arc4.StartBreakPoint) / 2;
            var c4 = (Arc4.EndBreakPoint + Arc1.StartBreakPoint) / 2;
            c1 = new Vector3(c1.X, c1.Y, 1);
            c2 = new Vector3(c2.X, c2.Y, 1);
            c3 = new Vector3(c3.X, c3.Y, 1);
            c4 = new Vector3(c4.X, c4.Y, 1);
            return (c4,c1, c2, c3);
        }
        public float InR
        {
            get => _inR;
            set
            {
                if (value <= 0 || value > MidR - Len1) return; 
                SetProperty(ref _inR, value, nameof(InR));
            }
        }

        public float OutR
        {
            get => _outR;
            set
            {
                if (value <= MidR + GetSemiCircleR()) return;
                SetProperty(ref _outR, value, nameof(OutR));
            }
        }

        public float MidR
        {
            get => _midR; 
            set 
            {  
                if(value - Len1 <= InR || value >= OutR - GetSemiCircleR()) return;
                SetProperty(ref _midR, value, nameof(MidR));
            }
        }

        public float Len1
        {
            get => _len1;
            set 
            {
                if (value > MidR - InR) return;
                SetProperty(ref _len1, value, nameof(Len1));
            }
        }

        #endregion

        private float GetSemiCircleR()
        {
            if (Arc1 != null && Arc2 != null)
            {
                var c1 = (Arc1.EndBreakPoint + Arc2.StartBreakPoint) / 2;
                c1 = new Vector3(c1.X, c1.Y, 1);
                var r = (float)Math.Sqrt(Math.Pow(c1.X - Arc1.EndBreakPoint.X, 2) + Math.Pow(c1.Y - Arc1.EndBreakPoint.Y, 2));
                return r;
            }
            return 0;
        }

        private IEnumerable<Vector3> GetInnerHexagon()
        {
            yield return Line.GetLineEndPoint(Arc1.StartBreakPoint,Len1,Transformation2DService.AngleFromDegreeToRadian(180));
            yield return Line.GetLineEndPoint(Arc1.EndBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(270));
            yield return Line.GetLineEndPoint(Arc2.StartBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(270));
            yield return Line.GetLineEndPoint(Arc2.EndBreakPoint, Len1, 0);
            yield return Line.GetLineEndPoint(Arc3.StartBreakPoint, Len1, 0);
            yield return Line.GetLineEndPoint(Arc3.EndBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(90));
            yield return Line.GetLineEndPoint(Arc4.StartBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(90));
            yield return Line.GetLineEndPoint(Arc4.EndBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(180));
            yield return Line.GetLineEndPoint(Arc1.StartBreakPoint, Len1, Transformation2DService.AngleFromDegreeToRadian(180));
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return InnerCircle.GetCirclePoints();
            yield return OuterCircle.GetCirclePoints();
            yield return Arc1.GetCirclePoints();
            yield return Arc2.GetCirclePoints();
            yield return Arc3.GetCirclePoints();
            yield return Arc4.GetCirclePoints();
            yield return SemiCircle1.GetCirclePoints();
            yield return SemiCircle2.GetCirclePoints();
            yield return SemiCircle3.GetCirclePoints();
            yield return SemiCircle4.GetCirclePoints();
            yield return GetInnerHexagon();
            yield return new[] { Arc1.StartBreakPoint, Line.GetLineEndPoint(Arc1.StartBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(180)) 
                               };
            yield return new[] { Arc1.EndBreakPoint, Line.GetLineEndPoint(Arc1.EndBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(270))
                               };
            yield return new[] { Arc2.StartBreakPoint, Line.GetLineEndPoint(Arc2.StartBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(270))
                               };
            yield return new[] { Arc2.EndBreakPoint, Line.GetLineEndPoint(Arc2.EndBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(0))
                               };
            yield return new[] { Arc3.StartBreakPoint, Line.GetLineEndPoint(Arc3.StartBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(0))
                               };
            yield return new[] { Arc3.EndBreakPoint, Line.GetLineEndPoint(Arc3.EndBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(90))
                               };
            yield return new[] { Arc4.StartBreakPoint, Line.GetLineEndPoint(Arc4.StartBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(90))
                               };
            yield return new[] { Arc4.EndBreakPoint, Line.GetLineEndPoint(Arc4.EndBreakPoint,
                                                                            Len1,
                                                                            Transformation2DService.AngleFromDegreeToRadian(180))
                               };
            yield return new[] { Arc1.EndBreakPoint, Arc2.StartBreakPoint };
            yield return new[] { Arc2.EndBreakPoint, Arc3.StartBreakPoint };
            yield return new[] { Arc3.EndBreakPoint, Arc4.StartBreakPoint };
            yield return new[] { Arc4.EndBreakPoint, Arc1.StartBreakPoint };
        }
    }
}
