using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    /// <summary>
    /// Animation that translates the shape in WC fo n game Ticks
    /// </summary>
    class AnimationTranslationWC : Animation
    {
        public Vector3 TranslationVector { get; set; }
        /// <summary>
        /// Animation that translates the shape in WC fo n game Ticks
        /// </summary>
        /// <param name="shape3D">Shape to animate.</param>
        /// <param name="ticks">Total of ticks to apply the animation.</param>
        /// <param name="vector3">Vector for the WC translation.</param>
        public AnimationTranslationWC(Shape3D shape3D, int ticks, Vector3 vector3)
        {
            Shape3D = shape3D;
            Ticks = ticks;
            TranslationVector = vector3;
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
                Shape3D.TranslateWC(TranslationVector.X,
                                    TranslationVector.Y, TranslationVector.Z);
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
