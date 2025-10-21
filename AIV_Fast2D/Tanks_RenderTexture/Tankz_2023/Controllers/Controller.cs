using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    abstract class Controller
    {
        protected int index;

        public Controller(int ctrlIndex)
        {
            index = ctrlIndex;
        }

        public abstract bool IsJumpPressed();
        public abstract bool IsFirePressed();
        public abstract float GetHorizontal();
        public abstract float GetVertical();

        public abstract bool IsNextWeaponPressed();
        public abstract bool IsPrevWeaponPressed();
    }
}
