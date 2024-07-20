namespace project_logic.Balls
{
    public abstract class Ball
    {
        public int _len { get; set; }
        public int _cellSize { get; set; }
        public PointD _position { get; set; }
        protected Ball(int len, PointD position, int cellSize)
        {
            _len = len;
            _position = position;
            _cellSize = cellSize;
        }

        public bool CanRemoveBall(Point pacmanPos)
        {
            int startX = pacmanPos.x + 5;
            int endX = pacmanPos.x + _cellSize - 5;
            int startY = pacmanPos.y + 5;
            int endY = pacmanPos.y + _cellSize - 5;

            double startBallX = _position.x * _cellSize + ((_cellSize - _len) / 2);
            double endBallX = _position.x * _cellSize + ((_cellSize - _len) / 2 * 3);
            double startBallY = _position.y * _cellSize + ((_cellSize - _len) / 2);
            double endBallY = _position.y * _cellSize + ((_cellSize - _len) / 2 * 3);

            if (startBallX >= startX && endBallX <= endX && startBallY >= startY && endBallY <= endY)
            {
                return true;
            }

            return false;
        }
    }
}
