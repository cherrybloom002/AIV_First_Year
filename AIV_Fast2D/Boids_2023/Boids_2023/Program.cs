using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boids_2023
{
    class Program
    {
        public static Window Win;
        public static List<Boid> Boids;
        public static Texture BoidTexture { get; private set; }

        static void Main(string[] args)
        {
            Win = new Window(1500, 800, "Boids");

            BoidTexture = new Texture("boid.png");
            Boids = new List<Boid>();

            float spawnCounter = 0;
            float spawnRate = 10;
            float spawnTime = 1f / spawnRate;

            while (Win.IsOpened)
            {
                spawnCounter += Win.DeltaTime;

                //input
                if (Win.MouseLeft)
                {
                    if(spawnCounter >= spawnTime)
                    {
                        spawnCounter = 0;
                        Boid b = new Boid(Win.MousePosition);
                        Boids.Add(b);
                    }
                }

                //update

                for (int i = 0; i < Boids.Count; i++)
                {
                    Boids[i].Update();
                }

                //draw

                for (int i = 0; i < Boids.Count; i++)
                {
                    Boids[i].Draw();
                }

                Win.Update();
            }
        }
    }
}
