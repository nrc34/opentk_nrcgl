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
using OpenTK.Graphics.OpenGL4;
using OpenTK_NRCGL.NRCGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK_NRCGL.Game;
using OpenTK_NRCGL.NRCGL.Shapes;
using OpenTK_NRCGL.NRCGL.Audio;

namespace OpenTK_NRCGL
{
    class GL4Window : GameWindow
    {
        OpenTK.Input.MouseState oldMouseState;

        private Dictionary<string, Shape3D> shapes3D;
        private Camera camera;

        private ShadowMap ShadowMap;

        private TextRender textRender;

        private int coolDown = 0;
        private int audioCoolDown = 0;
        private int updateCount = 0;

        private float aspectRatio;

        private Audio audio;

        public GL4Window(int width = 1024, int height = 728)
            : base(width, height,
            new GraphicsMode(new ColorFormat(8, 8, 8, 8), 16, 8),
            "NRCGL",
            GameWindowFlags.Fullscreen,
            DisplayDevice.Default,
                //Major Minor implicitly assigned to 4.0
                //It's best to set to your version of GL
                //so look at the method below for help.
                //**do not set to a version above your own
            4, 0,
                //Make sure that we are only using 4.0 related stuff.
            OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)
        {
            Run(20, 60);
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            CursorVisible = false;

            #region GL_VERSION
            //this will return your version of opengl
            int major, minor;
            GL.GetInteger(GetPName.MajorVersion, out major);
            GL.GetInteger(GetPName.MinorVersion, out minor);
            Console.WriteLine("Major {0}\nMinor {1}", major, minor);
            //you can also get your GLSL version, although not sure if it varies from the above
            Console.WriteLine("GLSL {0}", GL.GetString(StringName.ShadingLanguageVersion));
            Console.WriteLine("GLSL {0}", GL.GetString(StringName.Renderer));
            Console.WriteLine("GLSL {0}", GL.GetString(StringName.Vendor));
            Console.WriteLine("GLSL {0}", GL.GetString(StringName.Version));

            Console.WriteLine(OpenTK.Graphics.GraphicsContext.CurrentContext.GraphicsMode.ToString());
            
            
            #endregion

            //set clear color [black]
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            Texture.InitTexturing();

            aspectRatio = (float)Width / Height;

            #region multiview
            if (MyGame.MultiView)
            {
                MyGame.Width = 800;
                MyGame.Height = (int)(MyGame.Width / aspectRatio);
            }
            else
            {
                MyGame.Width = Width;
                MyGame.Height = Height;
            }
            #endregion

            //setup projection 
            //ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            //    MyGame.Width / (float)MyGame.Height, 0.5f, 10000.0f);

            MyGame.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                MyGame.Width / (float)MyGame.Height, 0.5f, 1000.0f);

            GL.Viewport(0, 0, MyGame.Width, MyGame.Height);

            // shadow map
            Camera lightView = new Camera();
            lightView.Position = new Vector3(-150, -150, 10);
            //lightView.Position = new Vector3(0, -10, 0);
            lightView.Rotate(new Vector3(lightView.CameraUVW.Row0), MathHelper.PiOver2);
            lightView.Rotate(new Vector3(Vector3.UnitZ), MathHelper.PiOver6);
            //lightView.Rotate(new Vector3(lightView.CameraUVW.Row1), -MathHelper.PiOver4);
            //lightView.RotateV(-MathHelper.PiOver4);
            //lightView.RotateU(MathHelper.PiOver6);
            lightView.LevelU2XZ(0.9998470f);
            lightView.Update();

            ShadowMap = new ShadowMap(lightView);

            shapes3D = MyGame.LoadShapes(ShadowMap);

            camera = new Camera();

