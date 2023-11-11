using System;
using UnityEngine;

namespace SpleenTween 
{
    public interface Tween
    {
        /// <summary>
        /// Gets called every frame to calculate and update the values of the tween
        /// </summary>
        void Run();

        /// <summary>
        /// Returns true if the tween is finished or its target is null
        /// </summary>
        bool Complete();


        Tween OnStart(Action onStart);
        Tween OnUpdate<TU>(Action<TU> onUpdate);
        Tween OnComplete(Action onComplete);
        Tween SetLoop(Loop loopType, int cycles = -1);
        Tween SetDelay(float delay, bool startDelay = true);
        Tween StopIfNull(GameObject target);
        Tween StopIf(Func<bool> stopCondition, bool invokeComplete = false);

        Tween Pause();
        Tween Play();
        Tween Toggle();

        Tween SetPlaybackSpeed(float targetSpeed);

        /// <summary>
        /// Tweens the playback speed from the current playback speed
        /// </summary>
        Tween SetPlaybackSpeed(float targetSpeed, float smoothTime, Ease ease);

        /// <summary>
        /// Tweens the playback speed from the specified value
        /// </summary>
        Tween SetPlaybackSpeed(float startSpeed, float targetSpeed, float smoothTime, Ease ease);


        GameObject Identifier { get; }
        object CurrentValue { get; }
        object From { get; set; }
        object To { get; set; }
        float PlaybackSpeed { get; }
        float CurrentTime { get; } 
        float Duration { get; }
        float Delay { get; }
        float LerpProgress { get; }
        float EasedLerpProgress { get; }
        bool Active { get; }
        Ease EaseType { get; }
        Loop LoopType { get; }
        int Cycles { get; }
        int CycleCount { get; }
        int Direction { get; }
        bool Paused { get; }
    }
}
