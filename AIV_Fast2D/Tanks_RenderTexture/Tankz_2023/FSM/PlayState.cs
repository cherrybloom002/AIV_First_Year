using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class PlayState : State
    {
        public PlayState(Player owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            ((PlayScene)Game.CurrentScene).ResetTimer();

        }

        public override void OnExit()
        {
            Owner.RigidBody.Velocity.X = 0;
        }

        public override void Update()
        {
            if(((PlayScene)Game.CurrentScene).PlayerTimer < 0)
            {
                if (Owner.IsLoading)
                {
                    Owner.StopLoading();
                    return;
                }

                fsm.GoTo(StateEnum.WAIT);
                return;
            }

            Owner.Input();
        }
    }
}
