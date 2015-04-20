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
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    /// <summary>
    /// Entity for geometry, texture and transformations of a 3D Shape
    /// </summary>
    class Shape3D
    {
        private List<Matrix4> shapeMesh;
        private Vector3 position;
        private bool collision;
        private Quaternion quaternion;
        private Vector3 scale;
        private Matrix4 shapeVersorsUVW;
        private int textureId;
        private int textureShadowMap;
        private int textureBumpMap;
        private List<Matrix4> texCoords;
        private Shader shader;
        private Color4 color;
        private VertexFloatBuffer vertexBuffer;
        private VertexFormat vertexFormat;
        private Vector3 lightPosition;
        private VertexsIndicesData vertexsIndicesData;
        private Physic physic;
        private Bounding bounding;
        private string model;
        private Matrix4 modelMatrix;
        private Matrix4 modelViewMatrix;
        private Matrix4 shadowMatrix;
        private Matrix4 texMatrix;
        public Vector3 firstPosition;
        private bool isVisible;
        private bool isShadowCaster;
        private SpotLight spotLight;



        

        public SpotLight SpotLight
        {
            get { return spotLight; }
            set { spotLight = value; }
        }
        

        public bool IsShadowCaster
        {
            get { return isShadowCaster; }
            set { isShadowCaster = value; }
        }
        

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        

        public Vector3 FirstPosition
        {
            get { return firstPosition; }
            set { firstPosition = value; }
        }
        

        public Matrix4 ShadowMatrix
        {
            get { return shadowMatrix; }
            set { shadowMatrix = value; }
        }
        

        public Matrix4 TexMatrix
        {
            get { return texMatrix; }
            set { texMatrix = value; }
        }
        

        public string Model
        {
            get { return model; }
            set { model = value; }
        }


        public Matrix4 ModelMatrix
        {
            get { return modelMatrix; }
            set { modelMatrix = value; }
        }


        public Matrix4 ModelViewMatrix
        {
            get { return modelViewMatrix; }
            set { modelViewMatrix = value; }
        }
        

        /// <summary>
        /// Texture ID
        /// </summary>
        public int TextureID
        {
            get { return textureId; }
            set { textureId = value; }
        }
        

        public int TextureShadowMap
        {
            get { return textureShadowMap; }
            set { textureShadowMap = value; }
        }
        

        public int TextureBumpMap
        {
            get { return textureBumpMap; }
            set { textureBumpMap = value; }
        }
        
        

        /// <summary>
        /// Manages the List of animations to apply to the shape
        /// </summary>
        public AnimationsManager AnimationsManager { get; set; }


        public bool Collision
        {
            get { return collision; }
            set { collision = value; }
        }
        

        /// <summary>
        /// Shape Position in WC
        /// </summary>
        public virtual Vector3 Position
        {
            get { return position; }
            set 
            {
                position = value;
            }
        }


        public Quaternion Quaternion
        {
            get { return quaternion; }
            set { quaternion = value; }
        }
        
        /// <summary>
        /// Shape physic variables.
        /// </summary>
        public Physic Physic
        {
            get { return physic; }
            set { physic = value; }
        }

        /// <summary>
        /// Shape Bounding volume to check for collisions.
        /// </summary>
        public Bounding Bounding
        {
            get { return bounding; }
            set { bounding = value; }
        }
        
        /// <summary>
        /// Matrix4 with shape local coordinate system versors(u, v, w).
        /// Row0 = ux, uy, uz, 0
        /// Row1 = vx, vy, vz, 0
        /// Row3 = wx, wy, wz. 0
        /// Row4 = not used
        /// 
        /// </summary>
        public Matrix4 ShapeVersorsUVW
        {
            get { return shapeVersorsUVW; }
            set { shapeVersorsUVW = value; }
        }
       
        /// <summary>
        /// Shape Mesh formed with Matrix4 that contains 
        /// a triangle with the normal.
        /// </summary>
        [Obsolete("Use VertexesIndices Model Class")]
        public List<Matrix4> ShapeMesh
        {
            get { return shapeMesh; }
            set { shapeMesh = value; }
        }
        
        /// <summary>
        /// List of shape texture coords. Only first 3 rows have data. 
        /// Convert Vector4 to Vector2
        /// </summary>
        [Obsolete("Use VertexesIndices Model Class")]
        public List<Matrix4> TexCoords
        {
            get { return texCoords; }
            set { texCoords = value; }
        }

        /// <summary>
        /// Vertexs data (position, normal, color, uv) and Indices.
        /// </summary>
        public VertexsIndicesData VertexsIndicesData
        {
            get { return vertexsIndicesData; }
            set { vertexsIndicesData = value; }
        }



        /// <summary>
        /// Initializes the shape with Position = Vector3.Zero and Front Direction
        /// </summary>
        public Shape3D()
        {
            position = Vector3.Zero;
            quaternion = Quaternion.Identity;
            shapeVersorsUVW = Matrix4.Identity;
            Physic = new Physic(this);
            collision = true;
            isVisible = true;
            isShadowCaster = true;

            SpotLight = new SpotLight
            {
                Color = Vector3.Zero,
                Intensity = 1f,
                Position = Vector3.Zero
            };
        }



        /// <summary>
        /// Traslate the shape in world coords.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void TranslateWC(float x, float y, float z)
        {
            position.X += x;
            position.Y += y;
            position.Z += z;

        }

        /// <summary>
        /// Rotate the shape using an axis and an angle
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        public virtual void Rotate(Vector3 axis, float angle)
        {
            //Quaternion
            Quaternion q = Quaternion.FromAxisAngle(axis, angle);
            Quaternion = q * Quaternion;

            shapeVersorsUVW = Matrix4.Mult(shapeVersorsUVW, Matrix4.CreateFromQuaternion(q));

        }

        /// <summary>
        /// Rotate at X axis
        /// </summary>
        /// <param name="angle">Rotation angle</param>
        public virtual void RotateX(float angle)
        {
            Rotate(Vector3.UnitX, angle);
        }

        /// <summary>
        /// Rotate at Y axis
        /// </summary>
        /// <param name="angle"></param>
        public virtual void RotateY(float angle)
        {
            Rotate(Vector3.UnitY, angle);
        }

        /// <summary>
        /// Rotate at Z axis
        /// </summary>
        /// <param name="angle"></param>
        public virtual void RotateZ(float angle)
        {
            Rotate(Vector3.UnitZ, angle);
        }

        /// <summary>
        /// Traslate the shape in shape coords.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void TranslateLC(float u, float v, float w)
        {
            // xTranslation =  ux * u + vx * v + wx * w
            // yTranslation =  uy * u + vy * v + wy * w
            // zTranslation =  uz * u + vz * v + wz * w

            Vector4 wcTranslation = Vector4.Transform(new Vector4(u, v, w, 0), shapeVersorsUVW);

            TranslateWC(wcTranslation.X, wcTranslation.Y, wcTranslation.Z);
        }

        /// <summary>
        /// Rotate at U axis
        /// </summary>
        /// <param name="angle"></param>
        public void RotateU(float angle)
        {
            Rotate(new Vector3(shapeVersorsUVW.Row0.X, 
                               shapeVersorsUVW.Row0.Y, 
                               shapeVersorsUVW.Row0.Z), angle);
        }

        /// <summary>
        /// Rotate at V axis
        /// </summary>
        /// <param name="angle"></param>
        public void RotateV(float angle)
        {
            Rotate(new Vector3(shapeVersorsUVW.Row1.X,
                               shapeVersorsUVW.Row1.Y,
                               shapeVersorsUVW.Row1.Z), angle);
        }

        /// <summary>
        /// Rotate at W axis
        /// </summary>
        /// <param name="angle"></param>
        public void RotateW(float angle)
        {
            Rotate(new Vector3(shapeVersorsUVW.Row2.X,
                               shapeVersorsUVW.Row2.Y,
                               shapeVersorsUVW.Row2.Z), angle);
        }

        /// <summary>
        /// Scales the Shape.
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(float scale)
        {
            this.scale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Scales the Shape.
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(float x, float y, float z)
        {
            this.scale = new Vector3(x, y, z);
        }

        /// <summary>
        /// Draws the shape mesh
        /// </summary>
        public virtual void Draw()
        {
            
        }

        /// <summary>
        /// Set shape color.
        /// </summary>
        /// <param name="color4"></param>
        public void SetColor(Color4 color4)
        {
            color = color4;

            foreach (var item in vertexsIndicesData.Vertexs)
            {
                item.Color = color4;
            }
        }

        /// <summary>
        /// Invert the RGB to BGR of the shape color.
        /// </summary>
        public void InvertColor()
        {
            Color4 invColor = Color4.CadetBlue;
            foreach (var item in VertexsIndicesData.Vertexs)
            {
                item.Color = invColor;
            }

            color = invColor;
        }

        /// <summary>
        /// Animate the shape. Runs the Shape Animations pipeline. To be
        /// used at the game tick function.
        /// </summary>
        public void Tick()
        {
            if(AnimationsManager is AnimationsManager)
                AnimationsManager.Animate();
        }

        /// <summary>
        /// Draw shape to buffer using VertexFormat
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="vertexFormat"></param>
        public virtual void DrawBufer(VertexFloatBuffer buffer, VertexFormat vertexFormat)
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
                case VertexFormat.XYZ_NORMAL:
                    #region xyz_normal
                    foreach (Vertex vertex in VertexsIndicesData.Vertexs)
                    {
                        buffer.AddVertex(vertex.Position.X, vertex.Position.Y, vertex.Position.Z,
                        new Vector3(vertex.Normal.X, vertex.Normal.Y, vertex.Normal.Z));
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

        /// <summary>
        /// Shader to apply to shape.
        /// </summary>
        public Shader Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        /// <summary>
        /// Shape vertex buffer.
        /// </summary>
        public VertexFloatBuffer VertexBuffer
        {
            get { return vertexBuffer; }
            set { vertexBuffer = value; }
        }

        /// <summary>
        /// Shape vertex format.
        /// </summary>
        public VertexFormat VertexFormat
        {
            get { return vertexFormat; }
            set { vertexFormat = value; }
        }

        /// <summary>
        /// Light Position.
        /// </summary>
        public Vector3 LightPosition
        {
            get { return lightPosition; }
            set { lightPosition = value; }
        }

        public virtual void Load()
        {
            GL.UseProgram(Shader.Program);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            DrawBufer(VertexBuffer, VertexFormat);

            VertexBuffer.IndexFromLength();
            VertexBuffer.Load();
            //VertexBuffer.Reload();



            VertexBuffer.Bind(Shader);
            GL.UseProgram(0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public virtual void Update(Matrix4 ViewMatrix, Matrix4 ProjectionMatrix, 
            Dictionary<string, Shape3D> shapes3D, Camera mainCamera, GameWindow gameWindow)
        {
            Tick();
            
            if(Physic.Vxyz != Vector3.Zero)
               TranslateWC(Physic.Vxyz.X, Physic.Vxyz.Y, Physic.Vxyz.Z);

            // Scale / Rotate / Translate

            Matrix4 SM = Matrix4.CreateScale(this.scale);

            Matrix4 RM = Matrix4.CreateFromQuaternion(this.quaternion);

            Matrix4 TM = Matrix4.CreateTranslation(this.position);

            ModelMatrix = SM * RM * TM;

            ModelViewMatrix = ModelMatrix * ViewMatrix;

            //VertexsIndicesData = tempVID;

            //send to shader
            GL.UseProgram(Shader.Program);
            //will return -1 without useprogram
            /*
            int mvp_matrix_location = GL.GetUniformLocation(Shader.Program, "mvp_matrix");
            GL.UniformMatrix4(mvp_matrix_location, false, ref MVP_Matrix);
            */

            int ShadowMatrix_location = GL.GetUniformLocation(Shader.Program, "shadow_matrix");
            GL.UniformMatrix4(ShadowMatrix_location, false, ref shadowMatrix);

            int TexMatrix_location = GL.GetUniformLocation(Shader.Program, "texture_matrix");
            GL.UniformMatrix4(TexMatrix_location, false, ref texMatrix);

            int ModelviewMatrix_location = GL.GetUniformLocation(Shader.Program, "modelview_matrix");
            GL.UniformMatrix4(ModelviewMatrix_location, false, ref modelViewMatrix);

            int ModelMatrix_location = GL.GetUniformLocation(Shader.Program, "model_matrix");
            GL.UniformMatrix4(ModelMatrix_location, false, ref modelMatrix);

            int ProjectionMatrix_location = GL.GetUniformLocation(Shader.Program, "projection_matrix");
            GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);
            /*
            int NormalMatrix_location = GL.GetUniformLocation(cube.Shader.Program, "normal_matrix");
            GL.UniformMatrix4(NormalMatrix_location, false, ref NormalMatrix);
            */

            float[] LightPositionShader = new float[4] { LightPosition.X, 
                LightPosition.Y, LightPosition.Z, 0f };
            int light_position_location = GL.GetUniformLocation(Shader.Program, "light_position");
            GL.Uniform1(light_position_location, 1, LightPositionShader);

            Vector3 SpotLightPositionShader = new Vector3 ( spotLight.Position.X, 
                spotLight.Position.Y, spotLight.Position.Z );
            int spot_light_position_location = GL.GetUniformLocation(Shader.Program, "spot_light_position");
            GL.Uniform3(spot_light_position_location, SpotLightPositionShader);




            /*
            float[] Kd = new float[3] { 0.9f, 0.5f, 0.3f };
            int Kd_location = GL.GetUniformLocation(Shader.Program, "kd");
            GL.Uniform1(Kd_location, 1, Kd);

            float[] Ld = new float[3] { 0.5f, 0.0f, 0.5f };
            int Ld_location = GL.GetUniformLocation(Shader.Program, "ld");
            GL.Uniform1(Ld_location, 1, Ld);
            */
            GL.UseProgram(0);
        }

        public virtual void Render()
        {
            if (!IsVisible) return;
                

            GL.UseProgram(Shader.Program);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit0"), TextureUnit.Texture0 - TextureUnit.Texture0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, TextureShadowMap);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit1"), TextureUnit.Texture1 - TextureUnit.Texture0);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, TextureBumpMap);
            GL.Uniform1(GL.GetUniformLocation(Shader.Program, "TextureUnit2"), TextureUnit.Texture2 - TextureUnit.Texture0);

            VertexBuffer.Bind(Shader);
        }

    }
}
