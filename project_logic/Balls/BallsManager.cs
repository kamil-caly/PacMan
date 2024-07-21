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
                new PointD(8, 3), new PointD(10, 3),
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
                new PointD(6.4, 1), new PointD(7.2, 1), new PointD(10.8, 1), new PointD(11.6, 1), new PointD(12.4, 1),
                new PointD(13.2, 1), new PointD(14.75, 1), new PointD(15.5, 1), new PointD(16.25, 1),
                // 1 kolumna
                new PointD(1, 3.67), new PointD(1, 4.33),
                new PointD(1, 14), new PointD(1, 17.67), new PointD(1, 18.33),
                // 4 kolumna
                new PointD(4, 1.67), new PointD(4, 2.33), new PointD(4, 3.67), new PointD(4, 4.33),
                new PointD(4, 5.8), new PointD(4, 6.6), new PointD(4, 7.4), new PointD(4, 8.2),
                new PointD(4, 9.8), new PointD(4, 10.6), new PointD(4, 11.4), new PointD(4, 12.2),
                new PointD(4, 13.67), new PointD(4, 14.33),
                new PointD(4, 15.67), new PointD(4, 16.33),
                // 14 kolumna
                new PointD(14, 1.67), new PointD(14, 2.33), new PointD(14, 3.67), new PointD(14, 4.33),
                new PointD(14, 5.8), new PointD(14, 6.6), new PointD(14, 7.4), new PointD(14, 8.2),
                new PointD(14, 9.8), new PointD(14, 10.6), new PointD(14, 11.4), new PointD(14, 12.2),
                new PointD(14, 13.67), new PointD(14, 14.33),
                new PointD(14, 15.67), new PointD(14, 16.33),
                // 3 wiersz
                new PointD(1.75, 3), new PointD(2.5, 3), new PointD(3.25, 3),
                new PointD(4.67, 3), new PointD(5.33, 3),
                new PointD(6.67, 3), new PointD(7.33, 3),
                new PointD(8.67, 3), new PointD(9.33, 3),
                new PointD(10.67, 3), new PointD(11.33, 3),
                new PointD(12.67, 3), new PointD(13.33, 3),
                new PointD(14.75, 3), new PointD(15.5, 3), new PointD(16.25, 3),
                // 5 wiersz
                new PointD(1.75, 5), new PointD(2.5, 5), new PointD(3.25, 5),
                new PointD(6.67, 5), new PointD(7.33, 5),
                new PointD(10.67, 5), new PointD(11.33, 5),
                new PointD(14.75, 5), new PointD(15.5, 5), new PointD(16.25, 5),
                // 13 wiersz
                new PointD(1.75, 13), new PointD(2.5, 13), new PointD(3.25, 13),
                new PointD(4.67, 13), new PointD(5.33, 13),
                new PointD(6.67, 13), new PointD(7.33, 13),
                new PointD(10.67, 13), new PointD(11.33, 13),
                new PointD(12.67, 13), new PointD(13.33, 13),
                new PointD(14.75, 13), new PointD(15.5, 13), new PointD(16.25, 13),
                // 15 wiersz
                new PointD(4.67, 15), new PointD(5.33, 15),
                new PointD(6.67, 15), new PointD(7.33, 15),
                new PointD(8.67, 15), new PointD(9.33, 15),
                new PointD(10.67, 15), new PointD(11.33, 15),
                new PointD(12.67, 15), new PointD(13.33, 15),
                // 17 wiersz
                new PointD(2.67, 17), new PointD(3.33, 17),
                new PointD(6.67, 17), new PointD(7.33, 17),
                new PointD(10.67, 17), new PointD(11.33, 17),
                new PointD(14.67, 17), new PointD(15.33, 17),
                // 19 wiersz
                new PointD(1.86, 19), new PointD(2.72, 19), new PointD(3.58, 19), new PointD(4.44, 19), new PointD(5.30, 19),
                new PointD(6.16, 19), new PointD(7.02, 19),
                new PointD(8.67, 19), new PointD(9.33, 19),
                new PointD(10.86, 19), new PointD(11.72, 19), new PointD(12.58, 19), new PointD(13.44, 19), new PointD(14.30, 19),
                new PointD(15.16, 19), new PointD(16.02, 19),
                // 2 kolumna
                new PointD(2, 15.67), new PointD(2, 16.33),
                // 6 kolumna
                new PointD(6, 3.67), new PointD(6, 4.33),
                new PointD(6, 15.67), new PointD(6, 16.33),
                // 8 kolumna
                new PointD(8, 1.67), new PointD(8, 2.33),
                new PointD(8, 13.67), new PointD(8, 14.33),
                new PointD(8, 17.67), new PointD(8, 18.33),
                // 10 kolumna
                new PointD(10, 1.67), new PointD(10, 2.33),
                new PointD(10, 13.67), new PointD(10, 14.33),
                new PointD(10, 17.67), new PointD(10, 18.33),
                // 12 kolumna
                new PointD(12, 3.67), new PointD(12, 4.33),
                new PointD(12, 15.67), new PointD(12, 16.33),
                // 16 kolumna
                new PointD(16, 15.67), new PointD(16, 16.33),
                // 17 kolumna
                new PointD(17, 3.67), new PointD(17, 4.33),
                new PointD(17, 13.67), new PointD(17, 14.33),
                new PointD(17, 17.67), new PointD(17, 18.33),
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
