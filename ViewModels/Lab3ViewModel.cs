using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GeometricModeling.Models.DrawingModels;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace GeometricModeling.ViewModels
{
   
    public class Lab3ViewModel : Axis3DViewModel
    {
        public ContourCreatorWithCubicParabolaCurves Lab3 { get; set; }
        public BackgroundImageModel BackgroundImage { get; set; }
        public DrawingEngineService DrawingEngine { get; set; }

        public Transformation2DService Transformation2D { get; set; }
        private bool _isDialogModeActive = false;

        public bool IsDialogModeActive
        {
            get =>_isDialogModeActive;
            set => SetProperty(ref _isDialogModeActive, value, nameof(IsDialogModeActive));
        }

        private Visibility _dialogModeSettingsVisibility = Visibility.Collapsed;

        public Visibility DialogModeSettingsVisibility
        {
            get =>_dialogModeSettingsVisibility;
            set => SetProperty(ref _dialogModeSettingsVisibility, value, nameof(DialogModeSettingsVisibility));
        }

        private Color PointColor { get => Color.Blue; }
        private Color LineColor { get => Color.Purple; }

        public Lab3ViewModel(DrawingEngineService drawingEngineService, Transformation3DService transformation3D, Transformation2DService transformation2DService)
            : base(drawingEngineService, transformation3D)
        {
            DrawingEngine = drawingEngineService;
            Transformation2D = transformation2DService;
            Lab3 = new ContourCreatorWithCubicParabolaCurves(DrawingEngine, PointColor, LineColor);
            Lab3.Transformations.AddTransformation(Transformation2D.Rotate);
            Lab3.Transformations.AddTransformation(Transformation2D.Offset);
            Lab3.LineThickness = 2;
            DrawingEngine.Models.AddModel(Lab3);
            Lab3.IsCurvesCarcasVisible = true;
            BackgroundImage = new BackgroundImageModel(DrawingEngine);
            
        }
        private DragAblePoint FirstContourPoint { get; set; }
        private DragAblePoint Start { get; set; }
        private DragAblePoint End { get; set; }
        private void Lab3_MouseDown(object sender, MouseButtonEventArgs mouseArgs)
        {
            if (mouseArgs == null) return;
            if (mouseArgs.RightButton == MouseButtonState.Pressed)
            {
                var mousePos = mouseArgs.GetPosition((IInputElement)mouseArgs.Source);
                var mouse = new Vector3((float)mousePos.X, (float)mousePos.Y,0);
                var world = DrawingEngine.ToWorldCoord(mouse);
                if(Start == null)
                {
                    Start = new DragAblePoint(DrawingEngine, world, PointColor);
                    FirstContourPoint = Start;
                    Start.AddPointOnCanvas(DrawingEngine.Canvas);
                }
                else if(End == null) 
                { 
                    End = new DragAblePoint(DrawingEngine,world, PointColor);
                    End.AddPointOnCanvas(DrawingEngine.Canvas);
                    Lab3.AddCurve(new CubicParabolaCurve(DrawingEngine, Start, End, PointColor, LineColor));
                    Start = End;
                    End = null;
                }
            }

        }

        private void DialogModeOn()
        {
            DialogModeSettingsVisibility = Visibility.Visible;
            if (DrawingEngine.Canvas != null)
            {
                DrawingEngine.Canvas.MouseDown += Lab3_MouseDown;
            }
        }

        private void DialogModeOff()
        {
            DialogModeSettingsVisibility = Visibility.Collapsed;
            if (DrawingEngine.Canvas != null)
            {
                DrawingEngine.Canvas.MouseDown -= Lab3_MouseDown;
            }
        }
        private RelayCommand _isDialogModeOn;
        public RelayCommand DialogModeOnCommand
        {
            get => _isDialogModeOn ?? (_isDialogModeOn = new RelayCommand(DialogModeOn));
        }

        private RelayCommand _isDialogModeOff;
        public RelayCommand DialogModeOffCommand
        {
            get => _isDialogModeOff ?? (_isDialogModeOff = new RelayCommand(DialogModeOff));
        }

        private void CompleteCurrentContour()
        {
            Lab3.AddCurve(new CubicParabolaCurve(DrawingEngine, Start, FirstContourPoint, PointColor, LineColor));
            Lab3.AddContour(new List<CubicParabolaCurve>());
            Start =  null;
        }

        private RelayCommand _completeCurrentContour;
        public RelayCommand CompleteCurrentContourCommand
        {
            get => _completeCurrentContour ?? (_completeCurrentContour = new RelayCommand(CompleteCurrentContour));
        }

        private void SaveCurrentContourToFile()
        {
            SaveFileDialog saveCurveDialog = new SaveFileDialog();
            saveCurveDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\");
            saveCurveDialog.Title = "Save current curve";
            saveCurveDialog.DefaultExt = "crv";
            saveCurveDialog.Filter = "Curve files (*.crv)| *.crv";
            saveCurveDialog.RestoreDirectory = true;
            if (saveCurveDialog.ShowDialog() ?? false)
            {
                Lab3.WriteContourToFile(saveCurveDialog.FileName);
            }
        }


        private RelayCommand _saveCurrentContourToFile;
        public RelayCommand SaveCurrentContourToFileCommand
        {
            get => _saveCurrentContourToFile ?? (_saveCurrentContourToFile = new RelayCommand(SaveCurrentContourToFile));
        }

        private void LoadNewContourFromFile()
        {
            OpenFileDialog loadCurveDialog = new OpenFileDialog();
            loadCurveDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\");
            loadCurveDialog.Title = "Load new curve";
            loadCurveDialog.DefaultExt = "crv";
            loadCurveDialog.Filter = "Curve files (*.crv)| *.crv";
            loadCurveDialog.RestoreDirectory = true;
            if (loadCurveDialog.ShowDialog() ?? false)
            {
                Lab3.ReadContourFromFile(loadCurveDialog.FileName);
            }
        }

        private RelayCommand _loadNewContourFromFile;
        public RelayCommand LoadNewContourFromFileCommand
        {
            get => _loadNewContourFromFile ?? (_loadNewContourFromFile = new RelayCommand(LoadNewContourFromFile));
        }

        private void LoadBackgroundImageFromFile()
        {
            OpenFileDialog loadImageDialog = new OpenFileDialog();
            loadImageDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\");
            loadImageDialog.Title = "Load image";
            loadImageDialog.DefaultExt = "png";
            loadImageDialog.Filter = "Image files (*.png;*jpg)| *.png;*.jpg";
            loadImageDialog.RestoreDirectory = true;
            if (loadImageDialog.ShowDialog() ?? false)
            {
                BackgroundImage.ImagePath = loadImageDialog.FileName;
                BackgroundImage.AddToDrawingEngine();
                DrawingEngine.Models.AddModel(BackgroundImage);
            }
        }

        private RelayCommand _loadBackgroundImage;
        public RelayCommand LoadBackgroundImageCommand
        {
            get => _loadBackgroundImage ?? (_loadBackgroundImage = new RelayCommand(LoadBackgroundImageFromFile));
        }
        private void DeleteBackgroundImage()
        {
            
            BackgroundImage.RemoveFromDrawingEngine();
            DrawingEngine.Models.RemoveModel(BackgroundImage);
        }
        private RelayCommand _deleteBackgroundImage;
        public RelayCommand DeleteBackgroundImageCommand
        {
            get => _deleteBackgroundImage ?? (_deleteBackgroundImage = new RelayCommand(DeleteBackgroundImage));
        }

        private async Task RunContourChangeAnimation()
        {
            OpenFileDialog loadCurveDialog = new OpenFileDialog();
            loadCurveDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\");
            loadCurveDialog.Title = "Load new curve to run animation";
            loadCurveDialog.DefaultExt = "crv";
            loadCurveDialog.Filter = "Curve files (*.crv)| *.crv";
            loadCurveDialog.RestoreDirectory = true;
            ContourCreatorWithCubicParabolaCurves tmp = new ContourCreatorWithCubicParabolaCurves(DrawingEngine, PointColor, LineColor);
            tmp.IsCurvesPointsVisible = false;
            if (loadCurveDialog.ShowDialog() ?? false)
            {
                tmp.ReadContourFromFile(loadCurveDialog.FileName);
                await Lab3.ChangeAnimationToOtherContour(tmp, AnimationStepAmount);
            }
        }
        private int _animationStepAmount = 100;

        public int AnimationStepAmount
        {
            get => _animationStepAmount;
            set
            {
                if (value < 0) return;
                SetProperty(ref _animationStepAmount, value, nameof(AnimationStepAmount));
            }
        }

        private AsyncRelayCommand _runContourChangeAnimation;
        public AsyncRelayCommand RunContourChangeAnimationCommand
        {
            get => _runContourChangeAnimation ?? (_runContourChangeAnimation = new AsyncRelayCommand(RunContourChangeAnimation));
        }

        public void ClearContour()
        {
            FirstContourPoint = null;
            Lab3.ClearContour();
            if(Start != null)
            {
                Start.RemovePointFromCanvas(DrawingEngine.Canvas);
                Start = null;
            }
        }
        private RelayCommand _clearContour;

        public RelayCommand ClearContourCommand
        {
            get => _clearContour ?? (_clearContour = new RelayCommand(ClearContour));
        }
    }
}
