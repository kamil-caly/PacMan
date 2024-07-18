namespace project_logic.characters
{
    public class Packman : Character
    {
        public int healf { get; set; }
        public int speed { get; set; }
        public Point position { get; set; }
        public Direction direction { get; set; }

        public Packman(int cellSize) : base(cellSize)
        {
            healf = 3;
            speed = 10;
            position = new Point(10 * cellSize, 10 * cellSize);
            direction = Direction.Up;
        }
    }
}
