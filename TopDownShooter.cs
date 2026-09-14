using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using game_1.Agents;
using game_1.Projectiles;
using game_1.Weapons;

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

    private ProjectileManager _projectileManager;

    private GameState _state = GameState.Start;

    private string _prompt;

    private Vector2 _promptPosition;

    // Kept so firing and swinging trigger on the press, not every frame held.
    private KeyboardState _previousKeyboard;

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
        _projectileManager = new();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Bangers");
        _player.LoadContent(Content);
        _mobManager.LoadContent(Content);
        _projectileManager.LoadContent(Content);

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
                UpdatePlaying(gameTime, keyboard);
                break;

            // GameState.GameOver waits on the Escape check above.
        }

        _previousKeyboard = keyboard;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        if (_state == GameState.Playing)
        {
            _mobManager.Draw(gameTime, _spriteBatch);
            _projectileManager.Draw(gameTime, _spriteBatch);
        }
        else
            _spriteBatch.DrawString(_font, _prompt, _promptPosition, Color.White);

        // The player is on screen in every state.
        _player.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdatePlaying(GameTime gameTime, KeyboardState keyboard)
    {
        _player.AimAt(Mouse.GetState().Position.ToVector2());
        _player.Update(gameTime);

        HandleWeapons(keyboard);
        ResolveProjectileHits();

        // Settle contacts before the manager moves and prunes, so a mob killed
        // here is gone from the list before it can be drawn again.
        foreach (Mob mob in _mobManager.Mobs)
        {
            // Skip anything already killed this frame, so cutting a mob down as
            // it closes in does not still cost a life.
            if (!mob.IsAlive || !mob.Bounds.CollidesWith(_player.Bounds)) continue;

            mob.Kill();
            _player.Touch();
        }

        if (!_player.IsAlive)
        {
            FinishRun();
            return;
        }

        _mobManager.Update(gameTime, _player.Position);
        _projectileManager.Update(gameTime, GraphicsDevice.Viewport.Bounds);
    }

    // Both weapons fire on the press rather than while held, so one tap is one shot.
    private void HandleWeapons(KeyboardState keyboard)
    {
        if (WasJustPressed(keyboard, Keys.Space))
            _player.TryFire(_projectileManager);

        if (WasJustPressed(keyboard, Keys.Q) && _player.TryMelee(out MeleeStrike strike))
        {
            foreach (Mob mob in _mobManager.Mobs)
            {
                if (strike.Area.CollidesWith(mob.Bounds))
                    mob.TakeDamage(strike.Damage);
            }
        }
    }

    private void ResolveProjectileHits()
    {
        foreach (Projectile projectile in _projectileManager.Projectiles)
        {
            if (!projectile.IsAlive) continue;

            foreach (Mob mob in _mobManager.Mobs)
            {
                if (!mob.IsAlive || !projectile.Bounds.CollidesWith(mob.Bounds)) continue;

                mob.TakeDamage(projectile.Damage);

                // A round is spent on the first thing it hits.
                projectile.Kill();
                break;
            }
        }
    }

    private bool WasJustPressed(KeyboardState keyboard, Keys key) =>
        keyboard.IsKeyDown(key) && _previousKeyboard.IsKeyUp(key);

    private void StartRun()
    {
        _state = GameState.Playing;
        _prompt = null;
        _player.Revive();
        _player.Position = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
        _mobManager.Reset();
        _projectileManager.Reset();
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
