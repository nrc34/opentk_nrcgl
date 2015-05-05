using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    /// <summary>
    /// General lifetime definition.
    /// </summary>
    class LifeTime
    {
        private long counter;
        private long max;


        /// <summary>
        /// Gets or Sets the lifetime counter. 
        /// </summary>
        public long Counter
        {
            get { return counter; }
            set { counter = value; }
        }
        
        /// <summary>
        /// Gets or Sets the max value for the lifetime.
        /// </summary>
        public long Max
        {
            get { return max; }
            set { max = value; }
        }


        /// <summary>
        /// Creates a new lifetime.
        /// </summary>
        /// <param name="max">Maximum value for the lifetime. If zero, 
        /// lifetime is infinite, and IsFinish will always be false.</param>
        public LifeTime(long max)
        {
            Max = max;

            Counter = 0;
        }


        /// <summary>
        /// Adds one to the counter.
        /// </summary>
        public void Count()
        {
            counter++;
        }


        /// <summary>
        /// True if Counter is greater than Maximum Lifetime and if max != 0.
        /// Max = 0, means that the entity with this lifetime is imortal.
        /// </summary>
        /// <returns></returns>
        public bool IsFinish()
        {
            return (counter > max) && (max != 0);
        }
        
    }
}
