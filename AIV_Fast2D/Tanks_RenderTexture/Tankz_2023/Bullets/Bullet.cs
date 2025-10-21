using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    enum BulletType { StdBullet, Rocket, LAST}

    abstract class Bullet : GameObject
    {

        protected int damage = 25;
        protected float maxSpeed = 10.0f;
        protected SoundEmitter shootSound;
        public BulletType Type { get; protected set; }

        public Bullet(string texturePath, int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base(texturePath, DrawLayer.Foreground, textOffsetX, textOffsetY, spriteW, spriteH)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.IsGravityAffected = true;
        }

        public virtual void Shoot(Vector2 shootPos, Vector2 shootDir)
        {
            Position = shootPos;
            RigidBody.Velocity = shootDir * maxSpeed;
            shootSound.Play(shootDir.Length);
        }

        public virtual void Reset()
        {
            IsActive = true;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 cameraDist = Position - CameraMgr.MainCamera.position;//vector from camera to bullet

                if (cameraDist.LengthSquared > CameraMgr.HalfDiagonalSquared)
                {
                    BulletMngr.RestoreBullet(this);
                    return;
                }

                if(RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);

            Explosion exp = (Explosion)GfxMngr.GetSpecialFX(SpecialFX.Explosion_1);

            if (exp != null)
            {
                exp.Position = this.Position;
                exp.Play();
            }
        }
    }
}
