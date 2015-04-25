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
using OpenTK_NRCGL.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class ShadowMap
    {
        private Shader Shader;
        private Matrix4 ScaleBiasMatrix;
        private int textSize = 1024 * 3;
        private Matrix4 projectionMatrixLightView;



        public Matrix4 ProjectionMatrixLightView
        {
            get { return projectionMatrixLightView; }
            set { projectionMatrixLightView = value; }
        }
        
        public Camera LightView { get; set; }

        public Matrix4 ShadowMatrix { get; set; }

        public int DepthTexture { get; set; }

        public int FBO { get; set; }

        public ShadowMap(Camera lightView)
        {
            LightView = lightView;


            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_ShadowMap.txt");
            string fs = File.ReadAllText("Shaders\\fShader_ShadowMap.txt");
            Shader = new Shader(ref vs, ref fs, this);

            // initialize depth texture
            DepthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, DepthTexture);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24,
                textSize, textSize, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            // default filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            // depth comparison mode
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int) All.CompareRefToTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int) All.Lequal);
            // wrapping modes
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            // create FBO to render depth into
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            // attach the depth texture
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DepthTexture, 0);
            // disable color render
            GL.DrawBuffer(DrawBufferMode.None);

            // restore texture and framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // initialize scale bias matrix

            ScaleBiasMatrix = new Matrix4(new Vector4(0.5f, 0.0f, 0.0f, 0.0f),
                                          new Vector4(0.0f, 0.5f, 0.0f, 0.0f),
                                          new Vector4(0.0f, 0.0f, 0.5f, 0.0f),
                                          new Vector4(0.5f, 0.5f, 0.5f, 1f));

            //ScaleBiasMatrix.Transpose();

            ProjectionMatrixLightView = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                MyGame.Width / (float)MyGame.Height, 0.5f, 1000.0f);

            //ProjectionMatrixLightView = Matrix4.CreateOrthographic(104, 104, -1, 1);

            // create shadow matrix
            //ShadowMatrix = ScaleBiasMatrix * ProjectionMatrixLightView * LightView.View;
            ShadowMatrix = ProjectionMatrixLightView * ScaleBiasMatrix;
            //ShadowMatrix = Matrix4.LookAt(new Vector3(0, 100, 150), Vector3.Zero, Vector3.UnitY) * ShadowMatrix;
            ShadowMatrix = LightView.View * ShadowMatrix;
            //ShadowMatrix =  ScaleBiasMatrix * ProjectionMatrixLightView * Matrix4.LookAt(new Vector3(0, 100, 150), Vector3.Zero, Vector3.UnitY);
        }

        public void Update(Dictionary<string, Shape3D> shapes3D, Matrix4 ProjectionMatrix, GameWindow gameWindow)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            //GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
            //GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, DepthTexture, 0);

            //GL.Disable(EnableCap.DepthTest);

            GL.Viewport(0, 0, textSize, textSize);

            GL.ClearDepth(1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(2f, 4f);

            GL.UseProgram(Shader.Program);

            

            int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
            GL.UniformMatrix4(ProjectionMatrix_location, false, ref projectionMatrixLightView);

            foreach (var item in shapes3D)
            {


                if (!item.Value.IsVisible || !item.Value.IsShadowCaster) continue;

                //GL.ActiveTexture(TextureUnit.Texture0);
                //GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureID);
                //GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit0"),
                //    TextureUnit.Texture0 - TextureUnit.Texture0);

                //Matrix4 ModelViewMatrix = item.Value.ModelMatrix * Matrix4.LookAt(new Vector3(0, 100, 150), Vector3.Zero, Vector3.UnitY);

                Matrix4 ModelViewMatrix = item.Value.ModelMatrix * LightView.View;

                int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
                GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
               
                /*
                float[] LightPositionShader = new float[4] { -LightView.Position.X, 
                -LightView.Position.Y, -LightView.Position.Z, 0f };
                int light_position_location = GL.GetUniformLocation(item.Value.Shader.Program, "light_position");
                GL.Uniform1(light_position_location, 1, LightPositionShader);
                */

                item.Value.VertexBuffer.Bind(Shader);

                //ModelViewMatrix = item.Value.ModelViewMatrix;
                //GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
                //GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
                

            }

            GL.UseProgram(0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.PolygonOffsetFill);
            GL.Enable(EnableCap.DepthTest);

            if (MyGame.MultiView)
            {
                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                    //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    //GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

                GL.Viewport(MyGame.Width, 0, 256, 256);

                foreach (var item in shapes3D)
                {
                    if (!item.Value.IsVisible || !item.Value.IsShadowCaster) continue;
                    
                    GL.UseProgram(Shader.Program);


                    //GL.ActiveTexture(TextureUnit.Texture0);
                    //GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureID);
                    //GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit0"),
                    //    TextureUnit.Texture0 - TextureUnit.Texture0);

                    //Matrix4 ModelViewMatrix = item.Value.ModelMatrix * Matrix4.LookAt(new Vector3(0, 100, 150), Vector3.Zero, Vector3.UnitY);

                    Matrix4 ModelViewMatrix = item.Value.ModelMatrix * LightView.View;

                    int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
                    GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);

                    int ProjectionMatrix_location1 = GL.GetUniformLocation(Shader.Program, "projection_matrix");
                    GL.UniformMatrix4(ProjectionMatrix_location1, false, ref projectionMatrixLightView);
                    /*
                    float[] LightPositionShader = new float[4] { -LightView.Position.X, 
                    -LightView.Position.Y, -LightView.Position.Z, 0f };
                    int light_position_location = GL.GetUniformLocation(item.Value.Shader.Program, "light_position");
                    GL.Uniform1(light_position_location, 1, LightPositionShader);
                    */

                    item.Value.VertexBuffer.Bind(Shader);

                    //ModelViewMatrix = item.Value.ModelViewMatrix;
                    //GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
                    //GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
                    GL.UseProgram(0);

                }

                gameWindow.SwapBuffers();
            }

            GL.Viewport(0, 0, MyGame.Width, MyGame.Height);

        }

        public void UpdateView()
        {
            LightView.Update();

            ShadowMatrix = ProjectionMatrixLightView * ScaleBiasMatrix;
            //ShadowMatrix = Matrix4.LookAt(new Vector3(0, 100, 150), Vector3.Zero, Vector3.UnitY) * ShadowMatrix;
            ShadowMatrix = LightView.View * ShadowMatrix;
        }

        
    }
}
