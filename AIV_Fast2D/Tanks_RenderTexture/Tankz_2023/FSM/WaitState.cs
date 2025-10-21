using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class WaitState : State
    {
        public WaitState(Player owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            ((PlayScene)Game.CurrentScene).NextPlayer();
        }

        public override void Update()
        {

        }
    }
}
