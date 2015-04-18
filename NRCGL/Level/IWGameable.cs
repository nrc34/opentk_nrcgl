using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    /// <summary>
    /// Define the necessary methods to be used at the GameWindow. 
    /// </summary>
    interface IWGameable
    {
        void Update();
        void Render();
    }
}
