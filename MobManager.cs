using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game_1;

public class MobManager
{
    private readonly List<Mob> _mobs = [];

    private ContentManager _content;

    public MobManager()
    {
        Add(new Zombie(new Vector2(120f, 120f)));
        Add(new Zombie(new Vector2(300f, 300f)));
        Add(new Tick(new Vector2(50f, 50f)));
        Add(new Tick(new Vector2(80f, 80f)));
    }

    public void Add(Mob mob)
    {
        // Mobs spawned after LoadContent still need their texture.
        if (_content != null)
            mob.LoadContent(_content);
        _mobs.Add(mob);
    }

    public void LoadContent(ContentManager content)
    {
        _content = content;
        foreach (Mob mob in _mobs)
            mob.LoadContent(content);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (Mob mob in _mobs)
            mob.Draw(gameTime, spriteBatch);
    }
}
