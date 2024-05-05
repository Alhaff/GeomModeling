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
    public class Line : ModelBase
    {
        #region Variables
        private Vector3 _start;
        private Vector3 _end;
        #endregion

        #region Properties
        public Vector3 Start
        {
            get => _start;
            set => SetProperty(ref _start, value, nameof(Start));
        }

        public Vector3 End
        {
            get  => _end;
            set => SetProperty(ref _end, value, nameof(End));
        }
        #endregion

        #region Constructors
        public Line(Vector3 start, Vector3 end, Color color)
        {
            Start = start;
            End = end;
            LineColor = color;
        }

        public Line(Vector3 start, float length, double angle, Color color)
        {
            Start = start;
            End = GetLineEndPoint(start, length, angle);
            LineColor = color;
        }
        #endregion

        internal IEnumerable<Vector3> GetLinePoint()
        {
            yield return Start;
            yield return End;
        }
        static public Vector3 GetLineEndPoint(Vector3 startPoint, float length, double angle = 0)
        {
            var tmp = startPoint + new Vector3(length, 0, 1).Rotate(angle);
            return new Vector3(tmp.X, tmp.Y, 1);
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return GetLinePoint();
        }
    }
}
