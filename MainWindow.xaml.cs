using GrafikaKomputerowa2.Algebra;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using GrafikaKomputerowa2.Graphics;
using System.IO;

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

            renderContext.Texture = new Texture(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "bricks.jpg"));
        }

        private void PrepareScene()
        {
            var radius = (float)Math.Min(CanvasContainer.ActualWidth, CanvasContainer.ActualHeight) / 2 - 5;
            var precisionFactor = (float)TriangulationPrecisionSlider.Value / 100.0f;
            scene.PrepareScene(radius, precisionFactor, 100);
        }

        // Event handlers:

        private void Render(object? sender, EventArgs e)
        {
            stopwatch.Stop();

            if (AnimateLightCheckox.IsChecked == true)
                scene.AnimateLight((float)stopwatch.Elapsed.TotalSeconds);

            renderContext.Color = new Vec3(
                ColorPicker.SelectedColor!.Value.R / 255.0f,
                ColorPicker.SelectedColor!.Value.G / 255.0f,
                ColorPicker.SelectedColor!.Value.B / 255.0f);
            renderContext.Wireframe = WireframeCheckbox.IsChecked == true;
            renderContext.Kd = (float)(KdSlider.Value / 100);
            renderContext.Ks = (float)(KsSlider.Value / 100);
            renderContext.M = (float)MSlider.Value;
            renderContext.K = 0;

            if (TexturePickerRadio.IsChecked != true) renderContext.Texture = null;

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

        private void LightZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scene.MoveLight((float)e.NewValue);

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Image |*.jpg;*.bmp;*.png",
                InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            };
            if (openFileDialog.ShowDialog() != true) return;

            renderContext.Texture = new Texture(openFileDialog.FileName);

            e.Handled = true;
        }
    }
}