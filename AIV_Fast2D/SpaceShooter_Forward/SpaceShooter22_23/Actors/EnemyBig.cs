using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    class EnemyBig : Enemy
    {
        public EnemyBig() : base("enemyBig")
        {
            Type = EnemyType.EnemyBig;
            bulletType = BulletType.FireGlobe;
            shootOffset = new Vector2(-sprite.pivot.X, sprite.pivot.Y * 0.5f);

            CompoundCollider compCollider = new CompoundCollider(RigidBody, ColliderFactory.CreateCircleFor(this, false));
            RigidBody.Collider = compCollider;

            BoxCollider box01 = new BoxCollider(RigidBody, (HalfWidth + 100), (HalfHeight * 2 - 20));
            box01.Offset = new Vector2(40, -10);
            compCollider.AddCollider(box01);

            BoxCollider box02 = new BoxCollider(RigidBody, (HalfWidth * 2), 25);
            box02.Offset = new Vector2(-10.0f, 62.0f);
            compCollider.AddCollider(box02);

            BoxCollider box03 = new BoxCollider(RigidBody, 80, 20);
            box03.Offset = new Vector2(-5.0f, 90.0f);
            compCollider.AddCollider(box03);

            Points = 50;
        }
    }
}
