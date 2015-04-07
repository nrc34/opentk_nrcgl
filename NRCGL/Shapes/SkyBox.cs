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
    class SkyBox : Shape3D
    {
        public SkyBox(Vector3 centerPosition, Color4 color, int textureId = 0)
            : base()
        {
            Bounding = new Bounding(this, 1, 1, 1);
            Scale(1);
            VertexsIndicesData =
                Tools.DeserializeModel(@"Models\skyBox.xml");

            Model = @"Models\skyBox.xml";
            Position = centerPosition;
            ShapeVersorsUVW = Matrix4.Identity;
            LightPosition = new Vector3(1000f, 1000f, 1000f);
            TextureID = textureId;
            if (textureId == 0)
            {
                foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    vertex.Color = color;

                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_Color_Normal1.txt");
                string fs = File.ReadAllText("Shaders\\fShader_Color_Normal1.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 512, BeginMode.Triangles);
            }
            else
            {
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal_SkyBox.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_SkyBox.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_UV_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 512);

            }
        }
    }
}
