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
    class Bounding
    {
        public enum Type
        {
            Sphere,
            Box
        }

        Shape3D shape3D;

        private float maxX;
        private float minX;
        private float maxY;
        private float minY;
        private float maxZ;
        private float minZ;
        private float r;
        private float boxXLength;
        private float boxYLength;
        private float boxZLength;
        private Type boundingType;


        public Type BoundingType
        {
            get { return boundingType; }
            set { boundingType = value; }
        }

        public float MaxX
        {
            get 
            {
                if (boundingType == Type.Sphere)
                    maxX = shape3D.Position.X + r;
                else
                    maxX = shape3D.Position.X + boxXLength / 2;

                return maxX; 
            }
            private set { maxX = value; }
        }

        public float MinX
        {
            get 
            {
                if (boundingType == Type.Sphere)
                    minX = shape3D.Position.X - r;
                else
                    minX = shape3D.Position.X - boxXLength / 2;
                

                return minX; 
            }
            private set { minX = value; }
        }

        public float MaxY
        {
            get
            {
                if (boundingType == Type.Sphere)
                    maxY = shape3D.Position.Y + r;
                else
                    maxY = shape3D.Position.Y + boxYLength / 2;

                return maxY;
            }
            private set { maxY = value; }
        }

        public float MinY
        {
            get
            {
                if (boundingType == Type.Sphere)
                    minY = shape3D.Position.Y - r;
                else
                    minY = shape3D.Position.Y - boxYLength / 2;
                
                return minY;
            }
            private set { minY = value; }
        }

        public float MaxZ
        {
            get
            {
                if (boundingType == Type.Sphere)
                    maxZ = shape3D.Position.Z + r;
                else
                    maxZ = shape3D.Position.Z + boxZLength / 2;
                
                return maxZ;
            }
            private set { maxZ = value; }
        }

        public float MinZ
        {
            get
            {
                if (boundingType == Type.Sphere)
                    minZ = shape3D.Position.Z - r;
                else
                    minZ = shape3D.Position.Z - boxZLength / 2;
                
                return minZ;
            }
            private set { minZ = value; }
        }

        public float R
        {
            get { return r; }
            set { r = value; }
        }

        public float BoxXLength
        {
            get { return boxXLength; }
            set { boxXLength = value; }
        }

        public float BoxYLength
        {
            get { return boxYLength; }
            set { boxYLength = value; }
        }

        public float BoxZLength
        {
            get { return boxZLength; }
            set { boxZLength = value; }
        }
        

        public Bounding(Shape3D shape3D, float r, Type type = Type.Sphere)
        {
            this.shape3D = shape3D;

            this.r = r;

            this.boundingType = type;

            if (type == Type.Box) 
                throw new ArgumentException("Bounding Type must be Sphere.");
        }

        public Bounding(Shape3D shape3D, 
            float boxXLength, float boxYLength, float boxZLength,
            Type type = Type.Box)
        {
            this.shape3D = shape3D;

            this.boxXLength = boxXLength;
            this.boxYLength = boxYLength;
            this.boxZLength = boxZLength;
            
            this.boundingType = type;

            if (type == Type.Sphere)
                throw new ArgumentException("Bounding Type must be Box.");
        }
    }
}
