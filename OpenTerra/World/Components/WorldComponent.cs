using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTerra.World.Components
{
    /// <summary>
    /// The base class for all parts of the world that get rendered and/or saved.
    /// </summary>
    public abstract class WorldComponent
    {
        /// <summary>
        /// Gets called when another <see cref="WorldComponent"/> collides with this one.
        /// The other one will be moving.
        /// </summary>
        /// <param name="other">The other <see cref="WorldComponent"/>.</param>
        public abstract void Collide(WorldComponent other);
    }
}