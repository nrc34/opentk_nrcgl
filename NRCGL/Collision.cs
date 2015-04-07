using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class Collision : IEquatable<Collision>
    {
        private string shape3Da;
        private string shape3Db;
        private Vector3 collisionOverllap;



        public Vector3 CollisionOverllap
        {
            get { return collisionOverllap; }
            set { collisionOverllap = value; }
        }

        public string Shape3Da
        {
            get { return shape3Da; }
            set { shape3Da = value; }
        }

        public string Shape3Db
        {
            get { return shape3Db; }
            set { shape3Db = value; }
        }

        public bool Equals(Collision other)
        {
            return (this.shape3Da == other.Shape3Da &&
                    this.shape3Db == other.Shape3Db) ||
                   (this.shape3Da == other.Shape3Db &&
                    this.shape3Db == other.Shape3Da);
        }
    }
}
