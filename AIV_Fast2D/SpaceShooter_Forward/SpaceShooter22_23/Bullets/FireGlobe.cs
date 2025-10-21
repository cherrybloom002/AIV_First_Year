using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    class FireGlobe : EnemyBullet
    {
        protected float rotationSpeed;
        protected float accumulator;

        public FireGlobe() : base("fireGlobe", 0, 0, 0, 0)
        {
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this, true);
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType((uint)RigidBodyType.Player | (uint)RigidBodyType.PlayerBullet);

            maxSpeed = -500.0f;
            RigidBody.Velocity.X = maxSpeed;

            Type = BulletType.FireGlobe;

            rotationSpeed = -3.5f;
        }

        public override void Update()
        {
            base.Update();//bullet update

            if (IsActive)
            {//bullet could be deactivated by previous update

                //sprite rotation
                sprite.Rotation += rotationSpeed * Game.DeltaTime;

                accumulator += Game.DeltaTime * 10f;
                RigidBody.Velocity.Y = (float)Math.Cos(accumulator) * 800f;
            }
        }

        public override void Reset()
        {
            base.Reset();
            accumulator = 0;
        }
    }
}
