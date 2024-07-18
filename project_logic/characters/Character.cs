namespace project_logic.characters
{
    public abstract class Character
    {
        public int cellSize { get; set; }
        private int[,] board = GameTools._boardPattern;

        public Character(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public bool CanMove(Direction dir, Point pos)
        {
            int newY = 0;
            int newX = 0;
            int row = 0;
            int col = 0;
            switch (dir)
            {
                case Direction.Up:
                    newY = pos.y - 1;
                    row = newY / cellSize;
                    col = pos.x / cellSize;
                    return board[row, col] == 0;
                case Direction.Right:
                    newX = pos.x + 1;
                    row = pos.y / cellSize;
                    col = newX / cellSize;
                    return board[row, col] == 0;
                case Direction.Down:
                    newY = pos.y + 1;
                    row = newY / cellSize;
                    col = pos.x / cellSize;
                    return board[row, col] == 0;
                case Direction.Left:
                    newX = pos.x - 1;
                    row = pos.y / cellSize;
                    col = newX / cellSize;
                    return board[row, col] == 0;
                default:
                    return false;
            }
        }

        public void Move(Direction dir, Point pos)
        {
            int newY = 0;
            int newX = 0;
            switch (dir)
            {
                case Direction.Up:
                    newY = pos.y - 1;
                    pos.y = newY;
                    break;
                case Direction.Right:
                    newX = pos.x + 1;
                    pos.x = newX;
                    break;
                case Direction.Down:
                    newY = pos.y + 1;
                    pos.y = newY;
                    break;
                case Direction.Left:
                    newX = pos.x - 1;
                    pos.x = newX;
                    break;
                default:
                    break;
            }
        }
    }
}
