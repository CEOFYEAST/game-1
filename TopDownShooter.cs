using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game_1;

public class TopDownShooter : Game
{
    private const string StartMessage = "Press Enter to start";

    private const string GameOverMessage = "Game over - press Esc to exit";

    // Vertical space between a prompt and the player sprite under it.
    private const float PromptGap = 24f;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SpriteFont _font;

    private Player _player;

    private MobManager _mobManager;

    private GameState _state = GameState.Start;

    private string _prompt;

    private Vector2 _promptPosition;

    public TopDownShooter()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _player = new(
            GraphicsDevice.Viewport.Bounds.Center.ToVector2()
        );
        _mobManager = new(GraphicsDevice);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Bangers");
        _player.LoadContent(Content);
        _mobManager.LoadContent(Content);

        // Laying out the title screen needs the font and the player texture.
        ShowPrompt(StartMessage);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        switch (_state)
        {
            case GameState.Start:
                if (keyboard.IsKeyDown(Keys.Enter)) StartRun();
                break;

            case GameState.Playing:
                UpdatePlaying(gameTime);
                break;

            // GameState.GameOver waits on the Escape check above.
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        if (_state == GameState.Playing)
            _mobManager.Draw(gameTime, _spriteBatch);
        else
            _spriteBatch.DrawString(_font, _prompt, _promptPosition, Color.White);

        // The player is on screen in every state.
        _player.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdatePlaying(GameTime gameTime)
    {
        // Settle contacts before the manager moves and prunes, so a mob killed
        // here is gone from the list before it can be drawn again.
        foreach (Mob mob in _mobManager.Mobs)
        {
            if (!mob.Bounds.CollidesWith(_player.Bounds)) continue;

            mob.Kill();
            _player.Touch();
        }

        if (!_player.IsAlive)
        {
            FinishRun();
            return;
        }

        _mobManager.Update(gameTime, _player.Position);
    }

    private void StartRun()
    {
        _state = GameState.Playing;
        _prompt = null;
        _player.Revive();
        _player.Position = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
        _mobManager.Reset();
    }

    private void FinishRun()
    {
        _state = GameState.GameOver;
        ShowPrompt(GameOverMessage);
    }

    // Centres the message and the player sprite together, message on top.
    private void ShowPrompt(string message)
    {
        Vector2 center = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
        Vector2 textSize = _font.MeasureString(message);
        float blockHeight = textSize.Y + PromptGap + _player.DrawRadius * 2f;
        float top = center.Y - blockHeight / 2f;

        _prompt = message;
        _promptPosition = new Vector2(center.X - textSize.X / 2f, top);
        _player.Position = new Vector2(center.X, top + textSize.Y + PromptGap + _player.DrawRadius);
    }
}
