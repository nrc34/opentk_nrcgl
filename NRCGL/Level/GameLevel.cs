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
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    class GameLevel : Level
    {

        public GameLevel(int id, string name, GameWindow gameWindow)
        {
            ID = id;
            Name = name;

            IsFinished = false;

            GameWindow = gameWindow;

            TextRender = new TextRender(300, 300, new Vector2(10, 10), 
                                        FontFamily.Families[125], 9, false);

            TextRender.Load(GameWindow.Width, GameWindow.Height);

        }


        public override void Load()
        {
            LoadTextures();

            LoadAudio();

            LoadShadowMap();

            LoadShapes();
        }

        public virtual void LoadShadowMap()
        {
            Camera lightView = new Camera();

            // lighth view to initialize the shadow map. can be changed.
            lightView.Position = new Vector3(-125.5173f, -112.2577f, -1.61722f);
            //lightView.Rotate(new Vector3(lightView.CameraUVW.Row0), MathHelper.PiOver2);
            //lightView.Rotate(new Vector3(Vector3.UnitZ), MathHelper.PiOver6);
            //lightView.LevelU2XZ(0.9998470f);
            //V: (0,2715916; -0,6443541; -0,2702386), W: 0,6618307
            lightView.Quaternion = 
                new Quaternion(0.2715916f, -0.6443541f, -0.2702386f, 0.6618307f);
            lightView.Update();

            ShadowMap = new ShadowMap(lightView);
        }

        public virtual void LoadShapes() 
        {
        }

        public virtual void LoadAudio()
        {
        }

        public virtual void LoadTextures()
        {
        }


        public override void Unload()
        {
            throw new NotImplementedException();
        }


        public override void Update()
        {
            CheckKeyBoard();

            CheckMouse();

            Camera.LevelU2XZ(0.9998470f);

            Camera.Update();

            if (IsFinished)
            {
                Finish();
                return;
            }
        }

        public override void CheckMouse()
        {
        }

        public override void CheckKeyBoard()
        {
        }


        public override void Render()
        {
        }

        public override void Finish()
        {
        }

    }
}
