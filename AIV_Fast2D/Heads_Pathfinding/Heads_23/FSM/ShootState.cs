using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_23
{
    class ShootState : State
    {
        private Enemy owner;

        private float shootTimeLimit = 0.25f;
        private float shootCountdown = 0.0f;

        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;

        public ShootState(Enemy owner)
        {
            this.owner = owner;
            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);
        }

        public override void OnEnter()
        {
            owner.RigidBody.Velocity = Vector2.Zero;
        }

        protected virtual bool ContinueAttack(PowerUp nearestPowerUp)
        {
            //float rechargeDistFuzzy = 1 - (nearestPowerUp.Position - owner.Position).LengthSquared / (owner.VisionRadius * owner.VisionRadius);
            float rechargeNrgFuzzy = 1 - (float)owner.Energy / (float)owner.MaxEnergy;
            float rechargeSum = rechargeNrgFuzzy;

            //float attackDistFuzzy = 1 - (owner.Rival.Position - owner.Position).LengthSquared / (owner.VisionRadius * owner.VisionRadius);
            float attackNrgFuzzy = Math.Min((float)owner.Energy / (float)owner.Rival.Energy, 1);
            float attackSum = attackNrgFuzzy;

            return attackSum > rechargeSum;
        }

        public override void Update()
        {
            checkForNewPlayer.Tick();

            if (checkForNewPlayer.IsOver())
            {
                owner.Rival = owner.GetBestPlayerToFight();
                checkForNewPlayer.Reset();
            }

            checkForPowerUp.Tick();

            if (checkForPowerUp.IsOver())
            {
                PowerUp p = owner.GetNearestPowerUp();

                if (p != null)
                {
                    if (owner.Rival == null || !ContinueAttack(p))
                    {
                        owner.Target = p;
                        fsm.GoTo(StateEnum.RECHARGE);
                        checkForPowerUp.Reset();
                        return;
                    }
                }
            }

            shootCountdown -= Game.Window.DeltaTime;

            if (owner.Rival == null || !owner.CanAttackPlayer())
            {
                fsm.GoTo(StateEnum.WALK);
            }
            else
            {
                owner.LookAtPlayer();

                if (shootCountdown <= 0.0f)
                {
                    shootCountdown = shootTimeLimit;
                    owner.ShootPlayer();
                }
            }
        }
    }
}
