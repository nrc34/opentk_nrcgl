using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    interface ILevel : IWGameable
    {
        void Load();
        void Unload();
        void CheckMouse();
        void CheckKeyBoard();
    }
}
