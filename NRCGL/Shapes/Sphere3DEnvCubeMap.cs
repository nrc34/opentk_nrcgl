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

namespace OpenTK_NRCGL.NRCGL.Shapes
{
    class Sphere3DEnvCubeMap : Shape3D
    {
        private float r;

        private int[] textureCubeFBO;
        private int textureCubeDepth;
        private int textSize = 512;
        private static VertexsIndicesData vid;
        private GameWindow gameWindow;
        

        public Sphere3DEnvCubeMap(Vector3 position, float r) : base()
        {
            this.r = r;

            Bounding = new Bounding(this, r * 1.5f);

            if (vid is VertexsIndicesData)
            {
                VertexsIndicesData = vid;
            }
            else
            {
                vid = Tools.DeserializeModel(@"Models\sphere3D64x64x1.xml");
                //vid = Tools.DeserializeModel(@"Models\cube3D1x1.xml");
                VertexsIndicesData = vid;
            }
            

            Model = @"Models\Sphere3D.xml";

            //RotateU(MathHelper.PiOver2);

            Scale(r);

            Position = position;

            ShapeVersorsUVW = Matrix4.Identity;

            //GL.Disable(EnableCap.Texture2D);

            //GL.Enable(EnableCap.TextureCubeMap);

            // create cubemap

            int cubemapTexture = 0;

            cubemapTexture = GL.GenTexture();
            TextureID = cubemapTexture;
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemapTexture);
            //GL.TexStorage2D(TextureTarget2d.TextureCubeMap, 0, SizedInternalFormat.Rgba8, textSize, textSize);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            foreach (TextureTarget target in new TextureTarget[] {
				TextureTarget.TextureCubeMapPositiveX,
				TextureTarget.TextureCubeMapNegativeX,
				TextureTarget.TextureCubeMapPositiveY,
				TextureTarget.TextureCubeMapNegativeY,
				TextureTarget.TextureCubeMapPositiveZ,
				TextureTarget.TextureCubeMapNegativeZ,
			}) {
                GL.TexImage2D(target, 0, PixelInternalFormat.Rgba8, textSize, textSize, 0, 
                    PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
			}
            
            // create FBO
            textureCubeFBO = new int[6];

            for (int i = 0; i < 6; i++)
                GL.GenFramebuffers(1, out textureCubeFBO[i]);

			GL.GenRenderbuffers(1, out textureCubeDepth);

            for (int i = 0; i < 6; i++)
            {
                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, textureCubeFBO[i]);
                GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                    FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapPositiveX, cubemapTexture, 0);

                GL.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, textureCubeDepth);
                GL.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent24, textSize, textSize);
                GL.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt,
                    RenderbufferTarget.RenderbufferExt, textureCubeDepth);
            }
			

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_UV_Normal_CubeMap.txt");
            string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_CubeMap.txt");
            Shader = new Shader(ref vs, ref fs, this);
            // initialize buffer
            VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL;
            VertexBuffer = new VertexFloatBuffer(VertexFormat, 97920);

            
        }

        public override void Load()
        {
            //GL.UseProgram(Shader.Program);

            DrawBufer(VertexBuffer, VertexFormat);

            VertexBuffer.IndexFromLength();
            VertexBuffer.Load();

            //VertexBuffer.Bind(Shader);
            //GL.UseProgram(0);
        }

        public override void Update(Matrix4 ViewMatrix, Matrix4 ProjectionMatrix, 
            Dictionary<string, Shape3D> shapes3D, Camera mainCamera, GameWindow gameWindow)
        {

            this.gameWindow = gameWindow;

            //Quaternion qz = Quaternion.FromAxisAngle(Vector3.UnitZ, -Physic.Vxyz.X / r);
            //Quaternion qx = Quaternion.FromAxisAngle(Vector3.UnitX, Physic.Vxyz.Z / r);

            //Quaternion = qz * qx * Quaternion;

            base.Update(ViewMatrix, ProjectionMatrix, shapes3D, mainCamera, gameWindow);

            GL.UseProgram(Shader.Program);

            Matrix4 modelMatrix = ModelMatrix;
            int ModelMatrix_location = GL.GetUniformLocation(Shader.Program, "model_matrix");
            GL.UniformMatrix4(ModelMatrix_location, false, ref modelMatrix);
            /*
            Matrix4 NormalMatrix = ModelMatrix.Inverted();
            NormalMatrix.Transpose();
            int NormalMatrix_location = GL.GetUniformLocation(Shader.Program, "normal_matrix");
            GL.UniformMatrix4(NormalMatrix_location, false, ref NormalMatrix);
            */

            float[] eyePositionShader = new float[3] { -mainCamera.Position.X, 
                -mainCamera.Position.Y, -mainCamera.Position.Z};
            int light_position_location = GL.GetUniformLocation(Shader.Program, "eyePosition");
            GL.Uniform1(light_position_location, 1, eyePositionShader);
            
            GL.UseProgram(0);

            
            
            
            //GL.ClearColor(0.0f, 0.5f, 0.0f, 0.8f);
            
            //GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.TextureCubeMap);
            //GL.BindTexture(TextureTarget.TextureCubeMap, TextureID);

            for (int i = 0; i < 6; i++)
            {
                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, textureCubeFBO[i]);
                //GL.Enable(EnableCap.DepthTest);
                //GL.ClearDepth(1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                
                Camera camera = new Camera();
                //camera.Rotate(Vector3.UnitX, MyGame.XAngle);
                //camera.Rotate(Vector3.UnitY, MyGame.YAngle);
                //camera.Rotate(Vector3.UnitZ, MyGame.ZAngle);
                camera.Position = new Vector3(-Position.X, -Position.Y, -Position.Z);
                //camera.Quaternion = mainCamera.Quaternion;
                //camera.Quaternion.Invert();
                
                switch (i)
                {
                    case 0:
                        camera.RotateV(-MathHelper.PiOver2);
                        //camera.Rotate(mainCamera.CameraUVW.Column1.Xyz, -MathHelper.PiOver2);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapPositiveX, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(58.8f), MyGame.Width / (float)MyGame.Height, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix, i);
                        break;
                    case 1:
                        camera.RotateV(MathHelper.PiOver2);
                        //camera.Rotate(mainCamera.CameraUVW.Column1.Xyz, MathHelper.PiOver2);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapNegativeX, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix1 = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(58.8f), MyGame.Width / (float)MyGame.Height, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix1, i);
                        break;
                    case 2:
                        camera.RotateU(MathHelper.PiOver2);
                        camera.RotateW(MathHelper.Pi);
                        //camera.Rotate(mainCamera.CameraUVW.Column0.Xyz, -MathHelper.PiOver2);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapPositiveY, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix2 = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(121.1999f), 1f, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix2, i);
                        break;
                    case 3:
                        camera.RotateU(-MathHelper.PiOver2);
                        camera.RotateW(MathHelper.Pi);
                        //camera.Rotate(mainCamera.CameraUVW.Column0.Xyz, MathHelper.PiOver2);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapNegativeY, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix3 = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(121.1999f), 1f, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix3, i);
                        break;
                    case 4:
                        camera.RotateV(MathHelper.Pi);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapPositiveZ, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix4 = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(58.8f), MyGame.Width / (float)MyGame.Height, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix4, i);
                        break;
                    case 5:
                        //camera.Rotate(mainCamera.CameraUVW.Column1.Xyz, MathHelper.Pi);
                        GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureCubeMapNegativeZ, TextureID, 0);

                        Matrix4 CubeFaceProjectionMatrix5 = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(58.8f), MyGame.Width / (float)MyGame.Height, 0.2f, 10000f);
                        renderScene(shapes3D, camera, ProjectionMatrix, CubeFaceProjectionMatrix5, i);
                        break;
                    default:
                        break;
                }
                //GL.BindTexture(TextureTarget.TextureCubeMap, 0);
                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

            }

            GL.Viewport(0, 0, MyGame.Width, MyGame.Height);
        }

        private void renderScene(Dictionary<string, Shape3D> shapes3D, Camera camera, 
            Matrix4 ProjectionMatrix, Matrix4 CubeFaceProjectionMatrix, int face)
        {
            camera.Update();

            //Matrix4 CubeFaceProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
            //    MyGame.FOV, MyGame.Width / (float)MyGame.Height, 0.2f, 10000f);
            //Matrix4 CubeFaceProjectionMatrix = Matrix4.CreateOrthographic(2, 2, 0.1f, 10000); //MyGame.Width/ (float) MyGame.Height

            GL.Viewport(0, 0, textSize, textSize);

            foreach (var item in shapes3D)
            {
                if (item.Value is Sphere3DEnvCubeMap) continue;

                GL.UseProgram(item.Value.Shader.Program);


                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureID);
                GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit0"), 
                    TextureUnit.Texture0 - TextureUnit.Texture0);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureShadowMap);
                GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit1"),
                    TextureUnit.Texture1 - TextureUnit.Texture0);

                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureBumpMap);
                GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit2"), TextureUnit.Texture2 - TextureUnit.Texture0);


                Matrix4 shadowMatrix = ShadowMatrix;

                int ShadowMatrix_location = GL.GetUniformLocation(item.Value.Shader.Program, "shadow_matrix");
                GL.UniformMatrix4(ShadowMatrix_location, false, ref shadowMatrix);

                Matrix4 ModelViewMatrix = item.Value.ModelMatrix * camera.View;

                int ModelviewMatrix_location = GL.GetUniformLocation(item.Value.Shader.Program, "modelview_matrix");
                GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);

                int ProjectionMatrix_location = GL.GetUniformLocation(item.Value.Shader.Program, "projection_matrix");
                GL.UniformMatrix4(ProjectionMatrix_location, false, ref CubeFaceProjectionMatrix);

                int light_position_location = GL.GetUniformLocation(item.Value.Shader.Program, "light_position");
                GL.Uniform3(light_position_location, item.Value.Light.DirectionalLightPosition);

                item.Value.VertexBuffer.Bind(item.Value.Shader);

                ModelViewMatrix = item.Value.ModelViewMatrix;
                GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
                GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
                //GL.UseProgram(0);

                //GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            if (Game.MyGame.MultiView)
            {

                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                //GL.ClearColor(0.9f, 0.9f, 0.1f, 1.0f);

                GL.Viewport(64 * face, MyGame.Height, 64, 64);

                foreach (var item in shapes3D)
                {
                    if (item.Value is Sphere3DEnvCubeMap) continue;

                    GL.UseProgram(item.Value.Shader.Program);


                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureID);
                    GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit0"), 
                        TextureUnit.Texture0 - TextureUnit.Texture0);

                    GL.ActiveTexture(TextureUnit.Texture1);
                    GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureShadowMap);
                    GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit1"),
                        TextureUnit.Texture1 - TextureUnit.Texture0);

                    GL.ActiveTexture(TextureUnit.Texture2);
                    GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureBumpMap);
                    GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit2"), TextureUnit.Texture2 - TextureUnit.Texture0);

                    Matrix4 shadowMatrix = ShadowMatrix;

                    int ShadowMatrix_location = GL.GetUniformLocation(item.Value.Shader.Program, "shadow_matrix");
                    GL.UniformMatrix4(ShadowMatrix_location, false, ref shadowMatrix);
                    

                    Matrix4 ModelViewMatrix = item.Value.ModelMatrix * camera.View;

                    int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
                    GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);

                    int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
                    GL.UniformMatrix4(ProjectionMatrix_location, false, ref CubeFaceProjectionMatrix);

                    int light_position_location = GL.GetUniformLocation(Shader.Program, "light_position");
                    GL.Uniform3(light_position_location, Light.DirectionalLightPosition);

                    item.Value.VertexBuffer.Bind(item.Value.Shader);

                    ModelViewMatrix = item.Value.ModelViewMatrix;
                    GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
                    GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
                    GL.UseProgram(0);

                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
                if (false)
                {
                    GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                    //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    //GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

                    GL.Viewport(MyGame.Width, 0, 256, 256);

                    foreach (var item in shapes3D)
                    {
                        if (item.Key.Contains("CubeMap")) continue;

                        GL.UseProgram(item.Value.Shader.Program);


                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureID);
                        GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit0"), 
                            TextureUnit.Texture0 - TextureUnit.Texture0);

                        GL.ActiveTexture(TextureUnit.Texture1);
                        GL.BindTexture(TextureTarget.Texture2D, item.Value.TextureShadowMap);
                        GL.Uniform1(GL.GetUniformLocation(item.Value.Shader.Program, "TextureUnit1"),
                            TextureUnit.Texture1 - TextureUnit.Texture0);

                        Matrix4 ModelViewMatrix = item.Value.ModelMatrix * camera.View;

                        int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
                        GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);

                        int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
                        GL.UniformMatrix4(ProjectionMatrix_location, false, ref CubeFaceProjectionMatrix);

                        int light_position_location = GL.GetUniformLocation(Shader.Program, "light_position");
                        GL.Uniform3(light_position_location, Light.DirectionalLightPosition);

                        item.Value.VertexBuffer.Bind(item.Value.Shader);

                        ModelViewMatrix = item.Value.ModelViewMatrix;
                        GL.UniformMatrix4(ModelviewMatrix_location, false, ref ModelViewMatrix);
                        GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
                        GL.UseProgram(0);

                        GL.BindTexture(TextureTarget.Texture2D, 0);
                    }
                }

                gameWindow.SwapBuffers();

               // GL.BindFramebuffer(FramebufferTarget.FramebufferExt, textureCubeFBO);
            }
        }

        public override void Render()
        {

            GL.UseProgram(Shader.Program);

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.TextureCubeMap, TextureID);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "cubeMapTex"), 
                TextureUnit.Texture3 - TextureUnit.Texture0);

            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

        }

    }
    
}
