using GrafikaKomputerowa2.Algebra;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace GrafikaKomputerowa2
{
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas = new();
        private readonly Scene scene = new();

        private readonly RenderContext renderContext = new();

        private readonly Stopwatch stopwatch = new();

        public MainWindow()
        {
            InitializeComponent();

            CompositionTarget.Rendering += Render;
            stopwatch.Start();
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
            stopwatch.Stop();

            if (AnimateLightCheckox.IsChecked == true)
                scene.AnimateLight((float)stopwatch.Elapsed.TotalSeconds);

            renderContext.Color = new Vec3(1, 0, 0);
            renderContext.Kd = (float)(KdSlider.Value / 100);
            renderContext.Ks = (float)(KsSlider.Value / 100);
            renderContext.M = (float)MSlider.Value;
            renderContext.K = 0;
            canvas.Render(scene, renderContext);

            stopwatch.Start();
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