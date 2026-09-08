using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game_1;

public class Tick : Mob
{
    public Tick(Vector2 initialPosition) : base(initialPosition) { }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("EnemyPlaceholder");
    }

    // protected void Update(GameTime gameTime)
    // {
    //     throw new NotImplementedException();
    // }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: _texture,
            position: _position,
            sourceRectangle: null,
            color: Color.White,
            rotation: 0f,
            origin: new Vector2(_texture.Width / 2f, _texture.Height / 2f),
            scale: 0.15f,
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }
}