namespace SpleenTween
{
    using System;
    using UnityEngine;
    
    public enum Ease
    {
        Linear,

        InSine, OutSine, InOutSine,
        InQuad, OutQuad, InOutQuad,
        InCubic, OutCubic, InOutCubic,
        InQuart, OutQuart, InOutQuart,
        InQuint, OutQuint, InOutQuint,
        InExpo, OutExpo, InOutExpo,
        InCirc, OutCirc, InOutCirc,
        InBack, OutBack, InOutBack,
        InElastic, OutElastic, InOutElastic,
        InBounce, OutBounce, InOutBounce
    }
    
    /// <summary>
    /// Easing formulas
    /// </summary>
    public static class Easing
    {
        public static float EaseVal(float lerpValue, Ease easing)
        {
            return easing switch
            {
                Ease.Linear => lerpValue,

                Ease.InSine => 1 - Mathf.Cos(lerpValue * Mathf.PI / 2),
                Ease.OutSine => Mathf.Sin(lerpValue * Mathf.PI / 2),
                Ease.InOutSine => -(Mathf.Cos(Mathf.PI * lerpValue) - 1) / 2,

                Ease.InQuad => lerpValue * lerpValue,
                Ease.OutQuad => 1 - (1 - lerpValue) * (1 - lerpValue),
                Ease.InOutQuad => lerpValue < 0.5 ? 2 * lerpValue * lerpValue : 1 - Mathf.Pow(-2 * lerpValue + 2, 2) / 2,

                Ease.InCubic => lerpValue * lerpValue * lerpValue,
                Ease.OutCubic => 1 - Mathf.Pow(1 - lerpValue, 3),
                Ease.InOutCubic => lerpValue < 0.5 ? 4 * lerpValue * lerpValue * lerpValue : 1 - Mathf.Pow(-2 * lerpValue + 2, 3) / 2,

                Ease.InQuart => lerpValue * lerpValue * lerpValue * lerpValue,
                Ease.OutQuart => 1 - Mathf.Pow(1 - lerpValue, 4),
                Ease.InOutQuart => lerpValue < 0.5 ? 8 * lerpValue * lerpValue * lerpValue * lerpValue : 1 - Mathf.Pow(-2 * lerpValue + 2, 4) / 2,

                Ease.InQuint => lerpValue * lerpValue * lerpValue * lerpValue * lerpValue,
                Ease.OutQuint => 1 - Mathf.Pow(1 - lerpValue, 5),
                Ease.InOutQuint => lerpValue < 0.5 ? 16 * lerpValue * lerpValue * lerpValue * lerpValue * lerpValue : 1 - Mathf.Pow(-2 * lerpValue + 2, 5) / 2,

                Ease.InExpo => lerpValue == 0 ? 0 : Mathf.Pow(2, 10 * lerpValue - 10),
                Ease.OutExpo => Math.Abs(lerpValue - 1) < Mathf.Epsilon ? 1 : 1 - Mathf.Pow(2, -10 * lerpValue),
                Ease.InOutExpo => lerpValue == 0 ? 0 : Math.Abs(lerpValue - 1) < Mathf.Epsilon ? 1 : lerpValue < 0.5 ? Mathf.Pow(2, 20 * lerpValue - 10) / 2 : (2 - Mathf.Pow(2, -20 * lerpValue + 10)) / 2,

                Ease.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(lerpValue, 2)),
                Ease.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(lerpValue - 1, 2)),
                Ease.InOutCirc => lerpValue < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * lerpValue, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * lerpValue + 2, 2)) + 1) / 2,

                Ease.InBack => (1.70158f + 1) * lerpValue * lerpValue * lerpValue - 1.70158f * lerpValue * lerpValue,
                Ease.OutBack => 1 + (1.70158f + 1) * Mathf.Pow(lerpValue - 1, 3) + 1.70158f * Mathf.Pow(lerpValue - 1, 2),
                Ease.InOutBack => lerpValue < 0.5 ? (Mathf.Pow(2 * lerpValue, 2) * (((1.70158f * 1.525f) + 1) * 2 * lerpValue - (1.70158f * 1.525f))) / 2 : (Mathf.Pow(2 * lerpValue - 2, 2) * (((1.70158f * 1.525f) + 1) * (lerpValue * 2 - 2) + (1.70158f * 1.525f)) + 2) / 2,

                Ease.InElastic => lerpValue == 0 ? 0 : Math.Abs(lerpValue - 1) < Mathf.Epsilon ? 1 : -Mathf.Pow(2, 10 * lerpValue - 10) * Mathf.Sin((lerpValue * 10 - 10.75f) * ((2 * Mathf.PI) / 3)),
                Ease.OutElastic => lerpValue == 0 ? 0 : Math.Abs(lerpValue - 1) < Mathf.Epsilon ? 1 : Mathf.Pow(2, -10 * lerpValue) * Mathf.Sin((lerpValue * 10 - 0.75f) * ((2 * Mathf.PI) / 3)) + 1,
                Ease.InOutElastic => lerpValue == 0 ? 0 : Math.Abs(lerpValue - 1) < Mathf.Epsilon ? 1 : lerpValue < 0.5 ? -(Mathf.Pow(2, 20 * lerpValue - 10) * Mathf.Sin((20 * lerpValue - 11.125f) * ((2 * Mathf.PI) / 4.5f))) / 2 : (Mathf.Pow(2, -20 * lerpValue + 10) * Mathf.Sin((20 * lerpValue - 11.125f) * (2 * Mathf.PI) / 4.5f)) / 2 + 1,

                Ease.InBounce => InBounce(lerpValue),
                Ease.OutBounce => OutBounce(lerpValue),
                Ease.InOutBounce => InOutBounce(lerpValue),

                _ => throw new NotImplementedException(),
            };
        }

        static float InBounce(float lerpValue)
        {
            return 1 - OutBounce(1 - lerpValue);
        }
        static float OutBounce(float lerpValue)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            switch (lerpValue)
            {
                case < 1f / d1:
                    return n1 * lerpValue * lerpValue;
                case < 2f / d1:
                    return n1 * (lerpValue -= 1.5f / d1) * lerpValue + 0.75f;
                default:
                {
                    if (lerpValue < 2.5 / d1)
                        return n1 * (lerpValue -= 2.25f / d1) * lerpValue + 0.9375f;

                    return n1 * (lerpValue -= 2.625f / d1) * lerpValue + 0.984375f;
                }
            }
        }
        static float InOutBounce(float lerpValue)
        {
            return lerpValue < 0.5f ? (1 - OutBounce(1f - 2f * lerpValue)) / 2f : (1f + OutBounce(2f * lerpValue - 1f)) / 2f;
        }
    }
}