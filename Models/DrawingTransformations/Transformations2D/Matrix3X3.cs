using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class Matrix3X3
    {
        #region Variables
        public readonly float[,] matrix = new float[3, 3];
        #endregion

        #region Constructors
        public Matrix3X3(float[,] m)
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    matrix[i, j] = m[i, j];
                }
            }
        }
        public Matrix3X3(params float[] values)
        {
            if (values.Length >= 9)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        matrix[i, j] = values[3 * i + j];
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
        public static Vector3 operator *(Vector3 vect, Matrix3X3 Matrix)
        {
            var extVect = new List<float>() { vect.X, vect.Y, vect.Z };
            var res = new List<float>() { 0, 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    res[i] += extVect[j] * Matrix[j, i];
                }
            }
            res = res.Select(p => (float)(p / res[2])).ToList();
            return new Vector3(res[0], res[1], res[2]);
        }

        public static Matrix3X3 operator *(float num, Matrix3X3 Matrix)
        {
            return new Matrix3X3
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2]

                );
        }
        public static Matrix3X3 operator *(Matrix3X3 Matrix, float num)
        {
            return new Matrix3X3
                (
                     num * Matrix[0, 0], num * Matrix[0, 1], num * Matrix[0, 2],
                     num * Matrix[1, 0], num * Matrix[1, 1], num * Matrix[1, 2],
                     num * Matrix[2, 0], num * Matrix[2, 1], num * Matrix[2, 2]

                );
        }
        #endregion
    }
}
