using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game_1;

public class Player : Agent
{
    // Enemy touches a player survives; the third one kills them.
    private const int MaxTouches = 3;

    public Player(Vector2 initialPosition) : base(initialPosition) { }

    protected override float Scale => 0.5f;

    // Touches left before the player dies.
    public int RemainingTouches { get; private set; } = MaxTouches;

    public bool IsAlive => RemainingTouches > 0;

    public void Touch()
    {
        if (RemainingTouches > 0) RemainingTouches--;
    }

    // Puts the player back to full health for a fresh run.
    public void Revive()
    {
        RemainingTouches = MaxTouches;
    }

    public override void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("PlayerPlaceholder");
    }

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
