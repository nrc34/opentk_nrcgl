using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    abstract class Level : IWGameable, ILevel
    {
        public bool IsFinished { get; set; }

        public Dictionary<string, Shape3D> Shapes { get; set; }

        public Camera Camera { get; set; }

        public Audio.Audio Audio { get; set; }


        #region protected members

        protected string BlocksMapFilePath { get; set; }

        #endregion

        #region IWGameable implementation

        public abstract void Load();

        public abstract void Update();

        public abstract void Render();

        public abstract void CheckKeyBoard();

        #endregion

        #region ILevel implementation

        public abstract void LoadResources();

        #endregion
    }
}
