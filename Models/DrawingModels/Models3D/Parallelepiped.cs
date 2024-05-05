using GeometricModeling.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models3D
{
    public class Parallelepiped : ModelBase
    {
        #region Variables
        private Vector3 _bottomCornerPoint = new Vector3(0, 0, 0);
        private float _length = 6;
        private float _width =2;
        private float _height = 5;
        private double _angle = 0;
        #endregion

        #region Propreties
        public float Length
        {
            get => _length;
            set => SetProperty(ref _length, value, nameof(Length));
        }
        public float Width
        {
            get => _width;
            set => SetProperty(ref _width, value, nameof(Width));
        }
        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value, nameof(Height));
        }
        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle,value, nameof(Angle));
        }
        #endregion

        private IEnumerable<Vector3> XYPoints(Vector3 point, Vector3 direction)
        {
            yield return point;
            point += new Vector3(0, direction.Y, 0).Rotate(_angle);
            yield return point;
            point += new Vector3(direction.X, 0, 0);
            yield return point;
            point -= new Vector3(0, direction.Y, 0).Rotate(_angle);
            yield return point;
            yield return point - new Vector3(direction.X, 0, 0);
        }
        private IEnumerable<Vector3> ZYPoints(Vector3 point, Vector3 direction)
        {
            yield return point;
            yield return point + new Vector3(0, direction.Y, 0).Rotate(_angle);
            yield return point + new Vector3(0, direction.Y, direction.Z).Rotate(_angle);
            yield return point + new Vector3(0, 0, direction.Z);
            yield return point;
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            yield return XYPoints(_bottomCornerPoint, new Vector3(Length, Width, 0));
            yield return XYPoints(_bottomCornerPoint + new Vector3(0, 0, Height), new Vector3(Length, Width, 0));
            yield return ZYPoints(_bottomCornerPoint, new Vector3(0, Width, Height));
            yield return ZYPoints(_bottomCornerPoint + new Vector3(Length, 0, 0), new Vector3(0, Width, Height));
        }
    }
}
