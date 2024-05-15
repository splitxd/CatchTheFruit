using Microsoft.Xna.Framework.Audio;

namespace CatchTheFruitGame
{

    internal class ScorePoints
    {
        private static int previous = 0;
        private static bool isFirstTen = true;
        private static bool isFirtstHundred = true;
        private static bool isFirstTousand = true;
        private static SoundEffect thousenPoints;
        private static SoundEffect hundretPoints;
        private static SoundEffect tenPoints;
        public  static int backRectWidth = 152;
        public  static int backRectHeight = 46;

        public void Initialize(SoundEffect ThousenPoints, SoundEffect HundretPoints, SoundEffect TenPoints)
        {
            thousenPoints = ThousenPoints;
            hundretPoints = HundretPoints;
            tenPoints = TenPoints;
        }

        public static void ScoreSounds(int score)
        {
            if (score % 1000 == 0 && score != 0)
            {
                if (previous < score)
                {
                    thousenPoints.Play();
                    previous = score;
                }
                if (isFirstTousand)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirstTousand = false;
                }
            }
            else if (score % 100 == 0 && score != 0)
            {
                if (previous < score)
                {
                    hundretPoints.Play();
                    previous = score;
                }
                if (isFirtstHundred)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirtstHundred = false;
                }
            }
            else if (score % 10 == 0 && score != 0)
            {
                if (previous < score)
                {
                    tenPoints.Play();
                    previous = score;
                }
                if (isFirstTen)
                {
                    var temp = backRectWidth + 20;
                    backRectWidth = temp;
                    isFirstTen = false;
                }
            }
        }
    }
}
