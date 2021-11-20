using System;
using System.Windows;
using System.Windows.Media;

namespace GrafikaKomputerowa2
{
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas = new();
        private readonly Scene scene = new();

        public MainWindow()
        {
            InitializeComponent();

            CompositionTarget.Rendering += Render;
        }

        private void PrepareScene()
        {
            var radius = (float)Math.Min(CanvasContainer.ActualWidth, CanvasContainer.ActualHeight) / 2 - 5;
            var precisionFactor = (float)TriangulationPrecisionSlider.Value / 100.0f;
            scene.PrepareScene(radius, precisionFactor);
        }

        // Event handlers:

        private void Render(object? sender, EventArgs e)
        {
            canvas.Render(scene.Triangles);
        }

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;

            canvas.Resize(size.Width, size.Height);
            CanvasImage.Source = canvas.WriteableBitmap;

            PrepareScene();

            e.Handled = true;
        }

        private void TriangulationPrecisionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrepareScene();

            e.Handled = true;
        }
    }
}