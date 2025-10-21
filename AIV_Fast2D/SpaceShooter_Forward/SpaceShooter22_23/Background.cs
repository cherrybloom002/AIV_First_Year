using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace SpaceShooter22_23
{
    class Background : I_Updatable, I_Drawable
    {
        private Sprite head;
        private Sprite tail;
        private Texture texture;

        private Vector2 velocity;

        public Background()
        {
            texture = GfxMngr.GetTexture("spaceBg");
            head = new Sprite(texture.Width, texture.Height);
            tail = new Sprite(texture.Width, texture.Height);

            velocity.X = -800.0f;

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public void Update()
        {
            head.position.X += velocity.X * Game.DeltaTime;

            if (head.position.X <= -head.Width)
            {
                head.position.X += head.Width;
            }

            tail.position.X = head.position.X + head.Width;
        }

        public void Draw()
        {
            head.DrawTexture(texture);
            tail.DrawTexture(texture);
        }
    }
}
