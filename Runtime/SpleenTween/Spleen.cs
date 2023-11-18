namespace SpleenTween
{
    using System;
    using SpleenTween.Extensions;
    using UnityEngine;
    
    /// <summary>
    /// Call the functions in this to start a tween
    /// </summary>
    public static class Spleen
    {
        public static void AddTween(Tween tween) => SpleenTweenManager.Instance.AddTween(tween);
        public static void StopTween(Tween tween) => SpleenTweenManager.Instance.RemoveTween(tween);
        public static void StopAllTweens() => SpleenTweenManager.Instance.RemoveAllTweens();
        public static void StopAllTweens(GameObject target) => SpleenTweenManager.Instance.RemoveAllTweensWithIdentifier(target);

        static Tween CreateTween<T>(T from, T to, float duration, Ease easing, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, easing, onUpdate);
            AddTween(tween);
            return tween;
        }

        static Tween CreateTargetTween<T,K>(K target, GameObject identifier, T from, T to, float duration, Ease easing, Action<T> onUpdate)
        {
            Tween tween = new TweenInstance<T>(from, to, duration, easing, onUpdate, identifier, () => identifier == null || target == null || target.Equals(null));
            AddTween(tween);
            return tween;
        }

        static Tween CreateRelativeTargetTween<T, K>(K target, GameObject identifier, T increment, float duration, Ease easing, Func<T> currentVal, Action<T, T> onUpdate)
        {
            T current = currentVal.Invoke();
            T from = current;
            T to = SpleenExt.AddGenerics(current, increment);

            Tween tween = new TweenInstance<T>(from, to, duration, easing, (val) =>
            {
                onUpdate.Invoke(val, current);
                current = val;
            }, identifier, () => target == null || target.Equals(null) || identifier == null);

            tween.OnStart(() =>
            {
                if (Looping.IsLoopWeird(tween.LoopType)) return;

                current = currentVal.Invoke();
                from = current;
                to = SpleenExt.AddGenerics(current, increment);
                tween.From = from;
                tween.To = to;
            });

            AddTween(tween);
            return tween;
        }

        #region Create Tweens
        public static Tween Value(float from, float to, float duration, Ease easing, Action<float> onUpdate) => 
            CreateTween(from, to, duration, easing, onUpdate);
        public static Tween Value3(Vector3 from, Vector3 to, float duration, Ease easing, Action<Vector3> onUpdate) => 
            CreateTween(from, to, duration, easing, onUpdate);

        public static Tween DoAfter(float seconds, Action onComplete) =>
            CreateTween(0f, seconds, seconds, Ease.Linear, null).OnComplete(onComplete);
        public static Tween DoFor(float seconds, Action onUpdate) =>
            CreateTween(0f, seconds, seconds, Ease.Linear, (_) => onUpdate?.Invoke());
        public static Tween DoWhen(Func<bool> condition, Action doWhen, float timeOutAfterSeconds = float.MaxValue) =>
            CreateTween(0f, 1f, timeOutAfterSeconds, Ease.Linear, null).StopIf(condition, true).OnComplete(doWhen);
        public static Tween DoUntil(Func<bool> condition, Action doUntil, float timeOutAfterSeconds = float.MaxValue) =>
            CreateTween(0f, 1f, timeOutAfterSeconds, Ease.Linear, (_) => doUntil?.Invoke()).StopIf(condition);
        
        
        public static Tween Pos(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.position = val);
        public static Tween Pos(Transform target, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, target.transform.position, to, duration, easing, val => target.position = val);
        
        public static Tween PosAxis(Transform target, Axis axis, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetPosAxis(axis, target, val));
        public static Tween PosAxis(Transform target, Axis axis, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.position), to, duration, easing, val => SpleenExt.SetPosAxis(axis, target, val));
        
        public static Tween AddPos(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.position, (val, from) => target.position += val - from);
        public static Tween AddPosAxis(Transform target, Axis axis, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => SpleenExt.GetAxis(axis, target.position), (val, from) => SpleenExt.AddPosAxis(axis, target, val - from));
        
        
        public static Tween LocalPos(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.localPosition = val);
        public static Tween LocalPos(Transform target, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, target.transform.localPosition, to, duration, easing, val => target.localPosition = val);
        
        public static Tween LocalPosAxis(Transform target, Axis axis, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, 
                val => SpleenExt.SetLocalPosAxis(axis, target, val));
        public static Tween LocalPosAxis(Transform target, Axis axis, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.localPosition), to, duration, easing, val => SpleenExt.SetLocalPosAxis(axis, target, val));
        
        public static Tween AddLocalPos(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localPosition, (val, from) => target.localPosition += val - from);
        public static Tween AddLocalPosAxis(Transform target, Axis axis, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => SpleenExt.GetAxis(axis, target.localPosition), (val, from) => SpleenExt.AddLocalPosAxis(axis, target, val - from));
        
        
        public static Tween RBPos(Rigidbody target, Vector3 from, Vector3 to, float duration, Ease easing) => 
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, target.MovePosition);
        public static Tween RBPos(Rigidbody target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.position, to, duration, easing, target.MovePosition);
        
        public static Tween RBPosAxis(Rigidbody target, Axis axis, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetRBPosAxis(axis, target, val));
        public static Tween RBPosAxis(Rigidbody target, Axis axis, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.position), to, duration, easing, val => SpleenExt.SetRBPosAxis(axis, target, val));

        
        public static Tween Scale(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.localScale = val);
        public static Tween Scale(Transform target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.localScale, to, duration, easing, val => target.localScale = val);
        
        public static Tween ScaleAxis(Transform target, Axis axis, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetScaleAxis(axis, target, val));
        public static Tween ScaleAxis(Transform target, Axis axis, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.localScale), to, duration, easing, val => SpleenExt.SetScaleAxis(axis, target, val));

        public static Tween AddScale(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.localScale, (val, from) => 
            target.localScale += val - from);
        public static Tween AddScaleAxis(Transform target, Axis axis, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => SpleenExt.GetAxis(axis, target.localScale), (val, from) => SpleenExt.AddScaleAxis(axis, target, val - from));

        
        public static Tween Rot(Transform target, Vector3 from, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.eulerAngles = val);
        public static Tween Rot(Transform target, Vector3 to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.eulerAngles, to, duration, easing, val => target.eulerAngles = val);
        
        public static Tween RotAxis(Transform target, Axis axis, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => SpleenExt.SetRotAxis(axis, target, val));
        public static Tween RotAxis(Transform target, Axis axis, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, 
                SpleenExt.GetAxis(axis, target.eulerAngles), to, duration, easing, val => SpleenExt.SetRotAxis(axis, target, val));

        public static Tween AddRot(Transform target, Vector3 increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.eulerAngles, (val, from) => 
            target.eulerAngles += val - from);
        public static Tween AddRotAxis(Transform target, Axis axis, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => SpleenExt.GetAxis(axis, target.eulerAngles), (val, from) => SpleenExt.AddRotAxis(axis, target, val - from));
        

        public static Tween Vol(AudioSource target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.volume = val);
        public static Tween Vol(AudioSource target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.volume, to, duration, easing, val => target.volume = val);
        public static Tween AddVol(AudioSource target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => target.volume, (val, from) => target.volume += val - from);

        
        public static Tween SRColor(SpriteRenderer target, Color from, Color to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.color = val);
        public static Tween SRColor(SpriteRenderer target, Color to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.color, to, duration, easing, val => target.color = val);
        public static Tween AddSRColor(SpriteRenderer target, Color increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, () => target.color, (val, from) =>
            target.color += val - from);
        
        public static Tween SRAlpha(SpriteRenderer target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));
        public static Tween SRAlpha(SpriteRenderer target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.color.a, to, duration, easing, 
                val => target.color = new Color(target.color.r, target.color.g, target.color.b, val));


        public static Tween CamFOV(Camera target, float from, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, from, to, duration, easing, val => target.fieldOfView = val);
        public static Tween CamFOV(Camera target, float to, float duration, Ease easing) =>
            CreateTargetTween(target, target.gameObject, target.fieldOfView, to, duration, easing, val => target.fieldOfView = val);
        public static Tween AddCamFOV(Camera target, float increment, float duration, Ease easing) =>
            CreateRelativeTargetTween(target, target.gameObject, increment, duration, easing, 
                () => target.fieldOfView, (val, from) => target.fieldOfView += val - from);


        public static Tween TimeScale(float from, float to, float duration, Ease easing)
        {
            Tween tween = CreateTween(from, to, duration, easing, val =>
            {
                Time.timeScale = val;
                Time.fixedDeltaTime = SpleenTweenManager.fixedDeltaTime * val;
            });
            
            tween.OnUpdate<float>(val => tween.SetPlaybackSpeed(1 / val));
            return tween;
        }
        public static Tween TimeScale(float to, float duration, Ease easing)
        {
            Tween tween = CreateTween(Time.timeScale, to, duration, easing, val =>
            {
                Time.timeScale = val;
                Time.fixedDeltaTime = SpleenTweenManager.fixedDeltaTime * val;
            });
            
            tween.OnUpdate<float>(val => tween.SetPlaybackSpeed(1 / val));
            return tween;
        }


        #endregion
    }
}
