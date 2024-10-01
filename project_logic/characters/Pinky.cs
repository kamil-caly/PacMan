namespace project_logic.characters
{
    public class Pinky : Ghost
    {
        public Pinky(int cellSize, Pacman pacman) : base(cellSize, pacman)
        {
            kind = GhostKind.Pinky;
            steps = 0;
            speed = 5; // 6
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
            position = new Point(8 * cellSize, 9 * cellSize);
        }

        protected override IEnumerable<Direction> GetDirections()
        {
            var dirs = new List<Direction>();
            var opDir = GetOppositeDir();
            int criticalDistance = 8 * cellSize;

            int pX = pacman.position.x + (cellSize / 2);
            int pY = pacman.position.y + (cellSize / 2);
            int ppX = this.position.x + (cellSize / 2);
            int ppY = this.position.y + (cellSize / 2);

            int packmanBlinkyDistance = (int)Math.Sqrt(Math.Pow(pX - ppX, 2) + Math.Pow(pY - ppY, 2));

            // algorytm działa tylko w odległości 8 kratek i mniejszej
            if (packmanBlinkyDistance > criticalDistance)
            {
                dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                dirs.Remove(opDir);
                return dirs.OrderBy(x => random.Next()).ToList();
            }

            switch (pacman.direction)
            {
                case Direction.Left:
                    pX -= 4 * cellSize;
                    break;
                case Direction.Right:
                    pX += 4 * cellSize;
                    break;
                case Direction.Up:
                    pY -= 4 * cellSize;
                    break;
                case Direction.Down:
                    pY += 4 * cellSize;
                    break;
                default:
                    break;
            }

            // pole przed pacman'em jest na południowy-wschód od Pinky
            if (pX > ppX && pY > ppY)
            {
                if (pX - ppX > pY - ppY)
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

            // pole przed pacman'em jest na południowy-zachód od Pinky
            if (pX < ppX && pY > ppY)
            {
                if (ppX - pX > pY - ppY)
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

            // pole przed pacman'em jest na północny-wschód od Pinky
            if (pX > ppX && pY < ppY)
            {
                if (pX - ppX > ppY - pY)
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

            // pole przed pacman'em jest na północny-zachód od Pinky
            if (pX < ppX && pY < ppY)
            {
                if (ppX - pX > ppY - pY)
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

            // pole przed pacman'em jest na południe od Pinky
            if (pX == ppX && pY > ppY)
            {
                dirs.Add(Direction.Down);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Down);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // pole przed pacman'em jest na północ od Pinky
            if (pX == ppX && pY < ppY)
            {
                dirs.Add(Direction.Up);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Up);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // pole przed pacman'em jest na zachód od Pinky
            if (pX < ppX && pY == ppY)
            {
                dirs.Add(Direction.Left);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Left);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // pole przed pacman'em jest na wschód od Pinky
            if (pX > ppX && pY == ppY)
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
