using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class Player : Tank
    {
        // References
        protected Controller controller;
        protected ProgressBar loadingBar;
        protected TextObject playerName;
        protected TextObject scoreText;
        protected StateMachine stateMachine;
        protected WeaponsGUI weaponsGUI;

        // Variables
        protected bool isFirePressed;
        protected bool isLoading;
        protected bool isChangeWeaponPressed;

        protected float currentLoadingValue;
        protected float loadIncrease = 50.0f;
        protected float maxLoadingValue = 100.0f;
        protected Vector2 loadingBarOffset;
        //protected int playerId;
        //protected int score;

        // Properties
        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.Scale((float)value / (float)maxEnergy); } }

        public bool IsLoading { get { return isLoading; } }

        public Player(Controller ctrl, int id = 0) : base()
        {
            controller = ctrl;
            IsActive = true;
            maxSpeed = 6.0f;
            bulletType = BulletType.StdBullet;

            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile);
            RigidBody.IsGravityAffected = true;

            float unitDist = Game.PixelsToUnits(4);
            loadingBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist, unitDist));
            loadingBar.IsActive = false;
            loadingBarOffset = new Vector2(-loadingBar.HalfWidth, -1.3f);
            loadingBar.SetCamera(null);

            energyBar.Position = new Vector2(id * 3 + 0.2f, 0.5f);
            playerName = new TextObject(new Vector2(energyBar.Position.X, 0.25f), $"Player {id + 1}");
            playerName.IsActive = true;
            Reset();

            stateMachine = new StateMachine();
            //add states
            stateMachine.AddState(StateEnum.WAIT, new WaitState(this));
            stateMachine.AddState(StateEnum.PLAY, new PlayState(this));
            stateMachine.AddState(StateEnum.SHOOT, new ShootState(this));
            //stateMachine.GoTo(StateEnum.WAIT);

            weaponsGUI = new WeaponsGUI(new Vector2(energyBar.Position.X, 1f));
            weaponsGUI.IsActive = true;

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        protected virtual void StartLoading()
        {
            currentLoadingValue = 0;
            loadIncrease = Math.Abs(loadIncrease);

            loadingBar.Position = Position + loadingBarOffset;
            loadingBar.IsActive = true;
            isLoading = true;
        }

        public virtual void StopLoading()
        {
            if (isLoading)
            {
                isLoading = false;
                loadingBar.IsActive = false;
                Shoot(currentLoadingValue / maxLoadingValue);

                if (LastShotBullet != null)
                {
                    //switch camera on bullet
                    CameraMgr.SetTarget(LastShotBullet);

                    bulletType = weaponsGUI.DecrementBullets();
                }

                stateMachine.GoTo(StateEnum.SHOOT);
            }
        }

        //protected void UpdateScore()
        //{
        //    scoreText.Text = score.ToString("00000000");
        //}

        //public void AddScore(int points)
        //{
        //    score += points;
        //    UpdateScore();
        //}

        public void Input()
        {
            //movement
            float directionX = controller.GetHorizontal();

            if (directionX != 0 && !isLoading)
            {
                RigidBody.Velocity.X = directionX * maxSpeed;
            }
            else
            {
                RigidBody.Velocity.X = 0;
            }

            //cannon rotation
            float directionY = controller.GetVertical();
            if (directionY != 0)
            {
                cannon.Rotation += directionY * Game.DeltaTime;
                cannon.Rotation = MathHelper.Clamp(cannon.Rotation, maxCannonAngle, 0);
            }

            // shoot
            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    StartLoading();
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
                StopLoading();
            }

            //weapon change
            if (controller.IsNextWeaponPressed())
            {
                if (!isChangeWeaponPressed)
                {
                    isChangeWeaponPressed = true;
                    bulletType = weaponsGUI.NextWeapon();
                }
            }
            else if (controller.IsPrevWeaponPressed())
            {
                if (!isChangeWeaponPressed)
                {
                    isChangeWeaponPressed = true;
                    bulletType = weaponsGUI.NextWeapon(-1);
                }
            }
            else
            {
                isChangeWeaponPressed = false;
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (isLoading)
                {
                    currentLoadingValue += loadIncrease * Game.DeltaTime;

                    if (currentLoadingValue > maxLoadingValue)
                    {
                        currentLoadingValue = maxLoadingValue;
                        loadIncrease = -loadIncrease;
                    }
                    else if (currentLoadingValue < 0)
                    {
                        currentLoadingValue = 0;
                        loadIncrease = -loadIncrease;
                    }

                    loadingBar.Scale(currentLoadingValue / maxLoadingValue);
                }
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //((Enemy)collisionInfo.Collider).OnDie();
            //AddDamage(30);

            OnTileCollide(collisionInfo);
        }

        private void OnTileCollide(Collision collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if (X < collisionInfo.Collider.X)
                {
                    // Left to Right
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                // Apply DeltaX
                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0.0f;
            }
            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Top to Bottom
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0.0f;
                }
                else
                {
                    // Bottom to Top
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;
                }

                // Apply DeltaY
                Y += collisionInfo.Delta.Y;

            }
        }

        public override void OnDie()
        {
            IsActive = false;
            energyBar.IsActive = false;
            playerName.IsActive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies(this);
        }

        public virtual void UpdateStateMachine()
        {
            stateMachine.Update();
        }

        public virtual void Play()
        {
            stateMachine.GoTo(StateEnum.PLAY);
        }
    }
}
