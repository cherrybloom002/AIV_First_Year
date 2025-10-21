using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    class PurpleBullet : EnemyBullet
    {
        public PurpleBullet() : base("purpleLaser", 156, 227, 74, 46)
        {
            Type = BulletType.PurpleBullet;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
        }
    }
}
