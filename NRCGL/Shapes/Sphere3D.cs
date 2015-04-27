#region License
//
// The NRCGL License.
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Nuno Ramalho da Costa
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
#endregion

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
        private static VertexsIndicesData vid_smooth;


        public float R
        {
            get { return r; }
            set { r = value; }
        }
        

        public Sphere3D(Vector3 position, float r, Color4 color, 
            int textureId = 0, bool isSmoothShading = true) : base()
        {
            this.r = r;
            IsSmoothShading = isSmoothShading;
            Bounding = new Bounding(this, r);

            Model = @"Models\sphere3D64x64x1.xml";

            if (isSmoothShading)
            {
                // smooth shading
                if (vid_smooth is VertexsIndicesData)
                {
                    VertexsIndicesData = vid_smooth;
                }
                else
                {
                    vid_smooth = Tools.DeserializeModel(@"Models\sphere3D64x64x1_smooth.xml");

                    VertexsIndicesData = vid_smooth;
                }
            }
            else
            {
                // flat shading
                if (vid is VertexsIndicesData)
                {
                    VertexsIndicesData = vid;
                }
                else
                {
                    vid = Tools.DeserializeModel(Model);

                    VertexsIndicesData = vid;
                }
            }
            

            


            

            Scale(r);

            Position = position;

            FirstPosition = Position;

            ShapeVersorsUVW = Matrix4.Identity;

            TextureID = textureId;

            if (textureId == 0)
            {
                foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    vertex.Color = color;

                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_Color_Normal1.txt");
                string fs = File.ReadAllText("Shaders\\fShader_Color_Normal1.txt");
                Shader = new Shader(ref vs, ref fs, this);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat, 97920, BeginMode.Triangles);
            }
            else
            {
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_sphere.txt");
                Shader = new Shader(ref vs, ref fs, this);
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
