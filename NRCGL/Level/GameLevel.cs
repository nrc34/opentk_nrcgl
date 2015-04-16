using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Level
{
    class GameLevel : Level
    {

        public GameLevel()
        {
            LoadResources();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void CheckKeyBoard()
        {
            throw new NotImplementedException();
        }

        public override void LoadResources()
        {
            LoadTextures();

            LoadAudio();

            LoadShadowMap();

            LoadShapes();
        }

        private void LoadShadowMap()
        {
            throw new NotImplementedException();
        }

        private void LoadShapes()
        {
            throw new NotImplementedException();
        }

        private void LoadAudio()
        {
            throw new NotImplementedException();
        }

        private void LoadTextures()
        {
            throw new NotImplementedException();
        }
    }
}
