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

                    if ((collisionInXL || collisionInXR) && 
                        (collisionInYU || collisionInYD) && 
                        (collisionInZF || collisionInZB))
                    {
                        Collision collision = new Collision();
                        collision.Shape3Da = item2check.Key;
                        collision.Shape3Db = shapeName;
                        if (!collisions.Contains(collision))
                        {
                            collisions.Add(collision);
                            //move shapeName from colision
                            float deltaX = 0;
                            float deltaY = 0;
                            float deltaZ = 0;

                            bool asVx = Math.Abs(shapes3D[shapeName].Physic.Vxyz.X) > 0;
                            bool asVy = Math.Abs(shapes3D[shapeName].Physic.Vxyz.Y) > 0;
                            bool asVz = Math.Abs(shapes3D[shapeName].Physic.Vxyz.Z) > 0;
                            
                            if(asVx)
                            {
                                if(collisionInXL) 
                                    deltaX = item2check.Value.Bounding.MinX - shapes3D[shapeName].Bounding.MaxX;

                                if(collisionInXR) 
                                    deltaX = item2check.Value.Bounding.MaxX - shapes3D[shapeName].Bounding.MinX;
                            } 

                            if(asVy)
                            {
                                if(collisionInYD)
                                    deltaY =  item2check.Value.Bounding.MinY - shapes3D[shapeName].Bounding.MaxY;

                                if(collisionInYU)
                                    deltaY =  item2check.Value.Bounding.MaxY - shapes3D[shapeName].Bounding.MinY;
                            }

                            if(asVz)
                            {
                                if(collisionInZB)
                                    deltaZ = item2check.Value.Bounding.MinZ - shapes3D[shapeName].Bounding.MaxZ;

                                if(collisionInZF)
                                    deltaZ = item2check.Value.Bounding.MaxZ - shapes3D[shapeName].Bounding.MinZ;

                                
                            }

                            

                            //detect the face that has collided X or Z
                            float dpX = Math.Abs(shapes3D[shapeName].Position.X - item2check.Value.Position.X) / item2check.Value.Bounding.BoxXLength;
                            float dpZ = Math.Abs(shapes3D[shapeName].Position.Z - item2check.Value.Position.Z) / item2check.Value.Bounding.BoxZLength;

                            float el = -0.7f;

                            if (dpX > dpZ)
                            {
                                shapes3D[shapeName].Physic.Vxyz
                                 = new Vector3(
                                     shapes3D[shapeName].Physic.Vxyz.X * el,
                                     shapes3D[shapeName].Physic.Vxyz.Y,
                                     shapes3D[shapeName].Physic.Vxyz.Z);

                                Vector3 newPosition = new Vector3(
                                 shapes3D[shapeName].Position.X + deltaX * 1.01f,
                                 shapes3D[shapeName].Position.Y,
                                 shapes3D[shapeName].Position.Z);

                                shapes3D[shapeName].Position = newPosition;

                                //MyGame.Debug = "Col in X";
                            }

                            else if (dpX < dpZ)
                            {

                                shapes3D[shapeName].Physic.Vxyz
                                  = new Vector3(
                                      shapes3D[shapeName].Physic.Vxyz.X,
                                      shapes3D[shapeName].Physic.Vxyz.Y,
                                      shapes3D[shapeName].Physic.Vxyz.Z * el);

                                Vector3 newPosition = new Vector3(
                                  shapes3D[shapeName].Position.X,
                                  shapes3D[shapeName].Position.Y,
                                  shapes3D[shapeName].Position.Z + deltaZ * 1.01f);

                                shapes3D[shapeName].Position = newPosition;

                                //MyGame.Debug = "Col in Z";
                            }
                            else
                            {
                                shapes3D[shapeName].Physic.Vxyz
                                   = new Vector3(
                                       shapes3D[shapeName].Physic.Vxyz.X * el,
                                       shapes3D[shapeName].Physic.Vxyz.Y,
                                       shapes3D[shapeName].Physic.Vxyz.Z * el);

                                Vector3 newPosition = new Vector3(
                                  shapes3D[shapeName].Position.X + deltaX * 1.01f,
                                  shapes3D[shapeName].Position.Y,
                                  shapes3D[shapeName].Position.Z + deltaZ * 1.01f);

                                shapes3D[shapeName].Position = newPosition;

                                MyGame.Debug = "Col in ZX";

                            }
                            
                        }
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

        public static void SolveCollisionsOneShape(List<Collision> collisions, Dictionary<string, Shape3D> shapes3D, string shapeName)
        {
            foreach (var item in collisions)
            {
                
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
                           (Vector3.Dot(n, Vector3.Multiply(n, (1 / ma) + (1 / mb))));

                Vector3 nJma = Vector3.Multiply(n, (J / ma));

                // only moves shapeName
                if (item.Shape3Da == shapeName)
                    shapes3D[item.Shape3Da].Physic.Vxyz = Va + nJma;


                Vector3 nJmb = Vector3.Multiply(n, (J / mb));

                if (item.Shape3Db == shapeName)
                    shapes3D[item.Shape3Db].Physic.Vxyz = Vb - nJmb;



                /*

                Dictionary<string, Shape3D> checkOut = new Dictionary<string, Shape3D>();

                checkOut.Add(item.Shape3Da, shapes3D[item.Shape3Da]);
                checkOut.Add(item.Shape3Db, shapes3D[item.Shape3Db]);

                int colCount = CheckCollisionsOneShape(checkOut, shapeName).Count;
                int i = 0;
                while (colCount > 0)
                {
                    if (item.Shape3Da == shapeName)
                    {
                        checkOut[item.Shape3Da].TranslateWC(
                        checkOut[item.Shape3Da].Physic.Vxyz.X,
                        checkOut[item.Shape3Da].Physic.Vxyz.Y,
                        checkOut[item.Shape3Da].Physic.Vxyz.Z);
                    }

                    if (item.Shape3Db == shapeName)
                    {
                        checkOut[item.Shape3Db].TranslateWC(
                        checkOut[item.Shape3Db].Physic.Vxyz.X,
                        checkOut[item.Shape3Db].Physic.Vxyz.Y,
                        checkOut[item.Shape3Db].Physic.Vxyz.Z);
                    }
                    

                    colCount = CheckCollisions(checkOut).Count;

                    if (i == 10) break;
                    i++;
                }*/
            }
        }
    }
}
