using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DINAPP.Libs.Client.UWP
{
    internal class QrCodeCanvasBuilder
    {
        /// <summary>
        /// Renders QR code as canvas control
        /// </summary>
        /// <param name="qrMatrix">QR code bit matrix</param>
        /// <param name="canvasWidth">Canvas width</param>
        /// <param name="canvasHeight">Canvas height</param>
        /// <returns>Canvas with QR code</returns>
        public Canvas BuildQrCodeCanvas(bool[,] qrMatrix, double canvasWidth, double canvasHeight)
        {
            var canvas = new Canvas();
            canvas.Width = canvasWidth;
            canvas.Height = canvasHeight;
            canvas.Background = new SolidColorBrush(Colors.White);

            var arrayWidth = qrMatrix.GetLength(0);
            var arrayHeight = qrMatrix.GetLength(1);

            var xDimensionInterval = canvasWidth / arrayWidth;
            var yDimensionInterval = canvasHeight / arrayHeight;

            for (int i = 0; i < arrayWidth; i++)
                for (int j = 0; j < arrayHeight; j++)
                {
                    if (qrMatrix[i, j])
                    {
                        var top = j * yDimensionInterval;
                        var left = i * xDimensionInterval;

                        var rect = CreateRectangle(left, top, yDimensionInterval + 0.1, xDimensionInterval + 0.1);
                        canvas.Children.Add(rect);
                    }
                }

            return canvas;
        }

        private Rectangle CreateRectangle(double left, double top, double height, double width)
        {
            var rectangle = new Rectangle();
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Fill = new SolidColorBrush(Colors.Black);
            rectangle.Stroke = new SolidColorBrush(Colors.Black);
            rectangle.StrokeThickness = 1;

            Canvas.SetLeft(rectangle, left);
            Canvas.SetTop(rectangle, top);
            return rectangle;
        }

    }
}
