using OpenTK;
using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boids_2023
{
    class Boid
    {

        private Sprite sprite;
        private Texture texture;

        private float speed = 150;

        private float alignRadius = 200;
        private float cohesionRadius = 300;
        private float separationRadius = 70;

        private float separationWeight = 6;

        float alignHalfAngle = MathHelper.Pi;
        float cohesionHalfAngle = MathHelper.Pi;
        float separationHalfAngle = MathHelper.DegreesToRadians(150);

        float steerMul = 0.6f;

        Vector2 velocity;

        public Vector2 Position
        {
            get { return sprite.position; }
            set { sprite.position = value; }
        }

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }


        public Boid(Vector2 position)
        {
            texture = Program.BoidTexture;
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sprite.position = position;

            int randX = RandomGenerator.GetRandomInt(-100, 100);
            int randY = RandomGenerator.GetRandomInt(-100, 100);

            velocity = new Vector2(randX, randY).Normalized() * speed;
        }

        protected bool IsVisible(Vector2 point, float radius, float halfAngle, out Vector2 distance)
        {
            distance = point - Position;

            if (distance.LengthSquared <= radius * radius)
            {
                float angle = (float)Math.Acos(Vector2.Dot(Forward, distance.Normalized()));
                return angle <= halfAngle;
            }

            return false;
        }

        protected Vector2 GetAlignment()
        {
            Vector2 v = Vector2.Zero;
            int neighboursCount = 0;

            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];

                Vector2 dist;
                if(b != this && IsVisible(b.Position,alignRadius,alignHalfAngle, out dist))
                {
                    v += b.Forward;
                    neighboursCount++;
                }
            }

            if (neighboursCount > 0)
            {
                v /= neighboursCount;
                if(v.LengthSquared != 0)
                {
                    v.Normalize();
                }
            }

            return v;
        }

        protected Vector2 GetCohesion()
        {
            Vector2 v = Vector2.Zero;
            int neighboursCount = 0;

            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];

                Vector2 dist;
                if (b != this && IsVisible(b.Position, cohesionRadius, cohesionHalfAngle, out dist))
                {
                    v += b.Position;
                    neighboursCount++;
                }
            }

            if (neighboursCount > 0)
            {
                v /= neighboursCount;
                v = v - Position;

                if (v.LengthSquared != 0)
                {
                    v.Normalize();
                }
            }

            return v;
        }

        protected Vector2 GetSeparation()
        {
            Vector2 v = Vector2.Zero;
            int neighboursCount = 0;

            for (int i = 0; i < Program.Boids.Count; i++)
            {
                Boid b = Program.Boids[i];

                Vector2 dist;
                if (b != this && IsVisible(b.Position, separationRadius, separationHalfAngle, out dist))
                {
                    float weight = 1 - dist.Length / separationRadius;
                    v += dist * weight;
                    neighboursCount++;
                }
            }

            if (neighboursCount > 0)
            {
                v /= neighboursCount;
                v = -v;//v => my position
                if (v.LengthSquared != 0)
                {
                    v.Normalize();
                }
            }

            return v;
        }

        public void Update()
        {
            Vector2 alignment = GetAlignment();
            Vector2 cohesion = GetCohesion();
            Vector2 separation = GetSeparation() * separationWeight;

            Vector2 result = alignment + cohesion + separation;

            if(result != Vector2.Zero)
            {
                velocity = Vector2.Lerp(velocity, result * speed, Program.Win.DeltaTime * steerMul);
                velocity = velocity.Normalized() * speed;
            }

            //look to direction
            if(velocity.Length > 0)
            {
                Forward = velocity;
            }

            //move
            sprite.position += velocity * Program.Win.DeltaTime;

            CheckBorders();
        }

        public void Draw()
        {
            if (velocity.Length > 0)
            {
                Forward = velocity;
            }

            sprite.DrawTexture(texture);
        }

        protected void CheckBorders()
        {
            //Horizontal
            if (sprite.position.X >= Program.Win.Width)
            {
                sprite.position.X = 0;
            }
            else if (sprite.position.X < 0)
            {
                sprite.position.X = Program.Win.Width - 1;
            }

            //Vertical
            if (sprite.position.Y >= Program.Win.Height)
            {
                sprite.position.Y = 0;
            }
            else if (sprite.position.Y < 0)
            {
                sprite.position.Y = Program.Win.Height - 1;
            }
        }
    }

}
