using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Soul.Model.Runtime.TrackTime
{
    public static class Track
    {
        private class TaskInfo
        {
#if UNITY_EDITOR
            public int Id;
            public string Name;
            public float StartTime;
            public float Duration;
            public bool Keep;
            public readonly List<TaskInfo> Children = new();
            public bool IsRunning => Progress.Exists(Id) && Progress.GetStatus(Id) == Progress.Status.Running;
            public float CurrentProgress;
            public Progress.Status Status;
#endif
        }


        private static readonly Dictionary<string, TaskInfo> AllTasks = new Dictionary<string, TaskInfo>();
        private static bool _isInitialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (!_isInitialized)
            {
#if UNITY_EDITOR
                EditorApplication.update -= Update;
                EditorApplication.update += Update;
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
                _isInitialized = true;
            }
        }

        public static void Start(string name, float duration, bool keep = true, float initialProgress = 0f)
        {
#if UNITY_EDITOR
            if (AllTasks.TryGetValue(name, out TaskInfo existingTask))
            {
                if (existingTask.IsRunning)
                {
                    Debug.LogWarning($"Task '{name}' is already running. Updating its duration and progress.");
                    existingTask.Duration = duration;
                    SetProgress(name, initialProgress);
                    return;
                }
                else
                {
                    AllTasks.Remove(name);
                }
            }

            int id = Progress.Start(name, $"{DateTime.Now}", Progress.Options.Sticky);
            TaskInfo taskInfo = new TaskInfo
            {
                Id = id,
                Name = name,
                StartTime = (float)EditorApplication.timeSinceStartup,
                Duration = duration,
                Keep = keep,
                Status = Progress.Status.Running,
                CurrentProgress = initialProgress
            };

            AllTasks[name] = taskInfo;
            Progress.RegisterCancelCallback(id, () =>
            {
                End(name);
                return true;
            });

            SetProgress(name, initialProgress);
#endif
        }

        public static void SetProgress(string name, float progress)
        {
#if UNITY_EDITOR
            if (AllTasks.TryGetValue(name, out TaskInfo taskInfo) && taskInfo.IsRunning)
            {
                taskInfo.CurrentProgress = Mathf.Clamp01(progress);
                Progress.Report(taskInfo.Id, taskInfo.CurrentProgress, $"Progress: {taskInfo.CurrentProgress:P0}");
            }
            else
            {
                Debug.LogWarning($"Task '{name}' not found or not running. Unable to set progress.");
            }
#endif
        }

        public static void Note(string parentName, string noteName, float time, bool complete = false)
        {
#if UNITY_EDITOR
            if (AllTasks.TryGetValue(parentName, out TaskInfo parentTask) && parentTask.IsRunning)
            {
                string description = complete ? $"{noteName} after {time}s" : $"{noteName} in {time}s";
                int childId = Progress.Start(noteName, description, Progress.Options.Sticky, parentTask.Id);
                TaskInfo childTask = new TaskInfo
                {
                    Id = childId,
                    Name = noteName,
                    StartTime = (int)EditorApplication.timeSinceStartup,
                    Duration = time,
                    Keep = parentTask.Keep,
                    Status = complete ? Progress.Status.Canceled : Progress.Status.Running
                };

                parentTask.Children.Add(childTask);

                if (complete)
                {
                    Progress.Finish(childId, Progress.Status.Canceled);
                }
            }
            else
            {
                Debug.LogWarning($"Parent task '{parentName}' not found or not running. Unable to add note.");
            }
#endif
        }

        public static void End(string name)
        {
#if UNITY_EDITOR
            if (AllTasks.TryGetValue(name, out TaskInfo taskInfo) && taskInfo.IsRunning)
            {
                Progress.Finish(taskInfo.Id, Progress.Status.Succeeded);
                taskInfo.Status = Progress.Status.Succeeded;
                if (!taskInfo.Keep)
                {
                    AllTasks.Remove(name);
                }
            }
#endif
        }

        private static void Update()
        {
#if UNITY_EDITOR
            double now = EditorApplication.timeSinceStartup;

            foreach (var task in AllTasks.Values.ToList())
            {
                UpdateTaskAndChildren(task, now);
            }
#endif
        }

        private static void UpdateTaskAndChildren(TaskInfo task, double now)
        {
#if UNITY_EDITOR
            if (task.IsRunning)
            {
                float elapsedTime = (float)(now - task.StartTime);
                float progress = Mathf.Max(task.CurrentProgress, Mathf.Clamp01(elapsedTime / task.Duration));
                Progress.Report(task.Id, progress, $"Progress: {progress:P0}");

                if (progress >= 1f)
                {
                    Progress.Finish(task.Id, Progress.Status.Succeeded);
                    task.Status = Progress.Status.Succeeded;
                }
            }

            foreach (var childTask in task.Children.ToList())
            {
                UpdateTaskAndChildren(childTask, now);
            }
#endif
        }

#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearNonPersistentTasks();
            }
        }
#endif
        private static void ClearNonPersistentTasks()
        {
#if UNITY_EDITOR
            foreach (var task in AllTasks.Values.ToList())
            {
                if (!task.Keep)
                {
                    RemoveTaskAndChildren(task);
                    AllTasks.Remove(task.Name);
                }
            }
#endif
        }

        private static void RemoveTaskAndChildren(TaskInfo task)
        {
#if UNITY_EDITOR
            if (Progress.Exists(task.Id))
            {
                Progress.Remove(task.Id);
            }

            foreach (var childTask in task.Children)
            {
                RemoveTaskAndChildren(childTask);
            }
#endif
        }

        public static void Clear()
        {
#if UNITY_EDITOR
            foreach (var task in AllTasks.Values)
            {
                RemoveTaskAndChildren(task);
            }

            AllTasks.Clear();
#endif
        }

        public static void Show()
        {
#if UNITY_EDITOR
            Progress.ShowDetails();
#endif
        }
    }
}