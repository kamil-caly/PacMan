using project_logic.characters.Mediator;

namespace project_logic.characters
{
    public abstract class Ghost : Character
    {
        public GhostKind kind { get; set; }
        public bool isPanicMode { get;  private set; }
        public readonly int PanicModeTimeS;
        public Ghost(int cellSize, int panicModeTimeS, bool isPanicMode) : base(cellSize)
        {
            this.PanicModeTimeS = panicModeTimeS;
            this.isPanicMode = isPanicMode;
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
            foreach (var dir in isPanicMode ? GetEscapeDirections() : GetDirections())
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

        protected IEnumerable<Direction> GetEscapeDirections()
        {
            var dirs = new List<Direction>();
            var opDir = GetOppositeDir();

            int pX = mediator.GetPX() + (cellSize / 2);
            int pY = mediator.GetPY() + (cellSize / 2);
            int gX = this.position.x + (cellSize / 2);
            int gY = this.position.y + (cellSize / 2);

            // pacmam jest na południowy-wschód od ducha
            if (pX > gX && pY > gY)
            {
                if (pX - gX > pY - gY)
                {
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Up);
                }
                else
                {
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Left);
                }

                if (random.Next(2) == 0)
                {
                    dirs.Add(Direction.Right);
                    dirs.Add(Direction.Down);
                }
                else
                {
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Right);
                }

                dirs.Remove(opDir);
                return dirs;
            }

            // pacmam jest na południowy-zachód od ducha
            if (pX < gX && pY > gY)
            {
                if (gX - pX > pY - gY)
                {
                    dirs.Add(Direction.Right);
                    dirs.Add(Direction.Up);
                }
                else
                {
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Right);
                }

                if (random.Next(2) == 0)
                {
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Down);
                }
                else
                {
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Left);
                }

                dirs.Remove(opDir);
                return dirs;
            }

            // pacmam jest na północny-wschód od ducha
            if (pX > gX && pY < gY)
            {
                if (pX - gX > gY - pY)
                {
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Down);
                }
                else
                {
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Left);
                }

                if (random.Next(2) == 0)
                {
                    dirs.Add(Direction.Right);
                    dirs.Add(Direction.Up);
                }
                else
                {
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Right);
                }

                dirs.Remove(opDir);
                return dirs;
            }

            // pacmam jest na północny-zachód od ducha
            if (pX < gX && pY < gY)
            {
                if (gX - pX > gY - pY)
                {
                    dirs.Add(Direction.Right);
                    dirs.Add(Direction.Down);
                }
                else
                {
                    dirs.Add(Direction.Down);
                    dirs.Add(Direction.Right);
                }

                if (random.Next(2) == 0)
                {
                    dirs.Add(Direction.Left);
                    dirs.Add(Direction.Up);
                }
                else
                {
                    dirs.Add(Direction.Up);
                    dirs.Add(Direction.Left);
                }

                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na południe od ducha
            if (pX == gX && pY > gY)
            {
                dirs.Add(Direction.Up);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Up);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na północ od ducha
            if (pX == gX && pY < gY)
            {
                dirs.Add(Direction.Down);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Down);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na zachód od ducha
            if (pX < gX && pY == gY)
            {
                dirs.Add(Direction.Right);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Right);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na wschód od Blinky
            if (pX > gX && pY == gY)
            {
                dirs.Add(Direction.Left);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Left);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            dirs.Remove(opDir);
            return dirs.OrderBy(x => random.Next()).ToList();
        }

        protected bool HitPacman()
        {
            int pacmanStartHitX = mediator.GetPX() - 20;
            int pacmanEndHitX = mediator.GetPX() + 20;
            int pacmanStartHitY = mediator.GetPY() - 20;
            int pacmanEndHitY = mediator.GetPY() + 20;

            if (this.position.x >= pacmanStartHitX && this.position.x <= pacmanEndHitX 
                && this.position.y >= pacmanStartHitY && this.position.y <= pacmanEndHitY)
            {
                return true;
            }

            return false;
        }

        public bool PacmanHitLogic()
        {
            if (HitPacman())
            {
                if (!this.isPanicMode)
                {
                    mediator.Notify(this, EventType.LifeLoose);
                }
                return true;
            }

            return false;
        }

        public void SetPanicMode(bool panic)
        {
            isPanicMode = panic;
        }
    }
}
