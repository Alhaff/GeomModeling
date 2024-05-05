using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class Lab7ViewModel : ViewModelBase
    {
        public DrawingEngineService DrawingEngine {  get; set; }
        public Transformation2DService Transformation2D { get; set; }
        public SierpinskiNapkin SierpinskiNapkin { get; set; }
        public Lab7ViewModel(DrawingEngineService drawingEngine,Transformation2DService transformation2D)
        {
            DrawingEngine = drawingEngine;
            Transformation2D = transformation2D;
            SierpinskiNapkin = new SierpinskiNapkin();
            SierpinskiNapkin.LineColor = Color.Purple;
            SierpinskiNapkin.LineThickness = 1;
            SierpinskiNapkin.Transformations.AddTransformation(Transformation2D.Offset);
            SierpinskiNapkin.Transformations.AddTransformation(Transformation2D.Rotate);
            DrawingEngine.Models.AddModel(SierpinskiNapkin);
        }

       

    }
}
