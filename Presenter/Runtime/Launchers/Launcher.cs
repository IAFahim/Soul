using Cysharp.Threading.Tasks;
using Pancake;
using Sisus.Init;
using Soul.Model.Runtime;
using Soul.Model.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Soul.Presenter.Runtime.Launchers
{
    public class Launcher : MonoBehaviour<ILoading>
    {
        private ILoading _loading;

        protected override void Init(ILoading argument) { _loading = argument; }

        private void Awake()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = (int) HeartEditorSettings.TargetFrameRate;
#else
            Application.targetFrameRate = (int) HeartSettings.TargetFrameRate;
#endif

            Input.multiTouchEnabled = HeartSettings.EnableMultipleTouch;

            
        }

        private void Start()
        {
            LoadScene();
        }

        private async void LoadScene()
        {
            await SceneManager.LoadSceneAsync(Constant.Scene.Persistent, LoadSceneMode.Single);
            await SceneManager.LoadSceneAsync(Constant.Scene.Infrastructure, LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync(Constant.Scene.Environment, LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync(Constant.Scene.NPC, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnMenuSceneLoaded;
            await UniTask.WaitUntil(() => _loading.IsLoadingCompleted);
            await SceneManager.LoadSceneAsync(Constant.Scene.Menu, LoadSceneMode.Additive);
        }

        private void OnMenuSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMenuSceneLoaded;
            SceneManager.SetActiveScene(scene);
        }
    }
}