using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game_1;

public abstract class Agent
{
    protected Texture2D _texture;

    protected Agent(Vector2 initialPosition)
    {
        Position = initialPosition;
    }

    // Centre of the sprite, in screen space.
    public Vector2 Position { get; set; }

    // Size multiplier applied to the texture when drawing.
    protected abstract float Scale { get; }

    // How far the drawn sprite reaches from Position in any direction.
    public float Radius =>
        _texture is null ? 0f : MathF.Max(_texture.Width, _texture.Height) * Scale / 2f;

    public abstract void LoadContent(ContentManager content);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
