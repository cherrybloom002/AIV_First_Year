﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class ProgressBar : GameObject
    {
        protected Vector2 barOffset;

        protected Sprite barSprite;
        protected Texture barTexture;

        protected float barWidth;

        public override Vector2 Position { get => base.Position; set { base.Position = value; barSprite.position = value + barOffset; } }

        public ProgressBar(string frameTextureName, string barTextureName, Vector2 innerBarOffset) :base(frameTextureName, DrawLayer.GUI)
        {
            sprite.pivot = Vector2.Zero;
            IsActive = true;

            barOffset = innerBarOffset;

            barTexture = GfxMngr.GetTexture(barTextureName);
            barSprite = new Sprite(Game.PixelsToUnits(barTexture.Width), Game.PixelsToUnits(barTexture.Height));
            barWidth = barSprite.Width;

            sprite.Camera = CameraMgr.GetCamera("GUI");
            barSprite.Camera = sprite.Camera;

            DrawMngr.AddItem(this);
        }

        public virtual void Scale(float scale)
        {
            scale = MathHelper.Clamp(scale, 0, 1);

            barSprite.scale.X = scale;
            barWidth = (barSprite.Width * scale);

            barSprite.SetMultiplyTint((1 - scale) * 50, scale * 2, scale, 1);
        }

        public override void Draw()
        {
            if(IsActive)
            {
                base.Draw();
                barSprite.DrawTexture(barTexture, 0, 0, (int)barWidth, barTexture.Height);
            }
        }

        public override void SetCamera(Camera camera)
        {
            base.SetCamera(camera);
            barSprite.Camera = camera;
        }
    }
}
