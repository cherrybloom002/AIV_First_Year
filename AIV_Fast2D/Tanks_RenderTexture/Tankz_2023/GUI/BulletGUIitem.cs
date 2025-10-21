using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    class BulletGUIitem : GUIitem
    {
        protected int numBullets;
        protected TextObject numBulletsTxt;
        protected bool isInfinite;
        protected bool isAvailable;
        protected Vector4 unavailableColor;

        public bool IsAvailable
        {
            get { return isAvailable; }
            set
            {
                isAvailable = value;
                numBulletsTxt.IsActive = isAvailable;

                if (isAvailable)
                {
                    SetColor(Vector4.One);
                }
                else
                {
                    SetColor(unavailableColor);
                }

            }
        }

        public bool IsInfinite
        {
            get { return isInfinite; }
            set
            {
                isInfinite = value;
                numBulletsTxt.IsActive = !isInfinite;
            }
        }

        public int NumBullets
        {
            get
            {
                return numBullets;
            }
            set
            {
                numBullets = value;

                if (numBullets <= 0)
                {
                    //no more bullets
                    IsAvailable = false;
                }
                else
                {
                    //update bullets counter
                    numBulletsTxt.Text = numBullets.ToString();

                    if (!isAvailable)
                    {
                        IsAvailable = true;
                    }
                }
            }
        
        }

        public BulletGUIitem(Vector2 position, GameObject owner, string texturePath, int numBullets, float spriteWidth = 0, float spriteHeight = 0) : base(position, owner, texturePath, 0, 0, spriteWidth, spriteHeight)
        {
            numBulletsTxt = new TextObject(position);
            NumBullets = numBullets;
            unavailableColor = new Vector4(1.0f, 0, 0, 0.4f);
            IsActive = true;
        }

        public void DecrementBullets()
        {
            NumBullets--;
        }
    }
}
