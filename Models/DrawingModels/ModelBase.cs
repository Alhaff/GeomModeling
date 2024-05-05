using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformation;
using GeometricModeling.Models.DrawingTransformations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels
{
    public abstract class ModelBase : ObservableObject
    {
        #region Vaariables
        private int _drawPriority = 0;
        private float _lineThickness = 1;
        private Color _lineColor = Color.Black;
        private bool _isAffectedByExternalTransformations = true;
        private TransformationStore _transformations = new TransformationStore();
        #endregion

        public ModelBase()
        {
            _transformations.PropertyChanged += this.ModelBaseChanged;
        }

        #region Propreties
        public int DrawPriority
        {
            get => _drawPriority;
            set => SetProperty(ref _drawPriority, value, nameof(DrawPriority));
        }
        
        public float LineThickness
        {
            get => _lineThickness;
            set => SetProperty(ref _lineThickness, value, nameof(LineThickness));
        }
        public Color LineColor
        {
            get => _lineColor;
            set => SetProperty(ref _lineColor, value, nameof(LineColor));
        }
        public bool IsAffectedByExternalTransformation
        {
            get => _isAffectedByExternalTransformations;
            set => SetProperty(ref _isAffectedByExternalTransformations, value, nameof(IsAffectedByExternalTransformation));
        }
       
        #endregion
        protected abstract IEnumerable<IEnumerable<Vector3>> ModelContourPoints();
        public TransformationStore Transformations
        {
            get => _transformations;
            set
            {
                if (value != _transformations)
                {
                    _transformations.PropertyChanged -= this.ModelBaseChanged;
                    _transformations = value;
                    _transformations.PropertyChanged += this.ModelBaseChanged;
                    OnPropertyChanged(nameof(Transformations));
                }
            }
        }
        public IEnumerable<IEnumerable<Vector3>> GetModelContourPoints()
        {
            foreach(var modelContour in ModelContourPoints())
                yield return modelContour.Select(point => Transformations.ResultTransformation(point));
        }
        private void ModelBaseChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }
    }
}
