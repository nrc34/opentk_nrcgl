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
    class TextRender
    {
        private Bitmap text_bmp;


        public int TextTexture { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Vector2 Position { get; set; }

        public bool Visible { get; set; }

        public string Text { get; set; }

        public Font Font { get; set; }

        public FontStyle FontStyle { get; set; }

        public Brush TextBrush { get; set; }

        public Color BackgroundColor { get; set; }

        public Shader Shader { get; set; }

        public VertexFormat VertexFormat { get; set; }

        public VertexFloatBuffer VertexBuffer { get; set; }

        public VertexsIndicesData VertexsIndicesData { get; set; }


        public TextRender(int width, int height, Vector2 position,
                   FontFamily fontFamily, float fontSize = 12, bool visible = true)
        {
            Width = width;
            Height = height;
            Position = position;

            Font = new System.Drawing.Font(fontFamily,
                                           fontSize, 
                                           System.Drawing.FontStyle.Regular);
            TextBrush = Brushes.White;
            BackgroundColor = Color.FromArgb(150, 0, 0, 0);
            FontStyle = System.Drawing.FontStyle.Regular;

            Visible = visible;

            VertexsIndicesData =
                Tools.DeserializeModel(@"Models\Panel3D.xml");

            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_UV_Text.txt");
            string fs = File.ReadAllText("Shaders\\fShader_UV_Text.txt");
            Shader = new Shader(ref vs, ref fs, this);
            // initialize buffer
            VertexFormat = NRCGL.VertexFormat.XY_UV;
            VertexBuffer = new VertexFloatBuffer(VertexFormat, 512);
        }

        public virtual void DrawBufer(VertexFloatBuffer buffer, VertexFormat vertexFormat)
        {
            switch (vertexFormat)
            {
                case VertexFormat.XY:
                    break;
                case VertexFormat.XY_COLOR:
                    break;
                case VertexFormat.XY_UV:
                    #region xyz_normal_uv
                    foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    {
                        buffer.AddVertex(vertex.Position.X, vertex.Position.Y,
                        vertex.TexCoord.X, vertex.TexCoord.Y);
                    }
                    #endregion
                    break;
                case VertexFormat.XY_UV_COLOR:
                    break;
                case VertexFormat.XYZ:
                    break;
                case VertexFormat.XYZ_COLOR:
                    break;
                case VertexFormat.XYZ_UV:
                    break;
                case VertexFormat.XYZ_UV_COLOR:
                    break;
                case VertexFormat.XYZ_NORMAL_COLOR:
                    #region xyz_normal_color
                    foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    {
                        buffer.AddVertex(vertex.Position.X, vertex.Position.Y, vertex.Position.Z,
                        vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z,
                        vertex.Color.R, vertex.Color.G, vertex.Color.B, vertex.Color.A);
                    }
                    #endregion
                    break;
                case VertexFormat.XYZ_NORMAL_UV:
                    #region xyz_normal_uv
                    foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    {
                        buffer.AddVertex(vertex.Position.X, vertex.Position.Y, vertex.Position.Z,
                        vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z,
                        vertex.TexCoord.X, vertex.TexCoord.Y);
                    }
                    #endregion
                    break;
                case VertexFormat.XYZ_NORMAL_UV_COLOR:
                    #region xyz_normal_uv_color
                    foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    {
                        buffer.AddVertex(vertex.Position.X, vertex.Position.Y, vertex.Position.Z,
                        vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z,
                        vertex.TexCoord.X, vertex.TexCoord.Y,
                        vertex.Color.R, vertex.Color.G, vertex.Color.B, vertex.Color.A);
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        public void Load(int width, int height)
        {
            // Create Bitmap and OpenGL texture
            text_bmp = new Bitmap(Width, Height); // window size

            //TextTexture = Texture.Load(text_bmp);

            Text = "";

            using (Graphics gfx = Graphics.FromImage(text_bmp))
            {
                gfx.Clear(BackgroundColor);
                gfx.DrawString(Text, new Font(Font, 
                                              FontStyle), 
                                              TextBrush, 0, 0);
            }

            TextTexture = Texture.Load(text_bmp);

            GL.UseProgram(Shader.Program);

            Matrix4 SM = Matrix4.CreateScale((float)Width/width, (float)Height/height, 0f);

            float pixelW = (float)(1 / (float)(width / 2));
            float pixelH = (float)(1 / (float)(height / 2));

            Matrix4 TM = Matrix4.CreateTranslation(
                    (float)(Position.X - (float)(width / 2) + (float)Width / 2) * pixelW, 
                    (float)(-Position.Y + (float)(height / 2) - (float)Height / 2) * pixelH,
                    0f);

            Matrix4 ModelViewMatrix = SM * TM;

            Matrix4 ProjectionMatrix = Matrix4.Identity;

            int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
            GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);

            int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
            GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextTexture);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            DrawBufer(VertexBuffer, VertexFormat);

            VertexBuffer.IndexFromLength();
            VertexBuffer.Load();
            //VertexBuffer.Reload();



            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Update(string text)
        {
            if (text == Text) return;
            
            Text = text;

            using (Graphics gfx = Graphics.FromImage(text_bmp))
            {
                gfx.Clear(BackgroundColor);
                gfx.DrawString(Text, new Font(Font,
                                              FontStyle),
                                              TextBrush, 0, 0);
            }

            GL.DeleteTexture(TextTexture);

            TextTexture = Texture.Load(text_bmp);
        }

        public void Render()
        {

            if (!Visible) return;

            GL.UseProgram(Shader.Program);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextTexture);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
