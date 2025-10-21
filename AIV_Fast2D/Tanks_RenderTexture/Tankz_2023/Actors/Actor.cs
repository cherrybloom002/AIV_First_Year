using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Aiv.Audio;

namespace Tankz_2023
{
    enum WeaponType
    {
        Default,
        TripleShoot
    }

    abstract class Actor : Groundable
    {
        // References
        protected ProgressBar energyBar;
        public Bullet LastShotBullet { get; protected set; }

        // Variables
        protected BulletType bulletType;
        protected int energy;
        protected float maxSpeed;
        protected int maxEnergy;
        protected WeaponType weaponType;
        //protected AudioSource soundEmitter;
        //protected AudioClip shootSound;

        // Properties
        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }
        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }
        public bool IsJumping { get { return RigidBody.Velocity.Y < 0; } }
        public bool IsFalling { get { return RigidBody.Velocity.Y > 0; } }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, textOffsetX:textOffsetX, textOffsetY:textOffsetY, w: spriteWidth, h: spriteHeight)
        {
            //RigidBody = new RigidBody(this);
            maxEnergy = 100;

            float unitDist = Game.PixelsToUnits(4);
            energyBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            energyBar.IsActive = true;
            energyBar.Position = new Vector2(0.1f, 0.1f);
        }

        public void ChangeWeapon(WeaponType weapon)
        {
            weaponType = weapon;
        }

        protected virtual void Shoot(Vector2 velocity, Vector2 position)
        {
            Bullet b = BulletMngr.GetBullet(bulletType);

            if (b != null)
            {
                b.IsActive = true;
                position += velocity.Normalized() * (b.HalfWidth);
                b.Shoot(position, velocity);
            }

            LastShotBullet = b;
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void OnDie()
        {
            
        }
             

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }

        public override void Update()
        {
            float groundY = ((PlayScene)Game.CurrentScene).GroundY;

            if(Position.Y + HalfHeight > groundY)
            {
                sprite.position.Y = groundY - HalfHeight;
                RigidBody.Velocity.Y = 0;
            }
        }
    }
}
