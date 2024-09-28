namespace project_logic.characters
{
    public class Pacman : Character
    {
        public int healf { get; set; }
        public int score { get; set; }
        public static int highScore { get; set; }   

        public Pacman(int cellSize) : base(cellSize)
        {
            healf = 3;
            score = 0;
            speed = 5;
            position = new Point(9 * cellSize, 11 * cellSize);
            direction = Direction.Up;
            nextDirection = direction;
        }

        public override void TryChangeDirection()
        {
            var prevDir = direction;
            direction = nextDirection;
            if (!CanMove())
            {
                direction = prevDir;
            }

            // uniemożliwiamy wchodzenie do miejsca startowego duchow
            if (direction == Direction.Down && this.position.x == 360 && this.position.y == 280)
            {
                direction = prevDir;
            }
        }

        public override void Move()
        {
            base.Move();
            steps++;
            steps = steps % 10;
        }

        public void UpdatePoints(ScoreType type)
        {
            switch(type)
            {
                case ScoreType.BigBall:
                    score += 50;
                    break;
                case ScoreType.SmallBall:
                    score += 10;
                    break;
                case ScoreType.Ghost:
                    score += 200;
                    break;
                default:
                    break;
            }

            if (score > highScore)
            {
                highScore = score;
            }
        }
    }
}
