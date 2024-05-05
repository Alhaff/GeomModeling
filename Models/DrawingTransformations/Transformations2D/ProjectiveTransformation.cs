using GeometricModeling.Models.DrawingModels;
using GeometricModeling.Models.DrawingTransformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingTransformations.Transformations2D
{
    public class ProjectiveTransformation : TransformationBase
    {

        public ProjectiveTransformation(DragAblePoint R0,DragAblePoint Rx, DragAblePoint Ry)
        {
            ApplyPriority = 4;
            R0Point = R0;
            RXPoint = Rx;
            RYPoint = Ry;
        }
        #region Variables
        private DragAblePoint _r0Point;
        private DragAblePoint _rxPoint;
        private DragAblePoint _ryPoint;
        #endregion

        #region Propreties
        public DragAblePoint R0Point 
        {
            get => _r0Point;
            set => SetProperty(ref _r0Point, value, nameof(R0Point)); 
        }
        public DragAblePoint RXPoint 
        {
            get => _rxPoint;
            set => SetProperty(ref _rxPoint, value, nameof(RXPoint)); 
        }
        public DragAblePoint RYPoint 
        {
            get => _ryPoint;
            set => SetProperty(ref _ryPoint, value, nameof(RYPoint));
        }
        private Matrix3X3 ProjectiveMatrix
        {
            get => new Matrix3X3
                (
                RXPoint.Center.X * RXPoint.Center.Z, RXPoint.Center.Y * RXPoint.Center.Z, RXPoint.Center.Z,
                RYPoint.Center.X * RXPoint.Center.Z, RYPoint.Center.Y * RYPoint.Center.Z, RYPoint.Center.Z,
                R0Point.Center.X * R0Point.Center.Z, R0Point.Center.Y * R0Point.Center.Z, R0Point.Center.Z
                );
        }
        #endregion
        public override Transformation Transform => vector => vector * ProjectiveMatrix;
    }
}
