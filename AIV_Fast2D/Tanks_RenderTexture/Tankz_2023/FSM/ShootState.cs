using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class ShootState : State
    {
        public ShootState(Player owner) : base(owner)
        {
        }

        public override void Update()
        {
            //wait for shot bullet being destroyed
            if(Owner.LastShotBullet ==null || !Owner.LastShotBullet.IsActive)
            {
                fsm.GoTo(StateEnum.WAIT);
            }
        }
    }
}
