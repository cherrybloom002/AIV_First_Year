using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Car2D_2023
{
    class Program
    {
        public static Window Win;
        static void Main(string[] args)
        {
            Win = new Window(1900, 1000, "Car");

            //car instancing

            Car car = new Car();
            car.Position = new Vector2(600, 300);


            while (Win.IsOpened)
            {
                //input
                car.Input();
                //update
                car.Update();
                //draw
                car.Draw();
                Win.Update();
            }
            
        }
    }
}
