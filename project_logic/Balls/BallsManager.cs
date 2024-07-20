using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_logic.Balls
{
    public class BallsManager
    {
        private int[,] board = GameTools._boardPattern;
        public int _cellSize { get; set; }
        public List<Ball> _smallBalls { get; set; }
        public List<Ball> _bigBalls { get; set; }
        public BallsManager(int cellSize)
        {
            _cellSize = cellSize;
            _smallBalls = new List<Ball>();
            _bigBalls = new List<Ball>();
            SetBigBalls();
            SetSmallBalls();
        }

        private void SetBigBalls()
        {
            Point[] positions = new Point[] { new Point(1, 2), new Point(17, 2), new Point(1, 15), new Point(17, 15) };
            foreach (Point pos in positions)
            {
                _bigBalls.Add(new BigBall(30, pos));
            }
        }

        private void SetSmallBalls()
        {

        }
    }
}
