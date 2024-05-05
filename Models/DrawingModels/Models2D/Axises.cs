using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Shapes;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class Axises : ModelBase
    {
        private int _endXAxis = 100;
        private int _endYAxis = 100;
        private Color _gridColor = Color.Black;
        private Color _xAxisColor = Color.Red;
        private Color _yAxisColor = Color.Green;
        public int EndXAxis
        {
            get => _endXAxis;
            set => SetProperty(ref _endXAxis, value, nameof(EndXAxis));
        }

        public int EndYAxis
        {
            get => _endYAxis;
            set => SetProperty(ref _endYAxis, value, nameof(EndYAxis));
        }
        public Color GridColor 
        {
            get => _gridColor;
            set => SetProperty(ref _gridColor, value, nameof(GridColor));
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

        public Axises(int endX, int endY)
        {
            _endXAxis = endX;
            _endYAxis = endY;
        }

        public Axises(int endX, int endY, Color gridColor, Color xColor, Color yColor)
        {
            _endXAxis = endX;
            _endYAxis = endY;
            GridColor = gridColor;
            XAxisColor = xColor;
            YAxisColor = yColor;
        }
        private IEnumerable<Vector3> GetXLine(int y)
        {
            for(int i =0; i <= EndXAxis; i++)
            {
                yield return new Vector3(i, y, 0);
            }
        }
        private IEnumerable<Vector3> GetYLine(int x)
        {
            for (int i = 0; i <= EndYAxis; i++)
            {
                yield return new Vector3(x, i, 0);
            }
        }
        private IEnumerable<Vector3> ArrowX()
        {
            double angle = 30 * (Math.PI / 180);
            float len = 0.8f;
            var xEnd = new Vector3(EndXAxis + len * (float)Math.Cos(angle), 0, 0);
            yield return xEnd;
            yield return Line.GetLineEndPoint(xEnd, len, Math.PI - angle);
            yield return Line.GetLineEndPoint(xEnd, len, Math.PI + angle);
            yield return xEnd;
        }
        private IEnumerable<Vector3> ArrowY()
        {
            double angle = 30 * (Math.PI / 180);
            float len = 0.8f;
            var yEnd = new Vector3(0, EndYAxis + len * (float)Math.Cos(angle), 0);
            yield return yEnd;
            yield return Line.GetLineEndPoint(yEnd, len, 3 * Math.PI / 2 - angle);
            yield return Line.GetLineEndPoint(yEnd, len, 3 * Math.PI / 2 + angle);
            yield return yEnd;
        }

        private IEnumerable<Vector3> DrawUnitSegment(string axisName)
        {
            switch (axisName)
            {
                case "X":
                case "x":
                    yield return new Vector3(1, 0.3f, 0);
                    yield return new Vector3(1, -0.3f, 0);
                    break;
                case "Y":
                case "y":
                    yield return new Vector3(0.3f, 1, 0);
                    yield return new Vector3(-0.3f, 1, 0);
                    break;
                default:
                    break;
            }
        }

        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            //YAxis
            LineColor = YAxisColor;
            LineThickness = 2;
            yield return GetYLine(0);
            yield return ArrowY();
            yield return DrawUnitSegment("Y");
            //Grid
            LineThickness = 1;
            LineColor = GridColor;
            for (int x = 1; x <= EndXAxis; x++)
            {
                yield return GetYLine(x);
            }
            for (int y = 1; y <= EndYAxis; y++)
            {

                yield return GetXLine(y);
            }
            //XAxis
            LineThickness = 2;
            LineColor = XAxisColor;
            yield return GetXLine(0);
            yield return ArrowX();
            yield return DrawUnitSegment("X");
           
            
        }
    }
}
