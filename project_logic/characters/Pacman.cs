using project_logic.characters.Mediator;

namespace project_logic.characters
{
    public class Pacman : Character
    {
        public int life { get; set; }
        public int score { get; set; } = 0;
        public static int highScore { get; set; }
        public Direction nextDirection { private get; set; }

        public Pacman(int cellSize, int life = 3, int speed = 4, Direction dir = Direction.Up) : base(cellSize)
        {
            this.life = life;
            this.speed = speed; // 5
            SetStartPosition();
            direction = dir;
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
                    mediator.Notify(this, EventType.EatedBigBall);
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

        public override void SetStartPosition()
        {
            position = new Point(9 * cellSize, 11 * cellSize);
        }
    }
}
