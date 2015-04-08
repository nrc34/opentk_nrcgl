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
    class Physic
    {
        private Vector3 vxyz;
        private Vector3 aVuvw;
        private Vector3 aVxyz;
        private Vector3 fuvw;
        private Vector3 fxyz;
        private Vector3 tuvw;
        private Vector3 txyz;
        private float mass;
        private Vector3 iuvw;
        private Vector3 ixyz;
        private Shape3D shape3D;

        public Vector3 Vxyz
        {
            get { return vxyz; }
            set { vxyz = value;}
        }

        private Vector3 vuvw;

        public Vector3 Vuvw
        {
            get { return vuvw; }
            set 
            {
                Vxyz = Vector3.Transform(vuvw, shape3D.ShapeVersorsUVW);
                
                vuvw = value; 
            }
        }
        

        public Vector3 AVuvw
        {
            get { return aVuvw; }
            set { aVuvw = value; }
        }

        public Vector3 Fuvw
        {
            get { return fuvw; }
            set 
            {
                fxyz = new Vector3(Vector4.Transform(
                    new Vector4(value.X, value.Y, value.Z, 0), shape3D.ShapeVersorsUVW));
                
                fuvw = value; 
            }
        }

        public Vector3 Fxyz
        {
            get { return fxyz; }
            set 
            {
                Matrix4 versorUVW = new Matrix4(shape3D.ShapeVersorsUVW.Row0,
                                                shape3D.ShapeVersorsUVW.Row1,
                                                shape3D.ShapeVersorsUVW.Row2,
                                                shape3D.ShapeVersorsUVW.Row3);
                versorUVW.Transpose();

                fuvw = new Vector3(Vector4.Transform(
                    new Vector4(value.X, value.Y, value.Z, 0), versorUVW));
                
                fxyz = value; 
            }
        }

        public Vector3 Tuvw
        {
            get { return tuvw; }
            set { tuvw = value; }
        }

        public Vector3 Txyz
        {
            get { return txyz; }
            set { txyz = value; }
        }

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public Vector3 Iuvw
        {
            get { return iuvw; }
            set { iuvw = value; }
        }

        public Vector3 Ixyz
        {
            get { return ixyz; }
            set { ixyz = value; }
        }


        public Physic(Shape3D shape3D)
        {
            this.shape3D = shape3D;
            Vxyz = Vector3.Zero;
            AVuvw = Vector3.Zero;
            Fuvw = Vector3.Zero;
            Mass = 0f;
            Iuvw = Vector3.Zero;
        }
    }
}
