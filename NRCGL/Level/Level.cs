using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    abstract class Level : ILevel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public GameWindow GameWindow { get; set; }

        public bool IsFinished { get; set; }

        public Dictionary<string, Shape3D> Shapes3D { get; set; }

        public Dictionary<string, int> Textures { get; set; }

        public ShadowMap ShadowMap { get; set; }

        public Camera Camera { get; set; }

        public Audio.Audio Audio { get; set; }

        public TextRender TextRender { get; set; }



        #region ILevel implementation

        public abstract void Load();

        public abstract void Unload();

        #region IWGable implementation

        public abstract void Update();

        public abstract void Render();

        #endregion

        public abstract void CheckMouse();

        public abstract void CheckKeyBoard();

        #endregion

    }
}
