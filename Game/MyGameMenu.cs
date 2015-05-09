using OpenTK;
using OpenTK_NRCGL.NRCGL.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.Game
{
    class MyGameMenu : GameLevel
    {
        public MyGameMenu(GameWindow gameWindow, 
                          int id = 0, 
                          string name = "menu") 
            : base(id, name, gameWindow)
        {
            GameWindow = gameWindow;
            ID = id;
            Name = name;

        }

        public override void Load()
        {
            base.Load();
        }

        public override void LoadAudio()
        {
            base.LoadAudio();
        }

        public override void LoadShapes()
        {
            base.LoadShapes();
        }

        public override void LoadTextures()
        {
            base.LoadTextures();
        }

        public override void LoadShadowMap()
        {
            base.LoadShadowMap();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Run()
        {
            base.Run();
        }

        public override void CheckKeyBoard()
        {
            base.CheckKeyBoard();
        }

        public override void CheckMouse()
        {
            base.CheckMouse();
        }

        public override void Finish()
        {
            base.Finish();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Render()
        {
            base.Render();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
