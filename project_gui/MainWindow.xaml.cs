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
        private const int _bigBallLen = 30;
        private const int _smallBallLen = 24;
        private const int _characterLen = 38;
        private int[,] board = GameTools._boardPattern;
        private Pacman _packman;
        private Image _packmanImg;
        private bool _isRunning;
        private int _gameSpeed = 5;
        private BallsManager _ballsMng;
        private List<Image> _bigBallsImgs;
        private List<Image> _smallBallsImgs;

        public MainWindow()
        {
            InitializeComponent();
            _isRunning = false;
            SetStartTxtVisible();
            RestartGame();
        }

        private void RestartGame()
        {
            gameCanvas.Children.Clear();
            DrawBoard();
            _ballsMng = new(_cellSize, _bigBallLen, _smallBallLen);
            _bigBallsImgs = new();
            _smallBallsImgs = new();
            DrawBalls();
            _packman = new Pacman(_cellSize);

            // packman img
            _packmanImg = new Image();
            _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
            _packmanImg.Width = _characterLen;
            _packmanImg.Height = _characterLen;
            DrawPacmanImg(_packmanImg, _packman.position);
            gameCanvas.Children.Add(_packmanImg);

            UpdateScore();
            Draw();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // start game loop
            if (!_isRunning)
            {
                _isRunning = true;
                SetStartTxtVisible(false);
                await PacmanLoop();
                return;
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
                case Key.R:
                    RestartGame();
                    break;
                default:
                    break;
            }
        }

        private async Task PacmanLoop()
        {
            while (_isRunning) 
            {
                await Task.Delay(_packman.speed); // 2000
                _packman.TryChangeDirection();
                if (_packman.CanMove())
                {
                    _packman.Move();
                    if (_packman.steps <= 0)
                    {
                        _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
                    }

                    TryEatBigBall();
                    TryEatSmallBall();
                    UpdateScore();
                }
                Draw();
            }
        }

        private void TryEatBigBall()
        {
            var eatedBigBall = _ballsMng._bigBalls.FirstOrDefault(b => b.CanRemoveBall(_packman.position));
            if (eatedBigBall is not null)
            {
                _ballsMng.RemoveBall(eatedBigBall);
                var bigBallImg = _bigBallsImgs.FirstOrDefault(i => i.DataContext.ToString()!.Split(" ")[0] == eatedBigBall._position.x.ToString()
                    && i.DataContext.ToString()!.Split(" ")[1] == eatedBigBall._position.y.ToString());
                _bigBallsImgs.Remove(bigBallImg!);
                gameCanvas.Children.Remove(bigBallImg);

                _packman.UpdatePoints(ScoreType.BigBall);
            }
        }

        private void TryEatSmallBall()
        {
            var eatedSmallBall = _ballsMng._smallBalls.FirstOrDefault(b => b.CanRemoveBall(_packman.position));
            if (eatedSmallBall is not null)
            {
                _ballsMng.RemoveBall(eatedSmallBall);
                var smallBallImg = _smallBallsImgs.FirstOrDefault(i => i.DataContext.ToString()!.Split(" ")[0] == eatedSmallBall._position.x.ToString()
                    && i.DataContext.ToString()!.Split(" ")[1] == eatedSmallBall._position.y.ToString());
                _bigBallsImgs.Remove(smallBallImg!);
                gameCanvas.Children.Remove(smallBallImg);

                _packman.UpdatePoints(ScoreType.SmallBall);
            }
        }

        private void UpdateScore()
        {
            scoreTextBox.Text = $"SCORE: {_packman.score}";
            highScoreTextBox.Text = $"HIGH SCORE: {Pacman.highScore}";
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
            // big balls
            foreach (var bigBall in _ballsMng._bigBalls)
            {
                var bigBallImg = new Image();
                bigBallImg.Source = AssetsLoader.GetBigBallImg();
                bigBallImg.Width = _bigBallLen;
                bigBallImg.Height = _bigBallLen;
                bigBallImg.DataContext = bigBall._position.x.ToString() + " " + bigBall._position.y.ToString();
                var newPos = new project_logic.PointD(bigBall._position.x * _cellSize, bigBall._position.y * _cellSize);
                DrawBigBallImg(bigBallImg, newPos);

                gameCanvas.Children.Add(bigBallImg);
                _bigBallsImgs.Add(bigBallImg);
            }

            // small balls
            foreach (var smallBall in _ballsMng._smallBalls)
            {
                var smallBallImg = new Image();
                smallBallImg.Source = AssetsLoader.GetSmallBallImg();
                smallBallImg.Width = _smallBallLen;
                smallBallImg.Height = _smallBallLen;
                smallBallImg.DataContext = smallBall._position.x.ToString() + " " + smallBall._position.y.ToString();
                var newPos = new project_logic.PointD(smallBall._position.x * _cellSize, smallBall._position.y * _cellSize);
                DrawSmallBallImg(smallBallImg, newPos);

                gameCanvas.Children.Add(smallBallImg);
                _smallBallsImgs.Add(smallBallImg);
            }
        }

        private void SetStartTxtVisible(bool isVisible = true)
        {
            StartTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void DrawPacmanImg(Image img, project_logic.Point point)
        {
            Canvas.SetLeft(img, point.x + ((_cellSize - _characterLen) / 2));
            Canvas.SetTop(img, point.y + ((_cellSize - _characterLen) / 2));
        }

        private void DrawBigBallImg(Image img, PointD point)
        {
            Canvas.SetLeft(img, point.x + ((_cellSize - _bigBallLen) / 2));
            Canvas.SetTop(img, point.y + ((_cellSize - _bigBallLen) / 2));
        }

        private void DrawSmallBallImg(Image img, PointD point)
        {
            Canvas.SetLeft(img, point.x + ((_cellSize - _smallBallLen) / 2));
            Canvas.SetTop(img, point.y + ((_cellSize - _smallBallLen) / 2));
        }

        private void Draw()
        {
            DrawPacmanImg(_packmanImg, _packman.position);
        }
    }
}