### Package URL: 
```
https://github.com/NuiN99/SpleenTween.git
```

# SpleenTween - Tweening Library for Unity

# How To Use:

## Spleen.cs is the main way to start Tweens.

### Usually they follow the same structure:

Spleen.MethodName(target, from, to, duration, Ease type)

### Example:

Spleen.Pos(transform, from, to, duration, Ease)

### You can leave out the "from" paramater to default to what the value already was:

Spleen.ScaleX(Transform, to, duration, Ease type)


## Two main types of Tween - Target and Value

Value is for variables

Target is for Objects

### Example:

## Value: 

float someVariable;

Spleen.Value(from: 0f, to: 10f, duration: 2f, Ease: Ease.OutSine, onUpdate: tweenVal => someVariable = tweenVal);


## Target:

Spleen.Pos(target: this.transform, from: Vector3.zero, to: new Vector3(0, 10, 10), duration: 10f, Ease: Ease.InOutElastic);


# Tweens have various methods that you can chain:

## Callbacks:

.OnStart(Action onStart);

.OnUpdate(Action<T> onUpdate);

.OnComplete(Action onComplete);


## Modifiers:

.SetLoop(Loop loopType, int cycles = -1)); // default cycles is infinite

.SetDelay(float delay);

.SetPlaybackSpeed(float targetSpeed

.SetPlaybackSpeed(float targetSpeed, float smoothTime, Ease ease); // tween the playback speed


## Controls:

.Stop();

.StopIfNull(GameObject target); // this is automatically called on all Target tweens to avoid null reference exceptions

.StopIf(Func<bool> stopCondition);

.Pause();

.Play();

.Toggle();


### Examples:

Spleen.SRAlpha(SpriteRenderer, from, to, duration, Ease).OnComplete(DoSomething);

Spleen.RotZ(Transform, from, to, duration, Ease).SetDelay(2f).SetLoop(Loop.Yoyo);

Spleen.Pos(Transform, from, to, duration, Ease).OnUpdate(tweenVal => print(tweenVal)).StopIf(() => someConditon == false);


## Other Helpful Methods:

Spleen.DoAfter(float seconds, Action doAfterSeconds);

Spleen.DoFor(float seconds, Action doForSeconds);

Spleen.DoWhen(Func<bool> condition, Action doWhenConditionMet);

Spleen.DoUntil(Func<bool> condition, Action doUntilConditionMet)


Spleen.StopTween(Tween tween);

Spleen.StopAllTweens(); // stop EVERY tween

Spleen.StopAllTweens(GameObject target); // stop every tween that is targeting a specific GameObject

## Tweens have information:

### Example:

Tween tween = Spleen.RBPos(target: rb, to: new Vector3(10, 10, 10), duration: 3f, Ease.InBack);

tween.Active

tween.CurrentValue

## All Information

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





