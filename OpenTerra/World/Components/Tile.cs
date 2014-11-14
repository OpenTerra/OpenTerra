using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTerra.World.Components
{
    /// <summary>
    /// Abstract base class for all Tiles.
    /// </summary>
    public abstract class Tile : WorldComponent
    {
        /// <summary>
        /// Gets the Light Level of the Tile.
        /// <para/>
        /// A negative value means that light gets less when it moves through it,
        /// while a positive value means that light is produced by it.
        /// </summary>
        public virtual sbyte LightLevel
        {
            get { return 0; }
        }

        #region Make different Tile-Instances compare the same

        public static bool operator !=(Tile left, Tile right)
        {
            return !(left == right);
        }

        public static bool operator ==(Tile left, Tile right)
        {
            if (left == null && right != null)
                return false;

            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        #endregion Make different Tile-Instances compare the same

        /// <summary>
        /// Gets called when another <see cref="WorldComponent"/> collides with this one.
        /// The other one will be moving.
        /// </summary>
        /// <param name="other">The other <see cref="WorldComponent"/>.</param>
        public override void Collide(WorldComponent collider)
        {
            // Handle different colliding things.
        }
    }
}