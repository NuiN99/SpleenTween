namespace SpleenTween.Extensions
{
    using System;
    using UnityEngine;
    
    public enum SpleenAxis { X, Y, Z };

    /// <summary>
    /// Various helper functions for improved code readability
    /// </summary>
    public static class SpleenExt
    {
        static void SetAxis(SpleenAxis axis, Vector3 inVal, float targetVal, Action<Vector3> setAxis)
        {
            Vector3 newVal = inVal;
            switch (axis)
            {
                case SpleenAxis.X: newVal.x = targetVal; break;
                case SpleenAxis.Y: newVal.y = targetVal; break;
                case SpleenAxis.Z: newVal.z = targetVal; break;
            }
            setAxis?.Invoke(newVal);
        }
        static void AddAxis(SpleenAxis axis, float increment, Action<Vector3> addAxis)
        {
            Vector3 newVal = Vector3.zero;
            switch (axis)
            {
                case SpleenAxis.X: newVal.x = increment; break;
                case SpleenAxis.Y: newVal.y = increment; break;
                case SpleenAxis.Z: newVal.z = increment; break;
            }
            addAxis?.Invoke(newVal);
        }

        public static float GetAxis(SpleenAxis axis, Vector3 inVal)
        {
            return axis switch
            {
                SpleenAxis.X => inVal.x,
                SpleenAxis.Y => inVal.y,
                SpleenAxis.Z => inVal.z,
                _ => throw new MissingMemberException("Axis somehow does not exist")
            };
        }

        public static void SetPosAxis(SpleenAxis axis, Transform target, float targetVal) => SetAxis(axis, target.position, targetVal,
            (val) => target.transform.position = val);
        public static void SetLocalPosAxis(SpleenAxis axis, Transform target, float targetVal) => SetAxis(axis, target.localPosition, targetVal,
            (val) => target.transform.localPosition = val);
        public static void SetScaleAxis(SpleenAxis axis, Transform target, float targetVal) => SetAxis(axis, target.localScale, targetVal,
            (val) => target.transform.localScale = val);
        public static void SetRotAxis(SpleenAxis axis, Transform target, float targetVal) => SetAxis(axis, target.eulerAngles, targetVal,
            (val) => target.transform.eulerAngles = val);

        public static void SetRBPosAxis(SpleenAxis axis, Rigidbody target, float targetVal) => SetAxis(axis, target.position, targetVal, target.MovePosition);
        public static void SetRB2DPosAxis(SpleenAxis axis, Rigidbody2D target, float targetVal) => SetAxis(axis, target.position, targetVal, pos => target.MovePosition(pos));

        public static void AddPosAxis(SpleenAxis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.position += val);
        public static void AddLocalPosAxis(SpleenAxis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localPosition += val);
        public static void AddScaleAxis(SpleenAxis axis, Transform target, float increment) => AddAxis(axis, increment,
            (val) => target.transform.localScale += val);
        public static void AddRotAxis(SpleenAxis axis, Transform target, float increment) => AddAxis(axis, increment,
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

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a + (Vector3)(object)b);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)((Vector2)(object)a + (Vector2)(object)b);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a + (Color)(object)b);
            }

            throw new System.NotSupportedException("Type not supported for generic addition");
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

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)a - (Vector3)(object)b);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)((Vector2)(object)a - (Vector2)(object)b);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)((Color)(object)a - (Color)(object)b);
            }

            throw new System.NotSupportedException("Type not supported for generic subtraction");
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

            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)Vector3.LerpUnclamped((Vector3)(object)from, (Vector3)(object)to, lerpProgress);
            }
            
            if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)Vector2.LerpUnclamped((Vector2)(object)from, (Vector2)(object)to, lerpProgress);
            }

            if (typeof(T) == typeof(Color))
            {
                return (T)(object)Color.LerpUnclamped((Color)(object)from, (Color)(object)to, lerpProgress);
            }

            throw new NotSupportedException("Type not supported for generic lerping");
        }
    }
}
