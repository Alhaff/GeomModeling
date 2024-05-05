using GeometricModeling.Models.DrawingModels.Models2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models3D
{
    public class Axis3d : ModelBase
    {
        #region Variables
        private int _endX =50;
        private int _endY = 50;
        private int _endZ =50;
        private Vector3 _center = Vector3.Zero;
        private Color _xAxisColor = Color.Red;
        private Color _yAxisColor = Color.Green;
        private Color _zAxisColor = Color.Blue;
        #endregion
        public Axis3d() { }

     
        public int EndX
        {
            get => _endX;
            set => SetProperty(ref _endX, value, nameof(EndX));
        }
      
        public int EndY
        {
            get => _endY;
            set=> SetProperty(ref _endY, value, nameof(EndY));
        }
        public int EndZ
        {
            get => _endZ;
            set => SetProperty(ref _endZ, value, nameof(EndZ));
        }

       
        public Vector3 Center
        {
            get => _center;
            set => SetProperty(ref _center,value, nameof(Center));
        }
        public Color XAxisColor
        {
            get => _xAxisColor;
            set => SetProperty(ref _xAxisColor, value, nameof(XAxisColor));
        }

        public Color YAxisColor
        {
            get => _yAxisColor;
            set => SetProperty(ref _yAxisColor, value, nameof(YAxisColor));
        }
        public Color ZAxisColor
        {
            get => _zAxisColor;
            set => SetProperty(ref _zAxisColor, value, nameof(ZAxisColor));
        }
        private Vector3 ArrowVector(float EndAxis) 
        { 
           
                double angle = 30 * (Math.PI / 180);
                float len = 0.8f;
                var xEnd = new Vector3(EndAxis + len * (float)Math.Cos(angle), 0, 1);
                return Line.GetLineEndPoint(xEnd, len, Math.PI - angle);
           
        }
       

        private IEnumerable<Vector3> XAxis()
        {
            yield return Center;
            yield return Center + new Vector3(EndX, 0, 0);
        }
        private IEnumerable<Vector3> YAxis()
        {
            yield return Center;
            yield return Center + new Vector3(0, EndY, 0);
        }
        private IEnumerable<Vector3> ZAxis()
        {
            yield return Center;
            yield return Center + new Vector3(0, 0, EndZ);
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            LineColor = XAxisColor;
            yield return XAxis();
            
            LineColor = YAxisColor;
            yield return YAxis();
          
            LineColor = ZAxisColor;
            yield return ZAxis();
         
        }
    }
}
