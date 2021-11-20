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

        // Event handlers:

        protected void Render(object? sender, EventArgs e)
        {
            canvas.Render(scene.Triangles);
        }

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;

            canvas.Resize(size.Width, size.Height);
            CanvasImage.Source = canvas.WriteableBitmap;

            var radius = (float)Math.Min(size.Width, size.Height) / 2 - 5;
            scene.PrepareScene(radius, 0.35f);

            e.Handled = true;
        }
    }
}