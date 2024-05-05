using GeometricModeling.Extensions;
using GeometricModeling.Models.DrawingTransformations.Transformations2D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GeometricModeling.Models.DrawingModels.Models2D
{
    public class ContourCreatorWithCubicParabolaCurves : ModelBase
    {
        #region Variables
        private bool _isCurvesCarcasVisible = false;
        private bool _isCurvesPointsVisible = true;
        #endregion

        #region Propreties
        DrawingEngineService DrawingEngine { get; set; }
        public System.Drawing.Color PointColor { get; set; }

        private List<List<CubicParabolaCurve>> Contours { get; set; }
        public bool IsCurvesCarcasVisible
        {
            get => _isCurvesCarcasVisible;
            set
            {
                if (_isCurvesCarcasVisible == value) return;
                _isCurvesCarcasVisible = value;
                if (_isCurvesCarcasVisible)
                {
                    AddCurveCarcasOnCanvas();
                }
                else
                {
                    RemoveCurveCarcasFromCanvas();
                }

                OnPropertyChanged(nameof(IsCurvesCarcasVisible));
            }

        }


        public bool IsCurvesPointsVisible
        {
            get => _isCurvesPointsVisible;
            set
            {
                if (_isCurvesPointsVisible == value) return;
                _isCurvesPointsVisible = value;
                if (_isCurvesPointsVisible)
                {
                    AddCurvePointsOnCanvas();
                }
                else
                {
                    RemoveCurvePointsFromCanvas();
                }

                OnPropertyChanged(nameof(IsCurvesPointsVisible));
            }
        }
        #endregion

        public ContourCreatorWithCubicParabolaCurves(DrawingEngineService drawingEngine, System.Drawing.Color pointColor, System.Drawing.Color lineColor)
        {
            DrawingEngine = drawingEngine;
            PointColor = pointColor;
            LineColor = lineColor;
            Contours = new List<List<CubicParabolaCurve>>() { new List<CubicParabolaCurve>() };
            
        }
      
        #region ContoursMethods
        private void SetUpCurve(CubicParabolaCurve curve)
        {
            curve.DrawingEngine = DrawingEngine;
            curve.IsCarcasVisible = IsCurvesCarcasVisible;
            curve.LineColor = LineColor;
            curve.PointColor = PointColor;
            curve.Transformations = Transformations;
            curve.Ra.Ellipse.MouseDown += NodePointMouseDown;
            curve.Rd.Ellipse.MouseDown += NodePointMouseDown;
            curve.PropertyChanged += Curve_PropertyChanged;
        }

        private void Curve_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Point")
            {
                OnPropertyChanged();
            }
        }
        public void ClearContour()
        {
            foreach (var contour in Contours)
            {
                foreach (var curve in contour)
                {
                    curve.PropertyChanged -= Curve_PropertyChanged;
                }
            }
            RemoveCurvePointsFromCanvas();
            Contours.Clear();
            Contours.Add(new List<CubicParabolaCurve>());
            OnPropertyChanged("Contours");
        }
        public void AddCurve(CubicParabolaCurve curve)
        {
            SetUpCurve(curve);
            Contours.Last().Add(curve);
            if(IsCurvesCarcasVisible)
            {
                curve.AddCarcasPointsOnCanvas();
            }
            OnPropertyChanged("Contours");
        }
       
        public void AddCurve(params CubicParabolaCurve[] curves)
        {
            foreach (var curve in curves)
            {
                SetUpCurve(curve);
                Contours.Last().Add(curve);
                if (IsCurvesCarcasVisible)
                {
                    curve.AddCarcasPointsOnCanvas();
                }
            }
            OnPropertyChanged("Contours");
        }
        public void AddContour(List<CubicParabolaCurve> curves)
        {
            foreach (var curve in curves)
            {
                SetUpCurve(curve);
                if (IsCurvesCarcasVisible)
                {
                    curve.AddCarcasPointsOnCanvas();
                }
            }
            Contours.Add(curves);
            OnPropertyChanged("Contours");
        }
        #endregion

        #region AddPointsOnCanvas
        public void RemoveCurvePointsFromCanvas()
        {
            if (DrawingEngine.Canvas is not null)
            {
               
                _isCurvesCarcasVisible = false;
                foreach (var contour in Contours)
                {
                    foreach (var curve in contour)
                    {
                        curve.RemoveLinePointsFromCanvas();
                        curve.IsCarcasVisible = IsCurvesCarcasVisible;
                        curve.RemoveCarcasPointsFromCanvas();
                    }
                }
            }
        }
        public void AddCurvePointsOnCanvas()
        {
            if (DrawingEngine.Canvas is not null)
            {
                if (!IsCurvesPointsVisible) return;
                _isCurvesCarcasVisible = true;
                foreach (var contour in Contours)
                {
                    foreach (var curve in contour)
                    {
                        curve.AddLinePointsOnCanvas();
                        curve.IsCarcasVisible = IsCurvesCarcasVisible;
                        curve.AddCarcasPointsOnCanvas();
                    }
                }
            }
        }

        public void AddCurveCarcasOnCanvas()
        {
            if (DrawingEngine.Canvas is not null)
            {
                if (IsCurvesCarcasVisible)
                {
                    foreach (var contour in Contours)
                    {
                        foreach (var curve in contour)
                        {
                            curve.AddCarcasPointsOnCanvas();
                            curve.IsCarcasVisible = IsCurvesCarcasVisible;
                        }
                    }
                }
            }
        }

        public void RemoveCurveCarcasFromCanvas()
        {
            if (DrawingEngine.Canvas is not null)
            {
                if (!IsCurvesCarcasVisible)
                {
                    foreach (var contour in Contours)
                    {
                        foreach (var curve in contour)
                        {
                            curve.RemoveCarcasPointsFromCanvas();
                            curve.IsCarcasVisible = IsCurvesCarcasVisible;
                        }
                    }
                }
            }
        }
        #endregion

        #region WriteReadOperations
        public void WriteContourToFile(string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                foreach (var contour in Contours)
                {
                   // if (contour.Count == 1 || contour.Count == 16 || Contours[19] == contour) continue;
                    foreach (var curve in contour)
                    {
                       
                        foreach (var point in curve.CarcasPoints)
                        {
                            
                            writer.Write(point.X);
                            writer.Write(point.Y);
                            writer.Write(point.Z);
                        }
                    }
                    writer.Write(';');
                }
                writer.Close();
            }
        }
      
        private IEnumerable<Vector3> GetVectorsFromFile(BinaryReader reader, int vectorCount)
        {
            for (int i = 0; i < vectorCount; i++)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                yield return new Vector3(x, y, z);
            }
        }
        public void ReadContourFromFile(string filePath)
        {
            ClearContour();
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open), Encoding.ASCII))
            {
                var res = reader.PeekChar();
                try
                {
                   
                    var points = new Vector3[4];
                    while (res > -1)
                    {
                        points = GetVectorsFromFile(reader, 4).ToArray();
                        var curve = new CubicParabolaCurve(DrawingEngine, points[0], points[1],
                                                                                  points[2], points[3],
                                                                                  PointColor, LineColor);
                        SetUpCurve(curve);
                        if(Contours.Last().Count>0)
                        {
                            Contours.Last().Last().Rd = curve.Ra;
                        }
                        Contours.Last().Add(curve);
                        res = reader.PeekChar();
                        if((char)res == ';')
                        {
                            reader.ReadChar();
                            Contours.Last().First().Ra = Contours.Last().Last().Rd;
                            Contours.Add(new List<CubicParabolaCurve>());
                            res = reader.PeekChar();
                        }
                    }
                }
                catch
                {

                }
                AddCurvePointsOnCanvas();
            }
            OnPropertyChanged(nameof(Contours));
        }

        #endregion

        #region Animation
        private IEnumerable<CubicParabolaCurve> GetUnconnectedCurves()
        {

            foreach(var contour in Contours)
            {
                foreach(var curve in contour)
                {
                    curve.PropertyChanged -= Curve_PropertyChanged;
                    curve.Ra = new DragAblePoint(DrawingEngine, curve.Ra, PointColor);
                    curve.Rd = new DragAblePoint(DrawingEngine,curve.Rd, PointColor);
                    curve.PropertyChanged += Curve_PropertyChanged;
                    yield return curve;
                }
            }
        }
        private void PrepareCurrentContourToRunAnimation(ContourCreatorWithCubicParabolaCurves otherContour)
        {
            var allCurrentContourCurves = GetUnconnectedCurves().ToList();
            int i = 0;
            var newContour = new List<List<CubicParabolaCurve>>();
            foreach (var contour in otherContour.Contours)
            {
                newContour.Add(new List<CubicParabolaCurve>());
                foreach (var curve in contour)
                {
                    if (i < allCurrentContourCurves.Count)
                    {
                        newContour.Last().Add(allCurrentContourCurves[i]);
                    }
                    else
                    {
                        var j = i % allCurrentContourCurves.Count;
                        var newCurve = new CubicParabolaCurve(DrawingEngine, allCurrentContourCurves[j].Ra.Center, allCurrentContourCurves[j].Rb.Center,
                                                                          allCurrentContourCurves[j].Rc.Center, allCurrentContourCurves[j].Rd.Center,
                                                                        PointColor, LineColor);
                        SetUpCurve(newCurve);
                        newContour.Last().Add(newCurve);
                    }
                    i++;
                }
            }
            if( i < allCurrentContourCurves.Count)
            {
                for(int j = i; i < allCurrentContourCurves.Count;i++)
                {
                    allCurrentContourCurves[j].PropertyChanged -= Curve_PropertyChanged;
                }
            }
            Contours = newContour;
        }

        private List<List<List<Vector3>>> GetOffsetPointsArr(ContourCreatorWithCubicParabolaCurves otherContour, int animationStep)
        {
            var lst = new List<List<List<Vector3>>>();
             
            for (int i = 0; i < Contours.Count; i++)
            {
                lst.Add(new List<List<Vector3>>());
                for (int j = 0; j < Contours[i].Count; j++)
                {
                    lst[i].Add(new List<Vector3>() { (otherContour.Contours[i][j].Ra.Center - Contours[i][j].Ra.Center) / (float)animationStep,
                                         (otherContour.Contours[i][j].Rb.Center - Contours[i][j].Rb.Center) / (float)animationStep,
                                         (otherContour.Contours[i][j].Rc.Center - Contours[i][j].Rc.Center) / (float)animationStep,
                                         (otherContour.Contours[i][j].Rd.Center - Contours[i][j].Rd.Center) / (float)animationStep,
                    });
                }
            }
            return lst;
        }
        private void ConnectNewContourPoints()
        {
            for(int i = 0; i < Contours.Count;i++)
            {
                for(int j = 1; j < Contours[i].Count; j++)
                {
                    Contours[i][j].Ra = Contours[i][j - 1].Rd;
                }
                if (Contours[i].Count > 0) Contours[i][0].Ra = Contours[i][^1].Rd;
            }
        }

        public async Task ChangeAnimationToOtherContour(ContourCreatorWithCubicParabolaCurves otherContour, int animationStepAmount)
        {
            //IsCurvesPointsVisible = false;
            RemoveCurvePointsFromCanvas();
            PrepareCurrentContourToRunAnimation(otherContour);
            var offsets = await Task.Run(() =>GetOffsetPointsArr(otherContour, animationStepAmount));
            for(int step = 0; step < animationStepAmount; step++)
            {
                for(int i =0; i < Contours.Count; i++)
                {
                    for(int j =0; j < Contours[i].Count; j++)
                    {
                        Contours[i][j].Ra.CenterVithoutPropertyChaged += offsets[i][j][0];
                        Contours[i][j].Rb.CenterVithoutPropertyChaged += offsets[i][j][1];
                        Contours[i][j].Rc.CenterVithoutPropertyChaged += offsets[i][j][2];
                        Contours[i][j].Rd.CenterVithoutPropertyChaged += offsets[i][j][3];
                       
                    }
                }
                OnPropertyChanged(nameof(Contours));
                await Task.Delay(10);
            }
            ConnectNewContourPoints();
        }

        #endregion

        #region Lab4SmoothnessInPoint
        private bool IBreakPoint(CubicParabolaCurve left,CubicParabolaCurve right)
        {
            if (left.Rd.Center == right.Ra.Center)
            {
                var leftTng = left.RdLeftTangentValue;
                var rightTng = right.RaRightTangentValue;
                var diff = Math.Abs(Math.Abs(leftTng) - Math.Abs(rightTng));
                return !(diff <= 1E-4);
            }
            return true;
        }
        private (CubicParabolaCurve leftcurve,CubicParabolaCurve rightCurve) GetCurvesAroundPoint(Ellipse ellipse)
        {
            for(int i = 0; i < Contours.Count;i++)
            {
                if(ellipse == Contours[i][0].Ra.Ellipse)
                {
                    return (Contours[i][^1], Contours[i][0]);
                }
                for(int j = 1; j < Contours[i].Count; j++)
                {
                    if(ellipse == Contours[i][j].Ra.Ellipse)
                    {
                        return (Contours[i][j - 1],Contours[i][j]);

                    }
                }
            }
            return (null, null);
        }
        
        public void NodePointMouseDown(object sender,MouseEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                var pointEllipse = sender as System.Windows.Shapes.Ellipse;
                CubicParabolaCurve leftCurve, rightCurve;
                (leftCurve, rightCurve) = GetCurvesAroundPoint(pointEllipse);
                if (leftCurve == null || rightCurve == null) return;
                if (!leftCurve.IsCarcasVisible)
                {
                    leftCurve.IsCarcasVisible = true;
                    rightCurve.IsCarcasVisible = true;
                    leftCurve.AddCarcasPointsOnCanvas();
                    rightCurve.AddCarcasPointsOnCanvas();
                }else
                {
                    leftCurve.IsCarcasVisible = false;
                    rightCurve.IsCarcasVisible = false;
                    leftCurve.RemoveCarcasPointsFromCanvas();
                    rightCurve.RemoveCarcasPointsFromCanvas();
                }
                if(IBreakPoint(leftCurve,rightCurve))
                {
                    if (!leftCurve.IsRdLeftTangentVisible)
                    {
                      
                        pointEllipse.Fill = new SolidColorBrush(Colors.Red);
                        leftCurve.IsRdLeftTangentVisible = true;
                        rightCurve.IsRaRightTangentVisible = true;
                        leftCurve.IsCarcasVisible = true;
                        rightCurve.IsCarcasVisible = true;
                        leftCurve.AddCarcasPointsOnCanvas();
                        rightCurve.AddCarcasPointsOnCanvas();

                    }
                    else
                    {
                        
                        pointEllipse.Fill = new SolidColorBrush(
                            System.Windows.Media.Color.FromArgb(PointColor.A,
                                                                PointColor.R,
                                                                PointColor.G,
                                                                PointColor.B));
                        leftCurve.IsRdLeftTangentVisible = false;
                        rightCurve.IsRaRightTangentVisible = false;
                       
                    }
                }
                else
                {
                    if (leftCurve.IsCarcasVisible || leftCurve.IsRdLeftTangentVisible)
                    {
                        pointEllipse.Fill = new SolidColorBrush(Colors.Green);
                        leftCurve.Rc.Ellipse.MouseMove -= leftCurve.Rc.DragAblePoint_MouseMove;
                        leftCurve.Rd.Ellipse.MouseMove -= leftCurve.Rd.DragAblePoint_MouseMove;
                        rightCurve.Rb.Ellipse.MouseMove -= rightCurve.Rb.DragAblePoint_MouseMove;

                        leftCurve.Rc.Ellipse.PreviewMouseMove += NodeNeighbourMouseMove;
                        leftCurve.Rd.Ellipse.PreviewMouseMove += NodePointMouseMove;
                        rightCurve.Rb.Ellipse.PreviewMouseMove += NodeNeighbourMouseMove;

                        leftCurve.IsCarcasVisible = true;
                        rightCurve.IsCarcasVisible = true;
                        leftCurve.AddCarcasPointsOnCanvas();
                        rightCurve.AddCarcasPointsOnCanvas();
                        leftCurve.IsRdLeftTangentVisible = false;
                        rightCurve.IsRaRightTangentVisible = false;
                    }
                    else
                    {
                        pointEllipse.Fill = new SolidColorBrush(
                           System.Windows.Media.Color.FromArgb(PointColor.A,
                                                               PointColor.R,
                                                               PointColor.G,
                                                               PointColor.B));
                       

                        leftCurve.Rc.Ellipse.MouseMove += leftCurve.Rc.DragAblePoint_MouseMove;
                        leftCurve.Rd.Ellipse.MouseMove += leftCurve.Rd.DragAblePoint_MouseMove;
                        rightCurve.Rb.Ellipse.MouseMove += rightCurve.Rb.DragAblePoint_MouseMove;

                        leftCurve.Rc.Ellipse.MouseMove -= NodeNeighbourMouseMove;
                        leftCurve.Rd.Ellipse.MouseMove -= NodePointMouseMove;
                        rightCurve.Rb.Ellipse.MouseMove -= NodeNeighbourMouseMove;

                    }
                }
                OnPropertyChanged(nameof(Contours));
            }
        }
        public void NodePointMouseMove(object sender, MouseEventArgs e)
        {
            var pointEllipse = sender as System.Windows.Shapes.Ellipse;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CubicParabolaCurve leftCurve, rightCurve;
                (leftCurve, rightCurve) = GetCurvesAroundPoint(pointEllipse);
                var start = rightCurve.Ra.Center;
                DragDrop.DoDragDrop(pointEllipse, pointEllipse, DragDropEffects.Move);
                var plCoord = DrawingEngine.ToWorldCoord(new Vector3(
                                                       (float)(Canvas.GetLeft(pointEllipse) + pointEllipse.Width / 2),
                                                       (float)(Canvas.GetTop(pointEllipse) + pointEllipse.Height / 2), 1));
                var end = new Vector3(plCoord.X, plCoord.Y, rightCurve.Ra.Center.Z);
                var offset = end - start;
                var tmpOffset = new Offset2DTransformation() { DX = offset.X, DY = offset.Y };
                leftCurve.Rc.CenterVithoutPropertyChaged = tmpOffset.Transform(leftCurve.Rc.Center);
                rightCurve.Rb.CenterVithoutPropertyChaged = tmpOffset.Transform(rightCurve.Rb.Center);
                leftCurve.Rc.UpdatePointPos();
                rightCurve.Rb.UpdatePointPos();
                rightCurve.Ra.Center = end;
            }
        }
        private (DragAblePoint chosen, DragAblePoint mid, DragAblePoint other) GetNeighbours(Ellipse ellipse)
        {
            for (int i = 0; i < Contours.Count; i++)
            {
                if (Contours[i][0].Rb.Ellipse == ellipse)
                {
                    return (Contours[i][0].Rb, Contours[i][0].Ra, Contours[i][^1].Rc);
                }
                else if (Contours[i][0].Rc.Ellipse == ellipse)
                {
                    return (Contours[i][0].Rc, Contours[i][0].Rd, Contours[i][1].Rb);
                }
                else if (Contours[i][^1].Rb.Ellipse == ellipse)
                {
                    return (Contours[i][^1].Rb, Contours[i][^1].Ra, Contours[i][^2].Rc);
                }
                else if (Contours[i][^1].Rc.Ellipse == ellipse)
                {
                    return (Contours[i][^1].Rc, Contours[i][^1].Rd, Contours[i][0].Rb);
                }
                for (int j = 1; j < Contours[i].Count -1; j++)
                {
                    var curve = Contours[i][j];
                    if (curve.Rb.Ellipse == ellipse)
                    {
                        return (curve.Rb,curve.Ra,Contours[i][j - 1].Rc);
                    }
                    else if (curve.Rc.Ellipse == ellipse)
                    {
                        return (curve.Rc, curve.Rd,Contours[i][j + 1].Rb);
                    }
                }
            }
            throw new KeyNotFoundException("Ellipse doesn't exist in contour");
        }
       
        public void NodeNeighbourMouseMove(object sender, MouseEventArgs e) 
        {
            var pointEllipse = sender as System.Windows.Shapes.Ellipse;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var (chosenPoint, mid, otherPoint) = GetNeighbours(pointEllipse);
                var startAngle = (chosenPoint.Center - mid.Center).Angle();
                DragDrop.DoDragDrop(pointEllipse, pointEllipse, DragDropEffects.Move);
                var plCoord = DrawingEngine.ToWorldCoord(new Vector3(
                                                       (float)(Canvas.GetLeft(pointEllipse) + pointEllipse.Width / 2),
                                                       (float)(Canvas.GetTop(pointEllipse) + pointEllipse.Height / 2), 1));
                var pointCoord = new Vector3(plCoord.X,plCoord.Y,chosenPoint.Center.Z); 
                var endAngle = (pointCoord - mid.Center).Angle();
                var rotateAngle = endAngle - startAngle;
                var tmpRotate = new Rotate2DTransformation(mid);
                tmpRotate.Angle = rotateAngle;
                otherPoint.CenterVithoutPropertyChaged = tmpRotate.Transform(otherPoint.Center);
                otherPoint.UpdatePointPos();
                chosenPoint.Center = pointCoord;
            }
        }
       
        #endregion
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            foreach(var contour in Contours)
            {
                foreach(var curve in contour)
                {
                    var tmp = LineColor;
                    foreach (var point in curve.CurveContourPoints())
                    {
                        LineColor = curve.LineColor;
                        yield return point;
                    }
                    LineColor = tmp;
                }
            }
        }
    }
}
