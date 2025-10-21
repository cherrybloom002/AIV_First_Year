using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace SpaceShooter22_23
{
    class PlayScene : Scene
    {
        protected Background Bg;
        public Player Player { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();
            


            Bg = new Background();

            Player = new Player(Game.GetController(0), 0);
            Player.Position = new Vector2(100, Game.Window.Height * 0.5f);

            BulletMngr.Init();
            SpawnMngr.Init();
            PowerUpsMngr.Init();

            base.Start();
        }

        protected override void LoadAssets()
        {
            //images
            GfxMngr.AddTexture("player", "Assets/player_ship.png");
            GfxMngr.AddTexture("enemy", "Assets/enemy_ship.png");
            GfxMngr.AddTexture("enemyBig", "Assets/big_ship.png");
            GfxMngr.AddTexture("blueLaser", "Assets/blueLaser.png");
            GfxMngr.AddTexture("purpleLaser", "Assets/beams.png");
            GfxMngr.AddTexture("spaceBg", "Assets/spaceBg.png");
            GfxMngr.AddTexture("fireGlobe", "Assets/fireGlobe.png");

            GfxMngr.AddTexture("energyPowerUp", "Assets/powerUp_battery.png");
            GfxMngr.AddTexture("tripleShootPowerUp", "Assets/powerUp_triple.png");

            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
            GfxMngr.AddTexture("circleTexture", "Assets/circle.png");

            //sounds
            GfxMngr.AddClip("player_laser", "Assets/Sounds/player_laser.wav");
            GfxMngr.AddClip("enemy_laser", "Assets/Sounds/enemy_laser.wav");

            //fonts
            FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);
        }

        public override void Input()
        {
            Player.Input();
        }

        public override void Update()
        {
            if (!Player.IsAlive)
                IsPlaying = false;

            PhysicsMngr.Update();
            UpdateMngr.Update();
            SpawnMngr.Update();
            PowerUpsMngr.Update();
            Bg.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            Player = null;
            Bg = null;

            BulletMngr.ClearAll();
            SpawnMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();
            PowerUpsMngr.ClearAll();

            DebugMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            Bg.Draw();
            DrawMngr.Draw();

            DebugMngr.Draw();
        }
    }
}
