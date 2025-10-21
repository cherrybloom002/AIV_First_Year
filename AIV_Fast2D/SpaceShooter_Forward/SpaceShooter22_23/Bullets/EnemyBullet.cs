using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter22_23
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet(string texturePath, int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base(texturePath, textOffsetX, textOffsetY, spriteW, spriteH)
        {
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType((uint)RigidBodyType.Player | (uint)RigidBodyType.PlayerBullet);

            maxSpeed = -600.0f;
            RigidBody.Velocity.X = maxSpeed;
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (sprite.position.X + sprite.pivot.X < 0)
                {
                    BulletMngr.RestoreBullet(this);
                }
            }
        }

        //public override void Draw()
        //{
        //    if (IsActive)
        //    {
        //        sprite.DrawTexture(texture, 156, 227, (int)sprite.Width, (int)sprite.Height);
        //    }
        //}

        public override void OnCollide(GameObject other)
        {
            if (other is Player)
            {
                ((Player)other).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }

    }
}
