using GeometricModeling.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;
using System.Drawing;
using GeometricModeling.Models.DrawingModels.Models2D;
using System.Windows.Media;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using GeometricModeling.Models.DrawingTransformations.Transformations2D;

namespace GeometricModeling.ViewModels
{
    public class DrawingEngineViewModel : ViewModelBase
    {
		private double _width = 800;
		public DrawingEngineService DrawingEngine { get; set; }
        public Transformation2DService TransformationService { get; set; }

        private WriteableBitmap _screenBitmap;
        public WriteableBitmap ScreenBitmap 
        { 
            get => _screenBitmap;
            set => SetProperty(ref _screenBitmap,value,nameof(ScreenBitmap));
        }
        public Stopwatch Timer { get; set; } = new Stopwatch();
        private TimeSpan PreviousTick { get; set; }
        public float ElapsedMillisecondsSinceLastTick
        {
            get
            {
                return (float)(Timer.Elapsed - PreviousTick).TotalMilliseconds;
            }
        }
        public DrawingEngineViewModel(DrawingEngineService drawingEngineService)
		{
			DrawingEngine = drawingEngineService;
            
            DrawingEngine.PropertyChanged += (e, a) =>
            {
                if (ScreenBitmap != null && (a.PropertyName == "Models" || a.PropertyName == "PixelsInCell"))
                {
                    Draw();    
                }
            };
            Timer.Start();
        }
		public double Width
		{
			get => _width;
			set => SetProperty(ref _width, value, nameof(Width));
		}

		private double _height = 600;

		public double Height
		{
			get => _height;
			set => SetProperty(ref _height, value, nameof(Height));
		}
        private void MouseWheel(MouseWheelEventArgs mouseArgs)
        {
            if (mouseArgs == null) return;
            var mousePos = mouseArgs.GetPosition((IInputElement)mouseArgs.Source);
            var mouse = new Vector2((float)mousePos.X, (float)mousePos.Y);
            DrawingEngine.Zoom(new Vector2(mouse.X, mouse.Y), mouseArgs.Delta > 0);
            Draw();
        }

        private void Draw()
        {
            if (ElapsedMillisecondsSinceLastTick > 30)
            {
                var wr = ScreenBitmap;
                var w = wr.PixelWidth;
                var h = wr.PixelHeight;
                var stride = wr.BackBufferStride;
                var pixelPtr = wr.BackBuffer;
                var bm2 = new Bitmap(w, h, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, pixelPtr);
                wr.Lock();
                using (var g = Graphics.FromImage(bm2))
                {
                    DrawingEngine.Draw(g);
                }
                wr.AddDirtyRect(new Int32Rect(0, 0, (int)Width, (int)Height));
                wr.Unlock();
                PreviousTick = Timer.Elapsed;
            }
        }
       
        private void PreviewMouseDown(MouseButtonEventArgs mouseArgs)
        {
            if (mouseArgs == null) return;
            if (mouseArgs.LeftButton == MouseButtonState.Pressed)
            {
             
            }
            if (mouseArgs.RightButton == MouseButtonState.Pressed)
            {
              
            }
        }
        private void MouseDown(MouseButtonEventArgs mouseArgs)
        {
            if (mouseArgs == null) return;
            if (mouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = mouseArgs.GetPosition((IInputElement)mouseArgs.Source);
                var mouse = new Vector2((float)mousePos.X, (float)mousePos.Y);
                DrawingEngine.WorldStartPan = mouse;
            }

        }
        private void MouseMove(MouseEventArgs mouseArgs)
        {
            if (mouseArgs == null) return;
            if (mouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = mouseArgs.GetPosition((IInputElement)mouseArgs.Source);
                var mouse = new Vector2((float)mousePos.X, (float)mousePos.Y);
                float x = DrawingEngine.WorldOffset.X
                        - ((mouse.X - DrawingEngine.WorldStartPan.X) / DrawingEngine.WorldScale);
                float y = DrawingEngine.WorldOffset.Y
                    - ((mouse.Y - DrawingEngine.WorldStartPan.Y) / DrawingEngine.WorldScale);
                DrawingEngine.WorldOffset = new Vector2(x, y);
                DrawingEngine.WorldStartPan = mouse;
                Draw();
            }
        }

