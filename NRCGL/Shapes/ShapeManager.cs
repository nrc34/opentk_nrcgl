using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Shapes
{   
    /// <summary>
    /// Manages the shapes actions and lifetime.
    /// </summary>
    class ShapeManager
    {
        public static void Manage(Shape3D shape3D, 
                                  Dictionary<string, Shape3D> shapes3D)
        {
            // manage lifetime
            shape3D.LifeTime.Count();

            if (shape3D.LifeTime.IsFinish()) {
                        shape3D.IsVisible = false;
                        return;
            }

            // manage shape actions queue
            if (shape3D.ShapeActions == null) return;
            if (shape3D.ShapeActions.Count == 0) return;


            if (shape3D.ShapeActions.Peek().LifeTime.IsFinish())
                shape3D.ShapeActions.Dequeue().Execute(shape3D);
            else
                shape3D.ShapeActions.Peek().Execute(shape3D);

        }
    }
}
