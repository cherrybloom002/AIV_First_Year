using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class FollowTargetBehaviour : CameraBehaviour
    {
        public GameObject Target;
        public float CameraSpeed = 5;

        public FollowTargetBehaviour(Camera cam, GameObject target) : base(cam)
        {
            Target = target;
        }

        public override void Update()
        {
            if (Target != null)
            {
                pointToFollow = Target.Position;
                blendFactor = Game.DeltaTime * CameraSpeed;
                base.Update();
            }
        }
    }
}
