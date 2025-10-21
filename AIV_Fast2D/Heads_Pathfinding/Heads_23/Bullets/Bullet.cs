﻿using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_23
{
    enum BulletType { PlayerBullet, EnemyBullet, LAST}

    abstract class Bullet : GameObject
    {
        protected int damage = 25;
        public BulletType Type { get; protected set; }

        public Actor Shooter;

        public Bullet(string texturePath, int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base(texturePath, DrawLayer.Middleground, textOffsetX, textOffsetY, spriteW, spriteH)
        {
            //sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            //HalfWidth = (int)(sprite.Width * 0.5f);
            //HalfHeight = (int)(sprite.Height * 0.5f);

            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            maxSpeed = 10;

        }

        public virtual void Shoot(Vector2 shootPos, Vector2 shootDir)
        {
            Position = shootPos;
            RigidBody.Velocity = shootDir * maxSpeed;
            Forward = shootDir;
        }

        public virtual void Reset()
        {
            IsActive = true;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 cameraDist = Position - Game.ScreenCenter;//vector from camera to bullet

                if (cameraDist.LengthSquared > Game.HalfDiagonalSquared)
                {
                    BulletMngr.RestoreBullet(this);
                }
            }
        }

    }
}
