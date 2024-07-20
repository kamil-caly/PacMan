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
        public int _bigBallLen { get; set; }
        public int _smallBallLen { get; set; }
        public List<SmallBall> _smallBalls { get; set; }
        public List<BigBall> _bigBalls { get; set; }
        public BallsManager(int cellSize, int bigBallLen, int smallBallLen)
        {
            _cellSize = cellSize;
            _smallBalls = new List<SmallBall>();
            _bigBalls = new List<BigBall>();
            _bigBallLen = bigBallLen;
            _smallBallLen = smallBallLen;
            SetBigBalls();
            SetSmallBalls();
        }

        private void SetBigBalls()
        {
            PointD[] positions = new PointD[] { new PointD(1, 2), new PointD(17, 2), new PointD(1, 15), new PointD(17, 15) };

            foreach (PointD pos in positions)
            {
                _bigBalls.Add(new BigBall(_bigBallLen, pos, _cellSize));
            }
        }

        private void SetSmallBalls()
        {
            PointD[] positions = new PointD[] 
            { 
                // rogi i rozwidlenia
                // 1 wiersz
                new PointD(1, 1), new PointD(4, 1), new PointD(8, 1), new PointD(10, 1), new PointD(14, 1), new PointD(17, 1),
                // 3 wiersz
                new PointD(1, 3), new PointD(4, 3), new PointD(6, 3), new PointD(12, 3), new PointD(14, 3), new PointD(17, 3),
                // 5 wiersz
                new PointD(1, 5), new PointD(4, 5), new PointD(6, 5), new PointD(8, 5), new PointD(10, 5), new PointD(12, 5), new PointD(14, 5), new PointD(17, 5),
                // 9 wiersz
                new PointD(4, 9), new PointD(14, 9),
                // 13 wiersz
                new PointD(1, 13), new PointD(4, 13), new PointD(6, 13), new PointD(8, 13), new PointD(10, 13), new PointD(12, 13), new PointD(14, 13), new PointD(17, 13),
                // 15 wiersz
                new PointD(2, 15), new PointD(4, 15), new PointD(6, 15), new PointD(8, 15), new PointD(10, 15), new PointD(12, 15), new PointD(14, 15), new PointD(16, 15),
                // 17 wiersz
                new PointD(1, 17), new PointD(2, 17), new PointD(4, 17), new PointD(6, 17), new PointD(8, 17), new PointD(10, 17), new PointD(12, 17), new PointD(14, 17), new PointD(16, 17), new PointD(17, 17),
                // 19 wiersz
                new PointD(1, 19), new PointD(8, 19), new PointD(10, 19), new PointD(17, 19),

                // pozostałe
                // 1 wiersz
                new PointD(1.75, 1), new PointD(2.5, 1), new PointD(3.25, 1), new PointD(4.8, 1), new PointD(5.6, 1),
                new PointD(6.4, 1), new PointD(7.2, 1),
            };

            foreach (PointD pos in positions)
            {
                _smallBalls.Add(new SmallBall(_smallBallLen, pos, _cellSize));
            }
        }

        public void RemoveBall(Ball ball)
        {
            if (ball is BigBall)
            {
                _bigBalls.Remove((BigBall)ball);
            }
            else
            {
                _smallBalls.Remove((SmallBall)ball);
            }
        }
    }
}
