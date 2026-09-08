using Microsoft.Xna.Framework;

namespace game_1;

public abstract class Mob : Agent
{
    protected Mob(Vector2 initialPosition) : base(initialPosition) { }
}
