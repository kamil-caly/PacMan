namespace project_logic.characters
{
    public abstract class Ghost : Character
    {
        protected Pacman pacman { get; set; }
        public GhostKind kind { get; set; }
        public Ghost(int cellSize, Pacman pacman) : base(cellSize)
        {
            this.pacman = pacman;
        }

        public bool IsChangeDirectionPossible()
        {
            double x = (double)position.x / cellSize;
            double y = (double)position.y / cellSize;

            if (x == Math.Floor(x) && y == Math.Floor(y))
            {
                if ((position.x == 320 || position.x == 360 || position.x == 400) && position.y == 360)
                {
                    return true;
                }

                int possibleDirs = 0;
                if (CanMove(Direction.Up)) possibleDirs++;
                if (CanMove(Direction.Right)) possibleDirs++;
                if (CanMove(Direction.Down)) possibleDirs++;
                if (CanMove(Direction.Left)) possibleDirs++;

                if (possibleDirs >= 3)
                {
                    return true;
                }

                return (CanMove(Direction.Up) && CanMove(Direction.Right))
                    || CanMove(Direction.Right) && CanMove(Direction.Down)
                    || CanMove(Direction.Down) && CanMove(Direction.Left)
                    || CanMove(Direction.Left) && CanMove(Direction.Up);
            }

            return false;
        }

        public void ChangeGhostDirection()
        {
            foreach (var dir in GetDirections())
            {
                var prevDir = direction;
                direction = dir;

                // uniemożliwiamy wchodzenie do miejsca startowego duchow
                if (direction == Direction.Down && this.position.x == 360 && this.position.y == 280)
                {
                    direction = prevDir == Direction.Left ? Direction.Left : Direction.Right;
                }

                // wyjątkowy przypadek gdy trzeba zawrócić bo nie da się wybrać innego kierunku
                // dzieje się tak w miejscu startowym duchow
                if (!this.CanMove() && !this.CanMove(GetClosestDirection(true)) && !this.CanMove(GetClosestDirection(false)))
                {
                    direction = GetOppositeDir();
                    break;
                }

                if (this.CanMove())
                {
                    break;
                }

                direction = prevDir;
            }
        }

        protected abstract IEnumerable<Direction> GetDirections();
    }
}
