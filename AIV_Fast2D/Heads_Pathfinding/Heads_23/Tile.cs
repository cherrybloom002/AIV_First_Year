using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_23
{
    class Tile : GameObject
    {
        public Tile() : base("tile", spriteWidth: 1, spriteHeight: 1)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            IsActive = true;

            DrawMngr.AddItem(this);
            DebugMngr.AddItem(RigidBody.Collider);
        }
    }
}
