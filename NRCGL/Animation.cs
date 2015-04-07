using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    /// <summary>
    /// Animation to be aplied to the Shape at the tick game function
    /// </summary>
    abstract class Animation
    {
        public int TicksCount { get; set; }

        public int Ticks { get; set; }

        public Shape3D Shape3D { get; set; }

        public abstract bool Animate();
        
    }
}
