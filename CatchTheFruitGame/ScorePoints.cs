using Microsoft.Xna.Framework.Media;

namespace CatchTheFruitGame
{

    internal class ScorePoints
    {
        private static int previous = 0;
        private static bool isFirstTen = true;
        private static bool isFirtstHundred = true;
        private static bool isFirstTousand = true;
        private static Song thousenPoints;
        private static Song hundretPoints;
        private static Song tenPoints;
        public  static int backRectWidth = 152;
        public  static int backRectHeight = 46;

        public void LoadSounds(Song ThousenPoints, Song HundretPoints, Song TenPoints)
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
                    MediaPlayer.Play(thousenPoints);
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
                    MediaPlayer.Play(hundretPoints);
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
                    MediaPlayer.Play(tenPoints);
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
