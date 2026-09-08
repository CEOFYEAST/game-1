using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game_1;

public class Player : Agent
{
    public Player(Vector2 initialPosition) : base(initialPosition) { }

    protected override float Scale => 0.5f;

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("PlayerPlaceholder");
    }

    // protected void Update(GameTime gameTime)
    // {
    //     throw new NotImplementedException();
    // }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: _texture,
            position: Position,
            sourceRectangle: null,
            color: Color,
            rotation: 0f,
            origin: new Vector2(_texture.Width / 2f, _texture.Height / 2f),
            scale: Scale,
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }
}