        private void UpdateSceneSize(SizeChangedEventArgs args)
        {
            if (args is null) return;
            ChangeSceneSize((int)args.NewSize.Width, (int)args.NewSize.Height);
        }
        public void ChangeSceneSize(int width, int height)
        {
            Height = height;
            Width = width;
            ScreenBitmap = BitmapFactory.New(width, height);
            Draw();
        }

        private void Loaded(RoutedEventArgs args)
        {
            if (args == null) return;
            var container = args.Source as Grid;
            if (container == null) return;
            var width = (int)container.ActualWidth;
            var height = (int)container.ActualHeight;
            ChangeSceneSize(width, height);
        }
        private void LoadedCanvas(RoutedEventArgs args)
        {
            if (args == null) return;
            DrawingEngine.Canvas = args.Source as Canvas;
            
        }
        private RelayCommand<MouseWheelEventArgs> _DEMouseWheel;
        public RelayCommand<MouseWheelEventArgs> DEMouseWheel
        {
            get
            {
                return _DEMouseWheel ??
                    (_DEMouseWheel = new RelayCommand<MouseWheelEventArgs>(MouseWheel));
            }
        }

        private RelayCommand<MouseButtonEventArgs> _DEPreviewMouseDown;
        public RelayCommand<MouseButtonEventArgs> DEPreviewMouseDown
        {
            get
            {
                return _DEPreviewMouseDown ??
                    (_DEPreviewMouseDown = new RelayCommand<MouseButtonEventArgs>(PreviewMouseDown));
            }
        }

        private RelayCommand<MouseButtonEventArgs> _DEMouseDown;
        public RelayCommand<MouseButtonEventArgs> DEMouseDown
        {
            get
            {
                return _DEMouseDown ??
                    (_DEMouseDown = new RelayCommand<MouseButtonEventArgs>(MouseDown));
            }
        }

        private RelayCommand<MouseEventArgs> _DEMouseMove;
        public RelayCommand<MouseEventArgs> DEMouseMove
        {
            get
            {
                return _DEMouseMove ??
                    (_DEMouseMove = new RelayCommand<MouseEventArgs>(MouseMove));
            }
        }

        private RelayCommand<SizeChangedEventArgs> _DEsizeChanged;
        public RelayCommand<SizeChangedEventArgs> DESizeChanged
        {
            get
            {
                return _DEsizeChanged ??
                    (_DEsizeChanged = new RelayCommand<SizeChangedEventArgs>(UpdateSceneSize));
            }
        }

        private RelayCommand<RoutedEventArgs> _DELoaded;
        public RelayCommand<RoutedEventArgs> DELoaded
        {
            get
            {
                return _DELoaded ??
                    (_DELoaded = new RelayCommand<RoutedEventArgs>(Loaded));
            }
        }
        private RelayCommand<RoutedEventArgs> _canvasLoaded;
        public RelayCommand<RoutedEventArgs> CanvasLoaded
        {
            get
            {
                return _canvasLoaded ??
                    (_canvasLoaded = new RelayCommand<RoutedEventArgs>(LoadedCanvas));
            }
        }

        private void DragOver(DragEventArgs e)
        {
            var dropPosition = e.GetPosition((Canvas)e.Source);

            if (e.Data.GetDataPresent(typeof(System.Windows.Shapes.Ellipse)))
            {
                var point = (System.Windows.Shapes.Ellipse)e.Data.GetData(typeof(System.Windows.Shapes.Ellipse));
                {
                    Canvas.SetLeft(point, dropPosition.X - point.Width / 2);
                    Canvas.SetTop(point, dropPosition.Y - point.Height / 2);
                }
            }
        }

        private RelayCommand<DragEventArgs> _DEDragOver;

        public RelayCommand<DragEventArgs> DEDragOver
        {
            get
            {
                return _DEDragOver ??
                    (_DEDragOver = new RelayCommand<DragEventArgs>(DragOver));
            }
        }

    }
}
