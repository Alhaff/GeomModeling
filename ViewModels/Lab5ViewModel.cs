using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Models.DrawingModels.Models3D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class Lab5ViewModel : Axis3DViewModel
    {
        #region Variables
        private float _length = 6;
        private float _width = 2;
        private float _height = 5;
        private double _angle = 0;
        #endregion

        #region Propreties
        public float Length
        {
            get => _length;
            set => SetProperty(ref _length, value, nameof(Length));
        }
        public float Width
        {
            get => _width;
            set => SetProperty(ref _width, value, nameof(Width));
        }
        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value, nameof(Height));
        }
        public double Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value, nameof(Angle));
        }
        #endregion
        public Parallelepiped Parallelepiped { get; set; }
        public Lab5ViewModel(DrawingEngineService drawingEngineService, Transformation3DService transformation3DService) : 
            base(drawingEngineService, transformation3DService)
        {
            Parallelepiped = new Parallelepiped();
            Parallelepiped.DrawPriority = 3;
            Parallelepiped.LineThickness = 3;
            Parallelepiped.LineColor = Color.Purple;
            DrawingEngine.Models.AddModel(Parallelepiped);
            Parallelepiped.Transformations.AddTransformation(Transformation3D.Offset3D);
            Parallelepiped.Transformations.AddTransformation(Transformation3D.Rotate3D);
        }
        #region Commands
        private RelayCommand _setLengthCommand;
        private RelayCommand _setWidthCommand;
        private RelayCommand _setHeightCommand;
        private RelayCommand _setAngleCommand;
        public RelayCommand SetLengthCommand
        {
            get => _setLengthCommand ?? (_setLengthCommand = new RelayCommand(() => Parallelepiped.Length = Length));
        }
      
        public RelayCommand SetWidthCommand
        {
            get => _setWidthCommand ?? (_setWidthCommand = new RelayCommand(() => Parallelepiped.Width = Width));
        }
     
        public RelayCommand SetHeightCommand
        {
            get => _setHeightCommand ?? (_setHeightCommand = new RelayCommand(() => Parallelepiped.Height = Height));
        }
       
        public RelayCommand SetAngleCommand
        {
            get => _setAngleCommand ?? (_setAngleCommand = new RelayCommand(() => 
            Parallelepiped.Angle = -Transformation3DService.AngleFromDegreeToRadian(Angle)));
        }

        #endregion

    }
}
