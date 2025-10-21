using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Aiv.Audio;

namespace Heads_23
{
    abstract class Actor : GameObject
    {
        // Variables
        protected Vector2 shootOffset;
        protected BulletType bulletType;
        protected ProgressBar nrgBar;

        protected int energy;
        protected int maxEnergy;

        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }
        public int MaxEnergy { get { return maxEnergy; } }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, textOffsetX:textOffsetX, textOffsetY:textOffsetY, spriteWidth: spriteWidth, spriteHeight: spriteHeight)
        {
            maxEnergy = 100;
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            float unitDist = Game.PixelsToUnits(4);
            nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            nrgBar.IsActive = true;
        }

        protected virtual void Shoot(Vector2 direction)
        {
            Bullet b = BulletMngr.GetBullet(bulletType);

            if (b != null)
            {
                b.Shooter = this;
                b.Shoot(sprite.position + shootOffset, direction.Normalized());
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void AddEnergy(int amount)
        {
            Energy = Math.Min(Energy + amount, maxEnergy);
        }

        public virtual void OnDie()
        {
            IsActive = false;
            nrgBar.IsActive = false;
        }
             

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }

        public override void Update()
        {
            if(IsActive && RigidBody.Velocity != Vector2.Zero)
            {
                Forward = RigidBody.Velocity;
            }

            nrgBar.Position = new Vector2(Position.X - nrgBar.HalfWidth, Position.Y - HalfHeight - nrgBar.HalfHeight * 3);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            OnTileCollide(collisionInfo);
        }

        private void OnTileCollide(Collision collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if (X < collisionInfo.Collider.X)
                {
                    // Left to Right
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                // Apply DeltaX
                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0.0f;
            }
            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Top to Bottom
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                }

                // Apply DeltaY
                Y += collisionInfo.Delta.Y;
                RigidBody.Velocity.Y = 0.0f;
            }
        }
    }
}
