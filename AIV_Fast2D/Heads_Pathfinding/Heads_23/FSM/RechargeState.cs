using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_23
{
    class RechargeState : State
    {
        private Enemy owner;

        public RechargeState(Enemy owner)
        {
            this.owner = owner;
        }

        public override void OnEnter()
        {
            if(owner.Target != null && owner.Target.IsActive)
            {
                List<Node> path = ((PlayScene)Game.CurrentScene).map.GetPath((int)owner.Position.X, (int)owner.Position.Y, (int)owner.Target.Position.X, (int)owner.Target.Position.Y);
                owner.Agent.SetPath(path);
            }
        }

        public override void Update()
        {
            if(owner.Target == null || !owner.Target.IsActive)
            {
                owner.Target = null;

                if(owner.Rival != null && owner.Rival.IsActive)
                {
                    fsm.GoTo(StateEnum.FOLLOW);
                }
                else
                {
                    fsm.GoTo(StateEnum.WALK);
                }
            }
            else
            {
                owner.Agent.Update(owner.followSpeed);
            }
        }
    }
}
