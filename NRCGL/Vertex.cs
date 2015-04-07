using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    public class Vertex
    {
        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }

        public Vector2 TexCoord { get; set; }

        public Color4 Color { get; set; }
    }
}
