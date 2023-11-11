using System;
using UnityEngine;

namespace SpleenTween.Extensions
{
    public enum Axis { x, y, z };

    /// <summary>
    /// Various helper functions for improved code readability
    /// </summary>
    public static class SpleenExt
    {
        static void SetAxis(Axis axis, Vector3 inVal, float targetVal, Action<Vector3> setAxis)
        {
            Vector3 newVal = inVal;
            switch (axis)
            {
                case Axis.x: newVal.x = targetVal; break;
                case Axis.y: newVal.y = targetVal; break;
                case Axis.z: newVal.z = targetVal; break;
            }
            setAxis(newVal);
        }
        static void AddAxis(Axis axis, float increment, Action<Vector3> setAxis)
        {
            Vector3 newVal = Vector3.zero;
            switch (axis)
            {
                case Axis.x: newVal.x = increment; break;
                case Axis.y: newVal.y = increment; break;
                case Axis.z: newVal.z = increment; break;
            }
            setAxis(newVal);
        }

        public static float GetAxis(Axis axis, Vector3 inVal)
        {
            return axis switch
            {
                Axis.x => inVal.x,
                Axis.y => inVal.y,
                Axis.z => inVal.z,
                _ => throw new MissingMemberException("Axis somehow does not exist")
            };
        }

        public static void SetPosAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.position, targetVal,
            (val) => target.transform.position = val);
        public static void SetLocalPosAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.localPosition, targetVal,
            (val) => target.transform.localPosition = val);
        public static void SetScaleAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.localScale, targetVal,
            (val) => target.transform.localScale = val);
        public static void SetRotAxis(Axis axis, Transform target, float targetVal) => SetAxis(axis, target.eulerAngles, targetVal,
            (val) => target.transform.eulerAngles = val);
        
        public static void SetRBPosAxis(Axis axis, Rigidbody target, float targetVal) => SetAxis(axis, target.position, targetVal, target.MovePosition);

        public static void AddPosAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.position += val);
        public static void AddLocalPosAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localPosition += val);
        public static void AddScaleAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localScale += val);
        public static void AddRotAxis(Axis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.eulerAngles += val);


        /// <summary>
        /// Checks the type of passed in generic and if possible performs A + B
        /// </summary>
        public static T AddGenerics<T>(T a, T b)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)a + (float)(object)b);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a + (Vector3)(object)b);
            }
            else if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a + (Color)(object)b);
            }
            else
            {
                throw new System.NotSupportedException("Type not supported for generic addition");
            }
        }

        /// <summary>
        /// Checks the type of passed in generic and if possible performs A - B
        /// </summary>
        public static T SubtractGenerics<T>(T a, T b)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)a - (float)(object)b);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a - (Vector3)(object)b);
            }
            else if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a - (Color)(object)b);
            }
            else
            {
                throw new System.NotSupportedException("Type not supported for generic subtraction");
            }
        }

        /// <summary>
        /// Checks the type of passed in generic and if possible lerps unclamped between A and B
        /// </summary>
        public static T LerpUnclampedGeneric<T>(T from, T to, float lerpProgress)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)Mathf.LerpUnclamped((float)(object)from, (float)(object)to, lerpProgress);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)Vector3.LerpUnclamped((Vector3)(object)from, (Vector3)(object)to, lerpProgress);
            }
            else if (typeof(T) == typeof(Color))
            {
                return (T)(object)Color.LerpUnclamped((Color)(object)from, (Color)(object)to, lerpProgress);
            }
            else
            {
                throw new System.NotSupportedException("Type not supported for generic lerping");
            }
        }
    }
}
