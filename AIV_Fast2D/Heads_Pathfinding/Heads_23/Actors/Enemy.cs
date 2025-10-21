using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Heads_23
{

    enum EnemyType
    {
        Enemy01, EnemyBig, LAST
    }

    class Enemy : Actor
    {
        public Player Rival;
        public GameObject Target;

        // FSM Values
        protected float visionRadius;
        public float walkSpeed;
        public float followSpeed;
        protected float shootDistance;
        protected Vector2 pointToReach;
        protected float halfConeAngle = MathHelper.DegreesToRadians(40);

        protected StateMachine fsm;
        public Agent Agent;

        public float VisionRadius { get { return visionRadius; } }
        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }

        public Enemy() : base("enemy_1")
        {
            RigidBody.Type = RigidBodyType.Enemy;
            bulletType = BulletType.EnemyBullet;

            Position = new Vector2(15, 4);

            // FSM
            visionRadius = 5.0f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 3.0f;

            frameW = texture.Height;

            Agent = new Agent(this);

            fsm = new StateMachine();
            fsm.AddState(StateEnum.WALK, new WalkState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowState(this));
            fsm.AddState(StateEnum.SHOOT, new ShootState(this));
            fsm.AddState(StateEnum.RECHARGE, new RechargeState(this));
            fsm.GoTo(StateEnum.WALK);

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);

            Reset();
            IsActive = true;
        }

        //public List<Player> GetVisiblePlayers()
        //{
        //    // Get a reference to all Players from PlayScene and put them in a List
        //    List<Player> players = ((PlayScene)Game.CurrentScene).Players;
        //    // mpty List to store only Visible Players
        //    List<Player> visiblePlayers = new List<Player>();

        //    // Iterate for each Player in Players List (every Player in the Game)
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        // SKip an Element of the List if the current Player is Dead
        //        if (!players[i].IsAlive)
        //        {
        //            continue;
        //        }

        //        // Current Player is not Dead, get the Distance Vector to him (Enemy -> Player)
        //        Vector2 distVect = players[i].Position - Position;

        //        if (distVect.LengthSquared < visionRadius * visionRadius)
        //        {
        //            // Player is inside vision radius, now check for cone angle
        //            // DOT Product gives us the Cosine of the Angle between two Vectors
        //            double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);
        //            // Acos gives us the angle of the Cosine, so we know the angle between Enemy and current Player
        //            float playerAngle = (float)Math.Acos(angleCos);

        //            // If the found Angle is less than our halfConeAngle, we see the current Player
        //            if (playerAngle < halfConeAngle)
        //            {
        //                visiblePlayers.Add(players[i]);
        //            }
        //        }
        //    }

        //    return visiblePlayers;
        //}

        public List<Player> GetVisiblePlayersNodes()
        {
            List<Player> players = ((PlayScene)Game.CurrentScene).Players;
            List<Player> visiblePlayers = new List<Player>();

            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsAlive)
                {
                    continue;
                }

                foreach (Node n in Agent.SightCone)
                {
                    Node playerNode = ((PlayScene)Game.CurrentScene).map.GetNode((int)(players[i].Position.X + 0.5f), (int)(players[i].Position.Y + 0.5f));

                    if (playerNode == n)
                    {
                        visiblePlayers.Add(players[i]);
                    }
                }
            }

            return visiblePlayers;
        }

        public Player GetBestPlayerToFight()
        {
            Player bestPlayer = null;

            List<Player> visiblePlayers = GetVisiblePlayersNodes();

            if (visiblePlayers.Count > 0)
            {
                // We need to decide only if we currently have 2 Players
                if (visiblePlayers.Count > 1)
                {
                    // Init the FuzzySum variable to -1 (our min value)
                    float maxFuzzy = -1;

                    for (int i = 0; i < visiblePlayers.Count; i++)
                    {
                        // Distance
                        Vector2 distanceFromPlayer = Position - visiblePlayers[i].Position;
                        float fuzzyDistance = 1 - distanceFromPlayer.LengthSquared / (visionRadius * visionRadius);

                        // Energy
                        float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                        // Angle
                        float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward, distanceFromPlayer.Normalized()), -1, 1));
                        float fuzzyAngle = 1 - (playerAngle / (float)Math.PI);

                        // Sum
                        float fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                        // Check for best result
                        if (fuzzySum > maxFuzzy)
                        {
                            maxFuzzy = fuzzySum;
                            bestPlayer = visiblePlayers[i];
                        }
                    }
                }
                else
                {
                    // We only have 1 Player
                    bestPlayer = visiblePlayers[0];
                }
            }

            return bestPlayer;
        }


        public virtual PowerUp GetNearestPowerUp()
        {
            PowerUp nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < PowerUpsMngr.PowerUps.Count; i++)
            {
                Vector2 distanceVector;

                if (IsPointVisible(PowerUpsMngr.PowerUps[i].Position, out distanceVector))
                {
                    if (distanceVector.LengthSquared < minDistance)
                    {
                        nearest = PowerUpsMngr.PowerUps[i];
                        minDistance = distanceVector.LengthSquared;
                    }
                }
            }

            return nearest;
        }

        public bool IsPointVisible(Vector2 point, out Vector2 distanceVector)
        {
            distanceVector = point - Position;

            if (distanceVector.LengthSquared <= VisionRadius * VisionRadius)
            {
                float pointAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(Forward, distanceVector.Normalized()), -1.0f, 1.0f));

                if (pointAngle <= halfConeAngle)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanAttackPlayer()
        {
            // If selected Player is Dead
            if (Rival == null || !Rival.IsAlive)
            {
                return false;
            }
            // Selected Player is Alive

            // Get the distance Enemy -> Player
            Vector2 distVect = Rival.Position - Position;

            // Check if the found distance is less than the Shooting distance and return the result
            return distVect.LengthSquared < shootDistance * shootDistance;
        }

        public void HeadToPlayer()
        {
            if (Agent.Target == null)
            {
                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)Position.X, (int)Position.Y, (int)Rival.Position.X, (int)Rival.Position.Y);
                Agent.SetPath(path);
            }

            Agent.Update(followSpeed);
        }

        public void LookAtPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                Forward = direction;
            }
        }

        public void ShootPlayer()
        {
            Shoot(Forward);
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }

                fsm.Update();
            }

            base.Update();
        }

        public void ComputeRandomPoint()
        {
            float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1f;
            float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1f;

            pointToReach = new Vector2(randX, randY);
        }

        public void HeadToPoint()
        {
            Vector2 dist = pointToReach - Position;

            if (dist.LengthSquared <= 0.01f)
            {
                Agent.Target = null;
            }

            if(Agent.Target == null)
            {
                Node randomNode = ((PlayScene)Game.CurrentScene).map.GetRandomNode();
                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)Position.X, (int)Position.Y, (int)randomNode.X, (int)randomNode.Y);
                Agent.SetPath(path);
            }

            Agent.Update(walkSpeed);
        }

        public override void Draw()
        {
            Agent.DrawPath();
            base.Draw();
        }

        public void ChangeSpriteColor(Vector3 newCol)
        {
            sprite.SetMultiplyTint(new Vector4(newCol, 255.0f));
        }
    }
}
