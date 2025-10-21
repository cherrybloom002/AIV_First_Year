using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet(int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base("bullet", textOffsetX, textOffsetY, spriteW, spriteH)
        {
            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType((uint)RigidBodyType.Player | (uint)RigidBodyType.PlayerBullet);

            sprite.SetAdditiveTint(200.0f, 0.0f, 0.0f, 0.0f);

            damage = 5;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }

    }
}
