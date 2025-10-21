using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_23
{
    class WalkState : State
    {
        private Enemy owner;
        private RandomTimer checkForVisiblePlayer;
        private RandomTimer checkForVisiblePowerUp;

        public WalkState(Enemy owner)
        {
            this.owner = owner;
            checkForVisiblePlayer = new RandomTimer(0.2f, 0.9f);
            checkForVisiblePowerUp = new RandomTimer(0.5f, 1.4f);
        }

        public override void OnEnter()
        {
            owner.ChangeSpriteColor(new Vector3(1.0f, 1.0f, 1.0f));
            owner.ComputeRandomPoint();
            checkForVisiblePlayer.Cancel();
            checkForVisiblePowerUp.Cancel();
        }

        public override void Update()
        {
            checkForVisiblePlayer.Tick();

            if (checkForVisiblePlayer.IsOver())
            {
                Player p = owner.GetBestPlayerToFight();
                checkForVisiblePlayer.Reset();

                if (p != null)
                {
                    owner.Rival = p;
                    fsm.GoTo(StateEnum.FOLLOW);
                    owner.Agent.Target = null;
                    return;
                }
            }

            if (owner.Energy < owner.MaxEnergy)
            {
                checkForVisiblePowerUp.Tick();

                if (checkForVisiblePowerUp.IsOver())
                {
                    PowerUp p = owner.GetNearestPowerUp();

                    if (p != null)
                    {
                        owner.Target = p;
                        fsm.GoTo(StateEnum.RECHARGE);
                        return;
                    }

                    checkForVisiblePowerUp.Reset();
                }
            }

            owner.HeadToPoint();
        }
    }
}
