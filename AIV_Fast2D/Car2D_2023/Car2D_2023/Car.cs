using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Car2D_2023
{
    class Car : GameObject
    {
        protected float acceleration;
        protected float currentSpeed;
        protected float maxSpeed;
        protected float friction;

        protected float steerAngle;
        protected float maxSteerAngle;
        protected float steerSpeed;
        protected float resetSteerSpeed;

        protected float halfWheelBase;


        public Car(string texturePath = "Assets/car.png") : base(texturePath)
        {
            acceleration = 500;
            maxSpeed = 800;
            steerSpeed = 3;
            friction = 500;
            maxSteerAngle = MathHelper.PiOver6;
            halfWheelBase = sprite.Width * 0.28f;
            resetSteerSpeed = 2;
        }

        public void Input()
        {
            if (Program.Win.GetKey(KeyCode.W))
            {//forward

                currentSpeed += acceleration * Program.Win.DeltaTime;
                currentSpeed = Math.Min(currentSpeed, maxSpeed);
            }
            else if (Program.Win.GetKey(KeyCode.S))
            {//backward

                currentSpeed -= acceleration * Program.Win.DeltaTime;
                currentSpeed = Math.Max(currentSpeed, -maxSpeed);
            }
            else
            {
                //friction
                if (currentSpeed > 0)
                {//forward
                    currentSpeed = Math.Max(0, currentSpeed - friction * Program.Win.DeltaTime);
                }
                else if (currentSpeed < 0)
                {//backward
                    currentSpeed = Math.Min(0, currentSpeed + friction * Program.Win.DeltaTime);
                }
            }

            float steeringWheelDir = 0;

            //steer
            if (Program.Win.GetKey(KeyCode.D))
            {//right
                steeringWheelDir = 1f;
            }
            else if (Program.Win.GetKey(KeyCode.A))
            {//left
                steeringWheelDir = -1f;
            }
            else
            {
                //reset steer
                if (steerAngle > 0)
                {
                    steerAngle = Math.Max(0, steerAngle - resetSteerSpeed * Program.Win.DeltaTime);
                }
                else if (steerAngle < 0)
                {
                    steerAngle = Math.Max(0, steerAngle + resetSteerSpeed * Program.Win.DeltaTime);
                }

                return;
            }

            steerAngle += steerSpeed * steeringWheelDir * Program.Win.DeltaTime;
            steerAngle = MathHelper.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);
        }

        public override void Update()
        {
            if (currentSpeed != 0)
            {
                //computer wheels position 
                Vector2 frontWheel = Position + Forward * halfWheelBase;
                Vector2 backWheel = Position - Forward * halfWheelBase;

                //update wheels position
                backWheel += Forward * currentSpeed * Program.Win.DeltaTime;

                float globalRot = steerAngle + sprite.Rotation;
                frontWheel += currentSpeed * Program.Win.DeltaTime * new Vector2((float)Math.Cos(globalRot), (float)Math.Sin(globalRot));

                //compute car position
                Position = (frontWheel + backWheel) * 0.5f;

                //update car forward
                Forward = frontWheel - backWheel;
            }

        }
    }
}
