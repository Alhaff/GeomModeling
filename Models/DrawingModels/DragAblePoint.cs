using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GeometricModeling.Models.DrawingTransformation;

namespace GeometricModeling.Models.DrawingModels
{
    public class DragAblePoint : ModelBase
    {
        #region Variables
        private Vector3 _center = new Vector3(0, 0, 1);
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get => _center;
            set => SetProperty(ref _center, value, nameof(Center));
        }
        public Vector3 CenterVithoutPropertyChaged
        {
            get => _center;
            set => _center = value;
        }
        public System.Windows.Shapes.Ellipse Ellipse { get; set; }

        private bool DragIsOver { get; set; } = true;
        private bool IsPointOnCanvas { get; set; } = false;
        #endregion

        public DragAblePoint(DrawingEngineService drawingEngineService, Color color)
        {
            DrawingEngine = drawingEngineService;
            LineColor = color;
            DrawPriority = 0;
            Ellipse = new System.Windows.Shapes.Ellipse();
            Ellipse.Height = 0.4 * DrawingEngine.PixelsInCell;
            Ellipse.Width = 0.4 * DrawingEngine.PixelsInCell;
            Ellipse.Fill = new System.Windows.Media.SolidColorBrush(
                                      System.Windows.Media.Color.FromArgb(
                                                              LineColor.A,
                                                              LineColor.R,
                                                              LineColor.G,
                                                              LineColor.B));
            Ellipse.MouseMove += DragAblePoint_MouseMove;
        }

        public DragAblePoint(DrawingEngineService drawingEngineService, float posX, float posY, float posZ, Color color)
            : this(drawingEngineService, color)
        {
            _center = new Vector3(posX, posY, posZ);
        }

        public DragAblePoint(DrawingEngineService drawingEngineService, Vector3 pos, Color color)
            : this(drawingEngineService, pos.X, pos.Y, pos.Z, color)
        {

        }
        public void DragAblePoint_MouseMove(object sender, MouseEventArgs e)
        {
            Func<int, byte> opositeColor = (par) => (byte)Math.Abs(255 - par);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragIsOver = false;
                var chosenPoint = sender as System.Windows.Shapes.Ellipse;
                var savedColor = chosenPoint.Fill;
                chosenPoint.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(LineColor.A,
                                                                      opositeColor(LineColor.R),
                                                                      opositeColor(LineColor.G),
                                                                      opositeColor(LineColor.B)
                                                                      ));
                DragDrop.DoDragDrop(Ellipse, Ellipse, DragDropEffects.Move);
                chosenPoint.Fill = savedColor;
                var plCoord = DrawingEngine.ToWorldCoord(new Vector3(
                                                        (float)(Canvas.GetLeft(chosenPoint) + chosenPoint.Width / 2),
                                                        (float)(Canvas.GetTop(chosenPoint) + chosenPoint.Height / 2), 1));
                Center = new Vector3(plCoord.X, plCoord.Y, Center.Z);
                DragIsOver = true;
            }
        }

        public void AddPointOnCanvas(Canvas canvas)
        {
            if (!IsPointOnCanvas)
            {
                IsPointOnCanvas = true;
                canvas.Children.Add(Ellipse);
                var coord = DrawingEngine.ChooseScreenCoordTransformation(IsAffectedByExternalTransformation)(Center);
                Ellipse.Fill = new System.Windows.Media.SolidColorBrush(
                                      System.Windows.Media.Color.FromArgb(
                                                              LineColor.A,
                                                              LineColor.R,
                                                              LineColor.G,
                                                              LineColor.B));
                Canvas.SetLeft(Ellipse, coord.X - Ellipse.Width / 2);
                Canvas.SetTop(Ellipse, coord.Y - Ellipse.Height / 2);
                IsPointOnCanvas = true;
            }
        }
        public void RemovePointFromCanvas(Canvas canvas)
        {
            if (IsPointOnCanvas)
            {   
                canvas.Children.Remove(Ellipse);
                IsPointOnCanvas = false;
            }
        }

        public DrawingEngineService DrawingEngine { get; set; }

        public void UpdatePointPos()
        {
            if (DragIsOver)
            {
                var coord = DrawingEngine.ChooseScreenCoordTransformation(IsAffectedByExternalTransformation)(Center);
                Canvas.SetLeft(Ellipse, coord.X - Ellipse.Width / 2);
                Canvas.SetTop(Ellipse, coord.Y - Ellipse.Height / 2);
            }
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            UpdatePointPos();
            yield return new[] { _center, _center };
        }

        public static implicit operator Vector3(DragAblePoint dragAblePoint) => dragAblePoint.Center;
    }
}
