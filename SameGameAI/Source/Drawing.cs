using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SameGameAI
{
    class Drawing
    {
        public static void DrawTile(double x, double y, double size, SolidColorBrush color, Canvas canvas)
        {
            Rectangle temp = new Rectangle();
            temp.Fill = color;
            temp.Stroke = Brushes.Black;
            temp.Height = size;
            temp.Width = size;
            Canvas.SetBottom(temp, y);
            Canvas.SetLeft(temp, x);
            canvas.Children.Add(temp);
        }
    }
}
