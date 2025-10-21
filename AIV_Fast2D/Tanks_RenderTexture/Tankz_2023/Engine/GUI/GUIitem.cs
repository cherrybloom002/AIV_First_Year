using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class GUIitem : GameObject
    {
        public bool IsSelected;

        protected GameObject owner;
        protected Vector2 offset;

        public GUIitem(Vector2 position, GameObject owner, string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, DrawLayer.GUI, textOffsetX, textOffsetY, spriteWidth, spriteHeight)
        {
            this.owner = owner;
            sprite.position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");
            offset = position - owner.Position;

            DrawMngr.AddItem(this);
        }

        public void SetColor(Vector4 color)
        {
            sprite.SetMultiplyTint(color);
        }
    }
}
