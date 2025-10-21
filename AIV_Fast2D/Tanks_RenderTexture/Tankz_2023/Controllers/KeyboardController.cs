﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace Tankz_2023
{
    class KeyboardController : Controller
    {
        protected KeysList keysConfig;

        public KeyboardController(int ctrlIndex, KeysList keys) : base(ctrlIndex)
        {
            keysConfig = keys;
        }

        public override float GetHorizontal()
        {
            float direction = 0.0f;

            if(Game.Window.GetKey(keysConfig.GetKey(KeyName.Right)))
            {
                direction = 1;
            }
            else if(Game.Window.GetKey(keysConfig.GetKey(KeyName.Left)))
            {
                direction = -1;
            }

            return direction;
        }

        public override float GetVertical()
        {
            float direction = 0.0f;

            if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Up)))
            {
                direction = -1;
            }
            else if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Down)))
            {
                direction = 1;
            }

            return direction;
        }

        public override bool IsFirePressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.Fire));
        }

        public override bool IsJumpPressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.Jump));
        }

        public override bool IsNextWeaponPressed()
        {
            return Game.Window.GetKey(KeyCode.Tab);
        }

        public override bool IsPrevWeaponPressed()
        {
            return Game.Window.GetKey(KeyCode.Tab) && (Game.Window.GetKey(KeyCode.ShiftLeft) || Game.Window.GetKey(KeyCode.ShiftRight));
        }
    }
}
