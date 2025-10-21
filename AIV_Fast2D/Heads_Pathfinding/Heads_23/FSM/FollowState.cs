using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_23
{
    class FollowState : State
    {
        private Enemy owner;
        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;


        public FollowState(Enemy owner)
        {
            this.owner = owner;
            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);

            owner.Agent.Target = null;
        }

        public override void OnEnter()
        {
            owner.ChangeSpriteColor(new Vector3(255.0f, 0.8f, 0.8f));
        }

        protected virtual bool ContinueFollow(PowerUp nearestPowerUp)
        {
            float rechargeDistFuzzy = 1 - (nearestPowerUp.Position - owner.Position).LengthSquared / (owner.VisionRadius * owner.VisionRadius);
            float rechargeNrgFuzzy = 1 - (float)owner.Energy / (float)owner.MaxEnergy;
            float rechargeSum = rechargeDistFuzzy + rechargeNrgFuzzy;

            float followDistFuzzy = 1 - (owner.Rival.Position - owner.Position).LengthSquared / (owner.VisionRadius * owner.VisionRadius);
            float followNrgFuzzy = Math.Min((float)owner.Energy / (float)owner.Rival.Energy, 1);
            float followSum = followDistFuzzy + followNrgFuzzy;

            return followSum > rechargeSum;
        }

        public override void Update()
        {
            checkForNewPlayer.Tick();

            if (checkForNewPlayer.IsOver() && owner.Agent.Target == null)
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
                    if (owner.Rival == null || !ContinueFollow(p))
                    {
                        owner.Target = p;
                        fsm.GoTo(StateEnum.RECHARGE);
                        checkForPowerUp.Reset();
                        return;
                    }
                }
            }

            if (owner.Rival == null || !owner.Rival.IsAlive)
            {
                fsm.GoTo(StateEnum.WALK);
            }
            else if (owner.CanAttackPlayer())
            {
                fsm.GoTo(StateEnum.SHOOT);
            }
            else
            {
                owner.HeadToPlayer();
            }
        }
    }
}
