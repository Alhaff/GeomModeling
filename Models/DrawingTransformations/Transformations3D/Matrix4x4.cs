using GeometricModeling.Models.DrawingTransformations.Transformations2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations3D
{
    public class Matrix4X4
    {
        #region Variables
        public readonly float[,] matrix = new float[4, 4];
        #endregion

        #region Constructors
        public Matrix4X4(float[,] m)
        {
            for (var i = 0; i <4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix[i, j] = m[i, j];
                }
            }
        }
        public Matrix4X4(params float[] values)
        {
            if (values.Length >= 16)
            {
                for (var i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        matrix[i, j] = values[4 * i + j];
                    }
                }
            }
        }
        #endregion

        #region Operators
        public float this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < matrix.GetLength(0))
                {
                    if (j >= 0 && j < matrix.GetLength(1))
                    {
                        return matrix[i, j];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
        public static Vector3 operator *(Vector3 vect, Matrix4X4 Matrix)
        {
            var extVect = new List<float>() { vect.X, vect.Y, vect.Z, 1};
            var res = new List<float>() { 0, 0, 0 ,0};
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    res[i] += extVect[j] * Matrix[j, i];
                }
            }
            res = res.Select(p => (float)(p / res[3])).ToList();
            return new Vector3(res[0], res[1], res[2]);
        }

        public static Matrix4X4 operator *(float num, Matrix4X4 Matrix)
        {
            return new Matrix4X4
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2], num * Matrix[0, 3],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2], num * Matrix[1, 3],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2], num * Matrix[2, 3],
                     num * Matrix[3, 0], num * Matrix[3, 1], num * Matrix[3, 2], num * Matrix[3, 3]

                );
        }
        public static Matrix4X4 operator *(Matrix4X4 Matrix, float num)
        {
            return new Matrix4X4
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2], num * Matrix[0, 3],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2], num * Matrix[1, 3],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2], num * Matrix[2, 3],
                     num * Matrix[3, 0], num * Matrix[3, 1], num * Matrix[3, 2], num * Matrix[3, 3]

                );
        }
        #endregion
    }
}
