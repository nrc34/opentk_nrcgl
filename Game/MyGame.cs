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
using OpenTK_NRCGL.NRCGL;
using OpenTK_NRCGL.NRCGL.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.Game
{
    class MyGame
    {

        public static int sphere_texture;

        public static float TableXAngle = 0f;
        public static float TableZAngle = 0f;

        public static int Width;
        public static int Height;

        public static bool MultiView = false;

        public static Matrix4 ProjectionMatrix;
        private static int current_texture;
        private static int bump_texture;
        private static ShadowMap shadowMapG;
        private static Matrix4 texMatrixG;

        public static string Debug = "";
        public static double MaxTableAngle = 1;
        public static float DeltaTableAngle = MathHelper.PiOver6 / 50;

        public static bool[] collisionsAll = new bool[6]{true,true,true,true,true,true};

        public static float collisionOverllap;
        

        public static Dictionary<string, Shape3D> LoadShapes(ShadowMap shadowMap)
        {
            Dictionary<string, Shape3D> shapes3D = new Dictionary<string,Shape3D>();

            //float deltaRot = MathHelper.PiOver6 / 20;

            shadowMapG = shadowMap;

            //textures
            current_texture = Texture.LoadTexture(@"Textures\sand_texture1037.jpg", 0, false, false);

            int cube_texture = Texture.LoadTexture(@"Textures\cube3DTexture.png", 0, false, false);

            sphere_texture = Texture.LoadTexture(@"Textures\beach_ball.png", 0, false, false);

            int sky_texture = Texture.LoadTexture(@"Textures\Day_Skybox.png", 0, false, false);

            bump_texture = Texture.LoadTexture(@"Textures\Rock_01_local.jpg", 0, false, false);


            // create texture matrix
            Matrix4 ProjectionMatrixTex = Matrix4.CreateOrthographic(110, 110, -1, 1);

            Matrix4 ScaleBiasMatrix = new Matrix4(new Vector4(0.5f, 0.0f, 0.0f, 0.0f),
                                                  new Vector4(0.0f, 0.5f, 0.0f, 0.0f),
                                                  new Vector4(0.0f, 0.0f, 0.5f, 0.0f),
                                                  new Vector4(0.5f, 0.5f, 0.5f, 1f));

            Matrix4 texMatrix = ProjectionMatrixTex * ScaleBiasMatrix;

            Camera tex = new Camera();
            tex.Position = new Vector3(0, -10, 0);
            tex.Rotate(new Vector3(tex.CameraUVW.Row0), MathHelper.PiOver2);

            tex.Update();

            texMatrix = tex.View * texMatrix;
            texMatrixG = texMatrix;
            
            #region panels

            Shape3D basePanel = new Panel3D(new Vector3(0, -1f, 0), 0f, 0f, 0f, Color4.Chocolate, current_texture);

            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_UV_Normal_panel.txt");
            string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_panel_base.txt");
            Shader shaderBasePanel = new Shader(ref vs, ref fs);
            basePanel.Shader = shaderBasePanel;
            basePanel.Scale(55);
            //basePanel.Bounding = new Bounding(basePanel, 100, 1, 100);
            basePanel.Physic.Mass = 100000000f;
            basePanel.RotateU(-MathHelper.PiOver2);
            //basePanel.Position = new Vector3(0f, -20f, 0f);
            basePanel.TextureShadowMap = shadowMap.DepthTexture;
            basePanel.ShadowMatrix = shadowMap.ShadowMatrix;
            basePanel.TextureBumpMap = bump_texture;
            basePanel.TexMatrix = texMatrix;
            basePanel.Collision = false;
            basePanel.Load();
            shapes3D.Add("basePanel", basePanel);
            /*
            Shape3D leftPanel = new Panel3D(Vector3.Zero, 0f, 0f, 0f, Color4.Chocolate, current_texture);
            leftPanel.Scale(2, 52, 0);
            leftPanel.RotateZ(-MathHelper.PiOver2);
            leftPanel.RotateY(MathHelper.PiOver2);
            
            leftPanel.Position = new Vector3(-52, 2, 0);
            leftPanel.TextureShadowMap = shadowMap.DepthTexture;
            leftPanel.TexMatrix = texMatrix;
            leftPanel.Collision = false;
            leftPanel.Load();
            shapes3D.Add("leftPanel", leftPanel);

            Shape3D rightPanel = new Panel3D(Vector3.Zero, 0f, 0f, 0f, Color4.Chocolate, current_texture);
            rightPanel.Scale(2, 52, 0);
            rightPanel.RotateZ(-MathHelper.PiOver2);
            rightPanel.RotateY(-MathHelper.PiOver2);

            rightPanel.Position = new Vector3(52, 2, 0);
            rightPanel.TextureShadowMap = shadowMap.DepthTexture;
            rightPanel.TexMatrix = texMatrix;
            rightPanel.Collision = false;
            rightPanel.Load();
            shapes3D.Add("rightPanel", rightPanel);

            Shape3D topPanel = new Panel3D(Vector3.Zero, 0f, 0f, 0f, Color4.Chocolate, current_texture);
            topPanel.Scale(52, 2, 0);
            //topPanel.RotateZ(-MathHelper.PiOver2);
            //topPanel.RotateY(-MathHelper.PiOver2);

            topPanel.Position = new Vector3(0, 2, -52);
            topPanel.TextureShadowMap = shadowMap.DepthTexture;
            topPanel.TexMatrix = texMatrix;
            topPanel.Collision = false;
            topPanel.Load();
            shapes3D.Add("topPanel", topPanel);

            Shape3D bottomPanel = new Panel3D(Vector3.Zero, 0f, 0f, 0f, Color4.Black);
            bottomPanel.Scale(52, 2, 0);
            //topPanel.RotateZ(-MathHelper.PiOver2);
            //topPanel.RotateY(-MathHelper.PiOver2);

            bottomPanel.Position = new Vector3(0, 2, 52);
            //bottomPanel.TextureShadowMap = shadowMap.DepthTexture;
            bottomPanel.Collision = false;
            bottomPanel.Load();
            shapes3D.Add("bottomPanel", bottomPanel);
            */
            #endregion
            /*
            Shape3D sphere1 = new Sphere3D(new Vector3(20f, 2f, 5f), 2, Color4.Gold, sphere_texture);
            //sphere.Scale(10f);
            sphere1.Physic.Mass = 10f;
            sphere1.Physic.Vuvw = new Vector3(-0.05f, 0f, 0.0f);
            //sphere1.Physic.AVuvw = new Vector3(0, 0, sphere1.Physic.Vuvw.X / 2);
            sphere1.TextureShadowMap = shadowMap.DepthTexture;
            sphere1.Load();
            shapes3D.Add("sphere1", sphere1);

            Shape3D sphere2 = new Sphere3D(new Vector3(10f, 2f, 5f), 2, Color4.Gold, sphere_texture);
            //sphere.Scale(10f);
            sphere2.Physic.Mass = 10f;
            sphere2.Physic.Vuvw = new Vector3(0.05f, 0f, 0.0f);
            sphere2.TextureShadowMap = shadowMap.DepthTexture;
            sphere2.Load();
            shapes3D.Add("sphere2", sphere2);

            Shape3D sphere3 = new Sphere3D(new Vector3(0f, 2f, 0f), 2, Color4.Gold, sphere_texture);
            //sphere3.Scale(3f);
            sphere3.Physic.Mass = 1000000000f;
            //sphere3.Bounding = new Bounding(sphere3, 20, 20, 20);
            sphere3.Physic.Vuvw = new Vector3(0.0f, 0f, 0.0f);
            sphere3.TextureShadowMap = shadowMap.DepthTexture;
            sphere3.Load();
            shapes3D.Add("sphere3", sphere3);
            */

            string[] lines = File.ReadAllLines("levels\\level1.csv");

            foreach (var item in lines)
            {
                string[] lineStrings = item.Split(';');
                int xBlock = Convert.ToInt32(lineStrings[0]);
                int yBlock = Convert.ToInt32(lineStrings[1]);
                string blockName = lineStrings[2];

                insertBlocks(xBlock, yBlock, blockName, shapes3D);
            }
            /*
            Shape3D cube = new NRCGL.Shapes.Cube3D(new Vector3(-48f, 6f, 48f), Color4.AliceBlue, current_texture);
            cube.Physic.Mass = 100;
            cube.Physic.Vxyz = Vector3.Zero;
            cube.Bounding = new Bounding(cube, 20, 2, 20);
            cube.Scale(2, 6, 2);
            cube.TextureShadowMap = shadowMap.DepthTexture;
            cube.TexMatrix = texMatrix;
            cube.Load();
            shapes3D.Add("cube", cube);
            */
            Shape3D skyBox = new NRCGL.Shapes.SkyBox(new Vector3(-30f, 50f, 30f), Color4.AliceBlue, sky_texture);
            skyBox.Scale(500);
            skyBox.Collision = false;
            skyBox.Load();
            shapes3D.Add("skyBox", skyBox);
            /*
            Shape3D shpereEnvCubeMap = new Sphere3DEnvCubeMap(new Vector3(-45f, 1f, -45f), 1f);
            shpereEnvCubeMap.Physic.Mass = 10f;
            shpereEnvCubeMap.Physic.Vxyz = Vector3.Zero;
            shpereEnvCubeMap.ShadowMatrix = shadowMap.ShadowMatrix;
            shpereEnvCubeMap.Load();
            shapes3D.Add("sphereEnvCubeMap", shpereEnvCubeMap);
            */
            Shape3D sphere1 = new Sphere3D(new Vector3(-45f, 0.5f, -45f), 1.5f, Color4.Gold, sphere_texture);
            sphere1.Physic.Mass = 10f;
            sphere1.Physic.Vxyz = Vector3.Zero;
            sphere1.ShadowMatrix = shadowMap.ShadowMatrix;
            sphere1.TextureShadowMap = shadowMap.DepthTexture;
            sphere1.Bounding.R = 1.5f;
            sphere1.Load();
            shapes3D.Add("sphereEnvCubeMap", sphere1);
            
            return shapes3D;
        }

        private static void insertBlocks(int xBlock, int yBlock, 
            string blockName, Dictionary<string, Shape3D> shapes3D)
        {
            float x = (xBlock * 10) - 55;
            float y = (yBlock * 10) - 55;
            switch (blockName)
            {
                case "00":
                    insertBlock(x + 5f, y + 5f, shapes3D, 2f, 2f,
                        new bool[6] { true, false, true, true, false, true });
                    insertBlock(x + 5f, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    insertBlock(x + 8.5f, y + 5f, shapes3D, 1.5f, 2f,
                        new bool[6] { false, false, true, true, true, true });
                    break;
                case "10":
                case "14":
                case "30":
                case "34":
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true });
                    break;
                case "01":
                case "03":
                case "41":
                case "43":
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                case "02":
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false });
                    insertBlock(x + 8.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true });
                    break;
                case "20":
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true });
                    insertBlock(x + 5, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                case "42":
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false });
                    insertBlock(x + 1.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true });
                    break;
                case "24":
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true });
                    insertBlock(x + 5, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                case "21":
                    insertBlock(x + 5, y + 3.5f, shapes3D, 2, 3.5f,
                        new bool[6] { true, true, true, true, true, false });
                    break;
                case "23":
                    insertBlock(x + 5, y + 6.5f, shapes3D, 2, 3.5f,
                        new bool[6] { true, true, true, true, false, true });
                    break;
                case "22":
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false });
                    insertBlock(x + 1.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true });
                    insertBlock(x + 8.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true });
                    break;
                case "32":
                    insertBlock(x + 6.5f, y + 5, shapes3D, 3.5f, 2,
                        new bool[6] { true, false, true, true, true, true });
                    break;
                case "12":
                    insertBlock(x + 3.5f, y + 5, shapes3D, 3.5f, 2,
                        new bool[6] { false, true, true, true, true, true });
                    break;
                case "40":
                    insertBlock(x + 3.5f, y + 5, shapes3D, 3.5f, 2,
                        new bool[6] { false, true, true, true, true, true });
                    insertBlock(x + 5, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                case "44":
                    insertBlock(x + 3.5f, y + 5, shapes3D, 3.5f, 2,
                        new bool[6] { false, true, true, true, true, true });
                    insertBlock(x + 5, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                case "04":
                    insertBlock(x + 6.5f, y + 5, shapes3D, 3.5f, 2,
                        new bool[6] { true, false, true, true, true, true });
                    insertBlock(x + 5, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false });
                    break;
                default:
                    break;
            }
        }

        private static void insertBlock(float x, float y, 
            Dictionary<string, Shape3D> shapes3D, float xScale, float yScale, bool[] collisions)
        {
            Random r = new Random();

            Shape3D cube = new NRCGL.Shapes.Cube3D(new Vector3(x, 0.5f, y),
                new Color4((float)r.NextDouble(),
                               (float)r.NextDouble(),
                               (float)r.NextDouble(),
                               1f), 0); //current_texture);
            cube.Physic.Mass = 1000000;
            cube.Physic.Vxyz = Vector3.Zero;
            cube.Bounding = new Bounding(cube, xScale * 2f, 2, yScale * 2f);
            cube.Scale(xScale, 1f, yScale);
            cube.Bounding.CollisionInXL = collisions[0];
            cube.Bounding.CollisionInXR = collisions[1];
            cube.Bounding.CollisionInYU = collisions[2];
            cube.Bounding.CollisionInYD = collisions[3];
            cube.Bounding.CollisionInZF = collisions[4];
            cube.Bounding.CollisionInZB = collisions[5];
            cube.TextureShadowMap = shadowMapG.DepthTexture;
            cube.ShadowMatrix = shadowMapG.ShadowMatrix;
            cube.TextureBumpMap = bump_texture;
            cube.TexMatrix = texMatrixG;
            cube.Load();
            shapes3D.Add(Guid.NewGuid().ToString(), cube);
        }

       
    }
}
