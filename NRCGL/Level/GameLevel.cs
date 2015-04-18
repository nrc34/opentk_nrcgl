using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
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

            GameWindow = gameWindow;

            TextRender = new TextRender(300, 300, new Vector2(10, 10));

            TextRender.Load(GameWindow.Width, GameWindow.Height);

            Load();
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
            lightView.Position = new Vector3(-150, -150, 10);
            lightView.Rotate(new Vector3(lightView.CameraUVW.Row0), MathHelper.PiOver2);
            lightView.Rotate(new Vector3(Vector3.UnitZ), MathHelper.PiOver6);
            lightView.LevelU2XZ(0.9998470f);
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

    }
}
