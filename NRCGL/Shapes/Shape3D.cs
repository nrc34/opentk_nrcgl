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
using OpenTK_NRCGL.NRCGL.Shapes;
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
        private PointLight pointLight;
        private SpotLight spotLight;
        private int modelviewMatrix_location;
        private int shadowMatrix_location;
        private int texMatrix_location;
        private int modelMatrix_location;
        private int projectionMatrix_location;
        private int light_position_location;
        private int point_light_position_location;
        private int point_light_color_location;
        private int point_light_intensity_location;
        private int spot_light_position_location;
        private int spot_light_color_location;
        private int spot_light_intensity_location;
        private int spot_light_cone_angle_location;
        private int spot_light_cone_direction_location;
        private int ambient_light_location;
        private Light light;
        private int camera_position_location;
        private Material material;
        private int material_ambient_location;
        private int material_diffuse_location;
        private int material_specular_location;
        private int material_shininess_location;




        public int Material_ambient_location
        {
            get { return material_ambient_location; }
            set { material_ambient_location = value; }
        }
        

        public int Material_diffuse_location
        {
            get { return material_diffuse_location; }
            set { material_diffuse_location = value; }
        }
        

        public int Material_specular_location
        {
            get { return material_specular_location; }
            set { material_specular_location = value; }
        }
        

        public int Material_shininess_location
        {
            get { return material_shininess_location; }
            set { material_shininess_location = value; }
        }


        public Material Material
        {
            get { return material; }
            set { material = value; }
        }
        

        public int Camera_position_location
        {
            get { return camera_position_location; }
            set { camera_position_location = value; }
        }


        public int Ambient_light_location
        {
            get { return ambient_light_location; }
            set { ambient_light_location = value; }
        }


        public int Light_position_location
        {
            get { return light_position_location; }
            set { light_position_location = value; }
        }
        

        public int Point_light_position_location
        {
            get { return point_light_position_location; }
            set { point_light_position_location = value; }
        }
        

        public int Point_light_color_location
        {
            get { return point_light_color_location; }
            set { point_light_color_location = value; }
        }
        

        public int Point_light_intensity_location
        {
            get { return point_light_intensity_location; }
            set { point_light_intensity_location = value; }
        }
        

        public int Spot_light_position_location
        {
            get { return spot_light_position_location; }
            set { spot_light_position_location = value; }
        }
        

        public int Spot_light_color_location
        {
            get { return spot_light_color_location; }
            set { spot_light_color_location = value; }
        }
        

        public int Spot_light_intensity_location
        {
            get { return spot_light_intensity_location; }
            set { spot_light_intensity_location = value; }
        }
        

        public int Spot_light_cone_angle_location
        {
            get { return spot_light_cone_angle_location; }
            set { spot_light_cone_angle_location = value; }
        }
        

        public int Spot_light_cone_direction_location
        {
            get { return spot_light_cone_direction_location; }
            set { spot_light_cone_direction_location = value; }
        }


        public int ProjectionMatrix_location
        {
            get { return projectionMatrix_location; }
            set { projectionMatrix_location = value; }
        }


        public int ModelMatrix_location
        {
            get { return modelMatrix_location; }
            set { modelMatrix_location = value; }
        }


        public int TexMatrix_location
        {
            get { return texMatrix_location; }
            set { texMatrix_location = value; }
        }


        public int ShadowMatrix_location
        {
            get { return shadowMatrix_location; }
            set { shadowMatrix_location = value; }
        }


        public int ModelviewMatrix_location
        {
            get { return modelviewMatrix_location; }
            set { modelviewMatrix_location = value; }
        }


        public Light Light
        {
            get { return light; }
            set { light = value; }
        }
        

        public SpotLight SpotLight
        {
            get { return spotLight; }
            set { spotLight = value; }
        }
        

        public PointLight PointLight
        {
            get { return pointLight; }
            set { pointLight = value; }
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

            // default values for Light
            light = new Light();
            light.Ambient = Vector3.One;
            light.DirectionalLightPosition =
                        new Vector3(3000, 3000, 0);

            // default values for material
            Material = new Shapes.Material();
            Material.Ambient = Vector3.One;
            Material.Diffuse = Vector3.One;
            Material.Specular = Vector3.One;
            Material.Shininess = 30f;

            PointLight = new PointLight
            {
                Color = Vector3.Zero,
                Intensity = 1f,
                Position = Vector3.Zero
            };

            SpotLight = new SpotLight
            {
                Color = Vector3.Zero,
                Intensity = 1f,
                Position = Vector3.Zero,
                ConeAngle = MathHelper.PiOver6,
                ConeDirection = Vector3.Zero
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


            //send to shader
            GL.UseProgram(Shader.Program);
            //will return -1 without useprogram
           
            
            GL.UniformMatrix4(ShadowMatrix_location, false, ref shadowMatrix);

            GL.UniformMatrix4(TexMatrix_location, false, ref texMatrix);

            GL.UniformMatrix4(ModelviewMatrix_location, false, ref modelViewMatrix);

            GL.UniformMatrix4(ModelMatrix_location, false, ref modelMatrix);

            GL.UniformMatrix4(ProjectionMatrix_location, false, ref ProjectionMatrix);

            GL.Uniform3(Light_position_location, light.DirectionalLightPosition);
            
            GL.Uniform3(Point_light_position_location, pointLight.Position);

            GL.Uniform3(Point_light_color_location, pointLight.Color);

            GL.Uniform1(Point_light_intensity_location, pointLight.Intensity);

            GL.Uniform3(Spot_light_position_location, spotLight.Position);

            GL.Uniform3(Spot_light_color_location, spotLight.Color);

            GL.Uniform1(Spot_light_intensity_location, spotLight.Intensity);

            GL.Uniform1(Spot_light_cone_angle_location, spotLight.ConeAngle);

            GL.Uniform3(Spot_light_cone_direction_location, spotLight.ConeDirection);

            GL.Uniform3(Ambient_light_location, Light.Ambient);

            GL.Uniform3(Camera_position_location, mainCamera.Position * -1);

            GL.Uniform3(Material_ambient_location, Material.Ambient);

            GL.Uniform3(Material_diffuse_location, Material.Diffuse);

            GL.Uniform3(Material_specular_location, Material.Specular);

            GL.Uniform1(Material_shininess_location, Material.Shininess);
            
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
