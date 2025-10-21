using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2023
{
    class WeaponsGUI : GameObject
    {
        protected BulletGUIitem[] weapons;
        protected string[] textureNames = { "bullet_ico", "missile_ico" };

        protected int selectedWeapon;
        protected Sprite selection;
        protected Texture selectionTexture;
        protected float itemWidth;

        public int SelectedWeapon
        {
            get { return selectedWeapon; }
            protected set
            {
                selectedWeapon = value;
                selection.position = weapons[selectedWeapon].Position;
            }
        }


        public WeaponsGUI(Vector2 position) : base("weapons_frame", DrawLayer.GUI)
        {
            DrawMngr.AddItem(this);

            sprite.pivot = Vector2.Zero;
            sprite.position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");

            weapons = new BulletGUIitem[textureNames.Length];

            selectionTexture = GfxMngr.GetTexture("weapon_selection");
            itemWidth = Game.PixelsToUnits(selectionTexture.Width);

            selection = new Sprite(itemWidth, itemWidth);
            selection.pivot = new Vector2(itemWidth * 0.5f);
            selection.Camera = sprite.Camera;

            float itemPosY = position.Y + HalfHeight;
            float itemsHorizontalDistance = Game.PixelsToUnits(7);

            for (int i = 0; i < weapons.Length; i++)
            {
                Vector2 itemPos = new Vector2(position.X + itemsHorizontalDistance + itemWidth * 0.5f + itemWidth * i, itemPosY);
                weapons[i] = new BulletGUIitem(itemPos, this, textureNames[i], 2);
            }

            //default weapon config
            weapons[0].IsSelected = true;
            weapons[0].IsInfinite = true;

            SelectedWeapon = 0;

        }

        public override void Draw()
        {
            base.Draw();
            selection.DrawTexture(selectionTexture);
        }

        public BulletType NextWeapon(int direction = 1)
        {
            do
            {
                selectedWeapon += direction;

                if (selectedWeapon >= weapons.Length)
                {
                    selectedWeapon = 0;
                }
                else if (selectedWeapon < 0)
                {
                    selectedWeapon = weapons.Length - 1;
                }

            } while (!weapons[selectedWeapon].IsAvailable);

            SelectedWeapon = selectedWeapon;

            return (BulletType)selectedWeapon;
        }

        public BulletType DecrementBullets()
        {
            if (!weapons[selectedWeapon].IsInfinite)
            {
                weapons[selectedWeapon].DecrementBullets();

                if (!weapons[selectedWeapon].IsAvailable)
                {
                    NextWeapon();
                }
            }

            return (BulletType)selectedWeapon;
        }

        public void AddBullets(BulletType type, int amount)
        {
            BulletGUIitem weaponToIncrement = weapons[(int)type];

            if (!weaponToIncrement.IsInfinite)
            {
                weaponToIncrement.NumBullets += amount;
            }
        }
    }
}
