using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class MoveToPointBehaviour : CameraBehaviour
    {
        protected float duration;
        protected float counter;
        protected Vector2 cameraStartPosition;

        public MoveToPointBehaviour(Camera cam) : base(cam)
        {
        }

        public virtual void MoveTo(Vector2 point, float movementDuration)
        {
            cameraStartPosition = camera.position;
            pointToFollow = point;
            duration = movementDuration;
            counter = 0;
            blendFactor = 0;
        }

        public override void Update()
        {
            counter += Game.DeltaTime;

            if(counter >= duration)
            {
                //animation ended
                counter = duration;
                CameraMgr.OnMovementEnd();
            }

            blendFactor = counter / duration;

            camera.position = Vector2.Lerp(cameraStartPosition, pointToFollow, blendFactor);
        }
    }
}
