using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK_NRCGL.NRCGL.Level;
using OpenTK_NRCGL.NRCGL;

namespace OpenTK_NRCGL.Game
{
    class MyGameLevel : GameLevel
    {
        protected string BlocksMapFilePath { get; set; }

        public MyGameLevel(int id, string name)
            : base(id, name)
        {
            Textures.Add("textOne",
                         Texture.LoadTexture(@"Textures\sand_texture1037.jpg",
                         0, false, false));
        }

    }
}
