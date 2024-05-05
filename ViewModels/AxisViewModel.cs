using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GeometricModeling.ViewModels
{
   
    public class AxisViewModel : ViewModelBase
    {
        public DrawingEngineService DrawingEngine { get; set; }

        public Transformation2DService TransformationService { get; set; }
        public Axises Axis { get; set; }

        public TextModel XText { get; set; }

        public TextModel YText { get; set; }
        public TextModel XStep { get; set; }
        public TextModel YStep { get; set; }

        public AxisViewModel(DrawingEngineService drawingEngineService, Transformation2DService transformation2DService)
        {
            DrawingEngine = drawingEngineService;
            TransformationService = transformation2DService;
            Axis = new Axises(50, 50);
            Axis.DrawPriority = 0;
            XText = new TextModel(DrawingEngine, "X", new Vector3(Axis.EndXAxis, -0.3f, 1), Axis.XAxisColor);
            YText = new TextModel(DrawingEngine, "Y", new Vector3(-2, Axis.EndYAxis, 1), Axis.YAxisColor);
            XStep = new TextModel(DrawingEngine, "1", new Vector3(1, -0.3f, 1), Axis.XAxisColor);
            YStep = new TextModel(DrawingEngine, "1", new Vector3(-2, 1, 1), Axis.YAxisColor);
            DrawingEngine.Models.AddModel(Axis);
        }
    }
}
