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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    /// <summary>
    /// Manages the Animations pipeline for the shape.
    /// </summary>
    class AnimationsManager
    {
        public List<Animation> Animations { get; set; }

        public int CurrentAnimationIndex { get; set; }

        public int TotalOfAnimations { get; set; }

        public Shape3D Shape3D { get; set; }

        public bool Runing { get; set; }

        public bool Loop { get; set; }

        public AnimationsManager(Shape3D shape3D)
        {
            Shape3D = shape3D;

            Animations = new List<Animation>();

            CurrentAnimationIndex = 0;

            Runing = true;

            Loop = false;
        }

        public bool Animate()
        {
            TotalOfAnimations = Animations.Count;
            
            if (Runing)
            {
                if (Animations[CurrentAnimationIndex].Animate())
                {
                    return true;
                }
                else if (CurrentAnimationIndex < TotalOfAnimations - 1)
                {
                    CurrentAnimationIndex++;
                    Animations[CurrentAnimationIndex].Animate();
                    return true;
                }
                else if(Loop == true)
                {
                    CurrentAnimationIndex = 0;
                    Animations[CurrentAnimationIndex].Animate();
                    return true;
                }
                else
                {
                    CurrentAnimationIndex = 0;
                    Runing = false;
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
