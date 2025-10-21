﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace SpaceShooter22_23
{
    class TitleScene : Scene
    {
        protected Texture texture;
        protected Sprite sprite;

        protected string texturePath;
        protected KeyCode exitKey;

        public TitleScene(string t_Path, KeyCode exit = KeyCode.Return)
        {
            texturePath = t_Path;
            this.exitKey = exit;
        }

        public override void Start()
        {
            LoadAssets();

            texture = GfxMngr.GetTexture(texturePath);
            sprite = new Sprite(Game.Window.Width, Game.Window.Height);

            base.Start();
        }

        protected override void LoadAssets()
        {
            GfxMngr.AddTexture("titleScreen", "Assets/aivBG.png");
        }

        public override void Input()
        {
            if(Game.Window.GetKey(exitKey))
            {
                IsPlaying = false;
            }
        }

        public override Scene OnExit()
        {
            texture = null;
            sprite = null;

            return base.OnExit();
        }

        public override void Draw()
        {
            sprite.DrawTexture(texture);
        }
    }
}
