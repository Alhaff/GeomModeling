using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformation
{
    public delegate Vector3 Transformation(Vector3 point);
    public abstract class TransformationBase : ObservableObject
    {
        private int _applyPriority = 0;

        public int ApplyPriority
        {
            get => _applyPriority;
            set => SetProperty(ref _applyPriority, value, nameof(ApplyPriority));
        }

        public abstract Transformation Transform { get; }

        public static implicit operator Transformation(TransformationBase tr) => tr.Transform;
    }
}
