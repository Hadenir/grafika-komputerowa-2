using GrafikaKomputerowa2.Algebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace GrafikaKomputerowa2
{
    public partial class MainWindow : Window
    {
        private readonly Canvas canvas = new();
        private readonly Scene scene = new();

        private readonly IEnumerable<Vec3> vs;

        public MainWindow()
        {
            InitializeComponent();

            vs = SphereTriangulator.CreateSemiSphere(280, 0.5f);

            CompositionTarget.Rendering += Render;
        }

        // Event handlers:

        protected void Render(object? sender, EventArgs e)
        {
            canvas.Render(vs);
        }

        private void CanvasContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.Resize(e.NewSize.Width, e.NewSize.Height);
            CanvasImage.Source = canvas.WriteableBitmap;

            Debug.WriteLine($"{ActualWidth} / {ActualHeight}   {CanvasContainer.ActualWidth} / {CanvasContainer.ActualHeight}");
        }
    }
}