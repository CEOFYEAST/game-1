using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game_1;

public abstract class Agent
{
    protected Vector2 _position;

    protected Texture2D _texture;

    protected Agent(Vector2 initialPosition)
    {
        _position = initialPosition;
    }

    public abstract void LoadContent(ContentManager content);

    // protected void Update(GameTime gameTime)
    // {
    //     throw new NotImplementedException();
    // }

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}