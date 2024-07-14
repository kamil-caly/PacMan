using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace project_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int[] board =
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0 ,0 ,0, 0, 0, 0, 0,
            1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 1, 1, 0 ,1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1,
            1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
            1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1,
            1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        };

        private const int _rows = 21;
        private const int _cols = 19;
        private const int _cellSize = 40;

        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();

            // test
            string imagePath = "assets/Ghost 1 3.png";
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            img.Width = _cellSize - 2;
            img.Height = _cellSize - 2;
           
            Canvas.SetLeft(img, _cellSize * 3+ 1); 
            Canvas.SetTop(img, _cellSize * 3+ 1); 

            gameCanvas.Children.Add(img);
        }

        private void DrawField(int cornerRadius, int row, int col, bool isPath)
        {
            Rectangle rect = new Rectangle
            {
                Width = _cellSize,
                Height = _cellSize,
            };

            int value = isPath ? 1 : 0;
            bool top = (row > 0) && board[(row - 1) * _cols + col] == value;
            bool bottom = (row < _rows - 1) && board[(row + 1) * _cols + col] == value;
            bool left = (col > 0) && board[row * _cols + (col - 1)] == value;
            bool right = (col < _cols - 1) && board[row * _cols + (col + 1)] == value;

            Path path = new Path();
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = new Point(0, 0) };

            // Top-left corner
            figure.Segments.Add(new LineSegment(new Point(0, cornerRadius), true));
            if (top && left)
            {
                figure.Segments.Add(new ArcSegment(new Point(cornerRadius, 0), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new Point(0, 0), true));
            }

            // Top-right corner
            figure.Segments.Add(new LineSegment(new Point(_cellSize - cornerRadius, 0), true));
            if (top && right)
            {
                figure.Segments.Add(new ArcSegment(new Point(_cellSize, cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new Point(_cellSize, 0), true));
            }

            // Bottom-right corner
            figure.Segments.Add(new LineSegment(new Point(_cellSize, _cellSize - cornerRadius), true));
            if (bottom && right)
            {
                figure.Segments.Add(new ArcSegment(new Point(_cellSize - cornerRadius, _cellSize), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new Point(_cellSize, _cellSize), true));
            }

            // Bottom-left corner
            figure.Segments.Add(new LineSegment(new Point(cornerRadius, _cellSize), true));
            if (bottom && left)
            {
                figure.Segments.Add(new ArcSegment(new Point(0, _cellSize - cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new Point(0, _cellSize), true));
            }

            // Close the path
            if (top && left)
            {
                figure.Segments.Add(new LineSegment(new Point(0, cornerRadius), true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new Point(0, 0), true));
            }

            geometry.Figures.Add(figure);
            path.Data = geometry;
            path.Fill = isPath ? Brushes.Black : Brushes.Blue;

            if (isPath)
            {
                rect.Fill = Brushes.Blue;
                Canvas.SetLeft(rect, col * _cellSize);
                Canvas.SetTop(rect, row * _cellSize);
                gameCanvas.Children.Add(rect);
            }

            Canvas.SetLeft(path, col * _cellSize);
            Canvas.SetTop(path, row * _cellSize);
            gameCanvas.Children.Add(path);
        }

        private void CreateBoard()
        {
            int cornerRadius = 10;

            gameCanvas.Width = _cols * _cellSize;
            gameCanvas.Height = _rows * _cellSize;

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    int index = row * _cols + col;

                    // block
                    if (board[index] == 1)
                    {
                        DrawField(cornerRadius, row, col, false);
                    }
                    // path
                    else
                    {
                        DrawField(cornerRadius, row, col, true);
                    }
                }
            }
        }
    }
}