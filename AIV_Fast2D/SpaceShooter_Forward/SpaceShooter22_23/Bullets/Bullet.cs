using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    enum BulletType { PlayerBullet, PurpleBullet, FireGlobe, LAST}

    abstract class Bullet : GameObject
    {

        protected int damage = 25;
        public BulletType Type { get; protected set; }

        public Bullet(string texturePath, int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base(texturePath, textOffsetX, textOffsetY, spriteW, spriteH)
        {
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            HalfWidth = (int)(sprite.Width * 0.5f);
            HalfHeight = (int)(sprite.Height * 0.5f);

            RigidBody = new RigidBody(this);
        }

        public virtual void Shoot(Vector2 shootPos, Vector2 shootVel, float rot)
        {
            Position = shootPos;
            RigidBody.Velocity = shootVel;
            //sprite.Rotation = rot;
            Forward = shootVel;
        }

        public virtual void Reset()
        {
            IsActive = true;
        }

    }
}
