using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTerra.World.Components
{
    /// <summary>
    /// Abstract base class for all Projectiles.
    /// </summary>
    public abstract class Projectile : WorldComponent
    {
        /// <summary>
        /// Gets the effect this projectile has on the target's health.
        /// <para/>
        /// A positive value means that the target will be healed,
        /// a negative value means that the target will be damaged.
        /// </summary>
        public abstract int HealthEffect { get; }

        /// <summary>
        /// Gets called when another <see cref="WorldComponent"/> collides with this one.
        /// The other one will be moving.
        /// </summary>
        /// <param name="other">The other <see cref="WorldComponent"/>.</param>
        public override void Collide(WorldComponent other)
        {
            // Do nothing.
        }
    }
}