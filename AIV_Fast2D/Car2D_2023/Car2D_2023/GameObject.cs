using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car2D_2023
{
    class GameObject
    {
        protected Sprite sprite;
        protected Texture texture;

        public Vector2 Position { get { return sprite.position; } set{ sprite.position = value; } }

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

        public GameObject(string texturePath)
        {
            texture = new Texture(texturePath);
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            sprite.DrawTexture(texture);
        }

    }
}
