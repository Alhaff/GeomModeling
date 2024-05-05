using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Extensions;
using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Models.DrawingTransformation;
using GeometricModeling.Models.DrawingTransformations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Formats.Asn1.AsnWriter;

namespace GeometricModeling.Services
{
	public delegate void DrawNonModelObjects(Graphics graphics);
    public class DrawingEngineService : ObservableObject
    {
        public DrawingEngineService() 
        {
            _models = new ModelStore();
            _models.PropertyChanged += DataChanged;
            _transformations = new TransformationStore();
            _transformations.PropertyChanged += DataChanged;
        }
        #region Variables
        private int _stepInPixels = 20;
        private float _worldScaleCoef = 1f;
        private Vector2 _worldOffset = new Vector2(-1,-40);
        private Vector2 _worldStartPan =  new Vector2(0,0);
        //private Vector2 _worldScale = new Vector2(1280/2.0f,720);
        private ModelStore _models;
        private TransformationStore _transformations;
        #endregion

        #region Propreties
        public float WorldScaleCoef
		{
			get => _worldScaleCoef;
			set => SetProperty(ref _worldScaleCoef, value,nameof(WorldScaleCoef));
		}

        public float WorldScale
        {
            get => PixelsInCell * WorldScaleCoef; 
           
        }
		public Vector2 WorldOffset
		{
			get => _worldOffset;
			set => SetProperty(ref _worldOffset, value,nameof(WorldOffset));
		}
		public Vector2 WorldStartPan
		{
			get => _worldStartPan;
			set => SetProperty(ref _worldStartPan,value,nameof(WorldStartPan));
		}
        public int PixelsInCell 
		{
			get => _stepInPixels;
            set
            {
                if (value <= 0) return;
                var mousePos = ToScreenCoord(new Vector3(0, 0, 1));
                var worldBeforeChange = ToWorldCoord(mousePos);
                _stepInPixels = value;
                var worldAfterChange = ToWorldCoord(mousePos);
                var tmp = (worldBeforeChange - worldAfterChange);
                WorldOffset += new Vector2(tmp.X, -tmp.Y);
                OnPropertyChanged(nameof(PixelsInCell));
            }
		}
        private bool _isDraawingEnd = true;
        public bool IsDrawingEnd
        {
            get => _isDraawingEnd;
            set => SetProperty(ref _isDraawingEnd, value,nameof(IsDrawingEnd));
        }
        public ModelStore Models
        {
            get => _models;
            set => SetProperty(ref _models, value,nameof(Models));
        }
        public TransformationStore Transformations
        {
            get => _transformations;
            set => SetProperty(ref _transformations, value,nameof(Transformations));
        }

        private System.Windows.Controls.Canvas _canvas;

        public System.Windows.Controls.Canvas Canvas
        {
            get => _canvas;
            set => SetProperty(ref _canvas, value, nameof(Canvas));
        }

        #endregion

        #region Events
        public event DrawNonModelObjects? DrawNonModel;
        #endregion

        public Vector3 ToWorldCoord(Vector3 screenCoord)
		{
            float x, y = 0;
            x = (screenCoord.X) / WorldScale + WorldOffset.X ;
            y = (-screenCoord.Y)/ WorldScale - WorldOffset.Y;
            return new Vector3(x,y,1);
        }

		public Vector3 ToScreenCoord(Vector3 worldCoord)
		{
            worldCoord = Transformations.ResultTransformation(worldCoord);
            float x, y = 0;
            x = (worldCoord.X - WorldOffset.X) * WorldScale ; 
            y = (-worldCoord.Y - WorldOffset.Y) * WorldScale ;
            return new Vector3(x, y, 1);
        }
        public Vector3 ToScreenCoordWithoutTransformation(Vector3 worldCoord)
        {
            float x, y = 0;
            x = ((worldCoord.X) - WorldOffset.X)  * WorldScale;
            y = ((-worldCoord.Y) - WorldOffset.Y) * WorldScale;
            return new Vector3(x, y , 1);
        }
        public void Zoom(Vector2 mousePosInScreen, bool IZoomIn)
        {
            var mouseBeforeZoom = ToWorldCoord(new Vector3(mousePosInScreen.X, mousePosInScreen.Y, 1));
            WorldScaleCoef *= IZoomIn ? 1.1f : 0.9f;
            var mouseAfterZoom = ToWorldCoord(new Vector3((float)mousePosInScreen.X, (float)mousePosInScreen.Y, 1));
            var tmp = (mouseBeforeZoom - mouseAfterZoom);
            WorldOffset += new Vector2(tmp.X, -tmp.Y);
        }

        protected virtual void OnDrawNonModelObjects(Graphics graphics)
        {
            DrawNonModel?.Invoke(graphics);
        }

        public void Draw(Graphics graphics)
        {
            IsDrawingEnd = false;
            graphics.Clear(System.Drawing.Color.White);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            foreach(var model in Models.OrderBy(model => model.DrawPriority))
            {
                if(model != null)
                {
                    DrawModel(graphics, model);
                }
            }
            IsDrawingEnd = true;
        }

        public Transformation ChooseScreenCoordTransformation(bool IsAffectedByExternalTransformation) =>
            IsAffectedByExternalTransformation? ToScreenCoord : ToScreenCoordWithoutTransformation;

        protected void DrawModel(Graphics graphics, ModelBase model)
        {
            foreach(var contour in model.GetModelContourPoints())
            {
                Transformation toScreen = ChooseScreenCoordTransformation(model.IsAffectedByExternalTransformation);
                foreach (var line in contour.Select(point => toScreen(point)).LineCreator())
                {
                    try
                    {
                        DrawLine(graphics, line, model.LineThickness, model.LineColor);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            OnDrawNonModelObjects(graphics);
        }
        public static bool IsNormalValue(Vector3 point)
        {
            return float.IsFinite(point.X) && float.IsFinite(point.Y) && float.IsFinite(point.Z);
            
        }
        protected void DrawLine(Graphics graphics, Tuple<Vector3, Vector3> line, float lineThickness, Color lineColor)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(new SolidBrush(lineColor), lineThickness);
            var vectStart = line.Item1;
            var vectEnd = line.Item2;
            if (IsNormalValue(vectStart) && IsNormalValue(vectEnd))
            {
                PointF start = new PointF((float)vectStart.X, (float)vectStart.Y);
                PointF end = new PointF((float)vectEnd.X, (float)vectEnd.Y);
                try
                {
                    graphics.DrawLine(pen, start, end);
                }catch
                {
                
                }
            }
        }

        private void DataChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Models");
        }
    }
}
