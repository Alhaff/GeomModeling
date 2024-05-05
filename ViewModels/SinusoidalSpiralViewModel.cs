using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GeometricModeling.ViewModels
{
    public class SinusoidalSpiralViewModel : AxisViewModel
    {
        #region Variables
        private SinusoidalSpiral _sinusoidalSpiral = new SinusoidalSpiral();
        private float _a;
        private float _n;
        private double _tangentAngle;
        private RelayCommand _setA;
        private RelayCommand _setN;
        private float _aStart = 1;
        private float _aEnd = 10;
        private float _nStart = 1;
        private float _nEnd = 10;
        private double _angleStart = 0;
        private double _angleEnd = 360;
        #endregion

        #region Propreties
        public float A
        {
            get => _a;
            set => SetProperty(ref _a, value, nameof(A));
        }
        public float N
        {
            get => _n;
            set => SetProperty(ref _n, value, nameof(N));
        }
        public double TangentAngle
        {
            get => _tangentAngle;
            set
            {
                SinusoidalSpiral.TangentTeta = Transformation2DService.AngleFromDegreeToRadian(value);
                SetProperty(ref _tangentAngle, value, nameof(TangentAngle));
            }
        }
        public float AStart
        {
            get =>_aStart;
            set => SetProperty(ref _aStart,value,nameof(AStart));
        }
        public float AEnd
        {
            get => _aEnd;
            set => SetProperty(ref _aEnd,value,nameof(AEnd));
        }

        public float NStart
        {
            get =>_nStart; 
            set => SetProperty(ref _nStart,value, nameof(NStart));
        }

        public float NEnd
        {
            get => _nEnd;
            set => SetProperty(ref _nEnd,value,nameof(NEnd));
        }

        public double AngleStart
        {
            get => _angleStart;
            set => SetProperty(ref _angleStart, value, nameof(AStart));
        }
        public double AngleEnd
        {
            get => _angleEnd;
            set => SetProperty(ref _angleEnd, value, nameof(AEnd));
        }

        #endregion

        public SinusoidalSpiral SinusoidalSpiral
        {
            get => _sinusoidalSpiral;
            set => SetProperty(ref _sinusoidalSpiral, value, nameof(SinusoidalSpiral));
        }

        public SinusoidalSpiralViewModel(DrawingEngineService drawingEngine, Transformation2DService transformation2DService)
            :base(drawingEngine,transformation2DService)
        {
            SinusoidalSpiral.Center = new System.Numerics.Vector3(10, 10, 1);
            SinusoidalSpiral.DrawPriority = 1;
            A = SinusoidalSpiral.A = 4;
            N = SinusoidalSpiral.N = 2;
            SinusoidalSpiral.LineThickness = 3;
            SinusoidalSpiral.LineColor = Color.Purple;
            SinusoidalSpiral.Transformations.AddTransformation(TransformationService.Offset);
            SinusoidalSpiral.Transformations.AddTransformation(TransformationService.Rotate);
            DrawingEngine.Models.AddModel(SinusoidalSpiral);
        }

        private void SetA()
        {
            SinusoidalSpiral.A = A;
            if (SinusoidalSpiral.A != A) A = SinusoidalSpiral.A;
        }
        private void SetN()
        {
            SinusoidalSpiral.N = N;
            if (SinusoidalSpiral.N != N) N = SinusoidalSpiral.N;
        }

        private async Task RunAChangeAnimation()
        {
            Func<float, bool> comp = num => AStart < AEnd ? num <= AEnd : num >= AEnd;
            var step = (AEnd - AStart) / (5*(float)GetAnimationStepAmount(AStart, AEnd));
            for (float start = AStart; comp(start); start += step)
            {
                SinusoidalSpiral.A = start;
                A = SinusoidalSpiral.A;
                await Task.Delay(15);
            }
            if (SinusoidalSpiral.A != AEnd)
            {
                SinusoidalSpiral.A = AEnd;
                A = SinusoidalSpiral.A;
            }
        }
        private double GetAnimationStepAmount(double start, double end)
        {
            start = Math.Abs(start);
            end = Math.Abs(end);
            var (min, max) = start < end ? (start, end) : (end, start);
            int diff = min == 0 ? (int)Math.Round(max) : (int)Math.Round(max / min);
            int power = 0;
            while(diff > 0)
            {
                power++;
                diff /= 10;
            }
            return Math.Pow(7, power);
        }
        private async Task RunNChangeAnimation()
        {
            Func<float, bool> comp = num => NStart < NEnd ? num <= NEnd : num >= NEnd;
            var step = (NEnd - NStart) / 200f;
            for (float start = NStart; comp(start); start += step)
            {
                SinusoidalSpiral.N = start;
                N = SinusoidalSpiral.N;
                await Task.Delay(15);
            }
            if(SinusoidalSpiral.N != NEnd)
            {
                await Task.Delay(15);
                SinusoidalSpiral.N = NEnd;
                N = SinusoidalSpiral.N;
                
            }
        }
        private async Task RunRotationAngleChangeAnimation()
        {
            Func<double, bool> comp = num => AngleStart < AngleEnd ? num <= AngleEnd : num >= AngleEnd;
            var step = (AngleEnd - AngleStart) / GetAnimationStepAmount(AngleStart,AngleEnd);
            for (var start = AngleStart; comp(start); start += step)
            {
                TransformationService.Rotate.Angle = Transformation2DService.AngleFromDegreeToRadian(start);
                await Task.Delay(15);
            }
                await Task.Delay(15);
                TransformationService.Rotate.Angle = Transformation2DService.AngleFromDegreeToRadian(AngleEnd);
        }

        private AsyncRelayCommand _AAnimationCommand;
        public AsyncRelayCommand AAnimationCommand
        {
            get => _AAnimationCommand ?? (_AAnimationCommand = new AsyncRelayCommand(RunAChangeAnimation));
        }
        private AsyncRelayCommand _NAnimationCommand;
        public AsyncRelayCommand NAnimationCommand
        {
            get => _NAnimationCommand ?? (_NAnimationCommand = new AsyncRelayCommand(RunNChangeAnimation));
        }
        private AsyncRelayCommand _angleAnimationCommand;
        public AsyncRelayCommand AngleAnimationCommand
        {
            get => _angleAnimationCommand ?? (_angleAnimationCommand = new AsyncRelayCommand(RunRotationAngleChangeAnimation));
        }

        public RelayCommand SetACommand
        {
            get => _setA ?? (_setA = new RelayCommand(SetA));
        }
        public RelayCommand SetNCommand
        {
            get => _setN ?? (_setN = new RelayCommand(SetN));
        }
    }
}