            //camera.Position = new Vector3(0, -100, -150);
            camera.Position = new Vector3(0, -150, 0);
            //camera.Position = new Vector3(-1000f, -1000f, -1000f);
            //camera.Rotate(new Vector3(camera.CameraUVW.Row0), MathHelper.PiOver6);
            camera.Rotate(new Vector3(camera.CameraUVW.Row0), MathHelper.PiOver2);
            //camera.Rotate(new Vector3(camera.CameraUVW.Row1), -MathHelper.PiOver6);
            //camera.RotateV(-MathHelper.PiOver4);
            //camera.RotateU(MathHelper.PiOver6);
            //camera.Position = new Vector3(-5f, -2f, 5f);
            //camera.Rotate(Vector3.UnitX, MathHelper.PiOver2);
            

            oldMouseState = OpenTK.Input.Mouse.GetCursorState();

            textRender = new TextRender(300, 300, new Vector2(10, 10));
            
            textRender.Load(Width, Height);

            shapes3D["skyBox"].Position = new Vector3(
                                          -camera.Position.X,
                                          -camera.Position.Y,
                                          -camera.Position.Z);

            string[] wavFilesNames = new string[4]{
                "Audio\\ocean-drift.wav",
                "Audio\\ball.wav",
                "Audio\\timer-with-ding.wav",
                "Audio\\yelling-yeah.wav"
            };

            audio = new Audio(wavFilesNames);

            audio.Play(0, Vector3.Zero, true, 2f); //main music
        }

