using System.Numerics;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using project_logic;
using project_logic.characters;

namespace project_gui
{
    public static class AssetsLoader
    {
        private const string PawnBlackUrl = "assets/PawnB.png";
        private const string PawnWhiteUrl = "assets/PawnW.png";
        private const string LadyBlackUrl = "assets/QueenB.png";
        private const string LadyWhiteUrl = "assets/QueenW.png";

        private static int _packmanDir = 1;
        private static int _packmanPhaze = 0;

        private static ImageSource GetPackmanImg()
        {
            return new BitmapImage(new Uri($"assets/Pacman {_packmanDir} {_packmanPhaze}.png", UriKind.Relative));
        }

        public static ImageSource GetBigBallImg()
        {
            return new BitmapImage(new Uri($"assets/Block 2.png", UriKind.Relative));
        }

        public static ImageSource GetSmallBallImg()
        {
            return new BitmapImage(new Uri($"assets/Block 1.png", UriKind.Relative));
        }

        public static ImageSource GetNextPackmanImg(Direction dir)
        {
            _packmanPhaze++;
            _packmanPhaze = _packmanPhaze % 4;
            switch (dir) 
            {
                case Direction.Up:
                    _packmanDir = 1;
                    break;
                case Direction.Down:
                    _packmanDir = 3;
                    break;
                case Direction.Right:
                    _packmanDir = 2;
                    break;
                case Direction.Left:
                    _packmanDir = 4;
                    break;
                default:
                    break;
            }

            return GetPackmanImg();
        }

        public static ImageSource GetNextGostImg(Direction dir, Ghost ghost)
        {
            return new BitmapImage(new Uri($"assets/Ghost {(int)ghost} {(int)dir + 1}.png", UriKind.Relative));
        }
    }
}
