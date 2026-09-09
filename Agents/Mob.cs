using Microsoft.Xna.Framework;

namespace game_1.Agents;

public abstract class Mob : Agent
{
    protected Mob(Vector2 initialPosition) : base(initialPosition) { }

    // Pixels travelled per second, so movement is independent of framerate.
    protected abstract float Speed { get; }

    public bool IsAlive { get; private set; } = true;

    public void Kill() => IsAlive = false;

    // Mobs walk themselves towards the target; the manager only says where it is.
    public virtual void Update(GameTime gameTime, Vector2 target)
    {
        float step = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 toTarget = target - Position;

        // Stop exactly on the target rather than overshooting and jittering around it.
        if (toTarget.LengthSquared() <= step * step)
        {
            Position = target;
            return;
        }

        Position += Vector2.Normalize(toTarget) * step;
    }
}
