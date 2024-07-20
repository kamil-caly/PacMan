namespace project_logic.characters
{
    public abstract class Character
    {
        public int cellSize { get; set; }
        private int[,] board = GameTools._boardPattern;
        public Point position { get; set; } = default!;
        public int speed { get; set; }
        public Direction direction { get; set; }
        public Direction nextDirection { get; set; }

        public Character(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public bool CanMove()
        {
            double newY = 0;
            double newX = 0;
            double row = 0;
            double col = 0;
            switch (direction)
            {
                case Direction.Up:
                    newY = position.y - 1;
                    row = Math.Floor(newY / cellSize);
                    col = (double)position.x / cellSize;
                    if (col % 1 == 0)
                    {
                        return board[(int)row, (int)col] == 0;
                    }
                    return false;
                case Direction.Right:
                    newX = position.x + 1;
                    col = (int)Math.Ceiling(newX / cellSize);
                    row = (double)position.y / cellSize;
                    if (row % 1 == 0)
                    {
                        return board[(int)row, (int)col] == 0;
                    }
                    return false;
                case Direction.Down:
                    newY = position.y + 1;
                    row = (int)Math.Ceiling(newY / cellSize);
                    col = (double)position.x / cellSize;
                    if (col % 1 == 0)
                    {
                        return board[(int)row, (int)col] == 0;
                    }
                    return false;
                case Direction.Left:
                    newX = position.x - 1;
                    col = (int)Math.Floor(newX / cellSize);
                    row = (double)position.y / cellSize;
                    if (row % 1 == 0)
                    {
                        return board[(int)row, (int)col] == 0;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public void TryChangeDirection()
        {
            var prevDir = direction;
            direction = nextDirection;
            if (!CanMove())
            {
                direction = prevDir;
            }
        }

        public virtual void Move()
        {
            int newY = 0;
            int newX = 0;
            switch (direction)
            {
                case Direction.Up:
                    newY = position.y - 1;
                    position.y = newY;
                    break;
                case Direction.Right:
                    newX = position.x + 1;
                    position.x = newX;
                    break;
                case Direction.Down:
                    newY = position.y + 1;
                    position.y = newY;
                    break;
                case Direction.Left:
                    newX = position.x - 1;
                    position.x = newX;
                    break;
                default:
                    break;
            }
        }
    }
}
