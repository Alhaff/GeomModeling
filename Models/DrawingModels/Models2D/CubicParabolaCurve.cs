using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    /// <summary>
    ///  Я не впевнений, але за аналогією з криві Без'є 2-го порядку, криві Без'є третього порядку фактично є дугами кубічних парабол.
    ///  Тож використаємо формули кривої Без'є 3-го порядку для побудову контуру. 
    /// </summary>
    public class CubicParabolaCurve : ModelBase
    {
        #region Variables
        private DragAblePoint _Ra;
        private DragAblePoint _Rb;
        private DragAblePoint _Rc;
        private DragAblePoint _Rd;
        private bool _IsCarcasVisible = false;
        #endregion

        #region Propreties
        public DragAblePoint Ra
        {
            get { return _Ra; }
            set
            {
                if (_Ra == value) return;
                if (_Ra != null) _Ra.PropertyChanged -= DragAblePointChanged; 
                _Ra = value;
                _Ra.PropertyChanged += DragAblePointChanged;
                OnPropertyChanged(nameof(Ra));
            }
        }

        public DragAblePoint Rb
        {
            get => _Rb;
            set
            {
                if (_Rb == value) return;
                if (_Rb != null) _Rb.PropertyChanged -= DragAblePointChanged;
                _Rb = value;
                _Rb.PropertyChanged += DragAblePointChanged;
                OnPropertyChanged(nameof(Rb));
            }
        }

        public DragAblePoint Rc
        {
            get => _Rc;
            set
            {
                if (_Rc == value) return;
                if (_Rc != null) _Rc.PropertyChanged -= DragAblePointChanged;
                _Rc = value;
                _Rc.PropertyChanged += DragAblePointChanged;
                OnPropertyChanged(nameof(Rc));
            }
        }

        public DragAblePoint Rd
        {
            get => _Rd;
            set
            {
                if (_Rd == value) return;
                if (_Rd != null) _Rd.PropertyChanged -= DragAblePointChanged;
                _Rd = value;
                _Rd.PropertyChanged += DragAblePointChanged;
                OnPropertyChanged(nameof(Rd));
            }
        }
        public DrawingEngineService DrawingEngine { get; set; }

        public Color PointColor { get; set; } 
        public IEnumerable<Vector3> CarcasPoints
        {
            get
            {
                if (Ra is not null && Rb is not null && Rc is not null && Rd is not null)
                {
                    return new[] { Ra.Center, Rb.Center, Rc.Center, Rd.Center };
                }
                else
                {
                    throw new ArgumentNullException("Carcas points is null");
                }
            }
        }
        public bool IsCarcasVisible
        {
            get => _IsCarcasVisible; 
            set => SetProperty(ref _IsCarcasVisible,value,nameof(IsCarcasVisible));
        }
        #endregion

        public CubicParabolaCurve(DrawingEngineService drawingEngineService, Color pointColor,Color lineColor)
        {
            DrawingEngine = drawingEngineService;
            PointColor = pointColor;
            LineColor = lineColor;
        }

        public CubicParabolaCurve(DrawingEngineService drawingEngineService, Vector3 ra,Vector3 rb, Vector3 rc, Vector3 rd, Color pointColor, Color lineColor) :
            this(drawingEngineService,pointColor,lineColor)
        {
            Ra = new DragAblePoint(drawingEngineService, ra, pointColor);
            Rb = new DragAblePoint(drawingEngineService, rb, pointColor);
            Rc = new DragAblePoint(drawingEngineService, rc, pointColor);
            Rd = new DragAblePoint(drawingEngineService, rd, pointColor);
        }

        public CubicParabolaCurve(DrawingEngineService drawingEngine, DragAblePoint ra,DragAblePoint rd,Color pointColor, Color lineColor)
            :this(drawingEngine,pointColor,lineColor)
        {
            Ra = ra;
            Rd = rd;
            var mid = (Ra.Center + Rd.Center) / 2;
            Rb = new DragAblePoint(drawingEngine,(Ra.Center + mid)/2, pointColor);
            Rc = new DragAblePoint(drawingEngine,(mid + Rd.Center)/2, pointColor);
            
        }
        public Vector3 CubicParabolaCurvePoint(double u)
        {
            var res = Ra.Center * (float)Math.Pow(1 - u, 3) +
                  3 * Rb.Center * (float)(u * Math.Pow(1 - u, 2)) +
                  3 * Rc.Center * (float)(u * u * (1 - u)) +
                      Rd.Center * (float)(Math.Pow(u, 3));
            return res;
        }
        public IEnumerable<Vector3> GetCubicParabolaCurveCountour()
        {
            for (double t = 0; t <= 1; t += 0.025)
            {
                yield return CubicParabolaCurvePoint(t);
            }
            yield return Rd.Center;
        }
        public IEnumerable<Vector3> GetCarcasCountour()
        {
            yield return Ra.Center;
            yield return Rb.Center;
            yield return Rc.Center;
            yield return Rd.Center;
        }
        public void AddLinePointsOnCanvas()
        {
            if(DrawingEngine.Canvas != null)
            {
                Ra.AddPointOnCanvas(DrawingEngine.Canvas);
                Rd.AddPointOnCanvas(DrawingEngine.Canvas);
            }
        }
        public void RemoveLinePointsFromCanvas()
        {
            if(DrawingEngine.Canvas != null)
            {
                Ra.RemovePointFromCanvas(DrawingEngine.Canvas);
                Rd.RemovePointFromCanvas(DrawingEngine.Canvas);
            }
        }
        public void AddCarcasPointsOnCanvas()
        {
            if(DrawingEngine.Canvas != null)
            {
               
                Rb.AddPointOnCanvas(DrawingEngine.Canvas);
                Rc.AddPointOnCanvas(DrawingEngine.Canvas);
              

            }
        }
        public void RemoveCarcasPointsFromCanvas()
        {
            if (DrawingEngine.Canvas != null)
            {
               
                Rb.RemovePointFromCanvas(DrawingEngine.Canvas);
                Rc.RemovePointFromCanvas(DrawingEngine.Canvas);
               
            }
        }
        private void DragAblePointChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Point");
        }
        internal IEnumerable<IEnumerable<Vector3>> CurveContourPoints()
        {
            if (IsCarcasVisible)
            {
                var tmp = LineColor;
                LineColor = Color.FromArgb(tmp.A, 255 - tmp.R, 255 - tmp.G, 255 - tmp.B);
                yield return GetCarcasCountour();
                LineColor = tmp;
            }
            Ra.UpdatePointPos();
            Rb.UpdatePointPos();
            Rc.UpdatePointPos();
            Rd.UpdatePointPos();
            yield return GetCubicParabolaCurveCountour();
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
           
          return CurveContourPoints();
           
        }
    }
}
