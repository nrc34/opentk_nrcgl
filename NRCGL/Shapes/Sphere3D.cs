using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class Sphere3D : Shape3D
    {
        private float r;

        private static VertexsIndicesData vid;

        public Sphere3D(Vector3 position, float r, Color4 color , int textureId = 0) : base()
        {
            this.r = r;

            Bounding = new Bounding(this, r);

            if (vid is VertexsIndicesData)
            {
                VertexsIndicesData = vid;
            }
            else
            {
                vid = Tools.DeserializeModel(@"Models\sphere3D128x128x1.xml");

                VertexsIndicesData = vid;
            }
            

            Model = @"Models\Sphere3D.xml";

            Scale(r);

            Position = position;

            FirstPosition = Position;

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
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 97920, BeginMode.Triangles);
            }
            else
            {
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_sphere.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_UV_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 97920);

            }
            
        }

        public override void Update(Matrix4 ViewMatrix, Matrix4 ProjectionMatrix,
            Dictionary<string, Shape3D> shapes3D, Camera mainCamera, GameWindow gameWindow)
        {


            //RotateZ(-Physic.Vxyz.X / 2f);

            Quaternion qz = Quaternion.FromAxisAngle(Vector3.UnitZ, -Physic.Vxyz.X / r);
            Quaternion qx = Quaternion.FromAxisAngle(Vector3.UnitX, Physic.Vxyz.Z / r);
            //Quaternion qy = Quaternion.FromAxisAngle(Vector3.UnitY, Physic.Vxyz.X / r);

            Quaternion = qz * qx * Quaternion;
            
            base.Update(ViewMatrix, ProjectionMatrix, shapes3D, mainCamera, gameWindow);
        }

    }
}
