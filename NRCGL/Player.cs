using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    abstract class Player
    {
        public string Name { get; set; }

        public List<string> FisishedLevels { get; set; }
    }
}
