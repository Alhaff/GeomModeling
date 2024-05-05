using GeometricModeling.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class Circle : ModelBase
    {
        #region Variables

        private float _r;
        private Vector3 _center;
        private double _startBreakPointAngle;
        private double _endBreakPointAngle;

        #endregion

        #region Propreties
        public double StartBreakPointAngle
        {
            get => _startBreakPointAngle;
            set => SetProperty(ref _startBreakPointAngle, value,nameof(StartBreakPointAngle));
        }
        public double EndBreakPointAngle
        {
            get => _endBreakPointAngle;
            set => SetProperty(ref _endBreakPointAngle, value,nameof(EndBreakPointAngle));
        }
        public Vector3 StartBreakPoint { get => GetBreakPoint(StartBreakPointAngle); }

        public Vector3 EndBreakPoint { get => GetBreakPoint(EndBreakPointAngle); }
        public Vector3 Center
        {
            get =>  _center;
            set => SetProperty(ref _center, value, nameof(Center));
            
        }

        public float R
        {
            get => _r; 
            set => SetProperty(ref _r,value, nameof(R));
        }
        #endregion

        #region Constructors
        public Circle()
        {
            Center = new Vector3(0, 0, 1);
            R = 0;
           
        }

        public Circle(Vector3 startCoord, float r, Color color)
        {
            Center = startCoord;
            R = r;
            LineColor = color;
            StartBreakPointAngle = 0;
            EndBreakPointAngle = 0;
        }

        public Circle(Vector3 startCoord, float r, double angleStartBreakPoint, double angleEndBreakPoint, Color color)
        {
            Center = startCoord;
            R = r;
            StartBreakPointAngle = angleStartBreakPoint;
            EndBreakPointAngle = angleEndBreakPoint;
            LineColor = color;
        }
        #endregion

        #region Methods
        internal IEnumerable<Vector3> GetCirclePoints()
        {
            var startAngle = (StartBreakPoint - Center).Angle();
            var endAngle = (EndBreakPoint - Center).Angle();
            var start = startAngle;
            var end = endAngle;
            if (end <= start)
            {
                end += 2 * Math.PI;
            }

            for (double t = start; t <= end; t += Math.PI / 180)
            {
                yield return Line.GetLineEndPoint(Center, R, t);
            }
            yield return Line.GetLineEndPoint(Center, R, end);

        }
        public Vector3 GetBreakPoint(double angle)
        {
            return Line.GetLineEndPoint(Center, R, angle);
        }
        #endregion
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return GetCirclePoints();
        }
    }
}
