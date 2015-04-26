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
using OpenTK_NRCGL.NRCGL.Level;
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
    static class MyGame
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

        public static string Debug1;
        public static string Debug2;

        static MyGame()
        {
            
        }

        public static Dictionary<string, Shape3D> LoadShapes(ShadowMap shadowMap, MyGameLevel gameLevel)
        {
            Dictionary<string, Shape3D> shapes3D = new Dictionary<string,Shape3D>();

            //float deltaRot = MathHelper.PiOver6 / 20;

            shadowMapG = shadowMap;

            //textures
            current_texture = gameLevel.Textures["current_texture"];

            int cube_texture = gameLevel.Textures["cube_texture"];

            sphere_texture = gameLevel.Textures["sphere_texture"];

            int sky_texture = gameLevel.Textures["sky_texture"];

            bump_texture = gameLevel.Textures["bump_texture"];

            int target_texture = gameLevel.Textures["target_texture"];


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

            Shape3D basePanel = new Panel3D(new Vector3(0f, 0f, 0), 0f, 0f, 0f, Color4.Chocolate, current_texture);

            // initialize shaders
            string vs = File.ReadAllText("Shaders\\vShader_UV_Normal_panel.txt");
            string fs = File.ReadAllText("Shaders\\fShader_UV_Normal_panel_base.txt");
            Shader shaderBasePanel = new Shader(ref vs, ref fs, basePanel);
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

            Shape3D target = 
                new Panel3D(new Vector3(gameLevel.TargetPosition.X, 
                                        0.05f, 
                                        gameLevel.TargetPosition.Y), 
                                        0f, 0f, 0f, 
                                        Color4.Chocolate, 
                                        target_texture);
            string vst = File.ReadAllText("Shaders\\vShader_UV_Normal_panel.txt");
            string fst = File.ReadAllText("Shaders\\fShader_UV_Normal_panel_.txt");
            Shader shaderTarget = new Shader(ref vst, ref fst, target);
            target.Shader = shaderTarget;
            target.Scale(3f);
            target.Bounding = new Bounding(target, 3f);
            target.Physic.Mass = 100000000f;
            target.RotateU(-MathHelper.PiOver3);
            target.TextureShadowMap = shadowMap.DepthTexture;
            target.ShadowMatrix = shadowMap.ShadowMatrix;
            //target.TextureBumpMap = bump_texture;
            //target.TexMatrix = texMatrix;
            target.Collision = true;
            target.IsShadowCaster = false;
            target.Load();
            shapes3D.Add("target", target);

            string[] lines = 
                File.ReadAllLines("levels\\" + gameLevel.Name + ".csv");

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
            Shape3D sphere1 = new Sphere3D(new Vector3(-45f, 1.5f, -45f), 1.5f, Color4.Gold, sphere_texture);
            sphere1.Physic.Mass = 10f;
            sphere1.Physic.Vxyz = Vector3.Zero;
            sphere1.ShadowMatrix = shadowMap.ShadowMatrix;
            sphere1.TextureShadowMap = shadowMap.DepthTexture;
            sphere1.Bounding.R = 1.5f;
            sphere1.Load();
            shapes3D.Add("sphereEnvCubeMap", sphere1);

            Shape3D sphere2 = new Sphere3D(new Vector3(-30f, 5f, -30f), 1.5f, new Color4(1f, 1f, 0.3f, 1f));
            string vs2 = File.ReadAllText("Shaders\\vShader_Color_Normal1.txt");
            string fs2 = File.ReadAllText("Shaders\\fShader_Color_Normal1.txt");
            Shader Shader2 = new Shader(ref vs, ref fs, sphere2);
            sphere2.Shader = Shader2;
            sphere2.Collision = false;
            sphere2.IsShadowCaster = false;
            sphere2.Physic.Mass = 10f;
            sphere2.Physic.Vxyz = Vector3.Zero;
            sphere2.ShadowMatrix = shadowMap.ShadowMatrix;
            sphere2.TextureShadowMap = shadowMap.DepthTexture;
            sphere2.Bounding.R = 1.5f;
            sphere2.Load();
            shapes3D.Add("pointLight", sphere2);

            //Shape3D shpereEnvCubeMap = new Sphere3DEnvCubeMap(new Vector3(0f, 30f, 0f), 10f);
            //shpereEnvCubeMap.Collision = false;
            //shpereEnvCubeMap.IsShadowCaster = false;
            //shpereEnvCubeMap.Physic.Mass = 10f;
            //shpereEnvCubeMap.Physic.Vxyz = Vector3.Zero;
            //shpereEnvCubeMap.ShadowMatrix = shadowMap.ShadowMatrix;
            ////shpereEnvCubeMap.TextureShadowMap = shadowMap.DepthTexture;
            //shpereEnvCubeMap.Load();
            //shapes3D.Add("sphereEnvM", shpereEnvCubeMap);

            Shape3D torus = new Torus3D(new Vector3(0f, 40f, 0f), 10f, Color4.Chocolate);
            torus.Collision = false;
            torus.IsShadowCaster = true;
            torus.Physic.Mass = 10f;
            torus.Physic.Vxyz = Vector3.Zero;
            torus.ShadowMatrix = shadowMap.ShadowMatrix;
            //torus.TextureShadowMap = shadowMap.DepthTexture;
            torus.Load();
            shapes3D.Add("sphereEnvM", torus);
            
            return shapes3D;
        }

        private static void insertBlocks(int xBlock, int yBlock, 
            string blockName, Dictionary<string, Shape3D> shapes3D)
        {

            Color4 color;

            float x = (xBlock * 10) - 55;
            float y = (yBlock * 10) - 55;
            switch (blockName)
            {
                case "00":
                    color = getRandomColor();
                    insertBlock(x + 3.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 5f, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 8.5f, y + 5f, shapes3D, 1.5f, 2f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 3.5f, y + 5.5f, shapes3D, 0.5f, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 5.5f, y + 3.5f, shapes3D, 1.5f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 5.5f, y + 5.5f, shapes3D, 1.5f, 1.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "10":
                case "14":
                case "30":
                case "34":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "01":
                case "03":
                case "41":
                case "43":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false }, color);
                    break;
                case "02":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 8.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "20":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 5, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    break;
                case "42":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 1.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "24":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 5, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 5, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    break;
                case "21":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 3f, shapes3D, 2, 3f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 5, y + 6.5f, shapes3D, 1f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 3.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 6.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    break;
                case "23":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 7f, shapes3D, 2, 3f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 5, y + 3.5f, shapes3D, 1f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 3.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 6.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    break;
                case "22":
                    color = getRandomColor();
                    insertBlock(x + 5, y + 5, shapes3D, 2, 5,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 1.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 8.5f, y + 5, shapes3D, 1.5f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "32":
                    color = getRandomColor();
                    insertBlock(x + 7f, y + 5, shapes3D, 3f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 3.5f, y + 5, shapes3D, 0.5f, 1f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 3.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 3.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    break;
                case "12":
                    color = getRandomColor();
                    insertBlock(x + 3f, y + 5, shapes3D, 3f, 2,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 6.5f, y + 5, shapes3D, 0.5f, 1f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 6.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 6.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    break;
                case "40":
                    color = getRandomColor();
                    insertBlock(x + 6.5f, y + 3.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 5f, y + 8.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 1.5f, y + 5f, shapes3D, 1.5f, 2f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 6.5f, y + 5.5f, shapes3D, 0.5f, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 4.5f, y + 3.5f, shapes3D, 1.5f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 4.5f, y + 5.5f, shapes3D, 1.5f, 1.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "44":
                    color = getRandomColor();
                    insertBlock(x + 6.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 5f, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 1.5f, y + 5f, shapes3D, 1.5f, 2f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 6.5f, y + 4.5f, shapes3D, 0.5f, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 4.5f, y + 6.5f, shapes3D, 1.5f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 4.5f, y + 4.5f, shapes3D, 1.5f, 1.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                case "04":
                    color = getRandomColor();
                    insertBlock(x + 3.5f, y + 6.5f, shapes3D, 0.5f, 0.5f,
                        new bool[6] { false, false, true, true, false, false }, color);
                    insertBlock(x + 5f, y + 1.5f, shapes3D, 2, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 8.5f, y + 5f, shapes3D, 1.5f, 2f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 3.5f, y + 4.5f, shapes3D, 0.5f, 1.5f,
                        new bool[6] { true, true, true, true, false, false }, color);
                    insertBlock(x + 5.5f, y + 6.5f, shapes3D, 1.5f, 0.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    insertBlock(x + 5.5f, y + 4.5f, shapes3D, 1.5f, 1.5f,
                        new bool[6] { false, false, true, true, true, true }, color);
                    break;
                default:
                    break;
            }
        }

        private static Color4 getRandomColor()
        {
            Random r = new Random();

            Color4 color = new Color4((float)r.NextDouble(),
                               (float)r.NextDouble(),
                               (float)r.NextDouble(),
                               1f);

            return color;
        }

        private static void insertBlock(float x, float y, 
            Dictionary<string, Shape3D> shapes3D, float xScale, float yScale, bool[] collisions, Color4 color)
        {
            Shape3D cube = new NRCGL.Shapes.Cube3D(new Vector3(x, 0.5f, y), color, current_texture);
                                                                            
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
