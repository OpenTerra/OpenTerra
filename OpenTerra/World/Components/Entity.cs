using BananaUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTerra.World.Components
{
    /// <summary>
    /// Abstract base class for all Entities (mobs, player, NPCs, etc.).
    /// </summary>
    public abstract class Entity : WorldComponent
    {
        /// <summary>
        /// Gets the Entity's health.
        /// </summary>
        public int Health { get; protected set; }

        public override void Collide(WorldComponent other)
        {
            if (other.Is(typeof(Projectile)))
                Health += ((Projectile)other).HealthEffect;

            // Check others?
        }
    }
}