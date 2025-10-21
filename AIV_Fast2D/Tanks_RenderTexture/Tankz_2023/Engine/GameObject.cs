using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class GameObject : I_Updatable, I_Drawable
    {
        // Variables
        protected int frameW;
        protected int frameH;

        protected Sprite sprite;
        protected Texture texture;

        public RigidBody RigidBody;
        public bool IsActive;

        protected Dictionary<ComponentType, Component> components;

        // Properties
        public virtual Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }
        public float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        public float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }
        public float HalfWidth { get { return sprite.Width * 0.5f; } protected set { } }
        public float HalfHeight { get { return sprite.Height * 0.5f; } protected set { } }

        protected int textOffsetX, textOffsetY;

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

        public DrawLayer Layer { get; protected set; }

        public GameObject(string texturePath, DrawLayer layer = DrawLayer.Playground, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0)
        {
            texture = GfxMngr.GetTexture(texturePath);
            float spriteW = spriteWidth > 0 ? spriteWidth : Game.PixelsToUnits(texture.Width);
            float spriteH = spriteHeight > 0 ? spriteHeight : Game.PixelsToUnits(texture.Height);
            sprite = new Sprite(spriteW, spriteH);

            components = new Dictionary<ComponentType, Component>();

            Layer = layer;

            frameW = texture.Width;
            frameH = texture.Height;

            this.textOffsetX = textOffsetX;
            this.textOffsetY = textOffsetY;

            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
        }

        public virtual void Update()
        {
            //sprite.position += velocity * Game.DeltaTime;
        }

        public virtual void OnCollide(Collision collisionInfo)
        {

        }

        public virtual void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, textOffsetX, textOffsetY, frameW, frameH);
            }
        }

        public virtual void Destroy()
        {
            sprite = null;
            texture = null;

            UpdateMngr.RemoveItem(this);
            DrawMngr.RemoveItem(this);

            if (RigidBody != null)
            {
                RigidBody.Destroy();
                RigidBody = null;
            }
        }

        public virtual void SetCamera(Camera camera)
        {
            sprite.Camera = camera;
        }

        public Component GetComponent(ComponentType type)
        {
            if (components.ContainsKey(type))
            {
                return components[type];
            }
            return null;
        }
    }
}
