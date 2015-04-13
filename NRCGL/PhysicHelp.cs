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
using OpenTK_NRCGL.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL
{
    class PhysicHelp
    {
        public static List<Collision> CheckCollisions(Dictionary<string, Shape3D> shapes3D)
        {
            List<Collision> collisions = new List<Collision>();

            foreach (var item2check in shapes3D)
            {
                if (item2check.Value.Collision == false) continue;
                foreach (var item in shapes3D)
                {
                    if (item.Value.Collision == false) continue;

                    if (item2check.Key != item.Key)
                    {
                        
                        bool collisionInX = 
                            (item2check.Value.Bounding.MaxX > item.Value.Bounding.MinX &&
                             item2check.Value.Position.X <= item.Value.Position.X) ||
                            (item2check.Value.Bounding.MinX < item.Value.Bounding.MaxX &&
                             item2check.Value.Position.X >= item.Value.Position.X);

                        bool collisionInY =
                            (item2check.Value.Bounding.MaxY > item.Value.Bounding.MinY &&
                             item2check.Value.Position.Y <= item.Value.Position.Y) ||
                            (item2check.Value.Bounding.MinY < item.Value.Bounding.MaxY &&
                             item2check.Value.Position.Y >= item.Value.Position.Y);

                        bool collisionInZ =
                            (item2check.Value.Bounding.MaxZ > item.Value.Bounding.MinZ &&
                             item2check.Value.Position.Z <= item.Value.Position.Z) ||
                            (item2check.Value.Bounding.MinZ < item.Value.Bounding.MaxZ &&
                             item2check.Value.Position.Z >= item.Value.Position.Z);

                        if (collisionInX && collisionInY && collisionInZ)
                        {
                            Collision collision = new Collision();
                            collision.Shape3Da = item2check.Key;
                            collision.Shape3Db = item.Key;
                            if(!collisions.Contains(collision))collisions.Add(collision);
                        }
                    }
                }
            }

            return collisions;
        }

        public static List<Collision> CheckCollisionsOneShape(Dictionary<string, Shape3D> shapes3D, string shapeName)
        {
            List<Collision> collisions = new List<Collision>();

            foreach (var item2check in shapes3D)
            {
                if (item2check.Value.Collision == false) continue;

                if (shapes3D[shapeName].Collision == false) return null;

                if (item2check.Key != shapeName)
                {

                    bool collisionInXL =
                        (item2check.Value.Bounding.MinX < shapes3D[shapeName].Bounding.MaxX &&
                            item2check.Value.Position.X >= shapes3D[shapeName].Position.X); 

                    bool collisionInXR =
                        (item2check.Value.Bounding.MaxX > shapes3D[shapeName].Bounding.MinX &&
                            item2check.Value.Position.X <= shapes3D[shapeName].Position.X);
                    
                    bool collisionInYU =
                        (item2check.Value.Bounding.MaxY > shapes3D[shapeName].Bounding.MinY &&
                            item2check.Value.Position.Y <= shapes3D[shapeName].Position.Y);

                    bool collisionInYD =
                        (item2check.Value.Bounding.MinY < shapes3D[shapeName].Bounding.MaxY &&
                            item2check.Value.Position.Y >= shapes3D[shapeName].Position.Y);

                    bool collisionInZF =
                        (item2check.Value.Bounding.MaxZ > shapes3D[shapeName].Bounding.MinZ &&
                            item2check.Value.Position.Z <= shapes3D[shapeName].Position.Z);

                    bool collisionInZB =
                        (item2check.Value.Bounding.MinZ < shapes3D[shapeName].Bounding.MaxZ &&
                            item2check.Value.Position.Z >= shapes3D[shapeName].Position.Z);

                    // collision detection
                    if ((collisionInXL || collisionInXR) && 
                        (collisionInYU || collisionInYD) && 
                        (collisionInZF || collisionInZB))
                    {
                        Collision collision = new Collision();
                        collision.Shape3Da = item2check.Key;
                        collision.Shape3Db = shapeName;

                        collision.CollisionInXL = collisionInXL;
                        collision.CollisionInXR = collisionInXR;
                        collision.CollisionInYD = collisionInYD;
                        collision.CollisionInYU = collisionInYU;
                        collision.CollisionInZB = collisionInZB;
                        collision.CollisionInZF = collisionInZF;

                        if (!collisions.Contains(collision))
                            collisions.Add(collision);

                    }
                    
                }
            }
            if (collisions.Count > 1) MyGame.Debug = "Multiple col";
            return collisions;
        }

        public static void SolveCollisions(List<Collision> collisions, Dictionary<string, Shape3D> shapes3D)
        {
            foreach (var item in collisions)
            {
                /*
                shapes3D[item.Shape3Da].Position = new Vector3(
                    shapes3D[item.Shape3Da].PositionOld.X,
                    shapes3D[item.Shape3Da].PositionOld.Y,
                    shapes3D[item.Shape3Da].PositionOld.Z);

                shapes3D[item.Shape3Db].Position = new Vector3(
                                    shapes3D[item.Shape3Db].PositionOld.X,
                                    shapes3D[item.Shape3Db].PositionOld.Y,
                                    shapes3D[item.Shape3Db].PositionOld.Z);*/
                


                Vector3 Va = shapes3D[item.Shape3Da].Physic.Vxyz;
                Vector3 Vb = shapes3D[item.Shape3Db].Physic.Vxyz;
                Vector3 Vab = new Vector3();

                Vab = Va - Vb;

                Vector3 Pa = shapes3D[item.Shape3Da].Position;
                Vector3 Pb = shapes3D[item.Shape3Db].Position;

                

                Vector3 n = new Vector3();

                n = Pa - Pb;

                if (n.Length < 3.7)
                {
                    shapes3D[item.Shape3Da].TranslateWC(shapes3D[item.Shape3Da].Physic.Vxyz.X * -2f,
                                                    shapes3D[item.Shape3Da].Physic.Vxyz.Y * -2f,
                                                    shapes3D[item.Shape3Da].Physic.Vxyz.Z * -2f);

                    shapes3D[item.Shape3Db].TranslateWC(shapes3D[item.Shape3Db].Physic.Vxyz.X * -2f,
                                                        shapes3D[item.Shape3Db].Physic.Vxyz.Y * -2f,
                                                        shapes3D[item.Shape3Db].Physic.Vxyz.Z * -2f);
                
                }

                n.Normalize();

                float Vn = Vector3.Dot(Vab, n);

                float coefElaticity = 0.6f;
                float ma = shapes3D[item.Shape3Da].Physic.Mass;
                float mb = shapes3D[item.Shape3Db].Physic.Mass;

                float J = (Vector3.Dot(Vector3.Multiply(Vab, -(1 + coefElaticity)), n))
                           /
                           (Vector3.Dot(n, Vector3.Multiply(n, (1/ma)+(1/mb))));

                Vector3 nJma = Vector3.Multiply(n, (J/ma));

                shapes3D[item.Shape3Da].Physic.Vxyz = Va + nJma;


                Vector3 nJmb = Vector3.Multiply(n, (J / mb));

                shapes3D[item.Shape3Db].Physic.Vxyz = Vb - nJmb;


                
                
                
                Dictionary<string, Shape3D> checkOut = new Dictionary<string, Shape3D>();

                checkOut.Add("A", shapes3D[item.Shape3Da]);
                checkOut.Add("B", shapes3D[item.Shape3Db]);

                int colCount = CheckCollisions(checkOut).Count;
                int i = 0;
                while (colCount > 0)
                {
                    
                        checkOut["A"].TranslateWC(
                        checkOut["A"].Physic.Vxyz.X,
                        checkOut["A"].Physic.Vxyz.Y,
                        checkOut["A"].Physic.Vxyz.Z);

                        checkOut["B"].TranslateWC(
                        checkOut["B"].Physic.Vxyz.X,
                        checkOut["B"].Physic.Vxyz.Y,
                        checkOut["B"].Physic.Vxyz.Z);

                    colCount = CheckCollisions(checkOut).Count;

                    if (i == 10) break;
                    i++;
                }
            }
        }

        public static void SolveCollisionsOneShape(List<Collision> collisions,
            Dictionary<string, Shape3D> shapes3D, string shapeName, string shape2Ignore)
        {

            //TODO: solve multiple collisions

            foreach (Collision item in collisions)
            {

                if (item.Shape3Da == shape2Ignore) continue;

                //move shapeName from colision
                float deltaX = 0;
                float deltaY = 0;
                float deltaZ = 0;

                bool asVx = Math.Abs(shapes3D[shapeName].Physic.Vxyz.X) > 0;
                bool asVy = Math.Abs(shapes3D[shapeName].Physic.Vxyz.Y) > 0;
                bool asVz = Math.Abs(shapes3D[shapeName].Physic.Vxyz.Z) > 0;

                if (asVx)
                {
                    if (item.CollisionInXL)
                        deltaX = shapes3D[item.Shape3Da].Bounding.MinX - shapes3D[shapeName].Bounding.MaxX;

                    if (item.CollisionInXR)
                        deltaX = shapes3D[item.Shape3Da].Bounding.MaxX - shapes3D[shapeName].Bounding.MinX;
                }

                if (asVy)
                {
                    if (item.CollisionInYD)
                        deltaY = shapes3D[item.Shape3Da].Bounding.MinY - shapes3D[shapeName].Bounding.MaxY;

                    if (item.CollisionInYU)
                        deltaY = shapes3D[item.Shape3Da].Bounding.MaxY - shapes3D[shapeName].Bounding.MinY;
                }

                if (asVz)
                {
                    if (item.CollisionInZB)
                        deltaZ = shapes3D[item.Shape3Da].Bounding.MinZ - shapes3D[shapeName].Bounding.MaxZ;

                    if (item.CollisionInZF)
                        deltaZ = shapes3D[item.Shape3Da].Bounding.MaxZ - shapes3D[shapeName].Bounding.MinZ;
                }


                item.CollisionOverllap = new Vector3(deltaX, deltaY, deltaZ);
                //detect the face that has collided X or Z
                //float dpX = Math.Abs(shapes3D[shapeName].Position.X - shapes3D[item.Shape3Da].Position.X) / shapes3D[item.Shape3Da].Bounding.BoxXLength;
                //float dpZ = Math.Abs(shapes3D[shapeName].Position.Z - shapes3D[item.Shape3Da].Position.Z) / shapes3D[item.Shape3Da].Bounding.BoxZLength;
                float el = -0.5f;
                float dd = 1f;
                MyGame.Debug1 = string.Empty;
                MyGame.Debug1 = item.CollisionInXL ? "XL :" : 
                                item.CollisionInXR ? "XR :" : "";
                MyGame.Debug1 += item.CollisionInYD ? "YD :" :
                                 item.CollisionInYU ? "YU :" : "";
                MyGame.Debug1 += item.CollisionInZB ? "ZB :" :
                                item.CollisionInZF ? "ZF" : "0";

                if (true && //dpX > dpZ && 
                    (shapes3D[item.Shape3Da].Bounding.CollisionInXR && item.CollisionInXR) ||
                    (shapes3D[item.Shape3Da].Bounding.CollisionInXL && item.CollisionInXL))
                {
                    shapes3D[shapeName].Physic.Vxyz
                     = new Vector3(
                         shapes3D[shapeName].Physic.Vxyz.X * el,
                         shapes3D[shapeName].Physic.Vxyz.Y,
                         shapes3D[shapeName].Physic.Vxyz.Z);

                    Vector3 newPosition = new Vector3(
                     shapes3D[shapeName].Position.X + deltaX * dd,
                     shapes3D[shapeName].Position.Y + deltaY * dd,
                     shapes3D[shapeName].Position.Z);

                    shapes3D[shapeName].Position = newPosition;

                    //MyGame.Debug = "Col in X";
                }

                else if (true && //dpX < dpZ &&
                         (shapes3D[item.Shape3Da].Bounding.CollisionInZF && item.CollisionInZF) ||
                         (shapes3D[item.Shape3Da].Bounding.CollisionInZB && item.CollisionInZB))
                {

                    shapes3D[shapeName].Physic.Vxyz
                      = new Vector3(
                          shapes3D[shapeName].Physic.Vxyz.X,
                          shapes3D[shapeName].Physic.Vxyz.Y,
                          shapes3D[shapeName].Physic.Vxyz.Z * el);

                    Vector3 newPosition = new Vector3(
                      shapes3D[shapeName].Position.X,
                      shapes3D[shapeName].Position.Y + deltaY * dd,
                      shapes3D[shapeName].Position.Z + deltaZ * dd);

                    shapes3D[shapeName].Position = newPosition;

                    //MyGame.Debug = "Col in Z";
                }
                //else
                //{
                //    shapes3D[shapeName].Physic.Vxyz
                //       = new Vector3(
                //           shapes3D[shapeName].Physic.Vxyz.X * el,
                //           shapes3D[shapeName].Physic.Vxyz.Y,
                //           shapes3D[shapeName].Physic.Vxyz.Z * el);

                //    Vector3 newPosition = new Vector3(
                //      shapes3D[shapeName].Position.X + deltaX * dd,
                //      shapes3D[shapeName].Position.Y + deltaY * dd,
                //      shapes3D[shapeName].Position.Z + deltaZ * dd);

                //    shapes3D[shapeName].Position = newPosition;

                //    MyGame.Debug = "Col in ZX";
                //}
            }
            
        }
    }
}
