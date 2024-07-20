namespace project_logic.characters
{
    public class Pacman : Character
    {
        public int healf { get; set; }
        public int steps {  get; set; }

        public Pacman(int cellSize) : base(cellSize)
        {
            healf = 3;
            steps = 0;
            speed = 10;
            position = new Point(9 * cellSize, 11 * cellSize);
            direction = Direction.Up;
            nextDirection = direction;
        }

        public override void Move()
        {
            base.Move();
            steps++;
            steps = steps % 10;
        }
    }
}
