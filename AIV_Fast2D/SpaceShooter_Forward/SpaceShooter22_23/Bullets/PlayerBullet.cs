using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter22_23
{
    class PlayerBullet : Bullet
    {
        
        public PlayerBullet():base("blueLaser")
        {
            RigidBody.Type = RigidBodyType.PlayerBullet;
            //RigidBody.AddCollisionType(RigidBodyType.Enemy);
            //RigidBody.AddCollisionType(RigidBodyType.EnemyBullet);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType((uint)RigidBodyType.Enemy | (uint)RigidBodyType.EnemyBullet);

            maxSpeed = 600.0f;
            RigidBody.Velocity.X = maxSpeed;

            Type = BulletType.PlayerBullet;
        }

        public override void Update()
        {
            if(IsActive)
            {
                if (sprite.position.X - sprite.pivot.X >= Game.Window.Width)
                {
                    BulletMngr.RestoreBullet(this);
                }
            }
        }

        public override void OnCollide(GameObject other)
        {
            if(other is Enemy)
            {
                Enemy enemy = ((Enemy)other);
                enemy.AddDamage(damage);

                if (!enemy.IsAlive)
                {
                    ((PlayScene)Game.CurrentScene).Player.AddScore(enemy.Points);
                }
            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
