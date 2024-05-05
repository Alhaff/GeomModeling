using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Models.DrawingModels.Models3D;
using GeometricModeling.Models.DrawingTransformation;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class Lab6ViewModel : Lab3ViewModel
    {
        #region Variables
        private float _a;
        private float _b;
        private float _c;
        private int _uStepCount;
        private int _vStepCount;
        private bool _isAAndBLinked = true;
        #endregion
        #region Propreties
        public Hyperboloid Hyperboloid { get; set; }
        public float A
        {
            get => _a;
            set
            {
                if (value == 0) return;
                SetProperty(ref _a, value, nameof(A));
            }
        }
       
        public float B
        {
            get => _b;
            set
            {
                if (value == 0) return;
                SetProperty(ref _b, value, nameof(B));
            }
        }
        
        public float C
        {
            get => _c;
            set
            {
                if (value == 0) return;
                SetProperty(ref _c, value, nameof(C));
            }
        }
      
        public int UStepCount
        {
            get => _uStepCount;
            set
            {
                if(value <= 0) return;
                SetProperty(ref _uStepCount, value, nameof(UStepCount));
            }
        }
      
        public int VStepCount
        {
            get => _vStepCount;
            set
            {
                if (value <= 0) return;
                SetProperty(ref _vStepCount, value, nameof(VStepCount));
            }
        }
        public bool IsAAndBLinked
        {
            get => _isAAndBLinked;
            set => SetProperty(ref _isAAndBLinked, value, nameof(IsAAndBLinked));
        }
        private float _height;
        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value, nameof(Height));
        }
        public Axises AxisesUV { get; set; }
        #endregion

        public Lab6ViewModel(DrawingEngineService drawingEngineService, Transformation2DService transformation2DService, Transformation3DService transformation3DService) 
            : base(drawingEngineService, transformation3DService, transformation2DService)
        {
            DrawingEngine.Models.RemoveModel(Lab3);
            Hyperboloid = new Hyperboloid();
            Hyperboloid.DrawPriority = 4;
            Hyperboloid.LineThickness = 1;
            Hyperboloid.LineColor = Color.FromArgb(150,0,0,0);
            Hyperboloid.Transformations.AddTransformation(Transformation3D.Offset3D);
            Hyperboloid.Transformations.AddTransformation(Transformation3D.Rotate3D);
            Transformation3D.ToHyperboloidGrid.Hyperboloid = Hyperboloid;
            Lab3.IsCurvesCarcasVisible = false;
            Lab3.IsCurvesPointsVisible = false;
            Lab3.LineColor = Color.Purple;
            Lab3.DrawPriority = 6;
            Lab3.LineThickness = 4;
            Lab3.Transformations.AddTransformation(Transformation2D.ReducingTransformation);
            Lab3.Transformations.AddTransformation(Transformation3D.ToHyperboloidGrid);
            Lab3.Transformations.AddTransformation(Transformation3D.Offset3D);
            Lab3.Transformations.AddTransformation(Transformation3D.Rotate3D);
            DrawingEngine.Models.AddModel(Lab3);
            DrawingEngine.Models.AddModel(Hyperboloid);
         
            A = Hyperboloid.A;
            B = Hyperboloid.B;
            C = Hyperboloid.C;
            Height = Hyperboloid.Height;
            UStepCount = Hyperboloid.UStepCount;
            VStepCount = Hyperboloid.VStepCount;
            Transformation2D.Rotate.RotatePoint.IsAffectedByExternalTransformation = true;
            Transformation2D.Rotate.RotatePoint.Transformations.AddTransformation(Transformation3D.ToHyperboloidGrid);
            Transformation2D.Rotate.RotatePoint.Transformations.AddTransformation(Transformation3D.Offset3D);
            Transformation2D.Rotate.RotatePoint.Transformations.AddTransformation(Transformation3D.Rotate3D);
            AxisesUV = new Axises(20, 10);
            AxisesUV.GridColor = Color.BlueViolet;
            AxisesUV.LineThickness = 3;
            AxisesUV.DrawPriority = 5;
            AxisesUV.Transformations.AddTransformation(Transformation3D.ToHyperboloidGrid);
            AxisesUV.Transformations.AddTransformation(Transformation3D.Offset3D);
            AxisesUV.Transformations.AddTransformation(Transformation3D.Rotate3D);
            DrawingEngine.Models.AddModel(AxisesUV);

        }

        #region Commands
        private RelayCommand _setACommand;
        private RelayCommand _setBCommand;
        private RelayCommand _setCCommand;
        private RelayCommand _setHeightCommand;
        private RelayCommand _setUStepCountCommand;
        private RelayCommand _setVStepCountCommand;
        public RelayCommand SetACommand
        {
            get => _setACommand??(_setACommand = new RelayCommand(
                () =>
                {
                    if(IsAAndBLinked)
                    {
                        Hyperboloid.SetBWithoutPropertyChanged = B = A;
                    }
                    Hyperboloid.A = A;
                }
                )); 
        }
        public RelayCommand SetBCommand
        {
            get => _setBCommand ?? (_setBCommand = new RelayCommand(
                () =>
                {
                    if (IsAAndBLinked)
                    {
                        Hyperboloid.SetAWithoutPropertyChanged = A = B;
                    }
                    Hyperboloid.B = B;
                }
                ));
        }
        public RelayCommand SetCCommand
        {
            get => _setCCommand?? (_setCCommand = new RelayCommand(()=> Hyperboloid.C = C));
        }
        public RelayCommand SetHeightCommand
        {
            get => _setHeightCommand ?? (_setHeightCommand = new RelayCommand(() => Hyperboloid.Height = Height));
        }
        public RelayCommand SetUStepCountCommand
        {
            get => _setUStepCountCommand ?? (_setUStepCountCommand = new RelayCommand(() => Hyperboloid.UStepCount = UStepCount));
        }
        public RelayCommand SetVStepCountCommand
        {
            get => _setVStepCountCommand ?? (_setVStepCountCommand = new RelayCommand(() => Hyperboloid.VStepCount = VStepCount));
        }
        #endregion
    }
}
