﻿using project_logic;
using project_logic.Balls;
using project_logic.characters;
using project_logic.characters.Mediator;
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
        List<Ghost> _ghosts;
        private Image _packmanImg;
        private Image _panicGhostImg;
        Dictionary<GhostKind, Image> _ghostsImg;
        private bool _isRunning;
        private bool _isGameOver;
        private const int _gameSpeed = 1;
        private BallsManager _ballsMng;
        private List<Image> _bigBallsImgs;
        private List<Image> _smallBallsImgs;
        private const string _winTxt = "You win!";
        private const string _looseTxt = "Game Over!";
        private const string _lifeImgSource = "assets/Pacman 2 1.png";
        private DateTime? currentEatBigBallTime = null;
        private DateTime? currentPanicModeSwitchSkinTime = null;
        private const int panicModeSwitchSkinMS = 200;
        private bool isDarkCurrentPanicModeImg = true;

        public MainWindow()
        {
            InitializeComponent();
            RestartGame();
        }

        private void RestartGame(bool isHardRestart = true)
        {
            _isGameOver = false;
            _isRunning = false;

            if (isHardRestart)
            {
                gameCanvas.Children.Clear();
                DrawBoard();
                UpdateLifeView(true);
                _ballsMng = new(_cellSize, _bigBallLen, _smallBallLen);
                _bigBallsImgs = new();
                _smallBallsImgs = new();
                DrawBalls();
                _packman = new Pacman(_cellSize);
            }
            else
            {
                _packman.SetStartPosition();
                gameCanvas.Children.Remove(_packmanImg);
                foreach (var ghost in _ghosts)
                {
                    gameCanvas.Children.Remove(_ghostsImg[ghost.kind]);
                }
                _isRunning = true;
            }

            _ghosts = new List<Ghost>() 
            {
                new Blinky(_cellSize),
                new Pinky(_cellSize),
                new Inky(_cellSize),
                new Clyde(_cellSize)
            };

            _ghostsImg = new Dictionary<GhostKind, Image>();

            foreach (var ghost in _ghosts)
            {
                _ghostsImg.Add(ghost.kind, new Image());
                _ghostsImg[ghost.kind].Source = AssetsLoader.GetNextGostImg(ghost.direction, ghost.kind);
                _ghostsImg[ghost.kind].Width = _characterLen;
                _ghostsImg[ghost.kind].Height = _characterLen;
                DrawCharacterImg(_ghostsImg[ghost.kind], ghost.position);
                gameCanvas.Children.Add(_ghostsImg[ghost.kind]);
            }

            // packman img
            _packmanImg = new Image();
            _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
            _packmanImg.Width = _characterLen;
            _packmanImg.Height = _characterLen;
            DrawCharacterImg(_packmanImg, _packman.position);
            gameCanvas.Children.Add(_packmanImg);

            UpdateScore();
            if (isHardRestart)
            {
                SetStartTxtVisible();
            }
            DrawCharacters();
            new RegisterCharacters(_packman, _ghosts);
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // start game loop
            if (!_isRunning && !_isGameOver)
            {
                _isRunning = true;
                _isGameOver = false;
                SetStartTxtVisible(false);
                await GameLoop();
                return;
            } 
            else if (_isGameOver && !_isRunning)
            {
                RestartGame();
            }

            if (e.Key != Key.R && StartTextBox.Text != _winTxt)
            {
                SetStartTxtVisible(false);
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

        private void PackmanMove()
        {
            if (_packman.IsMoveTime() && _packman.CanMove())
            {
                _packman.Move();
                if (_packman.steps <= 0)
                {
                    _packmanImg.Source = AssetsLoader.GetNextPackmanImg(_packman.direction);
                }

                if (TryEatBigBall())
                {
                    currentEatBigBallTime = DateTime.Now;
                    currentPanicModeSwitchSkinTime = currentEatBigBallTime;
                    isDarkCurrentPanicModeImg = true;

                    foreach (var ghostImg in _ghostsImg)
                    {
                        ghostImg.Value.Source = AssetsLoader.GetPanicGostImg(isDarkCurrentPanicModeImg);
                    }
                }
                TryEatSmallBall();
                UpdateScore();
            }
        }

        private bool TryEatBigBall()
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
                return true;
            }

            return false;
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

        private void GhostsMove()
        {
            foreach (var ghost in _ghosts)
            {
                if (ghost.PacmanHitLogic())
                {
                    if (ghost.isPanicMode)
                    {
                        _packman.UpdatePoints(ScoreType.Ghost);
                        ghost.SetPanicMode(false);
                        ghost.SetStartPosition();
                        ghost.ChangeGhostDirection();
                        _ghostsImg[ghost.kind].Source = AssetsLoader.GetNextGostImg(ghost.direction, ghost.kind);
                    }
                    else
                    {
                        UpdateLifeView();
                        if (_packman.life > 0)
                        {
                            RestartGame(false);
                        }
                    }
                }

                if (ghost.IsMoveTime())
                {
                    if (ghost.IsChangeDirectionPossible())
                    {
                        ghost.ChangeGhostDirection();
                        if (!ghost.isPanicMode)
                            _ghostsImg[ghost.kind].Source = AssetsLoader.GetNextGostImg(ghost.direction, ghost.kind);
                    }

                    // tymczasowe zablokowanie chodzenia Pinky
                    // if (ghost.kind == GhostKind.Blinky)
                    ghost.Move();
                }
            }
        }

        private void UpdateGhostsPanicMode()
        {
            if (currentEatBigBallTime != null)
            {
                TimeSpan diff = DateTime.Now - currentEatBigBallTime.Value;
                if (diff.TotalSeconds >= _ghosts.First().PanicModeTimeS)
                {
                    _ghosts.ForEach(g => g.SetPanicMode(false));
                    foreach (var ghost in _ghosts)
                    {
                        _ghostsImg[ghost.kind].Source = AssetsLoader.GetNextGostImg(ghost.direction, ghost.kind);
                    }
                    currentEatBigBallTime = null;
                }
                else if (diff.TotalSeconds >= _ghosts.First().PanicModeTimeS - 2)
                {
                    if (currentPanicModeSwitchSkinTime != null)
                    {
                        TimeSpan diff2 = DateTime.Now - currentPanicModeSwitchSkinTime.Value;
                        if (diff2.TotalMilliseconds >= panicModeSwitchSkinMS)
                        {
                            isDarkCurrentPanicModeImg = !isDarkCurrentPanicModeImg;
                            foreach (var ghost in _ghosts)
                            {
                                if (ghost.isPanicMode)
                                    _ghostsImg[ghost.kind].Source = AssetsLoader.GetPanicGostImg(isDarkCurrentPanicModeImg);
                            }
                            currentPanicModeSwitchSkinTime = DateTime.Now;
                        }
                    }
                }
            }
        }

        private async Task GameLoop()
        {
            while (_isRunning && !_isGameOver) 
            {
                await Task.Delay(_gameSpeed); 
                _packman.TryChangeDirection();

                // ruch Pacman'a
                PackmanMove();

                // ruch duchów
                GhostsMove();

                // do aktualizacji panic mode duchów
                UpdateGhostsPanicMode();

                DrawCharacters();

                if (_ballsMng.IsBallsEmpty() || _packman.life <= 0)
                {
                    _isGameOver = true;
                    _isRunning = false;
                    UpdateGameOverTxt();
                }
            }
        }

        

        #region Board Drawing
        private void DrawCharacterImg(Image img, project_logic.Point point)
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

        private void DrawCharacters()
        {
            DrawCharacterImg(_packmanImg, _packman.position);

            foreach (var ghostImg in _ghostsImg)
            {
                DrawCharacterImg(ghostImg.Value, GetGhost(ghostImg.Key).position);
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
        #endregion


        #region Updating Controls
        private void SetStartTxtVisible(bool isVisible = true)
        {
            StartTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            StartTextBox.Text = "Press any key to start...";
            StartTextBox.FontSize = 32;
        }

        private void UpdateGameOverTxt()
        {
            StartTextBox.Text = _isGameOver ? (_packman.life <= 0 ? _looseTxt : _winTxt) : "";
            StartTextBox.FontSize = _isGameOver ? 60 : 32;
            StartTextBox.Visibility = _isGameOver ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateLifeView(bool restart = false)
        {
            if (restart)
            {
                LifePanel.Children.Clear();
                for (int i = 0; i < 3; i++)
                {
                    LifePanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri(_lifeImgSource, UriKind.RelativeOrAbsolute)),
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(5)
                    });

                }
            }
            else if (LifePanel.Children.Count > 0)
            {
                LifePanel.Children.Remove(LifePanel.Children[LifePanel.Children.Count - 1]);
            }
        }

        private void UpdateScore()
        {
            scoreTextBox.Text = $"SCORE: {_packman.score}";
            highScoreTextBox.Text = $"HIGH SCORE: {Pacman.highScore}";
        }
        #endregion


        private Ghost GetGhost(GhostKind kind)
        {
            return _ghosts.First(g => g.kind == kind);
        }
    }
}