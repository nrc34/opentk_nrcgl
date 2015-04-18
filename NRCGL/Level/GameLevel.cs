using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    class GameLevel : Level
    {

        public GameLevel(int id, string name)
        {
            ID = id;
            Name = name;

            Load();
        }

        public override void Load()
        {
            LoadTextures();

            LoadAudio();

            LoadShadowMap();

            LoadShapes();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public void LoadShadowMap()
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

        public void LoadShapes()
        {
            throw new NotImplementedException();
        }

        public void LoadAudio()
        {
            throw new NotImplementedException();
        }

        public void LoadTextures()
        {
            throw new NotImplementedException();
        }

        public override void CheckMouse()
        {
            throw new NotImplementedException();
        }

        public override void CheckKeyBoard()
        {
            throw new NotImplementedException();
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
