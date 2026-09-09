using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game_1.Agents;

public class MobManager
{
    // Seconds between spawns. Every spawn is an equally likely Zombie or Tick.
    private const float SpawnInterval = 1f;

    private readonly List<Mob> _mobs = [];

    private readonly Random _random = new();

    private readonly GraphicsDevice _graphicsDevice;

    private ContentManager _content;

    private float _timeSinceLastSpawn = SpawnInterval;

    public IReadOnlyList<Mob> Mobs => _mobs;

    public MobManager(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public void LoadContent(ContentManager content)
    {
        _content = content;
    }

    // Clears the field so a new run starts with no mobs left over from the last.
    public void Reset()
    {
        _mobs.Clear();
        _timeSinceLastSpawn = SpawnInterval;
    }

    public void Update(GameTime gameTime, Vector2 playerPosition)
    {
        _timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // A loop rather than an if, so a long frame still owes the right number of spawns.
        while (_timeSinceLastSpawn >= SpawnInterval)
        {
            _timeSinceLastSpawn -= SpawnInterval;
            Spawn();
        }

        foreach (Mob mob in _mobs)
            mob.Update(gameTime, playerPosition);

        _mobs.RemoveAll(mob => !mob.IsAlive);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (Mob mob in _mobs)
            mob.Draw(gameTime, spriteBatch);
    }

    private void Spawn()
    {
        Mob mob = _random.Next(2) == 0
            ? new Zombie(Vector2.Zero)
            : new Tick(Vector2.Zero);

        // The texture has to be loaded before we know how far off-screen to place it.
        mob.LoadContent(_content);
        mob.Position = PointJustOffScreen(mob.DrawRadius);

        _mobs.Add(mob);
    }

    // Picks a point on the viewport rectangle grown by margin, uniformly by
    // distance along that perimeter. Choosing an edge first and then a point on
    // it would crowd the short edges, so instead one distance is drawn across
    // the whole loop and then walked around the corners.
    private Vector2 PointJustOffScreen(float margin)
    {
        Rectangle bounds = _graphicsDevice.Viewport.Bounds;

        float left = bounds.Left - margin;
        float top = bounds.Top - margin;
        float width = bounds.Width + margin * 2f;
        float height = bounds.Height + margin * 2f;

        float distance = (float)_random.NextDouble() * (width + height) * 2f;

        if (distance < width)
            return new Vector2(left + distance, top);

        distance -= width;
        if (distance < height)
            return new Vector2(left + width, top + distance);

        distance -= height;
        if (distance < width)
            return new Vector2(left + width - distance, top + height);

        distance -= width;
        return new Vector2(left, top + height - distance);
    }
}
