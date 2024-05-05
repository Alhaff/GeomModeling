using CommunityToolkit.Mvvm.ComponentModel;
using GeometricModeling.Models.DrawingTransformation;
using GeometricModeling.Models.DrawingTransformations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels
{
    public class ModelStore : ObservableObject, IEnumerable<ModelBase>
    {
       private List<ModelBase> _models = new List<ModelBase>();

        public ModelStore()
        {
            
        }
        public int Count { get { return _models.Count; } }

        private void ModelStoreStoreChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }

        public void AddModel(ModelBase model)
        {
            if (model != null)
            {
                _models.Add(model);
                model.PropertyChanged += ModelStoreStoreChanged;
                ModelStoreStoreChanged(this, null);
            }
        }
        public void AddModel(ModelStore models)
        {
            foreach (var model in models._models)
            {
                AddModel(model);
            }
        }

        public void RemoveModel(ModelBase model)
        {
            if (_models.Contains(model))
            {
                _models.Remove(model);
                model.PropertyChanged -= ModelStoreStoreChanged;
                ModelStoreStoreChanged(this, null);
            }
        }

        public void RemoveModel(ModelStore models)
        {
            foreach (var model in models)
            {
                RemoveModel(model);
            }
        }

        public IEnumerator<ModelBase> GetEnumerator()
        {
            foreach (var model in _models)
            {
                yield return model;
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
