using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hypar.Geometry
{
    /// <summary>
    /// A 3D vector.
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("x")]
        public double X{get;}

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("y")]
        public double Y{get;}

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("z")]
        public double Z{get;}

        /// <summary>
        /// Construct a vector at the origin.
        /// </summary>
        /// <returns></returns>
        public static Vector3 Origin()
        {
            return new Vector3();
        }

        /// <summary>
        /// Construct a vector along the X axis.
        /// </summary>
        /// <returns></returns>
        public static Vector3 XAxis()
        {
            return new Vector3(1.0,0.0,0.0);
        }

        /// <summary>
        /// Construct a vector along the Y axis.
        /// </summary>
        /// <returns></returns>
        public static Vector3 YAxis()
        {
            return new Vector3(0.0,1.0,0.0);
        }

        /// <summary>
        /// Construct a vector along the Z axis.
        /// </summary>
        /// <returns></returns>
        public static Vector3 ZAxis()
        {
            return new Vector3(0.0,0.0,1.0);
        }

        /// <summary>
        /// Construct a vector from x, y, and z components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 ByXYZ(double x, double y, double z)
        {
            return new Vector3(x,y,z);
        }

        /// <summary>
        /// Construct a vector from x, and y components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 ByXY(double x, double y)
        {
            return new Vector3(x,y);
        }

        /// <summary>
        /// Construct vectors at n equal spaces along the provided line.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="n"></param>
        /// <param name="includeEnds"></param>
        /// <returns></returns>
        public static IEnumerable<Vector3> AtNEqualSpacesAlongLine(Line l, int n, bool includeEnds = false)
        {   
            var div = 1.0/(double)(n + 1);
            for(var t=0.0; t<=1.0; t+=div)
            {
                var pt = l.PointAt(t);
                
                if((t == 0.0 && !includeEnds) || (t == 1.0 && !includeEnds))
                {
                    continue;
                }
                yield return pt;
            }
        }

        /// <summary>
        /// Construct vectors at n equal spaces along a collection of lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="n"></param>
        /// <param name="includeEnds"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<Vector3>> AtNEqualSpacesAlongLines(IEnumerable<Line> lines, int n, bool includeEnds = false)
        {
            foreach(var l in lines)
            {
                var vs1 = Vector3.AtNEqualSpacesAlongLine(l, n, includeEnds);
                yield return vs1;
            }
        }

        /// <summary>
        /// Construct a default vector at the origin.
        /// </summary>
        public Vector3()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
        
        /// <summary>
        /// Construct a vector from x, y, and z components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Construct a vector from x, and y components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector3(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Get the length of this vector.
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(Math.Pow(X,2) + Math.Pow(Y,2) + Math.Pow(Z,2));
        }

        /// <summary>
        /// Return a new vector which is the normalized version of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalized()
        {
            var length = Length();
            return new Vector3(X/length, Y/length, Z/length);
        }

        /// <summary>
        /// Compute the cross product of this vector and v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 Cross(Vector3 v)
        {   
            var x = Y * v.Z - Z * v.Y;
            var y = Z * v.X - X * v.Z;
            var z = X * v.Y - Y * v.X;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Compute the dot product of this vector a v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Dot(Vector3 v)
        {
            return v.X * X + v.Y * Y + v.Z * Z;
        }

        /// <summary>
        /// The angle in radians from this vector to another vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double AngleTo(Vector3 v)
        {
            return Math.Acos((Dot(v)/(Length()*v.Length())));
        }

        /// <summary>
        /// Compute the average of this vector and v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 Average(Vector3 v)
        {
            return new Vector3((this.X + v.X)/2, (this.Y + v.Y)/2, (this.Z + v.Z)/2).Normalized();
        }

        /// <summary>
        /// Project vector a onto this vector.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Vector3 ProjectOnto(Vector3 a)
        {   
            var b = this;
            return (a.Dot(b)/Math.Pow(a.Length(),2)) * a;
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector3 operator * (Vector3 v, double a)
        {
            return new Vector3(v.X * a, v.Y * a, v.Z * a);
        }

        /// <summary>
        /// Multiply a scalar and a vector.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 operator * (double a, Vector3 v)
        {
            return new Vector3(v.X * a, v.Y * a, v.Z * a);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator - (Vector3 a, Vector3 b)
        {
            return new Vector3((a.X - b.X), (a.Y - b.Y), (a.Z - b.Z));
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator + (Vector3 a, Vector3 b)
        {
            return new Vector3((a.X + b.X), (a.Y + b.Y), (a.Z + b.Z));
        }

        /// <summary>
        /// Compute whether all components of vector a are greater than those of vector b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator > (Vector3 a, Vector3 b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }

        /// <summary>
        /// Compute whether all components of vector a are less than those of vector b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator < (Vector3 a, Vector3 b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        /// <summary>
        /// Determine whether this vector is parallel to v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsParallelTo(Vector3 v)
        {
            var result = Math.Abs(Dot(v));
            return result == 1.0;
        }

        /// <summary>
        /// Construct a new vector which is the inverse of this vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 Negated()
        {
            return new Vector3(-X, -Y, -Z);
        }

        /// <summary>
        /// Convert a vector's components to an array.
        /// </summary>
        /// <returns></returns>
        public double[] ToArray()
        {
            return new[]{X, Y, Z};
        }

        /// <summary>
        /// A string representation of the vector.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"X:{X},Y:{Y},Z:{Z}";
        }

        /// <summary>
        /// Determine whether this vector's components are equal to those of v, within tolerance.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Vector3 v, double tolerance = 0.00001)
        {
            if(Math.Abs(this.X - v.X) < tolerance &&
                Math.Abs(this.Y - v.Y) < tolerance &&
                Math.Abs(this.Z - v.Z) < tolerance)
            {
                return true;
            }
            return false;
        }
    }
}