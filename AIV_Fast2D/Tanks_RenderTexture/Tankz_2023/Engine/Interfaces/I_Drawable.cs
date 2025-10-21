using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2023
{
    interface I_Drawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}
