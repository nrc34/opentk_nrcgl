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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK_NRCGL.NRCGL.Level;
using OpenTK_NRCGL.NRCGL;
using OpenTK;
using OpenTK.Input;
using OpenTK_NRCGL.NRCGL.Audio;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace OpenTK_NRCGL.Game
{
    class MyGameLevel : GameLevel
    {
        public string BlocksMapFilePath { get; set; }

        public Vector2 TargetPosition { get; set; }

        public int Clock = 100;

        DateTime DateTimeClock;

        TextRender TextRenderClock;

        private int coolDown = 0;
        private int audioCoolDown = 0;
        private int updateCount = 0;

        OpenTK.Input.MouseState oldMouseState;

        private float aspectRatio;

        private PointLight PointLight;

        private float pointLightAngle = 0; 



        public MyGameLevel(int id, string name, GameWindow gameWindow, 
                                                Vector2 targetPosition)
            : base(id, name, gameWindow)
        {

            aspectRatio = (float)GameWindow.Width / GameWindow.Height;

            oldMouseState = OpenTK.Input.Mouse.GetCursorState();

            TargetPosition = targetPosition;

            Camera = new Camera();

            Camera.Position = new Vector3(0, -150, 0);
            Camera.Rotate(new Vector3(Camera.CameraUVW.Row0), 
                                                MathHelper.PiOver2);

            Camera.Update();

            TextRenderClock = 
                new TextRender(150, 100, 
                               new Vector2(GameWindow.Width - 160 - 100, 10),
                               FontFamily.Families[19], 52);

            TextRenderClock.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

            TextRenderClock.FontStyle = FontStyle.Bold;

            TextRenderClock.Load(GameWindow.Width, GameWindow.Height);

            DateTimeClock = DateTime.Now;

            PointLight = new PointLight
            {
                Position = new Vector3(-30f, 5f, -30f),
                Color = new Vector3(0.9f, 0.9f, 0.8f),
                Intensity = 0.2f
            };

            Load();
        }

        public override void LoadShapes()
        {
            base.LoadShapes();

            Shapes3D = MyGame.LoadShapes(ShadowMap, this);
        }

        public override void LoadAudio()
        {
            base.LoadAudio();

            string[] wavFilesNames = new string[5]{
                "Audio\\ocean-drift.wav",
                "Audio\\ball.wav",
                "Audio\\timer-with-ding.wav",
                "Audio\\yelling-yeah.wav",
                "Audio\\levelFail.wav"
            };

            Audio = new Audio(wavFilesNames);

            Audio.Play(0, Vector3.Zero, true, 2f); //main music
        }

        public override void LoadTextures()
        {
            //textures

            Textures = new Dictionary<string, int>();

            Textures.Add("current_texture",
                         Texture.LoadTexture(
                         @"Textures\sand_texture1037.jpg", 0, false, false));

            Textures.Add("cube_texture",
                         Texture.LoadTexture(
                         @"Textures\cube3DTexture.png", 0, false, false));

            Textures.Add("sphere_texture",
                         Texture.LoadTexture(
                         @"Textures\beach_ball.png", 0, false, false));

            Textures.Add("sky_texture",
                         Texture.LoadTexture(
                         @"Textures\Day_Skybox.png", 0, false, false));

            Textures.Add("bump_texture",
                         Texture.LoadTexture(
                         @"Textures\Rock_01_local.jpg", 0, false, false));

            Textures.Add("target_texture",
                         Texture.LoadTexture(
                         @"Textures\target.png", 0, false, false));
        }

        public override void CheckKeyBoard()
        {
            base.CheckKeyBoard();

            if (GameWindow.Keyboard[Key.Escape]) GameWindow.Exit();

            if (IsFinished) return;

            if (GameWindow.Keyboard[Key.F10] && coolDown == 0)
            {
                Bitmap  bitmapScreenShot = Tools.GrabScreenshot(GameWindow);

                bitmapScreenShot.Save("ScreenShots\\screenshot.bmp");

                coolDown = 20;
            }

            if (GameWindow.Keyboard[Key.F11] && coolDown == 0)
            {
                Audio.Play(2, Vector3.Zero, true);
                coolDown = 20;
            }

            if (GameWindow.Keyboard[Key.F12] && coolDown == 0)
            {
                MyGame.Debug = String.Empty;
                MyGame.Debug1 = String.Empty;
                coolDown = 5;
            }

            if (GameWindow.Keyboard[Key.F3] && coolDown == 0)
            {
                TextRender.Visible = !TextRender.Visible;
                coolDown = 10;
            }

            if (GameWindow.Keyboard[Key.F1] && coolDown == 0)
            {
                Vector3 v1 = new Vector3(Camera.CameraUVW.Row0);
                Vector3 v2 = new Vector3(Camera.CameraUVW.Row0.X, 
                                         0f, Camera.CameraUVW.Row0.Z);

                v2.Normalize();

                float CameraUangle = Vector3.Dot(v2, v1);

                Vector3 axis = new Vector3(Camera.CameraUVW.Row2.X,
                                           Camera.CameraUVW.Row2.Y,
                                           Camera.CameraUVW.Row2.Z);

                float angle = (float)Math.Acos(Convert.ToDouble(CameraUangle));

                if (Camera.CameraUVW.Row0.Y > 0)
                    Camera.Rotate(axis, angle);
                else
                    Camera.Rotate(axis, -angle);


                coolDown = 20;
            }



            if (GameWindow.Keyboard[Key.I] && 
                (MyGame.TableXAngle < MathHelper.
                                       DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableXAngle += MyGame.DeltaTableAngle;

            }

            if (GameWindow.Keyboard[Key.K] && 
                (MyGame.TableXAngle > -MathHelper.
                                        DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableXAngle -= MyGame.DeltaTableAngle;

            }

            if (GameWindow.Keyboard[Key.J] && 
                (MyGame.TableZAngle < MathHelper.
                                        DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableZAngle += MyGame.DeltaTableAngle;

            }

            if (GameWindow.Keyboard[Key.L] && 
                (MyGame.TableZAngle > -MathHelper.
                                        DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableZAngle -= MyGame.DeltaTableAngle;

            }

            if (GameWindow.Keyboard[Key.Q]) Camera.TranslateLC(0, 0, 5);
            if (GameWindow.Keyboard[Key.A]) Camera.TranslateLC(0, 0, -5);


            if (GameWindow.Keyboard[Key.X])
                Camera.Position = new Vector3(Camera.Position.X,
                                              -5f + Camera.Position.Y,
                                              Camera.Position.Z);

            if (GameWindow.Keyboard[Key.LShift])
                Camera.Position = new Vector3(Camera.Position.X,
                                              5f + Camera.Position.Y,
                                              Camera.Position.Z);

            if (GameWindow.Keyboard[Key.F8] && coolDown == 0)
            {
                foreach (var item in Shapes3D)
                {
                    item.Value.LightPosition = 
                        new Vector3(item.Value.LightPosition.X + 50,
                                    item.Value.LightPosition.Y,
                                    item.Value.LightPosition.Z);
                }
                coolDown = 50;
            }

            if (GameWindow.Keyboard[Key.F7] && coolDown == 0)
            {
                foreach (var item in Shapes3D)
                {
                    item.Value.LightPosition = 
                        new Vector3(item.Value.LightPosition.X - 50,
                                    item.Value.LightPosition.Y,
                                    item.Value.LightPosition.Z);
                }


                coolDown = 50;
            }

            float speed = 2f;
            if (GameWindow.Keyboard[Key.Keypad8] && coolDown == 0)
            {

                Shapes3D["sphereEnvCubeMap"].Physic.Vuvw
                = new Vector3(Shapes3D["sphereEnvCubeMap"].Physic.Vuvw.X,
                              Shapes3D["sphereEnvCubeMap"].Physic.Vuvw.Y,
                              -speed);

                coolDown = 5;
            }
            if (GameWindow.Keyboard[Key.Keypad2] && coolDown == 0)
            {

                Shapes3D["sphereEnvCubeMap"].Physic.Vuvw = 
                                                new Vector3(0, 0, speed);

                coolDown = 5;
            }
            if (GameWindow.Keyboard[Key.Keypad4] && coolDown == 0)
            {
                Shapes3D["sphereEnvCubeMap"].Physic.Vxyz =
                                                new Vector3(-speed, 0, 0);

                coolDown = 5;
            }
            if (GameWindow.Keyboard[Key.Keypad6] && coolDown == 0)
            {
                Shapes3D["sphereEnvCubeMap"].Physic.Vxyz = 
                                                new Vector3(speed, 0, 0);

                coolDown = 5;
            }


            if (GameWindow.Keyboard[Key.Number6] && coolDown == 0)
            {
                ShadowMap.LightView.TranslateWC(-10, 0, 0);
                ShadowMap.UpdateView();

                Shapes3D["sphereEnvCubeMap"].ShadowMatrix = 
                                                ShadowMap.ShadowMatrix;

                coolDown = 30;
            }
            if (GameWindow.Keyboard[Key.Number7] && coolDown == 0)
            {
                ShadowMap.LightView.TranslateWC(10, 0, 0);
                ShadowMap.UpdateView();

                Shapes3D["sphereEnvCubeMap"].ShadowMatrix = 
                                                ShadowMap.ShadowMatrix;

                coolDown = 30;
            }



            if (GameWindow.Keyboard[Key.M] && coolDown == 0)
            {
                MyGame.MultiView = !MyGame.MultiView;

                if (MyGame.MultiView)
                {
                    MyGame.Width = 800;
                    MyGame.Height = (int)(MyGame.Width / aspectRatio);
                }
                else
                {
                    MyGame.Width = GameWindow.Width;
                    MyGame.Height = GameWindow.Height;
                }

                MyGame.ProjectionMatrix = 
                        Matrix4.CreatePerspectiveFieldOfView(
                                      MathHelper.PiOver4,
                                      MyGame.Width / (float)MyGame.Height,
                                      0.5f,
                                      1000f);

                ShadowMap.ProjectionMatrixLightView = MyGame.ProjectionMatrix;
                ShadowMap.UpdateView();

                foreach (var item in Shapes3D)
                    item.Value.ShadowMatrix = ShadowMap.ShadowMatrix;

                coolDown = 30;
            }
            //if (Keyboard[Key.S]) Shapes3D["sphere"].
            //        VertexBuffer.SerializeBufer(@"Models\sphere.xml");

            //if (Keyboard[Key.T]) Tools.
            //        GenerateModelFrom3DS(@"Models\sphere3D64x64x1.x3d");


        }

        public override void CheckMouse()
        {
            base.CheckMouse();

            //return; // disable mouse

            if (IsFinished) return;

            OpenTK.Input.MouseState mouseState = OpenTK.Input.Mouse.GetCursorState();

            float mouseStepW = 2 * MathHelper.Pi / GameWindow.Width;

            float mouseStepH = 2 * MathHelper.Pi / GameWindow.Height;

            Quaternion qcu = Quaternion.FromAxisAngle(
                new Vector3(Camera.CameraUVW.Row0), 
                           (mouseState.Y - oldMouseState.Y) * mouseStepH);

            Quaternion qcv = Quaternion.FromAxisAngle(
                Vector3.UnitY, (mouseState.X - oldMouseState.X) * mouseStepW);

            Camera.Rotate(qcv * qcu);



            if (mouseState.X > GameWindow.Width - 10) 
                OpenTK.Input.Mouse.SetPosition(
                    GameWindow.Width / 2, GameWindow.Height / 2);

            if (mouseState.X < 10)
                OpenTK.Input.Mouse.SetPosition(
                    GameWindow.Width, GameWindow.Height / 2);

            if (mouseState.Y > GameWindow.Height - 10) 
                OpenTK.Input.Mouse.SetPosition(
                    GameWindow.Width / 2, GameWindow.Height / 2);
            if (mouseState.Y < 10) 
                OpenTK.Input.Mouse.SetPosition(
                    GameWindow.Width / 2, GameWindow.Height);

            oldMouseState = OpenTK.Input.Mouse.GetCursorState();
        }


        public override void Update()
        {
            base.Update();

            if (IsFinished) return;

            if (updateCount == 20)
                updateCount = 1;
            else
                updateCount++;

            if (coolDown > 0) coolDown--;
            if (audioCoolDown > 0) audioCoolDown--;





            foreach (var item in Shapes3D)
            {
                if (item.Key == "target" && updateCount > 15)
                    item.Value.IsVisible = false;

                else if (item.Key == "target" && updateCount <= 15)
                    item.Value.IsVisible = true;

                if (item.Key == "sphereEnvCubeMap")
                {
                    float speed = 5f;
                    Shapes3D["sphereEnvCubeMap"].Physic.Vxyz
                        = new Vector3(Shapes3D["sphereEnvCubeMap"].
                                        Physic.Vxyz.X +
                                        MyGame.TableZAngle * speed *
                                        (float)Math.Cos(MyGame.TableZAngle),

                                      Shapes3D["sphereEnvCubeMap"].
                                        Physic.Vxyz.Y, // +
                        // MyGame.TableXAngle * speed * 
                        // ((float)Math.Sin(MyGame.TableXAngle)
                        //- (float)Math.Sin(MyGame.TableZAngle)),

                                      Shapes3D["sphereEnvCubeMap"].
                                        Physic.Vxyz.Z +
                                        MyGame.TableXAngle * speed *
                                        (float)Math.Cos(MyGame.TableXAngle));

                    float friction = 0.85f;
                    item.Value.Physic.Vxyz = new Vector3(
                        item.Value.Physic.Vxyz.X * friction,
                        item.Value.Physic.Vxyz.Y * friction,
                        item.Value.Physic.Vxyz.Z * friction);
                }


                if (item.Key == "basePanel" || item.Key == "target")
                {
                    item.Value.Quaternion =
                        Quaternion.
                            FromAxisAngle(
                                Vector3.UnitX,
                                MyGame.TableXAngle - MathHelper.PiOver2);

                    item.Value.Quaternion =
                        Quaternion.
                            FromAxisAngle(
                                Vector3.UnitZ, -MyGame.TableZAngle)
                        * item.Value.Quaternion;

                }
                else if (item.Key != "skyBox" &&
                         item.Key != "sphereEnvCubeMap" &&
                         item.Key != "spotLight")
                {
                    item.Value.Quaternion =
                        Quaternion.FromAxisAngle(
                                Vector3.UnitX, MyGame.TableXAngle);

                    item.Value.Quaternion =
                        Quaternion.FromAxisAngle(
                                Vector3.UnitZ, -MyGame.TableZAngle)
                        * item.Value.Quaternion;
                }


                float initXAngle = 0;
                float initZAngle = 0;
                double rx = 0;
                double rz = 0;
                if (item.Key != "sphereEnvCubeMap")
                {
                    initXAngle = (float)Math.Atan2(item.Value.FirstPosition.Y,
                                                   item.Value.FirstPosition.Z);

                    initZAngle = (float)Math.Atan2(item.Value.FirstPosition.Y,
                                                   item.Value.FirstPosition.X);

                    rx = Math.Sqrt(Math.Pow(item.Value.FirstPosition.Y, 2)
                                    + Math.Pow(item.Value.FirstPosition.Z, 2));

                    rz = Math.Sqrt(Math.Pow(item.Value.FirstPosition.Y, 2)
                                    + Math.Pow(item.Value.FirstPosition.X, 2));
                }
                else
                {
                    initXAngle = (float)Math.Atan2(item.Value.Position.Y,
                                                   item.Value.Position.Z);

                    initZAngle = (float)Math.Atan2(item.Value.Position.Y,
                                                   item.Value.Position.X);

                    rx = Math.Sqrt(Math.Pow(item.Value.Position.Y, 2)
                                    + Math.Pow(item.Value.Position.Z, 2));

                    rz = Math.Sqrt(Math.Pow(item.Value.Position.Y, 2)
                                    + Math.Pow(item.Value.Position.X, 2));
                }
                double x;
                double y;
                double z;
                if (item.Key == "sphereEnvCubeMap")
                {
                    y = -(item.Value as Sphere3D).R
                        + rx * Math.Sin(MyGame.TableXAngle - initXAngle);
                    z = rx * Math.Cos(MyGame.TableXAngle - initXAngle);
                    y -= rz * Math.Sin(-MyGame.TableZAngle - initZAngle);
                    x = rz * Math.Cos(-MyGame.TableZAngle - initZAngle);
                }
                else
                {
                    y = rx * Math.Sin(MyGame.TableXAngle - initXAngle);
                    z = rx * Math.Cos(MyGame.TableXAngle - initXAngle);
                    y += rz * Math.Sin(MyGame.TableZAngle - initZAngle);
                    x = rz * Math.Cos(MyGame.TableZAngle - initZAngle);
                }


                if (item.Key != "skyBox" && 
                    item.Key != "basePanel" && 
                    item.Key != "spotLight")
                        item.Value.Position =
                            new Vector3((float)x, -(float)y, (float)z);

                pointLightAngle += 0.0005f;

                PointLight.Position = 
                    new Vector3(40f * (float)Math.Cos(pointLightAngle),
                                PointLight.Position.Y,
                                40f * (float)Math.Sin(pointLightAngle));

                PointLight.Color =
                    new Vector3(pointLightAngle / 10, 0.9f, pointLightAngle / 10);

                item.Value.PointLight.Position = PointLight.Position;
                item.Value.PointLight.Color = PointLight.Color;
                item.Value.PointLight.Intensity = (float)Math.Sin(pointLightAngle) / 2;

                if (item.Key == "pointLight")
                        item.Value.Position = PointLight.Position;


                item.Value.Update(Camera.View,
                                  MyGame.ProjectionMatrix,
                                  Shapes3D,
                                  Camera,
                                  GameWindow);

            }

            List<Collision> collisions = 
                PhysicHelp.CheckCollisionsOneShape(
                                Shapes3D, "sphereEnvCubeMap");

            if (collisions.Count != 0)
            {

                foreach (var item in collisions)
                {
                    if (item.Shape3Da == "target")
                    {
                        MyGame.Debug2 = "TARGET";


                        Audio.Play(3, Vector3.Zero);

                        IsFinished = true;
                    }
                    else
                    {
                        MyGame.Debug2 = item.Shape3Da;
                    }
                }



                float velIni = Shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Length;


                PhysicHelp.SolveCollisionsOneShape(collisions, 
                                                   Shapes3D, 
                                                   "sphereEnvCubeMap", 
                                                   "target");

                float velFin = Shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Length;
                float deltaV = velIni - velFin;
                if (deltaV > 0.05f && audioCoolDown == 0)
                {
                    Audio.Play(1, 
                               Shapes3D["sphereEnvCubeMap"].Position,
                               false, 
                               0.3f * (deltaV * 5));

                    audioCoolDown = 4;

                    MyGame.Debug = ": " + deltaV.ToString();
                }
            }


            #region text render
            if (TextRender.Visible && (updateCount % 10 == 0))
            {
                Vector3 v1 = new Vector3(Camera.CameraUVW.Row0);
                Vector3 v2 = new Vector3(Camera.CameraUVW.Row0.X,
                                         0f, 
                                         Camera.CameraUVW.Row0.Z);

                v2.Normalize();

                float CameraUangle = Vector3.Dot(v2, v1);

                Vector3 axis = new Vector3(Camera.CameraUVW.Row2.X,
                                           Camera.CameraUVW.Row2.Y,
                                           Camera.CameraUVW.Row2.Z);

                float angle = (float)Math.Acos(Convert.ToDouble(CameraUangle));

                TextRender.Update("FPS : " + GameWindow.RenderFrequency + "\n" +
                              "UPS : " + GameWindow.UpdateFrequency + "\n" +
                              "WIDTH : " + MyGame.Width + "\n" +
                              "HEIGHT : " + MyGame.Height + "\n" +
                    // "COLLISIONS : " + collisions.Count + "\n" +
                              "Camera : " + "X= " + Camera.Position.X + "; "
                                          + "Y= " + Camera.Position.Y + "; "
                                          + "Z= " + Camera.Position.Z + "\n" +
                              "SKYBOX : " + "X= " + Shapes3D["skyBox"].Position.X + "; "
                                          + "Y= " + Shapes3D["skyBox"].Position.Y + "; "
                                          + "Z= " + Shapes3D["skyBox"].Position.Z + "\n" +
                               "BALL  : " + "X= " + Shapes3D["sphereEnvCubeMap"].Position.X + "; "
                                          + "Y= " + Shapes3D["sphereEnvCubeMap"].Position.Y + "; "
                                          + "Z= " + Shapes3D["sphereEnvCubeMap"].Position.Z + "\n" +
                              "Uangle : " + CameraUangle + " = "
                              + (float)Math.Acos(Convert.ToDouble(CameraUangle)) + " = " +
                              MathHelper.RadiansToDegrees((float)Math.Acos(Convert.ToDouble(CameraUangle))) + "\n" +
                              "Shapes : " + Shapes3D.Count + "\n" +
                              "Light_X : " + ShadowMap.LightView.Position.X + "\n" +
                              "Light_Y : " + ShadowMap.LightView.Position.Y + "\n" +
                              "Light_Z : " + ShadowMap.LightView.Position.Z + "\n" +
                              "Table_X_Angle : " + MathHelper.RadiansToDegrees(MyGame.TableXAngle) + "\n" +
                              "Table_Z_Angle : " + MathHelper.RadiansToDegrees(MyGame.TableZAngle) + "\n" +
                              "DEBUG : " + MyGame.Debug + "\n" +
                              "DEBUG1 : " + MyGame.Debug1 + "\n" +
                              "DEBUG2 : " + MyGame.Debug2 + "\n"
                              );
            }
            #endregion


            Clock =  80 + ((int)(DateTimeClock.Ticks - DateTime.Now.Ticks)/10000000);

            if (Clock <= 0)
            {
                IsFinished = true;
                Audio.Play(4, Vector3.Zero);
            }

            TextRenderClock.Update(Clock.ToString());

            Shapes3D["skyBox"].Position = new Vector3(
                                          -Camera.Position.X,
                                          -Camera.Position.Y,
                                          -Camera.Position.Z);

            ShadowMap.Update(Shapes3D, MyGame.ProjectionMatrix, GameWindow);
            
        }

        public override void Render()
        {
            base.Render();

            GL.Clear(ClearBufferMask.ColorBufferBit |
                                ClearBufferMask.DepthBufferBit);


            foreach (var item in Shapes3D) item.Value.Render();

            TextRender.Render();
            TextRenderClock.Render();

            GameWindow.SwapBuffers();
        }


        public override void Finish()
        {
            base.Finish();

            Shapes3D["sphereEnvCubeMap"].Position =
                new Vector3(TargetPosition.X, 
                            Shapes3D["sphereEnvCubeMap"].Position.Y, 
                            TargetPosition.Y);

            Shapes3D["sphereEnvCubeMap"].Physic.Vxyz = Vector3.Zero;

            Camera FinishCamera = new Camera();

            FinishCamera.Position = new Vector3(0, -150, 0);
            FinishCamera.Rotate(new Vector3(FinishCamera.CameraUVW.Row0),
                                                      MathHelper.PiOver2);

            FinishCamera.Update();
            /*
            Camera.Position = new Vector3(FinishCamera.Position.X,
                                          FinishCamera.Position.Y,
                                          FinishCamera.Position.Z);

            Camera.Quaternion = new Quaternion(FinishCamera.Quaternion.X,
                                               FinishCamera.Quaternion.Y,
                                               FinishCamera.Quaternion.Z,
                                               FinishCamera.Quaternion.W);

            Camera.Update();
            */
            float initXAngle = 
                (float)Math.Atan2(Shapes3D["sphereEnvCubeMap"].Position.Y,
                                  Shapes3D["sphereEnvCubeMap"].Position.Z);

            float initZAngle = 
                (float)Math.Atan2(Shapes3D["sphereEnvCubeMap"].Position.Y,
                                  Shapes3D["sphereEnvCubeMap"].Position.X);

            double rx = 
                Math.Sqrt(Math.Pow(Shapes3D["sphereEnvCubeMap"].Position.Y, 2)
                + Math.Pow(Shapes3D["sphereEnvCubeMap"].Position.Z, 2));

            double rz = 
                Math.Sqrt(Math.Pow(Shapes3D["sphereEnvCubeMap"].Position.Y, 2)
                + Math.Pow(Shapes3D["sphereEnvCubeMap"].Position.X, 2));

            double y = -(Shapes3D["sphereEnvCubeMap"] as Sphere3D).R
                        + rx * Math.Sin(MyGame.TableXAngle - initXAngle);
            double z = rx * Math.Cos(MyGame.TableXAngle - initXAngle);
            y -= rz * Math.Sin(-MyGame.TableZAngle - initZAngle);
            double x = rz * Math.Cos(-MyGame.TableZAngle - initZAngle);

            Shapes3D["sphereEnvCubeMap"].Position =
                        new Vector3((float)x, -(float)y, (float)z);

            Shapes3D["target"].IsVisible = true;

            foreach (Shape3D item in Shapes3D.Values)
            {
                item.Update(FinishCamera.View,
                            MyGame.ProjectionMatrix,
                            Shapes3D,
                            FinishCamera,
                            GameWindow);
            }

            

            ShadowMap.Update(Shapes3D, MyGame.ProjectionMatrix, GameWindow);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}
