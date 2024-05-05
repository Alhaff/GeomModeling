using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Models.DrawingModels
{
    public class TextModel : ModelBase
    {
        #region Variables
        private string _message;
        private Font _font = new Font("Times New Roman", 12);
        private Vector3 _position;
        #endregion

        #region Propreties
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value, nameof(Message));
        }

        public Font MessageFont
        {
            get => _font;
            set => SetProperty(ref _font, value, nameof(MessageFont));
        }

        public Vector3 Position
        {
            get => _position;
            set => SetProperty(ref _position, value, nameof(Position));
        }
        #endregion

        public DrawingEngineService DrawingEngine { get; set; }
        public TextModel(DrawingEngineService drawingEngineService, Color color)
        {
            DrawingEngine = drawingEngineService;
            LineColor = color;
            DrawingEngine.DrawNonModel += this.DrawText;
        }

        public TextModel(DrawingEngineService drawingEngine, string message, Vector3 pos,Color color)
            : this(drawingEngine,color)
        {
            Message = message;
            Position = pos;
        }

      

       
        public void DrawText(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(LineColor);
            var p = DrawingEngine.ChooseScreenCoordTransformation(IsAffectedByExternalTransformation)(Transformations.ResultTransformation(Position));
            graphics.DrawString(Message, MessageFont, brush, p.X, p.Y);
        }

        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            return new[] { new[] { Position, Position } };
        }
    }
}
