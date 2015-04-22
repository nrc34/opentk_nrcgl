using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class SpotLight : PointLight
    {
        public float ConeAngle { get; set; }

        public Vector3 ConeDirection { get; set; }
    }
}
