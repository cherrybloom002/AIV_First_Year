using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class Explosion : GameObject
    {
        protected Animation animation;

        public Explosion() : base("explosion_1", DrawLayer.Foreground, 0, 0, Game.PixelsToUnits(70), Game.PixelsToUnits(70))
        {
            animation = new Animation(this, 13, 70, 70, 16, false);
            animation.IsEnabled = true;
            components.Add(ComponentType.Animation, animation);

            DrawMngr.AddItem(this);
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)animation.Offset.X, (int)animation.Offset.Y, animation.FrameWidth, animation.FrameHeight);
            }
        }

        public virtual void Play()
        {
            IsActive = true;
            animation.Restart();
        }
    }
}
