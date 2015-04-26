using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Shapes
{
    class Torus3D : Shape3D
    {
        private static VertexsIndicesData vid;

        public Torus3D(Vector3 position, float scale, Color4 color, int textureId = 0)
            : base()
        {
            Bounding = new Bounding(this, scale);

            Model = @"Models\Torus.xml";

            if (vid is VertexsIndicesData)
            {
                VertexsIndicesData = vid;
            }
            else
            {
                vid = Tools.DeserializeModel(Model);

                VertexsIndicesData = vid;
            }


            Scale(scale);

            Position = position;

            FirstPosition = Position;

            ShapeVersorsUVW = Matrix4.Identity;

            TextureID = textureId;

            if (textureId == 0)
            {
                foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    vertex.Color = color;

                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_Color_Normal_Torus.txt");
                string fs = File.ReadAllText("Shaders\\fShader_Color_Normal_Torus.txt");
                Shader = new Shader(ref vs, ref fs, this);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 7650, BeginMode.Triangles);
            }
            else
            {
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_sphere.txt");
                Shader = new Shader(ref vs, ref fs, this);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_UV_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 7650);

            }
            
        }
    }
}
