using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Models.DrawingModels.Models3D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class Axis3DViewModel : ViewModelBase
    {
        public DrawingEngineService DrawingEngine { get; set; }

        public Transformation3DService Transformation3D { get; set; }

        public Axis3d CoordinateAxis { get; set; }

        public Axis3d RotateAxis { get; set; }
        public TextModel XText { get; set; }

        public TextModel YText { get; set; }
        public TextModel ZText { get; set; }

     
        public Axis3DViewModel(DrawingEngineService drawingEngineService, Transformation3DService transformation3DService) 
        {
            DrawingEngine = drawingEngineService;
            Transformation3D = transformation3DService;
            CoordinateAxis = new Axis3d();
            RotateAxis = new Axis3d();
            RotateAxis.XAxisColor = System.Drawing.Color.Magenta;
            RotateAxis.YAxisColor = System.Drawing.Color.Magenta;
            RotateAxis.ZAxisColor = System.Drawing.Color.Magenta;
            RotateAxis.DrawPriority = 1;
            CoordinateAxis.DrawPriority = 2;
            CoordinateAxis.LineThickness = 2;
            XText = new TextModel(DrawingEngine, "X", new Vector3(CoordinateAxis.EndX, -0.3f, 0), CoordinateAxis.XAxisColor);
            YText = new TextModel(DrawingEngine, "Y", new Vector3(-2, CoordinateAxis.EndY, 4), CoordinateAxis.YAxisColor);
            ZText = new TextModel(DrawingEngine, "Z", new Vector3(-2, 4 , CoordinateAxis.EndZ), CoordinateAxis.ZAxisColor);
            DrawingEngine.Models.AddModel(RotateAxis);
            DrawingEngine.Models.AddModel(CoordinateAxis);
            Transformation3D.Rotate3D.PropertyChanged += RotateCenter_PropertyChanged;
        }

        private void RotateCenter_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Center")
            {
                RotateAxis.Center = Transformation3D.Rotate3D.Center;
              
            }
        }
    }
}
