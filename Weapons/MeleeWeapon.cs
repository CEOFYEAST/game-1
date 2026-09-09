using System;
using Microsoft.Xna.Framework;
using game_1.Collisions;

namespace game_1.Weapons;

// A short swing in front of the wielder. Always available, so it works as a
// last resort when something has closed the distance.
public class MeleeWeapon : Weapon
{
    protected override float Cooldown => 0.4f;

    // How far ahead of the wielder the centre of the strike sits.
    protected virtual float Reach => 40f;

    // How wide an area the strike covers.
    protected virtual float Radius => 26f;

    protected virtual int Damage => 1;

    // Returns the area the swing covers, or false while still on cooldown.
    public bool TryStrike(Vector2 origin, float aimRadians, out MeleeStrike strike)
    {
        if (!IsReady)
        {
            strike = default;
            return false;
        }

        MarkUsed();

        Vector2 direction = new(MathF.Cos(aimRadians), MathF.Sin(aimRadians));
        strike = new MeleeStrike(new BoundingCircle(origin + direction * Reach, Radius), Damage);
        return true;
    }
}
