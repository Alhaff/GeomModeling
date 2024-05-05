using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class ProjectionSetingsViewModel : ViewModelBase
    {
        DrawingEngineService DrawingEngine { get; set; }

        Transformation3DService Transformation3D { get; set; }

        public ProjectionSetingsViewModel(DrawingEngineService drawingEngine, Transformation3DService transformation3D)
        {
            DrawingEngine = drawingEngine;
            Transformation3D = transformation3D;
            DrawingEngine.Transformations.AddTransformation(Transformation3D.IsometricProjection);
            DrawingEngine.Transformations.AddTransformation(Transformation3D.Swap);
            P = Transformation3D.IsometricProjection.P;
            CosA = Transformation3D.IsometricProjection.CosA;
            CosG = Transformation3D.IsometricProjection.CosG;
            SinA = Transformation3D.IsometricProjection.SinA;
            SinG = Transformation3D.IsometricProjection.SinG;
            AngleA = Transformation3DService.AngleFromRadianToDegree(Math.Asin(SinA));
            AngleG = Transformation3DService.AngleFromRadianToDegree(Math.Asin(SinG));
        }
        #region Variables
        private float _p;
        private float _cosA;
        private float _sinA;
        private float _cosG;
        private float _sinG;
        private double _angleA;
        private double _angleG;
       
        #endregion

        #region Propreties
        public float P
        {
            get => _p;
            set => SetProperty(ref _p, value, nameof(P));
        }
        public float CosA
        {
            get => _cosA;
            set => SetProperty(ref _cosA, value, nameof(CosA));
        }
        public float SinA
        {
            get => _sinA;
            set => SetProperty(ref _sinA, value, nameof(SinA));
        }
        public float CosG
        {
            get => _cosG;
            set => SetProperty(ref _cosG, value, nameof(CosG));
        }
        public float SinG
        {
            get => _sinG;
            set => SetProperty(ref _sinG, value, nameof(SinG));
        }
        public double AngleA
        {
            get => _angleA;
            set => SetProperty(ref _angleA, value, nameof(AngleA));
        }
        public double AngleG
        {
            get => _angleG;
            set => SetProperty(ref _angleG, value, nameof(AngleG));
        }
        #endregion
        #region Commands
        private RelayCommand _setPCommand;

        public RelayCommand SetPCommand
        {
            get => _setPCommand ?? (_setPCommand = new RelayCommand(() => Transformation3D.IsometricProjection.P = P));
        }

        private RelayCommand _setCosACommand;

        public RelayCommand SetCosACommand
        {
            get => _setCosACommand ?? (_setCosACommand = new RelayCommand(
                () =>
                {
                    Transformation3D.IsometricProjection.CosASeterWithoutPropertyChanged = CosA;
                    var angle = Math.Acos(CosA);
                    AngleA = Transformation3DService.AngleFromRadianToDegree(angle);
                    Transformation3D.IsometricProjection.SinA = SinA = (float)Math.Sin(angle);
                }));
        }

        private RelayCommand _setSinACommand;

        public RelayCommand SetSinACommand
        {
            get => _setSinACommand ?? (_setSinACommand = new RelayCommand(
                () =>
                {
                    Transformation3D.IsometricProjection.SinASeterWithoutPropertyChanged = SinA;
                    var angle = Math.Asin(SinA);
                    AngleA = Transformation3DService.AngleFromRadianToDegree(angle);
                    Transformation3D.IsometricProjection.CosA = CosA = (float)Math.Cos(angle);
                }));
        }
        private RelayCommand _setCosGCommand;

        public RelayCommand SetCosGCommand
        {
            get => _setCosGCommand ?? (_setCosGCommand = new RelayCommand(
               () =>
               {
                   Transformation3D.IsometricProjection.CosGSeterWithoutPropertyChanged = CosG;
                   var angle = Math.Acos(CosG);
                   AngleG = Transformation3DService.AngleFromRadianToDegree(angle);
                   Transformation3D.IsometricProjection.SinG = SinG = (float)Math.Sin(angle);
               }));
        }

        private RelayCommand _setSinGCommand;

        public RelayCommand SetSinGCommand
        {
            get => _setSinGCommand ?? (_setSinGCommand = new RelayCommand(
                () =>
                {
                    Transformation3D.IsometricProjection.SinGSeterWithoutPropertyChanged = SinG;
                    var angle = Math.Asin(SinG);
                    AngleG = Transformation3DService.AngleFromRadianToDegree(angle);
                    Transformation3D.IsometricProjection.CosG = CosG = (float)Math.Cos(angle);
                }));
        }
        #region AngleCommands
        private RelayCommand _setAngleACommand;
        public RelayCommand SetAngleACommand
        {
            get => _setAngleACommand ?? (_setAngleACommand = new RelayCommand(
                () => {
                    Transformation3D.IsometricProjection.SinASeterWithoutPropertyChanged = SinA = (float)Math.Sin(Transformation3DService.AngleFromDegreeToRadian(AngleA));
                    Transformation3D.IsometricProjection.CosA = CosA = (float)Math.Cos(Transformation3DService.AngleFromDegreeToRadian(AngleA));
                }));
        }

        private RelayCommand _setAngleGCommand;
        public RelayCommand SetAngleGCommand
        {
            get => _setAngleGCommand ?? (_setAngleGCommand = new RelayCommand(
                 () => {
                     Transformation3D.IsometricProjection.SinGSeterWithoutPropertyChanged = SinG = (float)Math.Sin(Transformation3DService.AngleFromDegreeToRadian(AngleG));
                     Transformation3D.IsometricProjection.CosG = CosG = (float)Math.Cos(Transformation3DService.AngleFromDegreeToRadian(AngleG));
                 }));
        }
        #endregion
        private RelayCommand _returnToDefaulCommand;
        public RelayCommand ReturnToDefaultCommand
        {
            get => _returnToDefaulCommand ?? (_returnToDefaulCommand = new RelayCommand(
                () =>
                {
                    double Sqrt2 = Math.Sqrt(2);
                    Transformation3D.IsometricProjection.CosASeterWithoutPropertyChanged = _cosA = (float)Math.Sqrt(Sqrt2 / 2d);
                    Transformation3D.IsometricProjection.SinASeterWithoutPropertyChanged = _sinA = (float)Math.Sqrt(1d - Sqrt2 / 2d);
                    Transformation3D.IsometricProjection.CosGSeterWithoutPropertyChanged = _cosG = (float)Math.Sqrt(2d - Sqrt2);
                    Transformation3D.IsometricProjection.CosGSeterWithoutPropertyChanged = _sinG = (float)Math.Sqrt(Sqrt2 - 1d);
                    Transformation3D.IsometricProjection.P = _p = 0;
                    _angleA = Transformation3DService.AngleFromRadianToDegree(Math.Asin(SinA));
                    _angleG =Transformation3DService.AngleFromRadianToDegree(Math.Asin(SinG));
                    OnPropertyChanged("CosA");
                    OnPropertyChanged("CosG");
                    OnPropertyChanged("SinA");
                    OnPropertyChanged("SinG");
                    OnPropertyChanged("AngleA");
                    OnPropertyChanged("AngleG");
                    OnPropertyChanged("P");
                }
           ));
        }
        #endregion
    }
}