        private void checkKeyBoard()
        {
            //OpenTK.Input.Keyboard.GetState();

            if (Keyboard[Key.F11] && coolDown == 0)
            {
                audio.Play(2, Vector3.Zero, true);
                coolDown = 20;
            }

            if (Keyboard[Key.F12] && coolDown == 0)
            {
                MyGame.Debug = String.Empty;
                MyGame.Debug1 = String.Empty;
                coolDown = 5;
            }

            if (Keyboard[Key.F3] && coolDown == 0)
            {
                textRender.Visible = !textRender.Visible;
                coolDown = 10;
            }

            if (Keyboard[Key.F1] && coolDown == 0)
            {
                Vector3 v1 = new Vector3(camera.CameraUVW.Row0);
                Vector3 v2 = new Vector3(camera.CameraUVW.Row0.X, 0f, camera.CameraUVW.Row0.Z);

                v2.Normalize();

                float cameraUangle = Vector3.Dot(v2, v1);

                Vector3 axis = new Vector3(camera.CameraUVW.Row2.X,
                                           camera.CameraUVW.Row2.Y, 
                                           camera.CameraUVW.Row2.Z);
                float angle = (float) Math.Acos(Convert.ToDouble(cameraUangle));

                if (camera.CameraUVW.Row0.Y > 0)
                    camera.Rotate(axis, angle);
                else
                    camera.Rotate(axis, -angle);
                

                coolDown = 20;
            }

            

            if (Keyboard[Key.I] && (MyGame.TableXAngle < MathHelper.DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableXAngle += MyGame.DeltaTableAngle;

            }

            if (Keyboard[Key.K] && (MyGame.TableXAngle > -MathHelper.DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableXAngle -= MyGame.DeltaTableAngle;

            }

            if (Keyboard[Key.J] && (MyGame.TableZAngle < MathHelper.DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableZAngle += MyGame.DeltaTableAngle;

            }

            if (Keyboard[Key.L] && (MyGame.TableZAngle > -MathHelper.DegreesToRadians(MyGame.MaxTableAngle)))
            {

                MyGame.TableZAngle -= MyGame.DeltaTableAngle;

            }
                
            if (Keyboard[Key.Q]) camera.TranslateLC(0, 0, 5);
            if (Keyboard[Key.A]) camera.TranslateLC(0, 0, -5);
            
            
            if (Keyboard[Key.X])
                camera.Position = new Vector3(camera.Position.X, -5f + camera.Position.Y, camera.Position.Z);
            if (Keyboard[Key.LShift])
                camera.Position = new Vector3(camera.Position.X, 5f + camera.Position.Y, camera.Position.Z);
            

            if (Keyboard[Key.Escape]) Exit();

            /*
            if (Keyboard[Key.Space] && coolDown == 0)
            {
                Random r = new Random();

                Shape3D sphere2 = new Sphere3D(new Vector3(-10f, 2f, -15f), 2, 
                    new Color4((float)r.NextDouble(),
                               (float)r.NextDouble(),
                               (float)r.NextDouble(),
                               1f));//, MyGame.sphere_texture);
                //sphere.Scale(10f);
                sphere2.Physic.Mass = 100f;
                sphere2.Physic.Vxyz = new Vector3(5f * (float)r.NextDouble(),
                                              0f, 5f * (float)r.NextDouble());
                sphere2.Physic.Fuvw = new Vector3(0f, 0f, 0f);
                sphere2.Load();
                shapes3D.Add(Guid.NewGuid().ToString(), sphere2);
                coolDown = 10;
            }

            if (Keyboard[Key.B] && coolDown == 0)
            {
                Shape3D sphere2 = new Sphere3D(new Vector3(10f, 2f, 10f), 2, Color4.Gold, MyGame.sphere_texture);
                //sphere.Scale(10f);
                sphere2.Physic.Mass = 10000000f;
                sphere2.Physic.Fuvw = new Vector3(0f, 0f, 0f);
                sphere2.Load();
                shapes3D.Add("movCube", sphere2);
                coolDown = 50;
            }
            
            if (Keyboard[Key.F] && coolDown == 0)
            {
                foreach (var item in shapes3D)
                {
                    if (item.Key.Contains("Panel")) continue;

                    item.Value.Physic.Vxyz = new Vector3(

                       item.Value.Physic.Vxyz.X + 1.5f,
                       item.Value.Physic.Vxyz.Y,
                       item.Value.Physic.Vxyz.Z
                    );
                }
                coolDown = 10;
            }

            if (Keyboard[Key.G] && coolDown == 0)
            {
                foreach (var item in shapes3D)
                {
                    if (item.Key.Contains("Panel")) continue;

                    item.Value.Physic.Vxyz = new Vector3(

                       item.Value.Physic.Vxyz.X - 0.5f,
                       item.Value.Physic.Vxyz.Y,
                       item.Value.Physic.Vxyz.Z
                    );
                }
                coolDown = 10;
            }*/
            
            if (Keyboard[Key.F8] && coolDown == 0)
            {
                foreach (var item in shapes3D)
                {
                    item.Value.LightPosition = new Vector3(item.Value.LightPosition.X + 50,
                                                           item.Value.LightPosition.Y,
                                                           item.Value.LightPosition.Z);
                }
                coolDown = 50;
            }

            if (Keyboard[Key.F7] && coolDown == 0)
            {
                foreach (var item in shapes3D)
                {
                    item.Value.LightPosition = new Vector3(item.Value.LightPosition.X - 50,
                                                           item.Value.LightPosition.Y,
                                                           item.Value.LightPosition.Z);
                }


                coolDown = 50;
            }

            float speed = 2f;
            if (Keyboard[Key.Keypad8] && coolDown == 0)
            {

                shapes3D["sphereEnvCubeMap"].Physic.Vuvw
                = new Vector3(shapes3D["sphereEnvCubeMap"].Physic.Vuvw.X,
                              shapes3D["sphereEnvCubeMap"].Physic.Vuvw.Y,
                              -speed);

                coolDown = 5;
            }
            if (Keyboard[Key.Keypad2] && coolDown == 0)
            {

                shapes3D["sphereEnvCubeMap"].Physic.Vuvw = new Vector3(0, 0, speed);

                coolDown = 5;
            }
            if (Keyboard[Key.Keypad4] && coolDown == 0)
            {
                shapes3D["sphereEnvCubeMap"].Physic.Vxyz = new Vector3(-speed, 0, 0);

                coolDown = 5;
            }
            if (Keyboard[Key.Keypad6] && coolDown == 0)
            {
                shapes3D["sphereEnvCubeMap"].Physic.Vxyz = new Vector3(speed, 0, 0);

                coolDown = 5;
            }
            

            if (Keyboard[Key.Number6] && coolDown == 0)
            {
                ShadowMap.LightView.TranslateWC(-10, 0, 0);
                ShadowMap.UpdateView();

                shapes3D["sphereEnvCubeMap"].ShadowMatrix = ShadowMap.ShadowMatrix;

                coolDown = 30;
            }
            if (Keyboard[Key.Number7] && coolDown == 0)
            {
                ShadowMap.LightView.TranslateWC(10, 0, 0);
                ShadowMap.UpdateView();

                shapes3D["sphereEnvCubeMap"].ShadowMatrix = ShadowMap.ShadowMatrix;

                coolDown = 30;
            }



            if (Keyboard[Key.M] && coolDown == 0)
            {
                MyGame.MultiView = !MyGame.MultiView;

                if (MyGame.MultiView)
                {
                    MyGame.Width = 800;
                    MyGame.Height = (int)(MyGame.Width / aspectRatio);
                }
                else
                {
                    MyGame.Width = Width;
                    MyGame.Height = Height;
                }

                MyGame.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                        MyGame.Width / (float)MyGame.Height, 0.5f, 1000f);

                ShadowMap.ProjectionMatrixLightView = MyGame.ProjectionMatrix;
                ShadowMap.UpdateView();

                foreach (var item in shapes3D)
                    item.Value.ShadowMatrix = ShadowMap.ShadowMatrix;

                coolDown = 30;
            }
            //if (Keyboard[Key.S]) shapes3D["sphere"].VertexBuffer.SerializeBufer(@"Models\sphere.xml");

            //if (Keyboard[Key.T]) Tools.GenerateModelFrom3DS(@"Models\sphere3D64x64x1.x3d");


            //mouse

            OpenTK.Input.MouseState mouseState = OpenTK.Input.Mouse.GetCursorState();

            float mouseStepW = 2 * MathHelper.Pi / Width;

            float mouseStepH = 2 * MathHelper.Pi / Height;

            Quaternion qcu = Quaternion.FromAxisAngle(
                new Vector3(camera.CameraUVW.Row0), (mouseState.Y - oldMouseState.Y) * mouseStepH);

            Quaternion qcv = Quaternion.FromAxisAngle(
                Vector3.UnitY, (mouseState.X - oldMouseState.X) * mouseStepW);

            camera.Rotate(qcv * qcu);

            

            if (mouseState.X > this.Width - 10) OpenTK.Input.Mouse.SetPosition(this.Width/2, this.Height/2);
            if (mouseState.X < 10) OpenTK.Input.Mouse.SetPosition(this.Width, this.Height / 2);

            if (mouseState.Y > this.Height - 10) OpenTK.Input.Mouse.SetPosition(this.Width / 2, this.Height / 2);
            if (mouseState.Y < 10) OpenTK.Input.Mouse.SetPosition(this.Width /2, this.Height);
            /*
            if (mouseState.IsButtonDown(MouseButton.Left) && coolDown == 0)
            {
                Random r = new Random();

                float bulletVeloc = 5f;
                Vector3 bulletVxyz = Vector3.Multiply(
                    new Vector3(camera.CameraUVW.Row2), -bulletVeloc);

                Shape3D bullet = new Bullet3D(Vector3.Multiply(camera.Position, -1f),
                                               bulletVxyz,
                                               new Color4((float)r.NextDouble(),
                                                          (float)r.NextDouble(),
                                                          (float)r.NextDouble(),
                                                1f));//, MyGame.sphere_texture);
                bullet.Scale(0.2f);
                bullet.Physic.Mass = 100f;
                bullet.Collision = true;
                bullet.Load();
                shapes3D.Add("bullet" + Guid.NewGuid().ToString(), bullet);
                coolDown = 5;
            }
            */

            oldMouseState = OpenTK.Input.Mouse.GetCursorState();

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (updateCount == 20)
                updateCount = 1;
            else
                updateCount++;

            if (coolDown > 0) coolDown--;
            if (audioCoolDown > 0) audioCoolDown--;


            checkKeyBoard();


            camera.LevelU2XZ(0.9998470f);

            camera.Update();

            
            foreach (var item in shapes3D)
            {
                if (item.Key == "target" && updateCount > 15) item.Value.IsVisible = false;
                else if (item.Key == "target" && updateCount <= 15) item.Value.IsVisible = true;

                if (item.Key == "sphereEnvCubeMap")
                {
                    float speed = 5f;
                    shapes3D["sphereEnvCubeMap"].Physic.Vxyz
                        = new Vector3(shapes3D["sphereEnvCubeMap"].Physic.Vxyz.X + MyGame.TableZAngle * speed * (float)Math.Cos(MyGame.TableZAngle),
                                      shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Y, // + MyGame.TableXAngle * speed * ((float)Math.Sin(MyGame.TableXAngle) - (float)Math.Sin(MyGame.TableZAngle)),
                                      shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Z + MyGame.TableXAngle * speed * (float)Math.Cos(MyGame.TableXAngle));
                    float friction = 0.85f;
                    item.Value.Physic.Vxyz = new Vector3(
                        item.Value.Physic.Vxyz.X * friction,
                        item.Value.Physic.Vxyz.Y * friction,
                        item.Value.Physic.Vxyz.Z * friction);
                }
                

                /*
                MyGame.Debug = "TabXAngl: " + MathHelper.RadiansToDegrees(MyGame.TableXAngle).ToString() +
                             "  TabZAngl: " + MathHelper.RadiansToDegrees(MyGame.TableZAngle).ToString();
                */




                if (item.Key == "basePanel" || item.Key == "target")
                {
                    item.Value.Quaternion = 
                        Quaternion.FromAxisAngle(Vector3.UnitX, MyGame.TableXAngle - MathHelper.PiOver2);
                    item.Value.Quaternion =
                        Quaternion.FromAxisAngle(Vector3.UnitZ, -MyGame.TableZAngle) * item.Value.Quaternion;
                }
                else if (item.Key != "skyBox" && item.Key != "sphereEnvCubeMap")
                {
                    item.Value.Quaternion = Quaternion.FromAxisAngle(Vector3.UnitX, MyGame.TableXAngle);
                    item.Value.Quaternion =
                        Quaternion.FromAxisAngle(Vector3.UnitZ, -MyGame.TableZAngle) * item.Value.Quaternion;
                }


                float initXAngle = 0;
                float initZAngle = 0;
                double rx = 0;
                double rz = 0;
                if (item.Key != "sphereEnvCubeMap")
                {
                    initXAngle = (float)Math.Atan2(item.Value.FirstPosition.Y, item.Value.FirstPosition.Z);
                    initZAngle = (float)Math.Atan2(item.Value.FirstPosition.Y, item.Value.FirstPosition.X);
                    rx = Math.Sqrt(Math.Pow(item.Value.FirstPosition.Y, 2) + Math.Pow(item.Value.FirstPosition.Z, 2));
                    rz = Math.Sqrt(Math.Pow(item.Value.FirstPosition.Y, 2) + Math.Pow(item.Value.FirstPosition.X, 2));
                }
                else
                {
                    initXAngle = (float)Math.Atan2(item.Value.Position.Y, item.Value.Position.Z);
                    initZAngle = (float)Math.Atan2(item.Value.Position.Y, item.Value.Position.X);
                    rx = Math.Sqrt(Math.Pow(item.Value.Position.Y, 2) + Math.Pow(item.Value.Position.Z, 2));
                    rz = Math.Sqrt(Math.Pow(item.Value.Position.Y, 2) + Math.Pow(item.Value.Position.X, 2));
                }
                double x;
                double y;
                double z;
                if (item.Key == "sphereEnvCubeMap")
                {
                    y = -(item.Value as Sphere3D).R + rx * Math.Sin(MyGame.TableXAngle - initXAngle);
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
               

                if (item.Key != "skyBox" && item.Key != "basePanel")
                    item.Value.Position = new Vector3((float)x, -(float)y, (float)z);
              

                item.Value.Update(camera.View, MyGame.ProjectionMatrix, shapes3D, camera, this);
            }


            List<Collision> collisions = PhysicHelp.CheckCollisionsOneShape(shapes3D, "sphereEnvCubeMap");

            if (collisions.Count != 0)
            {
                
                foreach (var item in collisions)
                {
                    if (item.Shape3Da == "target")
                    {
                        MyGame.Debug2 = "TARGET";
                        if(audioCoolDown == 0)audio.Play(3, Vector3.Zero);
                        audioCoolDown = 20;
                    }
                    else
                    {
                        MyGame.Debug2 = item.Shape3Da;
                    }
                }

                
                
                float velIni = shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Length;

                
                PhysicHelp.SolveCollisionsOneShape(collisions, shapes3D, "sphereEnvCubeMap", "target");
                
                float velFin = shapes3D["sphereEnvCubeMap"].Physic.Vxyz.Length;
                float deltaV = velIni - velFin;
                if (deltaV > 0.05f && audioCoolDown == 0)
                {
                    audio.Play(1, shapes3D["sphereEnvCubeMap"].Position, false, 0.3f * (deltaV * 5));
                    audioCoolDown = 4;

                    MyGame.Debug = ": " + deltaV.ToString();
                }
            }
               

            #region text render
                if (textRender.Visible && (updateCount % 10 == 0))
            {
                Vector3 v1 = new Vector3(camera.CameraUVW.Row0);
                Vector3 v2 = new Vector3(camera.CameraUVW.Row0.X, 0f, camera.CameraUVW.Row0.Z);

                v2.Normalize();

                float cameraUangle = Vector3.Dot(v2, v1);

                Vector3 axis = new Vector3(camera.CameraUVW.Row2.X,
                                           camera.CameraUVW.Row2.Y,
                                           camera.CameraUVW.Row2.Z);
                float angle = (float)Math.Acos(Convert.ToDouble(cameraUangle));

                textRender.Update("FPS : " + this.RenderFrequency + "\n" +
                              "UPS : " + this.UpdateFrequency + "\n" +
                              "WIDTH : " + MyGame.Width + "\n" +
                              "HEIGHT : " + MyGame.Height + "\n" +
                             // "COLLISIONS : " + collisions.Count + "\n" +
                              "CAMERA : " + "X= " + camera.Position.X + "; "
                                          + "Y= " + camera.Position.Y + "; "
                                          + "Z= " + camera.Position.Z + "\n" +
                              "SKYBOX : " + "X= " + shapes3D["skyBox"].Position.X + "; "
                                          + "Y= " + shapes3D["skyBox"].Position.Y + "; "
                                          + "Z= " + shapes3D["skyBox"].Position.Z + "\n" +
                               "BALL  : " + "X= " + shapes3D["sphereEnvCubeMap"].Position.X + "; "
                                          + "Y= " + shapes3D["sphereEnvCubeMap"].Position.Y + "; "
                                          + "Z= " + shapes3D["sphereEnvCubeMap"].Position.Z + "\n" +
                              "Uangle : " + cameraUangle + " = "
                              + (float)Math.Acos(Convert.ToDouble(cameraUangle)) + " = " +
                              MathHelper.RadiansToDegrees((float)Math.Acos(Convert.ToDouble(cameraUangle))) + "\n" +
                              "Shapes : " + shapes3D.Count + "\n" +
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



            shapes3D["skyBox"].Position = new Vector3(
                                          -camera.Position.X,
                                          -camera.Position.Y,
                                          -camera.Position.Z);

            ShadowMap.Update(shapes3D, MyGame.ProjectionMatrix, this);
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //render
            foreach (var item in shapes3D) item.Value.Render();
                
            textRender.Render();

            SwapBuffers();
        }
    }
}
