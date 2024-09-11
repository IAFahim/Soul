using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soul.Model.Runtime.Times
{
    [Serializable]
    public class RangedTimer
    {
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _timerCoroutine;

#if UNITY_EDITOR
#pragma warning disable CS0414 // Field is assigned but its value is never used
        [SerializeField] private bool isRunning;
#pragma warning restore CS0414 // Field is assigned but its value is never used
        [SerializeField] private float duration;
        [SerializeField] private int totalStages;
        [SerializeField] private int currentStage;
#endif

        public RangedTimer(MonoBehaviour coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Starts the staged timer.
        /// </summary>
        /// <param name="initialProgress">The initial progress of the timer (0 to 1).</param>
        /// <param name="fullDuration">The total duration of the timer in seconds.</param>
        /// <param name="maxExclusive">The total number of stages in the timer.</param>
        /// <param name="onStageChanged">Callback invoked when the timer enters a new stage with the current stage index (0-based).</param>
        /// <param name="minInclusive">The start index</param>
        public void Start(float initialProgress, float fullDuration, int maxExclusive, Action<int> onStageChanged, int minInclusive = 0)
        {
            if (_timerCoroutine != null) _coroutineRunner.StopCoroutine(_timerCoroutine);

            if (initialProgress >= 1 || maxExclusive <= 0)
            {
                onStageChanged?.Invoke(Mathf.Clamp(maxExclusive - 1, minInclusive, maxExclusive - 1));
                return;
            }

            _timerCoroutine = _coroutineRunner.StartCoroutine(
                TimerRoutine(initialProgress, fullDuration, minInclusive, maxExclusive, onStageChanged)
            );
        }

        public void Stop()
        {
            if (_timerCoroutine == null) return;
            _coroutineRunner.StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }


        private IEnumerator TimerRoutine(float initialProgress, float fullDuration, int minInclusive, int maxExclusive,
            Action<int> onStageChanged)
        {
            var elapsedTime = initialProgress * fullDuration;

#if UNITY_EDITOR
            isRunning = true;
            duration = fullDuration;
            totalStages = maxExclusive;
#endif

            var currentStage = Mathf.Clamp(Mathf.FloorToInt(elapsedTime / fullDuration * (maxExclusive)), minInclusive,
                maxExclusive - 1);

#if UNITY_EDITOR
            this.currentStage = currentStage;
#endif
            onStageChanged?.Invoke(currentStage);

            while (currentStage < maxExclusive - 1)
            {
                var nextStage = currentStage + 1;
                var timeToNextStage = nextStage * (fullDuration / (maxExclusive)) - elapsedTime;

                yield return new WaitForSecondsRealtime(timeToNextStage);

                elapsedTime += timeToNextStage;
                currentStage = nextStage;

#if UNITY_EDITOR
                this.currentStage = currentStage;
#endif
                onStageChanged?.Invoke(currentStage);
            }

#if UNITY_EDITOR
            isRunning = false;
#endif
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RangedTimer))]
    public class StagedTimerDrawer : PropertyDrawer
    {
        private static GUIStyle infoTextStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            infoTextStyle ??= new GUIStyle(EditorStyles.miniLabel)
            {
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = new Color(0.6f, 0.6f, 1f)
                }
            };

            EditorGUI.BeginProperty(position, label, property);

            // Get the properties
            var isRunningProp = property.FindPropertyRelative("isRunning");
            var durationProp = property.FindPropertyRelative("duration");
            var totalStagesProp = property.FindPropertyRelative("totalStages");
            var currentStageProp = property.FindPropertyRelative("currentStage");

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var infoRect = new Rect(labelRect.xMax, position.y, position.width - labelRect.width, position.height);

            EditorGUI.LabelField(labelRect, label);

            if (isRunningProp.boolValue)
            {
                var stageText = $"Stage: {currentStageProp.intValue} - {totalStagesProp.intValue - 1}";
                var durationText = $"Duration: {durationProp.floatValue:F2}s";

                var infoText = $"{stageText}  |  {durationText}";

                EditorGUI.LabelField(infoRect, infoText, infoTextStyle);
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