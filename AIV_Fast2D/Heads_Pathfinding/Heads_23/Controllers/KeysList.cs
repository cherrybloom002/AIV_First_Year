﻿using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_23
{
    enum KeyName { Up, Down, Right, Left, Fire, Jump, LAST}

    struct KeysList
    {
        private KeyCode[] keyCodes;

        public KeysList(List<KeyCode> keys)
        {
            keyCodes = new KeyCode[(int)KeyName.LAST];

            for (int i = 0; i < keys.Count; i++)
            {
                keyCodes[i] = keys[i];
            }
        }

        public void SetKey(KeyName name, KeyCode code)
        {
            keyCodes[(int)name] = code;
        }

        public KeyCode GetKey(KeyName name)
        {
            return keyCodes[(int)name];
        }
    }
}
