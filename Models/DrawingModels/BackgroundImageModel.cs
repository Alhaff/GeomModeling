using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GeometricModeling.Models.DrawingModels
{
    public class BackgroundImageModel : ModelBase
    {
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath == value) return;
                Image = null;
                Position = null;
                SetProperty(ref _imagePath, value, nameof(ImagePath));
            }
        }
        private Vector3? Position { get; set; } = null;
        public Image? Image { get; set; }
        public DrawingEngineService DrawingEngine { get; set; }

        public BackgroundImageModel(DrawingEngineService drawingEngineService)
        {
            DrawingEngine = drawingEngineService;
            DrawPriority = -2;
            IsAffectedByExternalTransformation = false;
        }
        private bool IsAddedToDrawingEngine { get; set; } = false;
        public void AddToDrawingEngine()
        {
            if (!IsAddedToDrawingEngine)
            {
                DrawingEngine.DrawNonModel += DrawImage;
                IsAddedToDrawingEngine = true;
            }
        }

        public void RemoveFromDrawingEngine()
        {
            if (IsAddedToDrawingEngine)
            {
                DrawingEngine.DrawNonModel -= DrawImage;
                IsAddedToDrawingEngine = false;
            }
        }

        public BackgroundImageModel(DrawingEngineService drawingEngineService, string filePath)
            :this(drawingEngineService)
        {
            ImagePath = filePath;
        }
        public void DrawImage(Graphics graphics)
        {
            var image =  Image?? (Image =  System.Drawing.Image.FromFile(ImagePath));
            var pos = Position ?? (Position = new Vector3(-DrawingEngine.WorldOffset.X, -DrawingEngine.WorldOffset.Y, 1));
            var sceneCoord = DrawingEngine.ChooseScreenCoordTransformation(IsAffectedByExternalTransformation)(pos??Vector3.Zero);
            using (var wrapMode = new ImageAttributes())
            {
                var destRect = new Rectangle((int)sceneCoord.X,
                (int)sceneCoord.Y,
                (int)(image.Width /20f * DrawingEngine.WorldScale),
                (int)(image.Height /20f * DrawingEngine.WorldScale));
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }
        protected override IEnumerable<IEnumerable<Vector3>> ModelContourPoints()
        {
            return new[] { new[] { Vector3.Zero, Vector3.Zero } };
        }
    }
}
