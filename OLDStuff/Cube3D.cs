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
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class Cube3D : Shape3D
    {
        private Color topFaceColor = Color.Blue;
        private Color bottomFaceColor = Color.Orange;
        private Color rightFaceColor = Color.Green;
        private Color leftFaceColor = Color.Brown;
        private Color frontFaceColor = Color.Red;
        private Color backFaceColor = Color.Black;
        private bool useTexture;
        private Vector3 position;

        public override Vector3 Position
        {
            get { return position; }
            set
            {
                Vector3 deltaPosition = value - position;

                for (int i = 0; i < ShapeMesh.Count; i++)
                {
                    ShapeMesh[i] = Matrix4.Mult(ShapeMesh[i],
                        Matrix4.CreateTranslation(deltaPosition.X,
                        deltaPosition.Y, deltaPosition.Z));
                }
                position = value;
            }
        }

        /// <summary>
        /// A cube with edge size at position (0,0,0) with default face colors
        /// </summary>
        /// <param name="size">size of the cube edge</param>
        public Cube3D(float size)
        {
            ShapeMesh = Mesh.Cube(size);
            Position = Vector3.Zero;
            useTexture = false;
            ShapeVersorsUVW = Matrix4.Identity;
            LightPosition = new Vector3(1000f, 1000f, 1000f);
            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_Color_Normal.txt");
            string fs = File.ReadAllText("Shaders\\fShader_Color_Normal.txt");
            Shader = new Shader(ref vs, ref fs);
            // initialize buffer
            VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_COLOR;
            VertexBuffer = new VertexFloatBuffer(VertexFormat);
        }

        /// <summary>
        /// A cube with edge size with position shapePosition vector3 with a face color
        /// </summary>
        /// <param name="size"></param>
        /// <param name="shapePosition"></param>
        public Cube3D(float size, Vector3 shapePosition, Color facesColor, 
            bool useTexture = false, int textureId = 0)
        {
            ShapeMesh = Mesh.Cube(size);
            Position = shapePosition;
            ShapeVersorsUVW = Matrix4.Identity;
            faceColors(facesColor);
            this.useTexture = useTexture;
            TextureID = textureId;
            LightPosition = new Vector3(1000f, 1000f, 1000f);
            if (useTexture)
            {
                TexCoords = LoadTexCoords();
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_UV_Normal.txt");
                string fs = File.ReadAllText("Shaders\\fShader_UV_Normal.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_UV;
                VertexBuffer = new VertexFloatBuffer(VertexFormat);
            }
            else
            {
                // initialize shaders
                string vs = File.ReadAllText("Shaders\\vShader_Color_Normal.txt");
                string fs = File.ReadAllText("Shaders\\fShader_Color_Normal.txt");
                Shader = new Shader(ref vs, ref fs);
                // initialize buffer
                VertexFormat = NRCGL.VertexFormat.XYZ_NORMAL_COLOR;
                VertexBuffer = new VertexFloatBuffer(VertexFormat);
            }
        }

        /// <summary>
        /// Load the List of Matriz4 with texture coords.
        /// </summary>
        /// <returns></returns>
        private List<Matrix4> LoadTexCoords()
        {
            IList<Vector2> textureCoordinates = new List<Vector2>();

            textureCoordinates.Add(new Vector2(0, 0));                      //AA - 0
            textureCoordinates.Add(new Vector2(0, 0.3333333f));             //AB - 1
            textureCoordinates.Add(new Vector2(0, 0.6666666f));             //AC - 2
            textureCoordinates.Add(new Vector2(0, 1));                      //AD - 3
            textureCoordinates.Add(new Vector2(0.3333333f, 0));             //BA - 4
            textureCoordinates.Add(new Vector2(0.3333333f, 0.3333333f));    //BB - 5
            textureCoordinates.Add(new Vector2(0.3333333f, 0.6666666f));    //BC - 6
            textureCoordinates.Add(new Vector2(0.3333333f, 1));             //BD - 7
            textureCoordinates.Add(new Vector2(0.6666666f, 0));             //CA - 8
            textureCoordinates.Add(new Vector2(0.6666666f, 0.3333333f));    //CB - 9
            textureCoordinates.Add(new Vector2(0.6666666f, 0.6666666f));    //CC -10
            textureCoordinates.Add(new Vector2(0.6666666f, 1));             //CD -11
            textureCoordinates.Add(new Vector2(1, 0));                      //DA -12
            textureCoordinates.Add(new Vector2(1, 0.3333333f));             //DB -13
            textureCoordinates.Add(new Vector2(1, 0.6666666f));             //DC -14
            textureCoordinates.Add(new Vector2(1, 1));                      //DD -15

            List<Matrix4> listMatrix4TexCoords = new List<Matrix4>();

            // front face
            #region front face texcoords
            Matrix4 frontFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[1]),
                                                     new Vector4(textureCoordinates[0]),
                                                     new Vector4(textureCoordinates[5]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(frontFaceTexCoord1);

            Matrix4 frontFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[5]),
                                                     new Vector4(textureCoordinates[0]),
                                                     new Vector4(textureCoordinates[4]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(frontFaceTexCoord2);
            #endregion

            // right face
            #region right face texcoords
            Matrix4 rightFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[2]),
                                                     new Vector4(textureCoordinates[1]),
                                                     new Vector4(textureCoordinates[6]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(rightFaceTexCoord1);

            Matrix4 rightFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[1]),
                                                     new Vector4(textureCoordinates[5]),
                                                     new Vector4(textureCoordinates[6]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(rightFaceTexCoord2);
            #endregion

            // top face
            #region top face texcoords
            Matrix4 topFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[5]),
                                                     new Vector4(textureCoordinates[9]),
                                                     new Vector4(textureCoordinates[10]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(topFaceTexCoord1);

            Matrix4 topFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[10]),
                                                     new Vector4(textureCoordinates[6]),
                                                     new Vector4(textureCoordinates[5]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(topFaceTexCoord2);
            #endregion

            // bottom face
            #region bottom face texcoords
            Matrix4 bottomFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[10]),
                                                     new Vector4(textureCoordinates[9]),
                                                     new Vector4(textureCoordinates[14]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(bottomFaceTexCoord1);

            Matrix4 bottomFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[13]),
                                                     new Vector4(textureCoordinates[14]),
                                                     new Vector4(textureCoordinates[9]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(bottomFaceTexCoord2);
            #endregion

            // left face
            #region left face texcoords
            Matrix4 leftFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[8]),
                                                     new Vector4(textureCoordinates[13]),
                                                     new Vector4(textureCoordinates[9]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(leftFaceTexCoord1);

            Matrix4 leftFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[8]),
                                                     new Vector4(textureCoordinates[12]),
                                                     new Vector4(textureCoordinates[13]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(leftFaceTexCoord2);
            #endregion

            // back face
            #region back face texcoords
            Matrix4 backFaceTexCoord1 = new Matrix4(new Vector4(textureCoordinates[4]),
                                                     new Vector4(textureCoordinates[9]),
                                                     new Vector4(textureCoordinates[5]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(backFaceTexCoord1);

            Matrix4 backFaceTexCoord2 = new Matrix4(new Vector4(textureCoordinates[9]),
                                                     new Vector4(textureCoordinates[4]),
                                                     new Vector4(textureCoordinates[8]),
                                                     Vector4.Zero);
            listMatrix4TexCoords.Add(backFaceTexCoord2);
            #endregion

            return listMatrix4TexCoords;
        }

        public override void Rotate(Vector3 axis, float angle)
        {
         
            Vector3 positionB = new Vector3(position);
            positionB = Vector3.Multiply(positionB, -1f);
            // translates to (0,0,0) before rotate
            TranslateWC(positionB.X, positionB.Y, positionB.Z);

            for (int i = 0; i < ShapeMesh.Count; i++)
            {
                ShapeMesh[i] = Matrix4.Mult(ShapeMesh[i], Matrix4.CreateFromAxisAngle(axis, angle));
            }

            // translate to original position
            positionB = Vector3.Multiply(positionB, -1f);
            TranslateWC(positionB.X, positionB.Y, positionB.Z);

            //rotate shapeUVW
            ShapeVersorsUVW = Matrix4.Mult(ShapeVersorsUVW, Matrix4.CreateFromAxisAngle(axis, angle));
        }

        /// <summary>
        /// Rotate at X axis
        /// </summary>
        /// <param name="angle">Rotation angle</param>
        public override void RotateX(float angle)
        {
            for (int i = 0; i < ShapeMesh.Count; i++)
            {
                ShapeMesh[i] = Matrix4.Mult(ShapeMesh[i], Matrix4.CreateRotationX(angle));
            }

            //rotate shapeUVW
            ShapeVersorsUVW = Matrix4.Mult(ShapeVersorsUVW, Matrix4.CreateRotationX(angle));
        }

        /// <summary>
        /// Rotate at Y axis
        /// </summary>
        /// <param name="angle"></param>
        public override void RotateY(float angle)
        {
            for (int i = 0; i < ShapeMesh.Count; i++)
            {
                ShapeMesh[i] = Matrix4.Mult(ShapeMesh[i], Matrix4.CreateRotationY(angle));
            }


            //rotate shapeUVW
            ShapeVersorsUVW = Matrix4.Mult(ShapeVersorsUVW, Matrix4.CreateRotationY(angle));
        }

        public override void RotateZ(float angle)
        {
            for (int i = 0; i < ShapeMesh.Count; i++)
            {
                ShapeMesh[i] = Matrix4.Mult(ShapeMesh[i], Matrix4.CreateRotationZ(angle));
            }

            //rotate shapeUVW
            ShapeVersorsUVW = Matrix4.Mult(ShapeVersorsUVW, Matrix4.CreateRotationZ(angle));
        }

        /// <summary>
        /// Translates the cube x,y,z.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public override void TranslateWC(float x, float y, float z)
        {
            Vector3 position = Position;
            position.X += x;
            position.Y += y;
            position.Z += z;

            Position = position;
            
        }

        #region old draw
        /*
        /// <summary>
        /// Draws the cube vertexs
        /// </summary>
        public override void DirectDraw()
        {
            if (useTexture)
            {
                #region use texture

                GL.BindTexture(TextureTarget.Texture2D, TextureID);
                GL.Color4(Color.White);
                GL.Begin(BeginMode.Triangles);
                
                int t = 0;
                foreach (Matrix4 matrix4 in ShapeMesh)
                {
                    GL.Normal3(matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z);
                    GL.TexCoord2(TexCoords[t].Row0.X, TexCoords[t].Row0.Y);
                    GL.Vertex3(matrix4.Row0.X, matrix4.Row0.Y, matrix4.Row0.Z);

                    GL.Normal3(matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z);
                    GL.TexCoord2(TexCoords[t].Row1.X, TexCoords[t].Row1.Y);
                    GL.Vertex3(matrix4.Row1.X, matrix4.Row1.Y, matrix4.Row1.Z);

                    GL.Normal3(matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z);
                    GL.TexCoord2(TexCoords[t].Row2.X, TexCoords[t].Row2.Y);
                    GL.Vertex3(matrix4.Row2.X, matrix4.Row2.Y, matrix4.Row2.Z);

                    t++;
                }
                
                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, 0);

                #endregion
            }
            else
            {
                #region use color
                int count = 0;

                GL.Begin(BeginMode.Triangles);

                foreach (Matrix4 matrix4 in ShapeMesh)
                {
                    switch (count)
                    {
                        case 0:
                            GL.Color3(frontFaceColor);
                            break;
                        case 2:
                            GL.Color3(rightFaceColor);
                            break;
                        case 4:
                            GL.Color3(topFaceColor);
                            break;
                        case 6:
                            GL.Color3(bottomFaceColor);
                            break;
                        case 8:
                            GL.Color3(leftFaceColor);
                            break;
                        case 10:
                            GL.Color3(backFaceColor);
                            break;
                        default:
                            break;
                    }

                    GL.Normal3(matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z);
                    GL.Vertex3(matrix4.Row0.X, matrix4.Row0.Y, matrix4.Row0.Z);
                    GL.Vertex3(matrix4.Row1.X, matrix4.Row1.Y, matrix4.Row1.Z);
                    GL.Vertex3(matrix4.Row2.X, matrix4.Row2.Y, matrix4.Row2.Z);

                    count++;
                }
                GL.End();

                #endregion
            }
          
        }*/
        #endregion

        /// <summary>
        /// Draw shape to buffer and bind shader,
        /// </summary>
        public override void Draw()
        {
            GL.UseProgram(Shader.Program);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            DrawBufer(VertexBuffer, VertexFormat);

            VertexBuffer.IndexFromLength();
            VertexBuffer.Load();
            VertexBuffer.Reload();



            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Draws buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="vertexFormat"></param>
        public override void DrawBufer(VertexFloatBuffer buffer, VertexFormat vertexFormat)
        {

            switch (vertexFormat)
            {
                case VertexFormat.XY:
                    break;
                case VertexFormat.XY_COLOR:
                    break;
                case VertexFormat.XY_UV:
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
                    int count = 0;
                    Vector4 color = Vector4.Zero;
                    foreach (Matrix4 matrix4 in ShapeMesh)
                    {
                        switch (count)
                        {
                            case 0:
                                color = Tools.ColorToVector4(frontFaceColor);
                                break;
                            case 2:
                                color = Tools.ColorToVector4(rightFaceColor);
                                break;
                            case 4:
                                color = Tools.ColorToVector4(topFaceColor);
                                break;
                            case 6:
                                color = Tools.ColorToVector4(bottomFaceColor);
                                break;
                            case 8:
                                color = Tools.ColorToVector4(leftFaceColor);
                                break;
                            case 10:
                                color = Tools.ColorToVector4(backFaceColor);
                                break;
                            default:
                                break;
                        }

                        buffer.AddVertex(matrix4.Row0.X, matrix4.Row0.Y, matrix4.Row0.Z, 
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z, 
                            color.X, color.Y, color.Z, color.W);

                        buffer.AddVertex(matrix4.Row1.X, matrix4.Row1.Y, matrix4.Row1.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            color.X, color.Y, color.Z, color.W);

                        buffer.AddVertex(matrix4.Row2.X, matrix4.Row2.Y, matrix4.Row2.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            color.X, color.Y, color.Z, color.W);

                        count++;
                    }

                #endregion
                    break;
                case VertexFormat.XYZ_NORMAL_UV:
                #region xyz_normal_uv_color
                    if (useTexture == false) 
                        throw new ApplicationException("Use texture must be true.");
                    int count1 = 0;
                    foreach (Matrix4 matrix4 in ShapeMesh)
                    {

                        buffer.AddVertex(matrix4.Row0.X, matrix4.Row0.Y, matrix4.Row0.Z, 
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count1].Row0.X, 1-TexCoords[count1].Row0.Y);

                        buffer.AddVertex(matrix4.Row1.X, matrix4.Row1.Y, matrix4.Row1.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count1].Row1.X, 1-TexCoords[count1].Row1.Y);

                        buffer.AddVertex(matrix4.Row2.X, matrix4.Row2.Y, matrix4.Row2.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count1].Row2.X, 1-TexCoords[count1].Row2.Y);

                        count1++;
                    }

                #endregion
                    break;
                case VertexFormat.XYZ_NORMAL_UV_COLOR:
                 #region xyz_normal_uv_color
                    if (useTexture == false) 
                        throw new ApplicationException("Use texture must be true.");
                    int count2 = 0;
                    Vector4 color1 = Vector4.Zero;
                    foreach (Matrix4 matrix4 in ShapeMesh)
                    {
                        switch (count2)
                        {
                            case 0:
                                color1 = Tools.ColorToVector4(frontFaceColor);
                                break;
                            case 2:
                                color1 = Tools.ColorToVector4(rightFaceColor);
                                break;
                            case 4:
                                color1 = Tools.ColorToVector4(topFaceColor);
                                break;
                            case 6:
                                color = Tools.ColorToVector4(bottomFaceColor);
                                break;
                            case 8:
                                color1 = Tools.ColorToVector4(leftFaceColor);
                                break;
                            case 10:
                                color1 = Tools.ColorToVector4(backFaceColor);
                                break;
                            default:
                                break;
                        }

                        buffer.AddVertex(matrix4.Row0.X, matrix4.Row0.Y, matrix4.Row0.Z, 
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count2].Row0.X, TexCoords[count2].Row0.Y,
                            color1.X, color1.Y, color1.Z, color1.W);

                        buffer.AddVertex(matrix4.Row1.X, matrix4.Row1.Y, matrix4.Row1.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count2].Row1.X, TexCoords[count2].Row1.Y,
                            color1.X, color1.Y, color1.Z, color1.W);

                        buffer.AddVertex(matrix4.Row2.X, matrix4.Row2.Y, matrix4.Row2.Z,
                            matrix4.Row3.X, matrix4.Row3.Y, matrix4.Row3.Z,
                            TexCoords[count2].Row2.X, TexCoords[count2].Row2.Y,
                            color1.X, color1.Y, color1.Z, color1.W);

                        count2++;
                    }

                #endregion
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Sets faces colors with color.
        /// </summary>
        /// <param name="color"></param>
        private void faceColors(Color color)
        {
            topFaceColor = color;
            bottomFaceColor = color;
            rightFaceColor = color;
            leftFaceColor = color;
            frontFaceColor = color;
            backFaceColor = color;
        }

        public override void Load()
        {
            Draw();
        }

        public override void Render()
        {
            VertexBuffer.Clear();

            Draw();

        }

        public override void Update(Matrix4 ViewMatrix, Matrix4 ProjectionMatrix,
            Dictionary<string, Shape3D> shapes3D, Camera mainCamera, GameWindow gameWindow)
        {
            Tick();
            //send to shader
            GL.UseProgram(Shader.Program);
            //will return -1 without useprogram
            /*
            int mvp_matrix_location = GL.GetUniformLocation(Shader.Program, "mvp_matrix");
            GL.UniformMatrix4(mvp_matrix_location, false, ref MVP_Matrix);
            */
            int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
            GL.UniformMatrix4(ModelviewMatrix_location, false, ref ViewMatrix);

            int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
            GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
            /*
            int NormalMatrix_location = GL.GetUniformLocation(cube.Shader.Program, "normal_matrix");
            GL.UniformMatrix4(NormalMatrix_location, false, ref NormalMatrix);
            */

            float[] LightPosition = new float[4] { 100f, 100f, 100f, 0f };
            int light_position_location = GL.GetUniformLocation(Shader.Program, "light_position");
            GL.Uniform1(light_position_location, 1, LightPosition);

            float[] Kd = new float[3] { 0.9f, 0.5f, 0.3f };
            int Kd_location = GL.GetUniformLocation(Shader.Program, "kd");
            GL.Uniform1(Kd_location, 1, Kd);

            float[] Ld = new float[3] { 0.5f, 0.0f, 0.5f };
            int Ld_location = GL.GetUniformLocation(Shader.Program, "ld");
            GL.Uniform1(Ld_location, 1, Ld);

            GL.UseProgram(0);
        }
    }
}
