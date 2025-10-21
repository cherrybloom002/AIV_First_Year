using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{
    class PlayScene : Scene
    {
        protected List<Player> players;
        protected List<Controller> controllers;
        protected GameObject bg;
        public Map map;
        
        protected int alivePlayers;

        public List<Player> Players { get { return players; } }
        public List<Enemy> Enemies { get; private set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            Controller controller1 = Game.GetController(0);
            Controller controller2 = Game.GetController(1);

            if(controller2 is KeyboardController)
            {
                List<KeyCode> keys = new List<KeyCode>();
                keys.Add(KeyCode.Up);
                keys.Add(KeyCode.Down);
                keys.Add(KeyCode.Right);
                keys.Add(KeyCode.Left);
                keys.Add(KeyCode.CtrlRight);

                KeysList keysList = new KeysList(keys);
                controller2 = new KeyboardController(1, keysList);
            }

            controllers = new List<Controller>();
            controllers.Add(controller1);
            controllers.Add(controller2);

            BulletMngr.Init();
            PowerUpsMngr.Init();

            LoadTiledMap();

            base.Start();
        }

        private void LoadObjects()
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(@".\Assets\Config\GameConfig.xml");
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            XmlNode rootNode = xmlDoc.SelectSingleNode("Objects");
            XmlNode actorsNode = rootNode.SelectSingleNode("Actors");

            // Players
            XmlNode playersNode = actorsNode.SelectSingleNode("Players");
            XmlNodeList playersNodes = playersNode.SelectNodes("Player");

            players = new List<Player>();

            for (int i = 0; i < playersNodes.Count; i++)
            {
                int x = int.Parse(playersNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
                int y = int.Parse(playersNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
                bool isActive = bool.Parse(playersNodes[i].SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

                Player player = new Player(controllers[i], i);
                player.Position = new Vector2(x, y);
                player.IsActive = isActive;

                players.Add(player);
            }

            alivePlayers = players.Count;

            // Enemies
            XmlNode enemiesNode = actorsNode.SelectSingleNode("Enemies");
            XmlNode enemyNode = enemiesNode.SelectSingleNode("Enemy");

            int enemyX = int.Parse(enemyNode.SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
            int enemyY = int.Parse(enemyNode.SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
            bool enemyIsActive = bool.Parse(enemyNode.SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

            Enemy enemy = new Enemy();
            enemy.Position = new Vector2(enemyX, enemyY);
            enemy.IsActive = enemyIsActive;

            //// Background
            XmlNode bgNode = enemiesNode.SelectSingleNode("Background");

            int bgX = int.Parse(enemyNode.SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
            int bgY = int.Parse(enemyNode.SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
            bool bgIsActive = bool.Parse(enemyNode.SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

            bg = new GameObject("bg", DrawLayer.Background);
            bg.Position = Game.ScreenCenter;
            bg.IsActive = bgIsActive;
        }

        private void LoadMap()
        {
            int[] cells = new int[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1,
                1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
            };

            map = new Map(21, 11, cells);
        }

        private void LoadTiledMap()
        {
            // Get Xml Document from file
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(@".\Assets\Config\HeadsTiled.xml");
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            // Get relevant Nodes
            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            XmlNode layerNode = mapNode.SelectSingleNode("layer");
            XmlNode dataNode = layerNode.SelectSingleNode("data");

            // Start Map Data Parsing
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

            string[] Ids = csvData.Split(',');
            int[] cells = new int[Ids.Length];

            for (int i = 0; i < Ids.Length; i++)
            {
                if (Ids[i] == "1")
                {
                    cells[i] = int.MaxValue;
                }
                else
                {
                    cells[i] = 1;
                }
            }

            int mapW = int.Parse(mapNode.Attributes.GetNamedItem("width").Value);
            int mapH = int.Parse(mapNode.Attributes.GetNamedItem("height").Value);

            map = new Map(mapW, mapH, cells);

            // Get Object Nodes
            XmlNodeList objectNodes = mapNode.SelectNodes("objectgroup");
            XmlNodeList playerNodes = objectNodes[0].SelectNodes("object");
            XmlNodeList enemyNodes = objectNodes[1].SelectNodes("object");

            // Players
            players = new List<Player>();

            for (int i = 0; i < playerNodes.Count; i++)
            {
                int x = int.Parse(playerNodes[i].Attributes.GetNamedItem("x").Value);
                int y = int.Parse(playerNodes[i].Attributes.GetNamedItem("y").Value);

                Player player = new Player(controllers[i], i);
                player.Position = new Vector2(x, y);
                player.IsActive = true;

                players.Add(player);
            }

            alivePlayers = players.Count;

            // Enemies
            Enemies = new List<Enemy>();

            for (int i = 0; i < enemyNodes.Count; i++)
            {
                int x = int.Parse(enemyNodes[i].Attributes.GetNamedItem("x").Value);
                int y = int.Parse(enemyNodes[i].Attributes.GetNamedItem("y").Value);

                Enemy enemy = new Enemy();
                enemy.Position = new Vector2(x, y);
                enemy.IsActive = true;

                Enemies.Add(enemy);
            }

            // Background
            bg = new GameObject("bg", DrawLayer.Background);
            bg.Position = Game.ScreenCenter;
            bg.IsActive = true;
        }

        protected override void LoadAssets()
        {
            //images
            GfxMngr.AddTexture("bg", "Assets/hex_grid_green.png");

            GfxMngr.AddTexture("player_1", "Assets/player_1.png");
            GfxMngr.AddTexture("player_2", "Assets/player_2.png");
            GfxMngr.AddTexture("enemy_0", "Assets/enemy_0.png");
            GfxMngr.AddTexture("enemy_1", "Assets/enemy_1.png");

            GfxMngr.AddTexture("bullet", "Assets/fireball.png");
            GfxMngr.AddTexture("heart", "Assets/heart.png");

            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
            GfxMngr.AddTexture("powerUp", "Assets/heart.png");

            GfxMngr.AddTexture("tile", "Assets/Levels/crate.png");
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsAlive)
                {
                    players[i].Input();
                }
            }
        }

        public override void Update()
        {
            PhysicsMngr.Update();
            UpdateMngr.Update();
            PowerUpsMngr.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            Players.Clear();
            bg = null;

            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            bg.Draw();
            DrawMngr.Draw();
            DebugMngr.Draw();
        }

        public virtual void OnPlayerDies()
        {
            alivePlayers--;
            if (alivePlayers <= 0)
            {
                IsPlaying = false;
            }
        }
    }
}
