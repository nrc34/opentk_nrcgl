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
    /// Animation that rotates the shape  with ans axis and an angle.
    /// </summary>
    class AnimationRotate : Animation
    {
        private Vector3 axis;
        private float angle;
        private bool isCenterWC;


        /// <summary>
        /// Defines if rotation axis base is at wcs(0,0,0) or lcs(0,0,0).
        /// </summary>
        public bool IsCenterWC
        {
            get { return isCenterWC; }
            set { isCenterWC = value; }
        }

        /// <summary>
        /// Rotation axis
        /// </summary>
        public Vector3 Axis
        {
            get { return axis; }
            set { axis = value; }
        }
        
        /// <summary>
        /// Rotation angle
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        
        /// <summary>
        /// Animation that rotates the shape  with an axis and an angle.
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="angle">Rotation angle</param>
        public AnimationRotate(Shape3D shape3D, int ticks, Vector3 axis, float angle)
        {
            Shape3D = shape3D;
            Ticks = ticks;
            Axis = axis;
            Angle = angle;
            TicksCount = 0;
            IsCenterWC = false;

        }

        /// <summary>
        /// Animation that rotates the shape  with an axis and an angle. Base of axis
        /// can be wcs(0,0,0) or lcs(0,0,0).
        /// </summary>
        /// <param name="shape3D"></param>
        /// <param name="ticks"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <param name="isCenterWC"></param>
        public AnimationRotate(Shape3D shape3D, int ticks, Vector3 axis, 
             float angle, bool isCenterWC)
        {
            Shape3D = shape3D;
            Ticks = ticks;
            Axis = axis;
            Angle = angle;
            TicksCount = 0;
            IsCenterWC = isCenterWC;
        }

        /// <summary>
        /// Animates the shape for 1 tick.
        /// </summary>
        /// <returns>True if ticks count is not equal to total ticks.</returns>
        public override bool Animate()
        {
            if (TicksCount < Ticks)
            {
                Shape3D.Rotate(Axis, Angle);

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
