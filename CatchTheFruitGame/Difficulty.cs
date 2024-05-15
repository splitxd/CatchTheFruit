
using Microsoft.Xna.Framework;

namespace CatchTheFruitGame
{
    internal class Difficulty
    {

        public static float fruitSpeedIncreasement = 10f;
        public static float fruitSpeedDecreasment = 20f;

        public static float spawnTimeIncreasement = 0.02f;
        public static float spawnTimeDecreasment = 0.04f;

        public static float plateSpeedIncreasement = 15f;
        public static float plateSpeedDecreasment = 30f;

        private double elapsedTime = 0;
        private const double interval = 10;

        public static void DifficultIncrease()
        {
            InteractionObjects.fruitSpeed += fruitSpeedIncreasement;
            if (InteractionObjects.fruitSpawnTime > 0.3)
                InteractionObjects.fruitSpawnTime -= spawnTimeDecreasment;
            Plate.DefaultSpeed += plateSpeedIncreasement;
            Plate.Speed += plateSpeedIncreasement;
        }

        public static void DifficultDecrease() 
        {
            if (InteractionObjects.fruitSpeed > fruitSpeedDecreasment && Plate.DefaultSpeed > plateSpeedDecreasment)
            {
                InteractionObjects.fruitSpeed -= fruitSpeedDecreasment;
                InteractionObjects.fruitSpawnTime += spawnTimeIncreasement;
                Plate.DefaultSpeed -= plateSpeedDecreasment;
                Plate.Speed -= plateSpeedDecreasment;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime >= interval)
            {
                DifficultIncrease();
                elapsedTime = 0;
            }
        }
    }
}
