using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Object
{
    public Texture2D Texture { get; private set; }
    public Vector2 Position { get; private set; }

    public Object(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
    }
}
