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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class Mesh
    {
        /// <summary>
        /// Mesh of a cube with size edge
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static List<Matrix4> Cube(float size)
        {
            // cube vertexs
            Vector4 v0 = new Vector4(size / 2, size / 2, size / 2, 1);
            Vector4 v1 = new Vector4(size / 2, size / 2, -size / 2, 1);
            Vector4 v2 = new Vector4(-size / 2, size / 2, -size / 2, 1);
            Vector4 v3 = new Vector4(-size / 2, size / 2, size / 2, 1);
            Vector4 v4 = new Vector4(size / 2, -size / 2, size / 2, 1);
            Vector4 v5 = new Vector4(size / 2, -size / 2, -size / 2, 1);
            Vector4 v6 = new Vector4(-size / 2, -size / 2, -size / 2, 1);
            Vector4 v7 = new Vector4(-size / 2, -size / 2, size / 2, 1);

            //cube normals
            Vector4 nFront = new Vector4(0f,0f,1f,0f);
            Vector4 nRight = new Vector4(1f,0f,0f,0f);
            Vector4 nTop = new Vector4(0f,1f,0f,0f);
            Vector4 nBottom = new Vector4(0f,-1f,0f,0f);
            Vector4 nLeft = new Vector4(-1f,0f,0f,0f);
            Vector4 nBack = new Vector4(0f,0f,-1f,0f);

            // mesh with 12 triangles and normals
            List<Matrix4> cubeMesh = new List<Matrix4>()
            {
                // front face
               
                new Matrix4(v3, v7, v0, nFront),
                new Matrix4(v0, v7, v4, nFront),

                // right face
                
                new Matrix4(v0, v4, v1, nRight),
                new Matrix4(v4, v5, v1, nRight),
                

                // top face
                new Matrix4(v3, v0, v1, nTop),
                new Matrix4(v1, v2, v3, nTop),

                // bottom face
                new Matrix4(v7, v6, v4, nBottom),
                new Matrix4(v5, v4, v6, nBottom),

                // left face
                new Matrix4(v6, v3, v2, nLeft),
                new Matrix4(v6, v7, v3, nLeft),

                // back face
                new Matrix4(v5, v2, v1, nBack),
                new Matrix4(v2, v5, v6, nBack)
            };

            return cubeMesh;
        }

        public static List<Matrix4> Panel(float width, float height, int widthDivision, int heightDivision)
        {
            List<Matrix4> panelMesh = new List<Matrix4>();

            for (int h = 0; h < heightDivision; h++)
            {
                for (int w = 0; w < widthDivision; w++)
                {
                    //Matrix4 triangleMatrix4 = new Matrix4()
                }
            }


            return panelMesh;
        }
    }
}
