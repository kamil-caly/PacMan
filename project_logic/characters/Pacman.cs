namespace project_logic.characters
{
    public class Pacman : Character
    {
        public int life { get; set; }
        public int score { get; set; }
        public static int highScore { get; set; }
        public Direction nextDirection { get; set; }

        public Pacman(int cellSize) : base(cellSize)
        {
            life = 3;
            score = 0;
            speed = 4; // 5
            SetStartPosition();
            direction = Direction.Up;
            nextDirection = direction;
        }

        public void TryChangeDirection()
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

        public void LifeLoose()
        {
            life--;
        }

        public void SetStartPosition()
        {
            position = new Point(9 * cellSize, 11 * cellSize);
        }
    }
}
