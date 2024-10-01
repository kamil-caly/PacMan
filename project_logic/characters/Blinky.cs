using System.ComponentModel;

namespace project_logic.characters
{
    public class Blinky : Ghost
    {
        public Blinky(int cellSize, Pacman pacman) : base(cellSize, pacman)
        {
            kind = GhostKind.Blinky;
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
            position = new Point(10 * cellSize, 9 * cellSize);
        }

        protected override IEnumerable<Direction> GetDirections()
        {
            var dirs = new List<Direction>();
            var opDir = GetOppositeDir();
            int criticalDistance = 8 * cellSize;

            int pX = pacman.position.x + (cellSize / 2);
            int pY = pacman.position.y + (cellSize / 2);
            int bX = this.position.x + (cellSize / 2);
            int bY = this.position.y + (cellSize / 2);

            int packmanBlinkyDistance = (int)Math.Sqrt(Math.Pow(pX - bX, 2) + Math.Pow(pY - bY, 2));

            // algorytm działa tylko w odległości 8 kratek i mniejszej
            if (packmanBlinkyDistance > criticalDistance)
            {
                dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                dirs.Remove(opDir);
                return dirs.OrderBy(x => random.Next()).ToList();
            }

            // pacmam jest na południowy-wschód od Blinky
            if (pX > bX && pY > bY)
            {
                if (pX - bX > pY - bY)
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

            // pacmam jest na południowy-zachód od Blinky
            if (pX < bX && pY > bY)
            {
                if (bX - pX > pY - bY)
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

            // pacmam jest na północny-wschód od Blinky
            if (pX > bX && pY < bY)
            {
                if (pX - bX > bY - pY)
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

            // pacmam jest na północny-zachód od Blinky
            if (pX < bX && pY < bY)
            {
                if (bX - pX > bY - pY)
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

            // packman jest na południe od Blinky
            if (pX == bX && pY > bY)
            {
                dirs.Add(Direction.Down);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Down);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na północ od Blinky
            if (pX == bX && pY < bY)
            {
                dirs.Add(Direction.Up);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Up);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na zachód od Blinky
            if (pX < bX && pY == bY)
            {
                dirs.Add(Direction.Left);
                var tmpDirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
                tmpDirs.Remove(Direction.Left);
                tmpDirs = tmpDirs.OrderBy(x => random.Next()).ToList();
                dirs.AddRange(tmpDirs);
                dirs.Remove(opDir);
                return dirs;
            }

            // packman jest na wschód od Blinky
            if (pX > bX && pY == bY)
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
