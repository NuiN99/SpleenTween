using System;
using SpleenTween.Extensions;
using UnityEngine;

namespace SpleenTween
{
    public class TweenInstance<T> : Tween
    {
        T _currentValue;
        T _from;
        T _to;

        Action<T> _onUpdate;
        Func<bool> _nullCheck;
        Func<bool> _stopCondition;

        Action _onComplete;
        Action _onStart;

        bool _started;
        bool _invokeCompleteAfterStopped;
        int _cycles;
        bool _completed;


        public GameObject Identifier { get; private set; }

        public object CurrentValue { get => _currentValue; set { } }
        public object From { get => _from; set { } }
        public object To { get => _to; set { } }

        public float CurrentTime { get; private set; }
        public float Duration { get; private set; }

        public float PlaybackSpeed { get; private set; } = 1f;

        public float Delay { get; private set; }

        public float LerpProgress { get => Mathf.Clamp01(CurrentTime / Duration); private set { } }

        public float EasedLerpProgress
        {
            get
            {
                float easeVal = Easing.EaseVal(LerpProgress, EaseType);

                switch (LoopType)
                {
                    case Loop.Rewind:
                        float backwardsLerp = 1 - LerpProgress;
                        float lerpBasedOnDirection = Direction == 0 ? backwardsLerp : LerpProgress;
                        return Easing.EaseVal(lerpBasedOnDirection, EaseType);
                    case Loop.Yoyo:
                        float backwardsEase = 1 - easeVal;
                        return Direction == 0 ? backwardsEase : easeVal;
                    default:
                        return easeVal;
                }
            }
            private set => _ = value;
        }

        public bool Active => CurrentTime < Duration;

        public Ease EaseType { get; }
        public Loop LoopType { get; private set; }

        /// <summary>
        /// How many times the tween will repeat
        /// </summary>
        public int Cycles => Mathf.Clamp(_cycles - CycleCount, -1, int.MaxValue);
        public int CycleCount { get; private set; }

        // will always be forward(1) if loop type is not rewind or yoyo
        public int Direction => (!Looping.IsLoopWeird(LoopType) || (CycleCount % 2) == 0) ? 1 : 0; 

        public bool Paused { get; private set; }

        public TweenInstance(T from, T to, float duration, Ease easeType, Action<T> onUpdate, GameObject identifier = null, Func<bool> nullCheck = null)
        {
            _from = from;
            _to = to;
            Duration = duration;
            EaseType = easeType;
            _onUpdate = onUpdate;
            Identifier = identifier;
            _nullCheck = nullCheck;
        }

        void Tween.Run()
        {
            if (NullTarget()) return; // stop if the target is null
            if (StopConditionMet() || Paused) return;

            CurrentTime += Time.deltaTime * PlaybackSpeed;

            switch (CurrentTime)
            {
                case < 0: return; // wait for delay

                case >= 0 when !_started: 
                    InvokeOnStart();  // tween has started
                    break;
            }

            if (!Active)
            {
                InvokeOnComplete();
                RestartLoop();

                EasedLerpProgress = Direction; // set final value for precision
            }

            UpdateValue();
        }

        bool Tween.Complete()
        {
            return !Active || NullTarget() || StopConditionMet();
        }

        void UpdateValue()
        {
            _currentValue = SpleenExt.LerpUnclampedGeneric(_from, _to, EasedLerpProgress);
            _onUpdate?.Invoke(_currentValue);
        }

        void InvokeOnComplete()
        {
            _started = false;
            _onComplete?.Invoke();
        } 
        void InvokeOnStart()
        {
            _started = true;
            _onStart?.Invoke();
        }

        void RestartLoop()
        {
            if (LoopType == Loop.None || Cycles is not (-1 or > 0)) return;

            Looping.RestartLoopTypes(LoopType, ref _from, ref _to);
            CurrentTime = 0;
            DelayCycle(Delay);
            CycleCount++;
        }

        void DelayCycle(float delay)
        {
            CurrentTime -= delay;
        }

        /// <summary>
        /// Checks if the target is null to avoid Null Ref Exceptions. The nullCheck callback is set when creating a tween if the tween has a target, eg. a transform.
        /// </summary>
        bool NullTarget()
        {
            return _nullCheck != null && _nullCheck.Invoke();
        }

        bool StopConditionMet()
        {
            bool conditionMet = _stopCondition != null && _stopCondition.Invoke();
            switch (conditionMet)
            {
                case true when _invokeCompleteAfterStopped && !_completed:
                    _completed = true;
                    InvokeOnComplete();
                    break;
            }
            return conditionMet;
        }

        #region Method Chains

        Tween Tween.OnComplete(Action onComplete)
        {
            _onComplete += onComplete;
            return this;
        }
        Tween Tween.OnStart(Action onStart)
        {
            _onStart += onStart;
            return this;
        }
        public Tween OnUpdate<U>(Action<U> onUpdate)
        {
            if (typeof(T) != typeof(U))
            {
                Debug.LogError("Incompatible Types in OnUpdate callback: OnUpdate callback will not run");
                return this;
            }

            _onUpdate += (value) => onUpdate((U)(object)value);
            return this;
        }

        Tween Tween.SetLoop(Loop loopType, int cycles)
        {
            if (cycles == 0)
                Spleen.StopTween(this);

            LoopType = loopType;

            _cycles = cycles - 1;

            return this;
        }

        Tween Tween.SetDelay(float delay, bool startDelay)
        {
            Delay = delay;
            if (startDelay) DelayCycle(delay);
            return this;
        }

        Tween Tween.StopIfNull(GameObject target)
        {
            _nullCheck += () => target == null;
            return this;
        }

        Tween Tween.StopIf(Func<bool> stopCondition, bool invokeComplete)
        {
            _invokeCompleteAfterStopped = invokeComplete;
            _stopCondition += stopCondition;
            return this;
        }

        Tween Tween.Pause()
        {
            Paused = true;
            return this;
        }
        Tween Tween.Play()
        {
            Paused = false;
            return this;
        }
        Tween Tween.Toggle()
        {
            Paused = !Paused;
            return this;
        }

        Tween Tween.SetPlaybackSpeed(float targetSpeed)
        {
            PlaybackSpeed = targetSpeed;
            return this;
        }
        Tween Tween.SetPlaybackSpeed(float targetSpeed, float smoothTime, Ease easing)
        {
            Spleen.Value(PlaybackSpeed, targetSpeed, smoothTime, easing, (val) => 
            {
                if(!Paused) PlaybackSpeed = val;
            });
            return this;
        }
        Tween Tween.SetPlaybackSpeed(float startSpeed, float targetSpeed, float smoothTime, Ease easing)
        {
            Spleen.Value(startSpeed, targetSpeed, smoothTime, easing, (val) =>
            {
                if (!Paused) PlaybackSpeed = val;
            });
            return this;
        }

        #endregion
    }
}

