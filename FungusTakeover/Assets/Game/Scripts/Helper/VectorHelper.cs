using System;
//using System.Numerics;
using UnityEngine;

namespace Helper
{
    public static class VectorHelper
    {
        /*public static Vector<T> AddTwoVectors<T>(Vector<T> vector1, Vector<T> vector2) where T : struct
        {
            return Vector.Add<T>(vector1,vector2); 
        }*/
        /*/// <summary> Uses system vectors.... </summary>
        public static Vector<T> AddForceToVector<T>(Vector<T> vector, Vector<T> forceDirection, T force) where T : struct
        {
            return Vector.Add<T>(vector, Vector.Multiply<T>(force, forceDirection));
        }*/

        /// <summary> Add two vectors together </summary>
        public static Vector3 AddTwoVectors(Vector3 vector, Vector3 secondVector) { return vector + secondVector; }
        /// <summary> Add two vectors together </summary>
        public static Vector2 AddTwoVectors(Vector2 vector, Vector2 secondVector) { return vector + secondVector; }
        /// <summary> Adding a normalized vector with a force to an already existing vector </summary>
        public static Vector3 AddForceToVector(Vector3 vector, Vector3 forceDirection, float force) { return vector + (forceDirection.normalized * force); }
        /// <summary> Adding a normalized vector with a force to an already existing vector </summary>
        public static Vector2 AddForceToVector(Vector2 vector, Vector2 forceDirection, float force) { return vector + (forceDirection.normalized * force); }
    }

}
