using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    class Enemy01 : Enemy
    {
        public Enemy01() : base("enemy")
        {
            Type = EnemyType.Enemy01;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            shootOffset = new Vector2(-sprite.pivot.X, sprite.pivot.Y * 0.5f);

            int r = RandomGenerator.GetRandomInt(0, 2);

            if (r == 0)
            {
                bulletType = BulletType.FireGlobe;
            }
            else
            {
                bulletType = BulletType.PurpleBullet;
            }

            Points = 25;
        }
    }
}
