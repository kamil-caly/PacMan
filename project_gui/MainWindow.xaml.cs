using project_logic;
using project_logic.Balls;
using project_logic.characters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private const int _rows = 21;
        private const int _cols = 19;
        private const int _cellSize = 40;
        private readonly int[,] board = GameTools._boardPattern;
        private readonly Pacman _packman;
        private readonly Image _packmanImg;
        private bool _isRunning;
        private int _gameSpeed = 5;
        private readonly BallsManager _ballsMng;
        private List<Image> _bigBallsImgs;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoard();
            _ballsMng = new(_cellSize);
            _bigBallsImgs = new();
            DrawBalls();
            SetStartTxtVisible();
            _isRunning = false;
            _packman = new Pacman(_cellSize);

            // packman img
            _packmanImg = new Image();
            _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
            _packmanImg.Width = _cellSize - 2;
            _packmanImg.Height = _cellSize - 2;
            DrawPacmanImg(_packmanImg, _packman.position);
            gameCanvas.Children.Add(_packmanImg);

            Draw();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // start game loop
            if (!_isRunning)
            {
                _isRunning = true;
                SetStartTxtVisible(false);
                await GameLoop();
            }

            switch (e.Key)
            {
                case Key.Right:
                    _packman.nextDirection = Direction.Right;
                    break;
                case Key.Left:
                    _packman.nextDirection = Direction.Left;
                    break;
                case Key.Up:
                    _packman.nextDirection = Direction.Up;
                    break;
                case Key.Down:
                    _packman.nextDirection = Direction.Down;
                    break;
                default:
                    break;
            }
        }

        private async Task GameLoop()
        {
            while (true) 
            {
                await Task.Delay(_gameSpeed); // 2000
                _packman.TryChangeDirection();
                if (_packman.CanMove())
                {
                    _packman.Move();
                    if (_packman.steps <= 0)
                    {
                        _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
                    }
                }
                Draw();
            }
        }

        private void CreateField(int cornerRadius, int row, int col, bool isPath)
        {
            Rectangle rect = new Rectangle
            {
                Width = _cellSize,
                Height = _cellSize,
            };

            int value = isPath ? 1 : 0;
            bool top = (row > 0) && board[row - 1, col] == value;
            bool bottom = (row < _rows - 1) && board[row + 1, col] == value;
            bool left = (col > 0) && board[row, col - 1] == value;
            bool right = (col < _cols - 1) && board[row, col + 1] == value;

            Path path = new Path();
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure { StartPoint = new System.Windows.Point(0, 0) };

            // Top-left corner
            figure.Segments.Add(new LineSegment(new System.Windows.Point(0, cornerRadius), true));
            if (top && left)
            {
                figure.Segments.Add(new ArcSegment(new System.Windows.Point(cornerRadius, 0), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(0, 0), true));
            }

            // Top-right corner
            figure.Segments.Add(new LineSegment(new System.Windows.Point(_cellSize - cornerRadius, 0), true));
            if (top && right)
            {
                figure.Segments.Add(new ArcSegment(new System.Windows.Point(_cellSize, cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(_cellSize, 0), true));
            }

            // Bottom-right corner
            figure.Segments.Add(new LineSegment(new System.Windows.Point(_cellSize, _cellSize - cornerRadius), true));
            if (bottom && right)
            {
                figure.Segments.Add(new ArcSegment(new System.Windows.Point(_cellSize - cornerRadius, _cellSize), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(_cellSize, _cellSize), true));
            }

            // Bottom-left corner
            figure.Segments.Add(new LineSegment(new System.Windows.Point(cornerRadius, _cellSize), true));
            if (bottom && left)
            {
                figure.Segments.Add(new ArcSegment(new System.Windows.Point(0, _cellSize - cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(0, _cellSize), true));
            }

            // Close the path
            if (top && left)
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(0, cornerRadius), true));
            }
            else
            {
                figure.Segments.Add(new LineSegment(new System.Windows.Point(0, 0), true));
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

        private void DrawBoard()
        {
            int cornerRadius = 10;

            gameCanvas.Width = _cols * _cellSize;
            gameCanvas.Height = _rows * _cellSize;

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _cols; col++)
                {
                    // block
                    if (GameTools._boardPattern[row, col] == 1)
                    {
                        CreateField(cornerRadius, row, col, false);
                    }
                    // path
                    else
                    {
                        CreateField(cornerRadius, row, col, true);
                    }
                }
            }
        }

        private void DrawBalls()
        {
            foreach (var bigBall in _ballsMng._bigBalls)
            {
                var bigBallImg = new Image();
                bigBallImg.Source = AssetsLoader.GetBigBallImg();
                bigBallImg.Width = 30;
                bigBallImg.Height = 30;
                var newPos = new project_logic.Point(bigBall._position.x * _cellSize, bigBall._position.y * _cellSize);
                DrawBigBallImg(bigBallImg, newPos);

                gameCanvas.Children.Add(bigBallImg);
                _bigBallsImgs.Add(bigBallImg);
            }
        }

        private void SetStartTxtVisible(bool isVisible = true)
        {
            StartTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void DrawPacmanImg(Image img, project_logic.Point point)
        {
            Canvas.SetLeft(img, point.x + 1);
            Canvas.SetTop(img, point.y + 1);
        }

        private void DrawBigBallImg(Image img, project_logic.Point point)
        {
            Canvas.SetLeft(img, point.x + 5);
            Canvas.SetTop(img, point.y + 5);
        }

        private void Draw()
        {
            DrawPacmanImg(_packmanImg, _packman.position);
        }
    }
}