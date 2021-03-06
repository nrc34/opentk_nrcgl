﻿#region License
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
    class Collision : IEquatable<Collision>
    {
        private string shape3Da;
        private string shape3Db;
        private Vector3 collisionOverllap;
        private bool collisionInXL;
        private bool collisionInXR;
        private bool collisionInYU;
        private bool collisionInYD;
        private bool collisionInZF;
        private bool collisionInZB;


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

        public bool CollisionInXL
        {
            get { return collisionInXL; }
            set { collisionInXL = value; }
        }

        public bool CollisionInXR
        {
            get { return collisionInXR; }
            set { collisionInXR = value; }
        }

        public bool CollisionInYU
        {
            get { return collisionInYU; }
            set { collisionInYU = value; }
        }

        public bool CollisionInYD
        {
            get { return collisionInYD; }
            set { collisionInYD = value; }
        }

        public bool CollisionInZF
        {
            get { return collisionInZF; }
            set { collisionInZF = value; }
        }

        public bool CollisionInZB
        {
            get { return collisionInZB; }
            set { collisionInZB = value; }
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
