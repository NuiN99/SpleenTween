using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpleenTween
{
    /// <summary>
    /// Spawned on load and persists through the game. Contains and runs all active tweens
    /// </summary>
    public class SpleenTweenManager : MonoBehaviour
    {
        public int ActiveTweensCount => _activeTweens.Count;

        readonly List<Tween> _activeTweens = new();

        public static float fixedDeltaTime;

        public static SpleenTweenManager Instance;

        /// <summary>
        /// Spawns a new instance when the game loads for the first time
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void Initialize() => new GameObject("SpleenTweenManager").AddComponent<SpleenTweenManager>();

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(this.gameObject);

            fixedDeltaTime = Time.fixedDeltaTime;
        }

        void Update() => RunTweens();

        /// <summary>
        /// Runs every frame updating every tween's values, and stops them when they are complete
        /// </summary>
        void RunTweens()
        {
            for (int i = 0; i < _activeTweens.Count; i++)
            {
                Tween tween = _activeTweens[i];
                tween.Run();
                if (tween.Complete())
                {
                    _activeTweens.RemoveAt(i);
                }
            }
        }

        public void AddTween(Tween tween) => _activeTweens.Add(tween);
        public void RemoveTween(Tween tween) 
        {
            _activeTweens.Remove(tween);
        }

        public void RemoveAllTweens() => _activeTweens.Clear();

        public void RemoveAllTweensWithIdentifier(GameObject identifier)
        {
            if (identifier == null) return;

            for(int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = _activeTweens[i];

                if (tween.Identifier != identifier) continue;

                _activeTweens[i] = null;
                _activeTweens.RemoveAt(i);
            }
        }

        void StopAllOnSceneLoad(Scene a, Scene b) => RemoveAllTweens();

        void OnEnable() => SceneManager.activeSceneChanged += StopAllOnSceneLoad;
        void OnDisable() => SceneManager.activeSceneChanged -= StopAllOnSceneLoad;
    }

}
