using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.Game
{
    abstract class GameEntity
    {
        public abstract void Load();

        public abstract void Update();

        public abstract void Render();

    }
}
