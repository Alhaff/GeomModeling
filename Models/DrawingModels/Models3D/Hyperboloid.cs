using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels.Models3D
{
    public class Hyperboloid : ModelBase
    {
        #region Variables
        private Vector3 _center = Vector3.Zero;
        private float _a = 10;
        private float _b = 10;
        private float _c = 10;
        private float _height = 15;
        private int _uStepCount = 30;
        private int _vStepCount = 20;
        #endregion

        #region Propreties
        public Vector3 Center
        {
            get => _center;
            set => SetProperty(ref _center, value, nameof(Center));
        }
        public float A
        {
            get => _a;
            set => SetProperty(ref _a, value, nameof(A));
        }
        public float B
        {
            get => _b;
            set => SetProperty(ref _b, value, nameof(B));
        }
        public float C
        {
            get => _c;
            set => SetProperty(ref _c, value, nameof(C));
        }
       
        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value, nameof(Height));
        }
        public float SetAWithoutPropertyChanged
        {
            set => _a = value;
        }

        public float SetBWithoutPropertyChanged
        {
            set => _b = value;
        }
        public int UStepCount
        {
            get => _uStepCount;
            set => SetProperty(ref _uStepCount, value, nameof(UStepCount));
        }
        public int VStepCount
        {
            get => _vStepCount;
            set => SetProperty(ref _vStepCount, value, nameof(VStepCount));
        }

        public double UStep { get => 2 * Math.PI / UStepCount; }
        public float HeightStep { get => 2*Height/VStepCount; }
       
        #endregion
       
        public Vector3 GetHyperboloidPoint(double u,double v)
        {
            var x = A * (float)(Math.Cosh(v) * Math.Cos(u));
            var y = B * (float)(Math.Cosh(v) * Math.Sin(u));
            var z = C * (float)Math.Sinh(v);
            return Center + new Vector3(x, y, z);
        }

        internal IEnumerable<Vector3> GetHyperboloidFragment(double v)
        {
            double uStep = UStep;
            double u = 0;
            for(int i =0; i < UStepCount;i++)
            {
                yield return GetHyperboloidPoint(u, v);
                u += uStep;
            }
            yield return GetHyperboloidPoint(2*Math.PI, v);
        }

        Queue<IEnumerable<Vector3>> fragments = new Queue<IEnumerable<Vector3>>();
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            IEnumerable<IEnumerable<Vector3>> ConnectFragment(List<Vector3> fr1, List<Vector3> fr2)
            {
                if (fr1.Count == fr2.Count)
                {
                    for (int i = 0; i < fr1.Count; i++)
                    {
                        yield return new List<Vector3>() { fr1[i], fr2[i] };
                    }
                }
            };
            var step = HeightStep;
            for (float i = -Height; i <= Height; i+=step)
            {
                var v = Math.Asinh(i / C);
                fragments.Enqueue(GetHyperboloidFragment(v));
                
                if (fragments.Count == 2)
                {
                    var fr = fragments.Dequeue();
                    yield return fr;

                    var res = ConnectFragment(fr.ToList(), fragments.Peek().ToList());
                    foreach (var line in res) yield return line;
                    yield return fragments.Peek();
                }
            }
            fragments.Clear();
        }
    }
}
