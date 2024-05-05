using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations
{
    public class TransformationStore : ObservableObject, IEnumerable<TransformationBase>
    {
        private LinkedList<TransformationBase> _transformations = new LinkedList<TransformationBase>();

        public TransformationStore()
        {
            _transformations.AddLast(new VoidTransformation());
        }
        public int Count { get { return _transformations.Count; } }

        public Transformation ResultTransformation
        {
            get => point =>
            {
                var tmp = point;
                foreach (var tr in _transformations.OrderBy(tr => tr.ApplyPriority))
                {
                    tmp = tr.Transform(tmp);
                }
                return tmp;
            };
        }
        private void TransformationStoreChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ResultTransformation));
            OnPropertyChanged(nameof(Count));
        }

        public void AddTransformation(TransformationBase t)
        {
            if(t != null && !_transformations.Contains(t))
            {
                _transformations.AddLast(t);
                t.PropertyChanged += TransformationStoreChanged;
                TransformationStoreChanged(this, null);
            }
        }
        public void AddTransformation(TransformationStore trs)
        {
            foreach (var tr in trs)
            {
                AddTransformation(tr);
            }
        }

        public void RemoveTransformation(TransformationBase t)
        {
            if (_transformations.Contains(t))
            {
                _transformations.Remove(t);
                t.PropertyChanged -= TransformationStoreChanged;
                TransformationStoreChanged(this, null);
            }
        }

        public void RemoveTransformation(TransformationStore trs)
        {
            foreach(var tr in trs)
            {
                RemoveTransformation(tr);
            }
        }

        public IEnumerator<TransformationBase> GetEnumerator()
        {
            foreach (var transformation in _transformations)
            {
                yield return transformation;
            }
        }
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }
}
