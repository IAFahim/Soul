using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soul.Model.Runtime.Times
{
    [Serializable]
    public class RealTimeIntervalTicker
    {
        private readonly WaitForSecondsRealtime tickDelay;

        private readonly MonoBehaviour coroutineRunner;
        private Coroutine timerCoroutine;

#if UNITY_EDITOR
        [SerializeField] private bool isRunning;
        [SerializeField] private float duration;
        [SerializeField] private float progress;
#endif

        public RealTimeIntervalTicker(MonoBehaviour coroutineRunner, float tickInterval = 1)
        {
            tickDelay = new WaitForSecondsRealtime(tickInterval);
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Starts the real-time ticker.
        /// </summary>
        /// <param name="initialProgress">The initial progress of the timer (0 to 1).</param>
        /// <param name="fullDuration">The total duration of the timer in seconds.</param>
        /// <param name="onTickProgress">Callback invoked on each tick with the progress (0 to 1).</param>
        /// <param name="onTickCompleted">Callback invoked when the timer completes.</param>
        public void Start(float initialProgress, float fullDuration, Action<float> onTickProgress, Action onTickCompleted)
        {
            if (timerCoroutine != null)
                coroutineRunner.StopCoroutine(timerCoroutine);

            if (initialProgress >= 1)
            {
                onTickProgress?.Invoke(1);
                onTickCompleted?.Invoke();
                return;
            }

            timerCoroutine = coroutineRunner.StartCoroutine(
                TimerRoutine(initialProgress, fullDuration, onTickProgress, onTickCompleted)
            );
        }

        public void Stop()
        {
            if (timerCoroutine == null) return;
            coroutineRunner.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        private IEnumerator TimerRoutine(float initialProgress, float fullDuration, Action<float> onTickProgress,
            Action onTickCompleted)
        {
            var elapsedTime = initialProgress * fullDuration;

#if UNITY_EDITOR
            isRunning = true;
            duration = fullDuration;
#endif
            float initialDelay = Mathf.Ceil(elapsedTime) - elapsedTime;
            onTickProgress?.Invoke(initialProgress);
            if (initialDelay > 0)
            {
#if UNITY_EDITOR
                progress = Mathf.Ceil(elapsedTime) / fullDuration;
#endif
                var initialDelayTime = new WaitForSecondsRealtime(initialDelay);
                yield return initialDelayTime;
                elapsedTime += initialDelayTime.waitTime; // Increment elapsedTime by the actual initial delay
#if UNITY_EDITOR
                progress = Mathf.Ceil(elapsedTime) / fullDuration;
#endif
            }

            onTickProgress?.Invoke(Mathf.Ceil(elapsedTime) / fullDuration);

            while (elapsedTime < fullDuration)
            {
                yield return tickDelay;
                elapsedTime += tickDelay.waitTime; // Increment elapsedTime by the actual tick duration
                onTickProgress?.Invoke(elapsedTime / fullDuration);

#if UNITY_EDITOR
                progress = elapsedTime / fullDuration;
#endif
            }

#if UNITY_EDITOR
            isRunning = false;
            progress = 1f;
#endif

            onTickCompleted?.Invoke();
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RealTimeIntervalTicker))]
    public class RealTimeIntervalTickerDrawer : PropertyDrawer
    {
        private static GUIStyle infoTextStyle;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            infoTextStyle ??= new GUIStyle(EditorStyles.miniLabel)
            {
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = new Color(0.6f, 0.6f, 1f)
                }
            };
            
            var isRunningProp = property.FindPropertyRelative("isRunning");
            var durationProp = property.FindPropertyRelative("duration");
            var progressProp = property.FindPropertyRelative("progress");

            
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var infoRect = new Rect(labelRect.xMax, position.y, position.width - labelRect.width, position.height);

            EditorGUI.LabelField(labelRect, label);

            if (isRunningProp.boolValue)
            {
                var durationText = $"Duration: {durationProp.floatValue:F2}s";
                var progressText = $"Progress: {(progressProp.floatValue * 100):F0}%";
                EditorGUI.LabelField(infoRect, $"{durationText} - {progressText}", infoTextStyle);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}