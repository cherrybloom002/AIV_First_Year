using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{
    class PlayerBullet : Bullet
    {
        
        public PlayerBullet():base("bullet")
        {
            Type = BulletType.PlayerBullet;

            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType((uint)RigidBodyType.Enemy | (uint)RigidBodyType.EnemyBullet);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Enemy)
            {
                Enemy enemy = ((Enemy)collisionInfo.Collider);
                enemy.AddDamage(damage);

            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
