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
        MyGameLevel myGameLevel;

        public GL4Window(int width = 1024, int height = 728)
            : base(width, height,
            new GraphicsMode(new ColorFormat(8, 8, 8, 8), 32, 8),
            "NRCGL",
            GameWindowFlags.Fullscreen,
            DisplayDevice.Default,
                // Major Minor implicitly assigned to 4.0
                // It's best to set to your version of GL
                // so look at the method below for help.
                // do not set to a version above your own
            4, 0,
                //Make sure that we are only using 4.0 related stuff.
            OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)
        {
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

            #region multiview
            if (MyGame.MultiView)
            {
                MyGame.Width = 800;
                MyGame.Height = (int)(MyGame.Width / (float)Width / Height);
            }
            else
            {
                MyGame.Width = Width;
                MyGame.Height = Height;
            }
            #endregion

            MyGame.ProjectionMatrix = 
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                MyGame.Width / (float)MyGame.Height, 
                0.5f, 
                1000.0f);

            GL.Viewport(0, 0, MyGame.Width, MyGame.Height);

            myGameLevel = 
                new MyGameLevel(1, "level1", this, new Vector2(35f, 35f));

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            myGameLevel.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            myGameLevel.Render();
        }
    }
}
