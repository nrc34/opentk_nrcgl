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
