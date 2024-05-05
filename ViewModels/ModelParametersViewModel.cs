using CommunityToolkit.Mvvm.Input;
using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Models.DrawingModels.Models2D;
using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.ViewModels
{
    public class ModelParametersViewModel : ViewModelBase
    {
        public DrawingEngineService DrawingEngine { get; set; }

        public Transformation2DService TransformationService { get; set; }
        private float EndXAxis { get; set; } = 50;
        private Lab1Model _lab1;
        public Lab1Model Lab1 
        {
            get => _lab1;
            set => SetProperty(ref _lab1, value,nameof(Lab1));
        }

        public Axises Axis { get; set; }

        public TextModel XText { get; set; }

        public TextModel YText { get; set; }
        public TextModel XStep { get; set; }
        public TextModel YStep { get; set; }
        public ModelParametersViewModel(DrawingEngineService drawingEngine, Transformation2DService transformationService)
        {
            DrawingEngine = drawingEngine;
            TransformationService = transformationService;
            Axis = new Axises(50, 50);
            Axis.DrawPriority = 0;
            Lab1 = new Lab1Model();
            Lab1.DrawPriority = 1;
            Lab1.LineThickness = 4;
            Lab1.Transformations.AddTransformation(TransformationService.Offset);
            Lab1.Transformations.AddTransformation(TransformationService.Rotate);
            XText = new TextModel(DrawingEngine, "X", new Vector3(Axis.EndXAxis, -0.3f, 1), Axis.XAxisColor);
            YText = new TextModel(DrawingEngine, "Y", new Vector3(-2, Axis.EndYAxis, 1), Axis.YAxisColor);
            XStep = new TextModel(DrawingEngine,"1", new Vector3(1,-0.3f,1), Axis.XAxisColor);
            YStep = new TextModel(DrawingEngine, "1", new Vector3(-2,1,1),Axis.YAxisColor);
            DrawingEngine.Models.AddModel(Axis);
            DrawingEngine.Models.AddModel(Lab1);
            InR = Lab1.InR;
            MidR = Lab1.MidR;
            OutR = Lab1.OutR;
            Len = Lab1.Len1;
        }

        private float _inR;

        public float InR
        {
            get =>_inR;
            set => SetProperty(ref _inR, value, nameof(InR));
        }

        private float _midR;

        public float MidR
        {
            get => _midR;
            set => SetProperty(ref _midR, value, nameof(MidR)); 
        }

        private float _outR;

        public float OutR
        {
            get => _outR;
            set => SetProperty(ref _outR, value,nameof(OutR));
        }

        private float _len;

        public float Len
        {
            get => _len;
            set => SetProperty(ref _len, value,nameof(Len));
        }

        private void SetInR()
        {
            Lab1.InR = InR;
            if(Lab1.InR != InR) InR = Lab1.InR;
        }
        private void SetMidR()
        {
            Lab1.MidR = MidR;
            if (Lab1.MidR != MidR) MidR = Lab1.MidR;
        }
        private void SetOutR()
        {
            Lab1.OutR = OutR;
            if (Lab1.OutR != OutR) OutR = Lab1.OutR;
        }
        private void SetLen()
        {
            Lab1.Len1 = Len;
            if (Lab1.Len1 != Len) Len = Lab1.Len1;
        }

        private RelayCommand _setInR;

        public RelayCommand SetInRCommand
        {
            get => _setInR ?? (_setInR = new RelayCommand(SetInR));
        }

        private RelayCommand _setLen;

        public RelayCommand SetLenCommand
        {
            get => _setLen ?? (_setLen = new RelayCommand(SetLen));
        }

        private RelayCommand _setMidR;

        public RelayCommand SetMidRCommand
        {
            get => _setMidR ?? (_setMidR = new RelayCommand(SetMidR));
        }

        private RelayCommand _setOutR;

        public RelayCommand SetOutRCommand
        {
            get => _setOutR ?? (_setOutR = new RelayCommand(() => Lab1.OutR = OutR));
        }



    }
}
