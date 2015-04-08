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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    public class VertexsIndicesData
    {
        
        public VertexFormat VertexFormat { get; set; }

        public int VertexCount { get; set; }

        public int IndicesCount { get; set; }

        public List<Vertex> Vertexs { get; set; }

        public List<uint> Indices { get; set; }

        public VertexsIndicesData()
        {
        }

        public static VertexsIndicesData Copy(VertexsIndicesData other)
        {
            VertexsIndicesData newVID = new VertexsIndicesData();
            newVID.VertexFormat = other.VertexFormat;
            newVID.VertexCount = other.VertexCount;
            newVID.IndicesCount = other.IndicesCount;
            newVID.Vertexs = new List<Vertex>();
            foreach (var item in other.Vertexs)
            {
                Vertex tempVertex = new Vertex();
                tempVertex.Color = new Color4(item.Color.R, item.Color.G, item.Color.B, item.Color.A);
                tempVertex.Normal = new Vector3(item.Normal.X, item.Normal.Y, item.Normal.Z);
                tempVertex.Position = new Vector3(item.Position.X, item.Position.Y, item.Position.Z);
                tempVertex.TexCoord = new Vector2(item.TexCoord.X, item.TexCoord.Y);
                newVID.Vertexs.Add(tempVertex);
            }

            newVID.Indices = new List<uint>();
            foreach (var item in other.Indices)
            {
                newVID.Indices.Add(item);
            }

            return newVID;
        }

        public static VertexsIndicesData CopyPositionsAndNormals(VertexsIndicesData other)
        {
            VertexsIndicesData newVID = new VertexsIndicesData();
            newVID.VertexFormat = other.VertexFormat;
            newVID.VertexCount = other.VertexCount;
            newVID.IndicesCount = other.IndicesCount;
            newVID.Vertexs = new List<Vertex>();
            foreach (var item in other.Vertexs)
            {
                Vertex tempVertex = new Vertex();
                tempVertex.Color = item.Color;
                tempVertex.Normal = new Vector3(item.Normal.X, item.Normal.Y, item.Normal.Z);
                tempVertex.Position = new Vector3(item.Position.X, item.Position.Y, item.Position.Z);
                tempVertex.TexCoord = item.TexCoord;
                newVID.Vertexs.Add(tempVertex);
            }

            newVID.Indices = other.Indices;
            

            return newVID;
        }
    }
}
