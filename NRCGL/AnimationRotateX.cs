using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class AnimationRotateX : Animation
    {
        private float angle;
        
        /// <summary>
        /// Rotation angle
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        
        
        /// <summary>
        /// Animation that rotates the shape at X with an angle.
        /// </summary>
        /// <param name="angle">Rotation angle</param>
        public AnimationRotateX(Shape3D shape3D, int ticks, float angle)
        {
            Shape3D = shape3D;
            Ticks = ticks;
            Angle = angle;
            TicksCount = 0;
        }

        /// <summary>
        /// Animates the shape for 1 tick.
        /// </summary>
        /// <returns>True if ticks count is not equal to total ticks.</returns>
        public override bool Animate()
        {
            if (TicksCount < Ticks)
            {
                Shape3D.RotateX(Angle);

                TicksCount++;

                return true;
            }
            else
            {
                TicksCount = 0;

                return false;
            }

        }
    }
}
