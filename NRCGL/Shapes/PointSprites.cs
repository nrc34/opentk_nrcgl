using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Shapes
{
    class PointSprites : Shape3D
    {

        public int NumberOfSprites 
        { get; set; }


        public PointSprites(string name, Vector3 position, float scale, int spriteTextureId = 0)
            : base()
        {
            Name = name;

            Bounding = new Bounding(this, scale);

            Scale(scale);

            Position = position;

            FirstPosition = Position;

            ShapeVersorsUVW = Matrix4.Identity;

            TextureID = spriteTextureId;

            

            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_PointSprites.txt");
            string fs = File.ReadAllText("Shaders\\fShader_PointSprites.txt");
            Shader = new Shader(ref vs, ref fs, this);

            NumberOfSprites = 1500;

            // initialize buffer
            VertexFormat = NRCGL.VertexFormat.XYZW;
            VertexBuffer = new VertexFloatBuffer(VertexFormat, NumberOfSprites, BeginMode.Points);
            

            GL.Enable(EnableCap.PointSprite);

            GL.Enable(EnableCap.VertexProgramPointSize);
        }

        public override void Load()
        {
            GL.UseProgram(Shader.Program);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            DrawBuferSpritesPoints(NumberOfSprites);

            VertexBuffer.IndexFromLength();
            VertexBuffer.Load();
            //VertexBuffer.Reload();



            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void DrawBuferSpritesPoints(int NumberOfSprites)
        {
            for (int i = 0; i < NumberOfSprites; i++)
            {
                var r = new Random((int)DateTime.Now.Ticks);

                float x = (float)r.NextDouble() * 110 - 55;
                float y = (float)r.NextDouble() * 110 - 55;
                float z = (float)r.NextDouble() * 110 - 55;
                float w = (float)r.NextDouble() * 55;

                VertexBuffer.AddVertex(x, y, z, w);

                Thread.Sleep(1);
            }

        }

        
    }
}
