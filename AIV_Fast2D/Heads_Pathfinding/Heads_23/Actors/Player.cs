using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{
    class Player : Actor
    {
        protected Controller controller;
        protected bool isFirePressed;
        
        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }
        
        public Player(Controller ctrl, int id = 0) : base("player_1")
        {
            IsActive = true;
            controller = ctrl;
            maxSpeed = 6f;
            bulletType = BulletType.PlayerBullet;

            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile);
            RigidBody.Friction = 40;

            Reset();

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public void Input()
        {
            Vector2 direction = new Vector2(controller.GetHorizontal(), controller.GetVertical());

            if(direction != Vector2.Zero)
            {
                RigidBody.Velocity = direction.Normalized() * maxSpeed;
            }

            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    Shoot(Forward);
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
            }
        }

        public override void OnDie()
        {
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
            base.OnDie();
        }
    }
}
