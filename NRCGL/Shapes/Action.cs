using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Shapes
{   
    /// <summary>
    /// Generic shape action with a lifetime, to be defined by an 
    /// action delegate anonymous method or a lambda expression.
    /// </summary>
    class ShapeAction
    {

        /// <summary>
        /// Shape action LifeTime.
        /// </summary>
        public LifeTime LifeTime { get; set; }

        /// <summary>
        /// Action delegate with Shape3D to be used and Lifetime to 
        /// define the Action duration.
        /// </summary>
        public Action<Shape3D, LifeTime> Action { get; set; }


        /// <summary>
        /// Creates a ShapeAction with an action delegate and lifetime.
        /// Lifetime defines the action duration and can be used to tween inside the
        /// action delegate method.
        /// </summary>
        /// <param name="action">Action delegate with Shape3D and Lifetime</param>
        /// <param name="lifeTime">Action LifeTime</param>
        public ShapeAction(Action<Shape3D, LifeTime> action, 
                           LifeTime lifeTime)
        {
            Action = action;

            LifeTime = lifeTime;
        }


        /// <summary>
        /// Executes the Action delegate method and increases 
        /// the LifeTime counter.
        /// </summary>
        /// <param name="shape3D"></param>
        public void Execute(Shape3D shape3D)
        {
            Action(shape3D, LifeTime);

            LifeTime.Count();
        }
    }
}
