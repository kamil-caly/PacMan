namespace project_logic.characters
{
    public class Inky : Ghost
    {
        public Inky(int cellSize, int panicModeTimeS = 5, bool isPanicMode = false, int speed = 5) : base(cellSize, panicModeTimeS, isPanicMode)
        {
            kind = GhostKind.Inky;
            steps = 0;
            this.speed = speed; 
            SetStartPosition();

            while (true)
            {
                direction = GetRandomDir();
                if (this.CanMove())
                {
                    break;
                }
            }
        }

        public override void SetStartPosition()
        {
            position = new Point(10 * cellSize - cellSize / 2, 9 * cellSize);
        }

        protected override IEnumerable<Direction> GetDirections()
        {
            var dirs = new List<Direction>();
            var opDir = GetOppositeDir();
            int criticalDistance = 12 * cellSize;

            int pX = mediator.GetPX() + (cellSize / 2);
            int pY = mediator.GetPY() + (cellSize / 2);
            int iX = this.position.x + (cellSize / 2);
            int iY = this.position.y + (cellSize / 2);

            int packmanInkyDistance = (int)Math.Sqrt(Math.Pow(pX - iX, 2) + Math.Pow(pY - iY, 2));

            // algorytm działa tylko w odległości 12 kratek i mniejszej
            if (packmanInkyDistance > criticalDistance)
            {
                dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                dirs.Remove(opDir);
                return dirs.OrderBy(x => random.Next()).ToList();
            }

            int bX = mediator.GetBlinkyX() + (cellSize / 2);
            int bY = mediator.GetBlinkyY() + (cellSize / 2);

            int targetX = (int)(pX + 0.5 * (bX - pX));
            int targetY = (int)(pY + 0.5 * (bY - pY));

            // cel jest na południowy-wschód od Inky
            if (targetX > iX && targetY > iY)
            {
                if (targetX - iX > targetY - iY)
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

            // cel jest na południowy-zachód od Inky
            if (targetX < iX && targetY > bY)
            {
                if (iX - targetX > targetY - bY)
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

            // cel jest na północny-wschód od Inky
            if (targetX > iX && targetY < iY)
            {
                if (targetX - iX > iY - targetY)
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

            // cel jest na północny-zachód od Inky
            if (targetX < iX && targetY < iY)
            {
                if (iX - targetX > iY - targetY)
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

            // cel jest na południe od Inky
            if (targetX == iX && targetY > iY)
            {
                dirs.Add(Direction.Down);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Down);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // cel jest na północ od Inky
            if (targetX == iX && targetY < iY)
            {
                dirs.Add(Direction.Up);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Up);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // cel jest na zachód od Inky
            if (targetX < iX && targetY == iY)
            {
                dirs.Add(Direction.Left);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Left);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // cel jest na wschód od Inky
            if (targetX > iX && targetY == iY)
            {
                dirs.Add(Direction.Right);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Right);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            dirs.Remove(opDir);
            return dirs.OrderBy(x => random.Next()).ToList();
        }
    }
}
