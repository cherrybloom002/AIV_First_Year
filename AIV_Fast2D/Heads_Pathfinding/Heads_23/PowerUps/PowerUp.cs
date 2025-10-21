using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_23
{
    class PowerUp : GameObject
    {
        public PowerUp() : base("powerUp", DrawLayer.Middleground)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Enemy);

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            ((Actor)collisionInfo.Collider).AddEnergy(25);
            IsActive = false;
        }
    }
}
