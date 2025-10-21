using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class PlayScene : Scene
    {
        protected List<Player> players;
        protected int currentPlayerIndex;

        protected List<Tile> tiles;

        protected int turnDuration = 20;
        protected TextObject timerTxt;

        protected Background Bg;
        public Player CurrentPlayer { get { return players[currentPlayerIndex];  } }
        public float GroundY { get; protected set; }

        public float PlayerTimer { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 1.23f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.4f);
            CameraMgr.Init(null, cameraLimits);

            CameraMgr.AddCamera("GUI", new Aiv.Fast2D.Camera());
            CameraMgr.AddCamera("Bg_0", cameraSpeed: 0.9f);
            CameraMgr.AddCamera("Bg_1", cameraSpeed: 0.95f);

            GfxMngr.InitFX();

            GroundY = 9.5f;

            Bg = new Background();

            LoadTiles();

            players = new List<Player>();

            Player player = new Player(Game.GetController(0), 0);
            player.Position = new Vector2(2, -10);

            CameraMgr.SetTarget(player);

            Controller controller1 = Game.GetController(1);

            if(controller1 is KeyboardController)
            {
                List<KeyCode> keys = new List<KeyCode>();
                keys.Add(KeyCode.Up);
                keys.Add(KeyCode.Down);
                keys.Add(KeyCode.Right);
                keys.Add(KeyCode.Left);
                keys.Add(KeyCode.CtrlRight);

                KeysList keyList = new KeysList(keys);
                controller1 = new KeyboardController(1, keyList);
            }

            Player player2 = new Player(controller1, 1);
            player2.Position = new Vector2(16, -10);

            players.Add(player);
            players.Add(player2);

            timerTxt = new TextObject(new Vector2(Game.Window.OrthoWidth * 0.5f, 3), "", FontMngr.GetFont("comics"));

            CurrentPlayer.Play();

            BulletMngr.Init();

            //Game.Window.AddPostProcessingEffect(new BlackBandFX());
            //Game.Window.AddPostProcessingEffect(new GrayScaleFX());
            //Game.Window.AddPostProcessingEffect(new SepiaFX());
            //Game.Window.AddPostProcessingEffect(new NegativeFX());
            //Game.Window.AddPostProcessingEffect(new BlurFX());
            //Game.Window.AddPostProcessingEffect(new WobbleFX());
            //Game.Window.AddPostProcessingEffect(new WobbleMouseFX());

            DrawMngr.AddFX("Sepia", new SepiaFX());

            base.Start();
        }

        protected override void LoadAssets()
        {
            //BG
            GfxMngr.AddTexture("bg", "Assets/bg_0.png");

            //images
            GfxMngr.AddTexture("tracks", "Assets/tanks_tankTracks1.png");
            GfxMngr.AddTexture("body", "Assets/tanks_tankGreen_body1.png");
            GfxMngr.AddTexture("cannon", "Assets/tanks_turret2.png");
            
            GfxMngr.AddTexture("stdBullet", "Assets/tank_bullet1.png");
            GfxMngr.AddTexture("rocketBullet", "Assets/tank_bullet3.png");
            GfxMngr.AddTexture("crate", "Assets/crate.png");

            //GUI
            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
            GfxMngr.AddTexture("weapons_frame", "Assets/weapons_GUI_frame.png");
            GfxMngr.AddTexture("weapon_selection", "Assets/weapon_GUI_selection.png");
            GfxMngr.AddTexture("bullet_ico", "Assets/bullet_ico.png");
            GfxMngr.AddTexture("missile_ico", "Assets/missile_ico.png");

            //fonts
            FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);

            //Sounds
            GfxMngr.AddClip("shoot", "Assets/sounds/cannonShoot.wav");
            GfxMngr.AddClip("crack_1", "Assets/sounds/wood_crack_1.ogg");
            GfxMngr.AddClip("crack_2", "Assets/sounds/wood_crack_2.ogg");
            GfxMngr.AddClip("whistle", "Assets/sounds/whistle.ogg");
            GfxMngr.AddClip("engineStart", "Assets/sounds/engineStart.wav");

            // FX
            GfxMngr.AddTexture("explosion_1", "Assets/explosion.png");
        }

        private void LoadTiles()
        {
            tiles = new List<Tile>();

            int[] cells = new int[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
                0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0

            };

            for (int y = 0; y < 11; y++)
            {
                for (int x = 0; x < 17; x++)
                {
                    if (cells[y * 17 + x] == 1)
                    {
                        Tile t = new Tile();
                        t.Position = new Vector2(x + 1, y * -2);
                        t.IsActive = true;

                        tiles.Add(t);
                    }
                }
            }
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].UpdateStateMachine();
            }
        }

        public override void Update()
        {
            if (timerTxt.IsActive)
            {
                PlayerTimer -= Game.DeltaTime;
                timerTxt.Text = ((int)PlayerTimer).ToString();
            }

            PhysicsMngr.Update();
            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();

            CameraMgr.Update();
        }

        public override Scene OnExit()
        {
            players.Clear();
            Bg = null;

            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            //Bg.Draw();
            DrawMngr.Draw();
        }

        public virtual void OnPlayerDies(Player deadPlayer)
        {
            players.Remove(deadPlayer);//TO FIX

            if (players.Count <= 1)
            {
                IsPlaying = false;
            }
        }

        public virtual void ResetTimer()
        {//show and start timer
            PlayerTimer = turnDuration;
            timerTxt.Text = turnDuration.ToString();
            timerTxt.IsActive = true;
        }

        public virtual void StopTimer()
        {
            timerTxt.IsActive = false;
        }

        public virtual void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            CameraMgr.SetTarget(CurrentPlayer,false);
            CameraMgr.MoveTo(CurrentPlayer.Position, 1f);

            //send current player in Play state
            CurrentPlayer.Play();
        }
    }
}
