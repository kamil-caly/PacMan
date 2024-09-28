namespace project_logic.characters
{
    public abstract class Character
    {
        public int cellSize { get; set; }
        private int[,] board = GameTools._boardPattern;
        public Point position { get; set; } = default!;
        public int speed { get; set; }
        public int counter { get; set; } = 0;
        public Direction direction { get; set; }
        public int steps { get; set; } = 0;
        protected Random random { get; private set; } = new Random();

        public Character(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public bool IsMoveTime()
        {
            counter++;
            if (counter >= speed)
            {
                counter = 0;
                return true;
            }
            
            return false;
        }

        public bool CanMove(Direction? dir = null)
        {
            double newY = 0;
            double newX = 0;
            double row = 0;
            double col = 0;

            switch (dir != null ? dir : direction)
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

                    // przypadek przechodzenia przez tunel
                    if (col >= board.GetLength(1) && row == 9)
                    {
                        return true;
                    }

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

                    // przypadek przechodzenia przez tunel
                    if (col <= 0 && row == 9)
                    {
                        return true;
                    }

                    if (row % 1 == 0)
                    {
                        return board[(int)row, (int)col] == 0;
                    }
                    return false;
                default:
                    return false;
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
                    // przypadek przechodzenia przez tunel
                    if (newX > (board.GetLength(1) - 1) * cellSize)
                    {
                        newX = 0;
                    }
                    position.x = newX;
                    break;
                case Direction.Down:
                    newY = position.y + 1;
                    position.y = newY;
                    break;
                case Direction.Left:
                    newX = position.x - 1;
                    // przypadek przechodzenia przez tunel
                    if (newX < 0)
                    {
                        newX = (board.GetLength(1) - 1) * cellSize;
                    }
                    position.x = newX;
                    break;
                default:
                    break;
            }
        }

        protected Direction GetRandomDir(Direction? blockedDir = null)
        {
            while (true)
            {
                var newDir = random.Next(4);

                if (blockedDir == null)
                {
                    return (Direction)newDir;
                }

                if (newDir != (int)blockedDir)
                {
                    return (Direction)newDir;
                }
            }
        }

        protected Direction GetOppositeDir()
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                default:
                    return Direction.Right;
            }
        }

        protected Direction GetClosestDirection(bool isNext)
        {
            int directionCount = Enum.GetValues(typeof(Direction)).Length;
            int currentDirection = (int)direction;

            if (isNext)
            {
                return (Direction)((currentDirection + 1) % directionCount);
            }
            else
            {
                return (Direction)((currentDirection - 1 + directionCount) % directionCount);
            }
        }
    }
}
