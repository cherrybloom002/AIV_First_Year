using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace Tankz_2023
{
    class Rocket : Bullet
    {
        protected bool engineIsOn;
        protected float startEngineAngle;

        protected AudioClip engineStart;

        public Rocket() : base("rocketBullet")
        {
            Type = BulletType.Rocket;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Tile);
            maxSpeed = 15;
            damage = 30;

            startEngineAngle = MathHelper.DegreesToRadians(-10);

            shootSound = new SoundEmitter(this, "whistle");
            components.Add(ComponentType.SoundEmitter, shootSound);

            engineStart = GfxMngr.GetClip("engineStart");
        }

        public override void Reset()
        {
            base.Reset();
            engineIsOn = false;
        }

        public override void Update()
        {
            base.Update();

            if (IsActive)
            {
                if(!engineIsOn && (sprite.Rotation> startEngineAngle || sprite.Rotation < -Math.PI - startEngineAngle))
                {
                    engineIsOn = true;
                    RigidBody.Velocity.X = 18 * Math.Sign(RigidBody.Velocity.X);
                    shootSound.Play(1f, 1f, engineStart);
                }
            }
        }
    }
}
