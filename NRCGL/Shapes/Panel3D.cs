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
    class Panel3D : Shape3D
    {
        public Panel3D(Vector3 centerPosition, float angleX, 
            float angleY, float angleZ, Color4 color, int textureId = 0)
            : base()
        {
            Bounding = new Bounding(this, 1, 0, 1);

            VertexsIndicesData =
                Tools.DeserializeModel(@"Models\Panel3D.xml");

            Model = @"Models\Panel3D.xml";
            Position = centerPosition;
            RotateX(angleX);
            RotateY(angleY);
            RotateZ(angleZ);
            ShapeVersorsUVW = Matrix4.Identity;
            LightPosition = new Vector3(0f, 100f, -150f);
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
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal_panel.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_panel.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_UV_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 512);

            }
        }
    }
}
