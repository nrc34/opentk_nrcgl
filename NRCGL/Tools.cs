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
using OpenTK.Graphics.OpenGL4;
using OpenTK_NRCGL.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenTK_NRCGL.NRCGL
{
    class Tools
    {

        public static Vector4 ColorToVector4(Color color)
        {
            Vector4 vector4 = new Vector4();

            vector4.X = Convert.ToSingle(color.R) / 256f;
            vector4.Y = Convert.ToSingle(color.G) / 256f;
            vector4.Z = Convert.ToSingle(color.B) / 256f;
            vector4.W = Convert.ToSingle(color.A) / 256f;

            return vector4;
        }

        public static void GenerateModelFrom3DS(string path)
        {
            var x3d = new X3D();

            var xmlSerializer = new XmlSerializer(x3d.GetType());
            var reader = new StreamReader(path);
            x3d = (X3D)xmlSerializer.Deserialize(reader);

            reader.Close();

            var scene = new X3DScene();
            scene = x3d.Scene[0];

            var transf0 = new Transform();

            transf0 = scene.Transform[0];

            var transf1 = new Transform();

            transf1 = transf0.Transform1[0];

            var transfGroup = new TransformGroup();

            transfGroup = transf1.Group[0];

            var transfGroupShape = new TransformGroupShape();

            transfGroupShape = transfGroup.Shape[0];

            var shapeIndexedFaceSet 
                    = new TransformGroupShapeIndexedFaceSet();

            // indices
            shapeIndexedFaceSet = transfGroupShape.IndexedFaceSet[0];

            // position coords
            TransformGroupShapeIndexedFaceSetCoordinate positionCoords 
                = new TransformGroupShapeIndexedFaceSetCoordinate();
            positionCoords = shapeIndexedFaceSet.Coordinate[0];

            // texture coords
            TransformGroupShapeIndexedFaceSetTextureCoordinate texCoords 
                = new TransformGroupShapeIndexedFaceSetTextureCoordinate();
            texCoords = shapeIndexedFaceSet.TextureCoordinate[0];
            /*
            string output = shapeIndexedFaceSet.coordIndex + "\n";
            output += shapeIndexedFaceSet.texCoordIndex + "\n";
            output += positionCoords.point + "\n";
            output += texCoords.point + "\n";
            */


            var positionsList = new List<Vector3>();

            string[] positionCoordsStr = positionCoords.point.Split(' ');

            for (int i = 0; i < positionCoordsStr.Length - 1; )
            {
                var vector3 = new Vector3(
                    Convert.ToSingle(positionCoordsStr[i++], CultureInfo.InvariantCulture),
                    Convert.ToSingle(positionCoordsStr[i++], CultureInfo.InvariantCulture),
                    Convert.ToSingle(positionCoordsStr[i++], CultureInfo.InvariantCulture));

                positionsList.Add(vector3);
            }

            var texCoordsList = new List<Vector2>();

            string[] texCoordsStr = texCoords.point.Split(' ');

            // OpenTK flips V texCoord. Used (U, 1-V) to convert from X3D to OpenTK
            for (int i = 0; i < texCoordsStr.Length - 1; )
            {
                var vector2 = new Vector2(
                    Convert.ToSingle(texCoordsStr[i++], CultureInfo.InvariantCulture),
                    1 - Convert.ToSingle(texCoordsStr[i++], CultureInfo.InvariantCulture));

                
                texCoordsList.Add(vector2);
            }



            // generate model xml

            var vertexsIndicesData = new VertexsIndicesData();
            vertexsIndicesData.VertexFormat = VertexFormat.XYZ_NORMAL_UV_COLOR;
            
            var vertexs = new List<Vertex>();
            int idx = 0;


            var texCoordIndexs = new List<uint>();
            var coordIndexs = new List<uint>();

            string[] coordsIndexStr = shapeIndexedFaceSet.coordIndex.Split(' ');
            string[] texCoordsIndexStr = shapeIndexedFaceSet.texCoordIndex.Split(' ');

            int vertCount = 0; // check if is 4 or 3 when -1 arrives
            int v = 0;
            for (int ic = 0; ic < coordsIndexStr.Length; ic++)
            {
                if (coordsIndexStr[ic] == "-1")
                {
                    switch (vertCount)
                    {
                        case 4:
                            #region quad
                            var vertex0 = new Vertex();
                            vertex0.Color = Color4.Chocolate;
                            vertex0.Normal = Vector3.Zero;
                            vertex0.Position = new Vector3(positionsList[(int)coordIndexs[idx]]);
                            vertex0.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx]]);
                            vertexs.Add(vertex0);
                            v++;
                            var vertex1 = new Vertex();
                            vertex1.Color = Color4.Chocolate;
                            vertex1.Normal = Vector3.Zero;
                            vertex1.Position = new Vector3(positionsList[(int)coordIndexs[idx+1]]);
                            vertex1.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx+1]]);
                            vertexs.Add(vertex1);
                            v++;
                            var vertex2 = new Vertex();
                            vertex2.Color = Color4.Chocolate;
                            vertex2.Normal = Vector3.Zero;
                            vertex2.Position = new Vector3(positionsList[(int)coordIndexs[idx + 2]]);
                            vertex2.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx + 2]]);
                            vertexs.Add(vertex2);
                            v++;
                            // normal
                            Vector3 edge1 = Vector3.Subtract(vertex1.Position, vertex0.Position);
                            Vector3 edge2 = Vector3.Subtract(vertex2.Position, vertex0.Position);
                            vertex0.Normal = Vector3.Cross(edge1, edge2);
                            vertex1.Normal = vertex0.Normal;
                            vertex2.Normal = vertex0.Normal;

                            Vertex vertex3 = new Vertex();
                            vertex3.Color = Color4.Chocolate;
                            vertex3.Normal = Vector3.Zero;
                            vertex3.Position = new Vector3(positionsList[(int)coordIndexs[idx + 2]]);
                            vertex3.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx + 2]]);
                            vertexs.Add(vertex3);
                            v++;
                            Vertex vertex4 = new Vertex();
                            vertex4.Color = Color4.Chocolate;
                            vertex4.Normal = Vector3.Zero;
                            vertex4.Position = new Vector3(positionsList[(int)coordIndexs[idx + 3]]);
                            vertex4.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx + 3]]);
                            vertexs.Add(vertex4);
                            v++;
                            Vertex vertex5 = new Vertex();
                            vertex5.Color = Color4.Chocolate;
                            vertex5.Normal = Vector3.Zero;
                            vertex5.Position = new Vector3(positionsList[(int)coordIndexs[idx]]);
                            vertex5.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx]]);
                            vertexs.Add(vertex5);
                            v++;
                            idx += 4;

                            // normal
                            Vector3 edge3 = Vector3.Subtract(vertex4.Position, vertex3.Position);
                            Vector3 edge4 = Vector3.Subtract(vertex5.Position, vertex3.Position);
                            vertex3.Normal = Vector3.Cross(edge3, edge4);
                            vertex4.Normal = vertex3.Normal;
                            vertex5.Normal = vertex3.Normal;
                            #endregion
                            break;
                        case 3:
                            #region triangle
                            Vertex vertex6 = new Vertex();
                            vertex6.Color = Color4.Chocolate;
                            vertex6.Normal = Vector3.Zero;
                            vertex6.Position = new Vector3(positionsList[(int)coordIndexs[idx]]);
                            vertex6.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx]]);
                            vertexs.Add(vertex6);
                            v++;
                            Vertex vertex7 = new Vertex();
                            vertex7.Color = Color4.Chocolate;
                            vertex7.Normal = Vector3.Zero;
                            vertex7.Position = new Vector3(positionsList[(int)coordIndexs[idx + 1]]);
                            vertex7.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx + 1]]);
                            vertexs.Add(vertex7);
                            v++;
                            Vertex vertex8 = new Vertex();
                            vertex8.Color = Color4.Chocolate;
                            vertex8.Normal = Vector3.Zero;
                            vertex8.Position = new Vector3(positionsList[(int)coordIndexs[idx + 2]]);
                            vertex8.TexCoord = new Vector2(texCoordsList[(int)texCoordIndexs[idx + 2]]);
                            vertexs.Add(vertex8);
                            v++;
                            idx += 3;
                            // normal
                            Vector3 edge5 = Vector3.Subtract(vertex7.Position, vertex6.Position);
                            Vector3 edge6 = Vector3.Subtract(vertex8.Position, vertex6.Position);
                            vertex6.Normal = Vector3.Cross(edge5, edge6);
                            vertex7.Normal = vertex6.Normal;
                            vertex8.Normal = vertex6.Normal;
                            #endregion
                            break;
                        default:
                            break;
                    }
                    vertCount = 0;
                    continue;
                }

                if (coordsIndexStr[ic] != String.Empty)
                    coordIndexs.Add(Convert.ToUInt16(coordsIndexStr[ic]));

                if (texCoordsIndexStr[ic] != String.Empty)
                    texCoordIndexs.Add(Convert.ToUInt16(texCoordsIndexStr[ic]));

                vertCount++;
            }


            // bug! there are triangles also
            vertexsIndicesData.VertexCount = (coordIndexs.Count / 4) * 6;
            vertexsIndicesData.IndicesCount = (coordIndexs.Count / 4) * 6;

            var indices = new List<uint>();
            for (int i = 0; i < vertexsIndicesData.IndicesCount; i++)
            {
                indices.Add((uint)i);
            }

            vertexsIndicesData.Vertexs = vertexs;
            vertexsIndicesData.Indices = indices;

            var xmlSerializerModel = new XmlSerializer(typeof(VertexsIndicesData));
            var textWriter = new StringWriter();
            xmlSerializerModel.Serialize(textWriter, vertexsIndicesData);

            File.WriteAllText(path + "_out", textWriter.ToString());

            //File.WriteAllText(@"Models\test.txt", output);

            // smooth shadow

            var solvedVertexesPositions = new List<Vector3>();

            for (int i = 0; i < vertexsIndicesData.Vertexs.Count; i++)
            {
                if (solvedVertexesPositions.Contains(
                                vertexsIndicesData.Vertexs[i].Position))
                    continue;

                solvedVertexesPositions.
                    Add(vertexsIndicesData.Vertexs[i].Position);

                var normal = new Vector3();

                normal = vertexsIndicesData.Vertexs[i].Normal;

                var vertexes2Solve = new Stack<Vertex>();
                vertexes2Solve.Push(vertexsIndicesData.Vertexs[i]);

                for (int j = 0; j < vertexsIndicesData.Vertexs.Count; j++)
                {
                    if (vertexsIndicesData.Vertexs[i].Position == 
                        vertexsIndicesData.Vertexs[j].Position &&
                        i != j)
                    {
                        normal += vertexsIndicesData.Vertexs[j].Normal;
                        vertexes2Solve.Push(vertexsIndicesData.Vertexs[j]);
                    }
                }

                normal.Normalize();

                while (vertexes2Solve.Count > 0)
                {
                    vertexes2Solve.Pop().Normal = normal;
                }

                
            }

            var xmlSerializerModelSmooth = new XmlSerializer(typeof(VertexsIndicesData));
            var textWriter1 = new StringWriter();
            xmlSerializerModelSmooth.Serialize(textWriter1, vertexsIndicesData);

            File.WriteAllText(path + "_out_smooth", textWriter1.ToString());

        }

        public static VertexsIndicesData DeserializeModel(string path)
        {
            VertexsIndicesData vertexsIndicesData = new VertexsIndicesData();

            XmlSerializer xmlSerializer = new XmlSerializer(vertexsIndicesData.GetType());
            StreamReader reader = new StreamReader(path);
            vertexsIndicesData = (VertexsIndicesData)xmlSerializer.Deserialize(reader);

            reader.Close();

            return vertexsIndicesData;
        }

        public static Bitmap GrabScreenshot(GameWindow gameWindow)
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            Bitmap bmp = new Bitmap(gameWindow.ClientSize.Width, 
                                    gameWindow.ClientSize.Height);

            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(gameWindow.ClientRectangle, 
                             System.Drawing.Imaging.ImageLockMode.WriteOnly, 
                             System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, gameWindow.ClientSize.Width, 
                          gameWindow.ClientSize.Height, 
                          PixelFormat.Bgr, 
                          PixelType.UnsignedByte, 
                          data.Scan0);

            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }

    }
}
