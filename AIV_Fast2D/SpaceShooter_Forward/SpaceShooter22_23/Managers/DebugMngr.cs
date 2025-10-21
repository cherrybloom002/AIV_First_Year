using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter22_23
{
    static class DebugMngr
    {
        static List<Collider> items;
        static List<Sprite> sprites;

        static Vector4 greenColor;
        static Texture circleTexture;

        static DebugMngr()
        {
            items = new List<Collider>();
            sprites = new List<Sprite>();

            greenColor = new Vector4(0f, 1f, 0f, 1f);

            circleTexture = GfxMngr.GetTexture("circleTexture");
        }

        public static void AddItem(Collider c)
        {
            items.Add(c);

            if(c is BoxCollider)
            {
                BoxCollider b = ((BoxCollider)c);
                Sprite s = new Sprite(b.Width, b.Height);
                s.pivot = new Vector2(b.Width * 0.5f, b.Height * 0.5f);
                sprites.Add(s);
            }
            else
            {
                float size = ((CircleCollider)c).Radius * 2f;
                Sprite s = new Sprite(size, size);
                s.pivot = new Vector2(size * 0.5f);
                sprites.Add(s);
            }
        }

        public static void RemoveItem(Collider c)
        {
            int colliderIndex = items.IndexOf(c);
            sprites.RemoveAt(colliderIndex);
            items.Remove(c);
        }

        public static void ClearAll()
        {
            items.Clear();
            sprites.Clear();

            circleTexture = null;
        }

        public static void Draw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RigidBody.IsActive)
                {
                    sprites[i].position = items[i].Position;

                    if(items[i] is BoxCollider)
                    {
                        sprites[i].DrawWireframe(greenColor);
                    }
                    else
                    {
                        sprites[i].DrawTexture(circleTexture);
                    }
                }
            }
        }
    }
}
