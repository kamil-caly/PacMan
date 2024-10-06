using System.ComponentModel;

namespace project_logic.characters.Mediator
{
    public class RegisterCharacters : IMediator
    {
        private Pacman pacman;
        private List<Ghost> ghosts;

        public RegisterCharacters(Pacman pacman, List<Ghost> ghosts)
        {
            this.pacman = pacman;
            this.ghosts = ghosts;

            this.pacman.SetMediator(this);
            this.ghosts.ForEach(g => g.SetMediator(this));
        }

        public int GetBlinkyX()
        {
            return this.ghosts.Where(g => g.kind == GhostKind.Blinky).First().position.x;
        }

        public int GetBlinkyY()
        {
            return this.ghosts.Where(g => g.kind == GhostKind.Blinky).First().position.y;
        }

        public Direction GetPDir()
        {
            return this.pacman.direction;
        }

        public int GetPX()
        {
            return this.pacman.position.x;
        }

        public int GetPY()
        {
            return this.pacman.position.y;
        }

        public void Notify(Character sender, EventType @event)
        {
            switch (@event)
            {
                case EventType.LifeLoose:
                    this.pacman.LifeLoose();
                    break;
                case EventType.EatedBigBall:
                    this.ghosts.ForEach(g => g.SetPanicMode(true));
                    break;
                default:
                    break;
            }
        }
    }
}
