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
