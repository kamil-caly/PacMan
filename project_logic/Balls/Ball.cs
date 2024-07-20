namespace project_logic.Balls
{
    public abstract class Ball
    {
        public int _len { get; set; }
        public Point _position { get; set; }
        protected Ball(int len, Point position)
        {
            _len = len;
            _position = position;
        }

        public bool CanRemoveBall(Point pacmanPos)
        {
            int startX = pacmanPos.x + 5;
            int endX = pacmanPos.x - 5;
            int startY = pacmanPos.y + 5;
            int endY = pacmanPos.y - 5;

            if (_position.x >= startX && _position.x + _len <= endX && _position.y >= startY && _position.y + _len <= endY)
            {
                return true;
            }

            return false;
        }
    }
}
