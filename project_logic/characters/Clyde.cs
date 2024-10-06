namespace project_logic.characters
{
    public class Clyde : Ghost
    {
        public Clyde(int cellSize, int panicModeTimeS = 5, bool isPanicMode = false, int speed = 5) : base(cellSize, panicModeTimeS, isPanicMode)
        {
            kind = GhostKind.Clyde;
            steps = 0;
            this.speed = speed; // 6
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
            position = new Point(9 * cellSize, 9 * cellSize);
        }

        protected override IEnumerable<Direction> GetDirections()
        {
            var dirs = new List<Direction>();
            var opDir = GetOppositeDir();
            int criticalDistance = 6 * cellSize;

            int pX = mediator.GetPX() + (cellSize / 2);
            int pY = mediator.GetPY() + (cellSize / 2);
            int cX = this.position.x + (cellSize / 2);
            int cY = this.position.y + (cellSize / 2);

            int packmanClydeDistance = (int)Math.Sqrt(Math.Pow(pX - cX, 2) + Math.Pow(pY - cY, 2));

            // algorytm działa tylko w odległości większej niż 6 kratek
            if (packmanClydeDistance <= criticalDistance)
            {
                dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                dirs.Remove(opDir);
                return dirs.OrderBy(x => random.Next()).ToList();
            }

            // pacmam jest na południowy-wschód od Clyde
            if (pX > cX && pY > cY)
            {
                if (pX - cX > pY - cY)
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

            // pacmam jest na południowy-zachód od Clyde
            if (pX < cX && pY > cY)
            {
                if (cX - pX > pY - cY)
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

            // pacmam jest na północny-wschód od Clyde
            if (pX > cX && pY < cY)
            {
                if (pX - cX > cY - pY)
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

            // pacmam jest na północny-zachód od Clyde
            if (pX < cX && pY < cY)
            {
                if (cX - pX > cY - pY)
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

            // packman jest na południe od Clyde
            if (pX == cX && pY > cY)
            {
                dirs.Add(Direction.Down);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Down);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na północ od Clyde
            if (pX == cX && pY < cY)
            {
                dirs.Add(Direction.Up);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Up);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na zachód od Clyde
            if (pX < cX && pY == cY)
            {
                dirs.Add(Direction.Left);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Left);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na wschód od Clyde
            if (pX > cX && pY == cY)
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
