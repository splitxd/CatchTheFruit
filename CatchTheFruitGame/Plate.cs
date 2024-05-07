using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CatchTheFruitGame
{
    public class Plate
    {
        public Vector2 Position = Vector2.Zero;
        private int ScreenWidth;
        private int ScreenHeight;
        public float Speed = 300f;
        public float DefaultSpeed = 300f;
        public int Size = 100;


        public Plate(int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Position = new Vector2((screenWidth - Size) / 2, screenHeight - Size);
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.A) || kstate.IsKeyDown(Keys.Left))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    Speed = DefaultSpeed * 1.5f;
                else
                    Speed = DefaultSpeed;
                Position.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.D) || kstate.IsKeyDown(Keys.Right))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    Speed = DefaultSpeed * 1.5f;
                else
                    Speed = DefaultSpeed;
                Position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Position.X <= 0)
                Position.X = 0;
            else if (Position.X >= ScreenWidth - Size)
                Position.X = ScreenWidth - Size;
        }
    }
}
