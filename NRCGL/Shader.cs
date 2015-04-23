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

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    
    class Shader
    {
        public string VertexSource { get; private set; }
        public string FragmentSource { get; private set; }

        public int VertexID { get; private set; }
        public int FragmentID { get; private set; }

        public int Program { get; private set; }

        public int PositionLocation { get; set; }
        public int NormalLocation { get; set; }
        public int TexCoordLocation { get; set; }
        public int ColorLocation { get; set; }

        public Shape3D Shape3D { get; set; }

        public ShadowMap ShadowMap { get; set; }

        public TextRender TextRender { get; set; }

        

        public Shader(ref string vs, ref string fs, Shape3D shape3D)
        {
            VertexSource = vs;
            FragmentSource = fs;

            Shape3D = shape3D;

            Build();
        }

        public Shader(ref string vs, ref string fs, ShadowMap shadowMap)
        {
            VertexSource = vs;
            FragmentSource = fs;

            ShadowMap = shadowMap;

            Build();
        }

        public Shader(ref string vs, ref string fs, TextRender textRender)
        {
            VertexSource = vs;
            FragmentSource = fs;

            TextRender = textRender;

            Build();
        }

        private void Build()
        {
            int status_code;
            string info;

            VertexID = GL.CreateShader(ShaderType.VertexShader);
            FragmentID = GL.CreateShader(ShaderType.FragmentShader);

            // Compile vertex shader
            GL.ShaderSource(VertexID, VertexSource);
            GL.CompileShader(VertexID);
            GL.GetShaderInfoLog(VertexID, out info);
            GL.GetShader(VertexID, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            // Compile fragment shader
            GL.ShaderSource(FragmentID, FragmentSource);
            GL.CompileShader(FragmentID);
            GL.GetShaderInfoLog(FragmentID, out info);
            GL.GetShader(FragmentID, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            Program = GL.CreateProgram();
            GL.AttachShader(Program, FragmentID);
            GL.AttachShader(Program, VertexID);

            GL.LinkProgram(Program);

            GL.UseProgram(Program);
            //layout dependent locations 
            PositionLocation = GL.GetAttribLocation(Program, "vertex_position");
            NormalLocation = GL.GetAttribLocation(Program, "vertex_normal");
            TexCoordLocation = GL.GetAttribLocation(Program, "vertex_texcoord");
            ColorLocation = GL.GetAttribLocation(Program, "vertex_color");

            if (PositionLocation >= 0)
                GL.BindAttribLocation(Program, PositionLocation, "vertex_position");
            if (NormalLocation >= 0)
                GL.BindAttribLocation(Program, NormalLocation, "vertex_normal");
            if (TexCoordLocation >= 0)
                GL.BindAttribLocation(Program, TexCoordLocation, "vertex_texcoord");
            if (ColorLocation >= 0)
                GL.BindAttribLocation(Program, ColorLocation, "vertex_color");

            if (Shape3D != null)
            {
                Shape3D.ShadowMatrix_location = GL.GetUniformLocation(Program, "shadow_matrix");
                Shape3D.TexMatrix_location = GL.GetUniformLocation(Program, "texture_matrix");
                Shape3D.ModelviewMatrix_location = GL.GetUniformLocation(Program, "modelview_matrix");
                Shape3D.ModelMatrix_location = GL.GetUniformLocation(Program, "model_matrix");
                Shape3D.ProjectionMatrix_location = GL.GetUniformLocation(Program, "projection_matrix");
                Shape3D.Light_position_location = GL.GetUniformLocation(Program, "light_position");
                Shape3D.Point_light_position_location = GL.GetUniformLocation(Program, "point_light_position");
                Shape3D.Point_light_color_location = GL.GetUniformLocation(Program, "point_light_color");
                Shape3D.Point_light_intensity_location = GL.GetUniformLocation(Program, "point_light_intensity");
                Shape3D.Spot_light_position_location = GL.GetUniformLocation(Program, "spot_light_position");
                Shape3D.Spot_light_color_location = GL.GetUniformLocation(Program, "spot_light_color");
                Shape3D.Spot_light_intensity_location = GL.GetUniformLocation(Program, "spot_light_intensity");
                Shape3D.Spot_light_cone_angle_location = GL.GetUniformLocation(Program, "spot_light_cone_angle");
                Shape3D.Spot_light_cone_direction_location = GL.GetUniformLocation(Program, "spot_light_cone_direction");
            }
            

            GL.UseProgram(0);
        }

        public void Dispose()
        {
            if (Program != 0)
                GL.DeleteProgram(Program);
            if (FragmentID != 0)
                GL.DeleteShader(FragmentID);
            if (VertexID != 0)
                GL.DeleteShader(VertexID);
        }
    }
}
